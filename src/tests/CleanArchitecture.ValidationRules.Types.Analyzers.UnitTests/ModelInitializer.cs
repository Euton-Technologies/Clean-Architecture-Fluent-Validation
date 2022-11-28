using System.Runtime.CompilerServices;
using VerifyTests;

namespace CleanArchitecture.ValidationRules.Types.Analyzers.UnitTests
{
    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Init()
        {
            VerifySourceGenerators.Enable();
        }
    }
}
