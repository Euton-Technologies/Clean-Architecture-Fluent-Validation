using CleanArchitecture.ValidatonRules.Types.Analyzers.Models;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.Text;
using static CleanArchitecture.ValidatonRules.Types.Analyzers.StringConstants;

namespace CleanArchitecture.ValidatonRules.Types.Analyzers.Helpers
{
    /// <summary>
    /// Generates the actual code from the validation modules/rules
    /// </summary>
    internal static class SourceGenerationHelper
    {
        internal static void GenerateValidationRules(List<ValidationModule> validationModules, SourceProductionContext context)
        {
            foreach (ValidationModule validationModule in validationModules)
            {
                StringBuilder code = new StringBuilder();
                code.AppendLine("using FluentValidation;");

                code.AppendLine();
                code.AppendLine($"namespace {validationModule.Namespace}");
                code.AppendLine("{");

                code.Append(Indent)
                    .Append("public static class ")
                    .Append(validationModule.Name)
                    .AppendLine("Validators");

                code.Append(Indent)
                    .AppendLine("{");

                foreach (ValidationRule rule in validationModule.Rules)
                {

                    code.Append(Indent)
                        .Append(Indent)
                        .AppendLine($"public static IRuleBuilderOptions<{validationModule.Name}, string> {rule.Name}<{validationModule.Name}>(this IRuleBuilder<{validationModule.Name}, string?> ruleBuilder)");

                    code.Append(Indent)
                        .Append(Indent)
                        .AppendLine("{");

                    code.Append(Indent)
                        .Append(Indent)
                        .Append(Indent)
                        .AppendLine("return ruleBuilder");

                    for (int idx = 0; idx < rule.Rules.Count; idx++)
                    {
                        code.Append(Indent)
                            .Append(Indent)
                            .Append(Indent)
                            .Append(Indent)
                            .AppendLine(rule.Rules[idx] + (idx == rule.Rules.Count - 1 ? ";" : string.Empty));
                    }

                    code.Append(Indent)
                        .Append(Indent)
                        .AppendLine("}");
                }
                code.Append(Indent)
                    .AppendLine("}");

                code.AppendLine("}");
                context.AddSource(validationModule.Name + ".g.cs", SourceText.From(code.ToString(), Encoding.UTF8));
            }
        }
    }
}
