using EutonTechnologies.ValidatonRules.Types;
using EutonTechnologies.ValidatonRules.Types.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace EutonTechnologies.ValidationRules.Types.Analyzers.UnitTests.Helpers
{
    public static class TestHelper
    {
        public static Task Verify(string source)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
            Assembly ns = Assembly.Load("netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51");
            IEnumerable<PortableExecutableReference> references = new[]
            {
                MetadataReference.CreateFromFile(typeof(ExtendValidationAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(ns.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree },
                references: references);

            ValidationRulesGenerator generator = new ValidationRulesGenerator();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGenerators(compilation);

            return Verifier
                .Verify(driver)
                .UseDirectory("../Snapshots");
        }
    }
}