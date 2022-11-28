using EutonTechnologies.ValidatonRules.Types.Analyzers.Helpers;
using EutonTechnologies.ValidatonRules.Types.Analyzers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

namespace EutonTechnologies.ValidatonRules.Types.Analyzers
{
    /// <summary>
    /// Validation Rules Generator is a Source Generator that will create a FluentValidation rule when it finds the Extend Validation Attribute.
    /// 
    /// </summary>
    [Generator]
    public class ValidationRulesGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<ValidationRuleInfo> declarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)!;

            IncrementalValueProvider<(Compilation Left, ImmutableArray<ValidationRuleInfo> Right)> valueProvider = context.CompilationProvider.Combine(declarations.Collect());

            context.RegisterSourceOutput(valueProvider, static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        /// <summary>
        /// We only care when there is at least one attribute.  We'll loop through them later to see if it is the one we care about.
        /// </summary>
        /// <param name="node">SyntaxNode to query</param>
        /// <returns>True/False if we should transform this node</returns>
        static bool IsSyntaxTargetForGeneration(SyntaxNode node)
            => node is BaseTypeDeclarationSyntax { AttributeLists.Count: > 0 };

        /// <summary>
        /// Transform the node into Validation Rule Info to be sent to for further processing later.
        /// The Validation Rule Info is for this node but can contain 1 or more rules.
        /// </summary>
        /// <param name="context">Generator Syntax Context to transform</param>
        /// <returns>ValidationRuleInfo if found</returns>
        static ValidationRuleInfo? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            if (!(context.Node is BaseTypeDeclarationSyntax { AttributeLists.Count: > 0 } baseTypeDeclarationSyntax))
            {
                return null;
            }
            string? dtoFullName = context.SemanticModel.GetDeclaredSymbol(baseTypeDeclarationSyntax)?.ToDisplayString();
            if (string.IsNullOrWhiteSpace(dtoFullName))
            {
                //The namespace should have been found. Ignore this one.
                return null;
            }

            foreach (AttributeListSyntax attributeListSyntax in baseTypeDeclarationSyntax.AttributeLists)
            {
                foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (context.SemanticModel.GetTypeInfo(attributeSyntax).Type is not ITypeSymbol typeSymbol)
                    {
                        //Symbol not found.  Ignore it.
                        continue;
                    }

                    string fullName = typeSymbol.ToDisplayString();

                    if (fullName == WellKnownAttributes.ExtendValidationAttribute)
                    {
                        if(attributeSyntax.ArgumentList?.Arguments.Count != 1)
                        {
                            return null;
                        }

                        SyntaxNode childNode = attributeSyntax.ArgumentList.Arguments[0].Expression.ChildNodes().FirstOrDefault();

                        ISymbol? childNodeSymbol = context.SemanticModel.GetSymbolInfo(childNode).Symbol;
                        if(childNodeSymbol == null)
                        {
                            return null;
                        }


                        string persistenceModelFullName = childNodeSymbol.ToDisplayString();
                        return GetValidationRuleInfos(context, dtoFullName!, persistenceModelFullName);
                    }
                }
            }

            // we didn't find the attribute we were looking for
            return null;

        }
        /// <summary>
        /// Recursive call to get all of the Validation Rule Infos for this Node.
        /// </summary>
        /// <param name="context">GeneratorSyntaxContext for this node</param>
        /// <param name="dtoFullName">Name of this data transfer object</param>
        /// <param name="modelFullName">Namespace of this data transfer object</param>
        /// <returns>ValidationRuleInfo if found</returns>
        internal static ValidationRuleInfo? GetValidationRuleInfos(GeneratorSyntaxContext context, string dtoFullName, string modelFullName)
        {
            INamedTypeSymbol? persistenceModel = context.SemanticModel.Compilation.GetTypeByMetadataName(modelFullName);
            if (persistenceModel == null)
            {
                return null;
            }

            bool hasMemberWithAttribute = false;
            foreach (ISymbol member in persistenceModel.GetMembers())
            {
                if (member is IPropertySymbol { DeclaredAccessibility: Accessibility.Public, IsAbstract: false } symbol)
                {
                    ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
                    if (attributes.Length > 0)
                    {
                        hasMemberWithAttribute = true;
                        break;
                    }
                }
            }
            ValidationRuleInfo? rtn = null;
            if (hasMemberWithAttribute)
            {
                rtn = new ValidationRuleInfo(dtoFullName!, modelFullName);
            }
            if(persistenceModel.BaseType != null)
            {
                ValidationRuleInfo? baseTypeValidationRuleInfo = GetValidationRuleInfos(context, dtoFullName, persistenceModel.BaseType.ToDisplayString());
                if(baseTypeValidationRuleInfo != null)
                {
                    if(rtn == null)
                    {
                        rtn = baseTypeValidationRuleInfo;
                    }
                    else
                    {
                        rtn.PersistenceObjects.AddRange(baseTypeValidationRuleInfo.PersistenceObjects);
                    }
                }
                return rtn;
            }
            return null;
        }

        /// <summary>
        /// Execute the transforms and create the Validation Rule files
        /// </summary>
        /// <param name="compilation">Context that we got the nodes from</param>
        /// <param name="validationRuleInfos">The transforms for the data transfer objects</param>
        /// <param name="context">Context for publishing the code</param>
        static void Execute(Compilation compilation, ImmutableArray<ValidationRuleInfo> validationRuleInfos, SourceProductionContext context)
        {
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
            if (validationRuleInfos.IsDefaultOrEmpty)
            {
                return;
            }
            List<ValidationModule> validationModules = new List<ValidationModule>();
            IEnumerable<ValidationRuleInfo> distinctValidationRules = validationRuleInfos.Distinct();

            foreach(ValidationRuleInfo validationRuleInfo in distinctValidationRules)
            {
                INamedTypeSymbol? dtoModel = compilation.GetTypeByMetadataName(validationRuleInfo.Dto);
                if (dtoModel == null)
                {
                    //We should still be able to get the model.  Maybe something changed.  Ignore this model
                    continue;
                }
                foreach(string persistenceObject in validationRuleInfo.PersistenceObjects)
                {
                    INamedTypeSymbol? persistenceModel = compilation.GetTypeByMetadataName(persistenceObject);
                    if (persistenceModel == null)
                    {
                        //We should still be able to get the model.  Maybe something changed.  Ignore this model
                        continue;
                    }

                    string dtoNamespace = dtoModel.ContainingNamespace.ToDisplayString();
                    string dtoName = dtoModel.Name;
                    ValidationModule validationModule = new ValidationModule(dtoName, dtoNamespace);

                    Dictionary<string, string> dtoProperties = GetProperties(compilation, dtoModel);

                    foreach (ISymbol member in persistenceModel.GetMembers())
                    {
                        if (member is IPropertySymbol { DeclaredAccessibility: Accessibility.Public, IsAbstract: false } symbol)
                        {
                            if (!dtoProperties.ContainsKey(member.Name))
                            {
                                //ignore this since the name doesn't match
                                continue;
                            }
                            if (!(
                                    dtoProperties[member.Name] == symbol.Type.ToDisplayString() ||
                                    (
                                        symbol.Type.ToDisplayString().EndsWith("?") &&
                                        dtoProperties[member.Name] == symbol.Type.ToDisplayString().Substring(0, symbol.Type.ToDisplayString().Length - 1)
                                    ) ||
                                    (
                                        dtoProperties[member.Name].EndsWith("?") &&
                                        symbol.Type.ToDisplayString() == dtoProperties[member.Name].Substring(0, dtoProperties[member.Name].Length - 1)
                                    )
                                ))
                            {
                                //skip this property as it doesn't match.
                                continue;
                            }

                            ValidationRule validationRule = new ValidationRule(member.Name);

                            ImmutableArray<AttributeData> attributes = member.GetAttributes();
                            if (attributes.Length > 0)
                            {

                                foreach (AttributeData attribute in attributes)
                                {
                                    string? attributeName = attribute.AttributeClass?.Name;
                                    string? attributeNamespace = attribute.AttributeClass?.ToDisplayString();
                                    if (attributeNamespace?.StartsWith("System.ComponentModel.DataAnnotations") != true)
                                    {
                                        continue;
                                    }

                                    AttributeSyntax? attributeSyntax = attribute.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;
                                    if (attributeSyntax == null)
                                    {
                                        context.ReportDiagnostic(Diagnostic.Create(
                                           new DiagnosticDescriptor(
                                               "VRG0001",
                                               $"Entity '{member.Name}' not found.",
                                               "If entity '{0}' is in a different project, you'll need to include the files via Compile instead of a project reference.",
                                               "Naming",
                                               DiagnosticSeverity.Error,
                                               true), member.Locations.FirstOrDefault(), member.Name));
                                    }
                                    SeparatedSyntaxList<AttributeArgumentSyntax> attributeArguments = attributeSyntax == null ? new SeparatedSyntaxList<AttributeArgumentSyntax>() : attributeSyntax.ArgumentList?.Arguments ?? new SeparatedSyntaxList<AttributeArgumentSyntax>();
                                    string? errorMessage = GetAttributeValue(attributeArguments, "ErrorMessage");

                                    switch (attributeName)
                                    {
                                        case ("RequiredAttribute"):
                                            {

                                                validationRule.Rules.Add($".NotEmpty().WithMessage({errorMessage ?? "\"{0} is required.\""})");
                                                break;
                                            }
                                        case ("MinLengthAttribute"):
                                            {
                                                string minLength = attributeArguments.First().ToFullString();
                                                if (string.IsNullOrWhiteSpace(errorMessage))
                                                {
                                                    errorMessage = $"\"{{0}} must be at least {minLength} characters.\"";
                                                }
                                                validationRule.Rules.Add($".MinimumLength({minLength}).WithMessage({errorMessage})");
                                                break;
                                            }
                                        case ("MaxLengthAttribute"):
                                            {
                                                string maxLength = attributeArguments.First().ToFullString();
                                                if (string.IsNullOrWhiteSpace(errorMessage))
                                                {
                                                    errorMessage = $"\"{{0}} must not exceed {maxLength} characters.\"";
                                                }
                                                validationRule.Rules.Add($".MaximumLength({maxLength}).WithMessage({errorMessage})");
                                                break;
                                            }
                                    }
                                }
                                if (validationRule.Rules.Count > 0)
                                {
                                    validationModule.Rules.Add(validationRule);
                                }
                            }
                        }
                    }
                    if (validationModule.Rules.Count > 0)
                    {
                        validationModules.Add(validationModule);
                    }
                }
            }
            SourceGenerationHelper.GenerateValidationRules(validationModules, context);
        }
        
        private static string? GetAttributeValue(SeparatedSyntaxList<AttributeArgumentSyntax> arguments, string argumentName)
        {
            foreach (AttributeArgumentSyntax argument in arguments)
            {
                if (argument.NameEquals?.Name.ToString() == argumentName)
                {
                    return argument.Expression.ToString();
                }
            }
            return null;
        }
        
        private static Dictionary<string, string> GetProperties(Compilation compilation, INamedTypeSymbol model)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (ISymbol member in model.GetMembers())
            {
                if (member is IPropertySymbol { DeclaredAccessibility: Accessibility.Public, IsAbstract: false } symbol)
                {
                    properties.Add(member.Name, symbol.Type.ToDisplayString());
                }
            }
            return properties;
        }
    }
}
