using System.Runtime.CompilerServices;
using VerifyXunit;

namespace EutonTechnologies.ValidationRules.Types.Analyzers.UnitTests
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
