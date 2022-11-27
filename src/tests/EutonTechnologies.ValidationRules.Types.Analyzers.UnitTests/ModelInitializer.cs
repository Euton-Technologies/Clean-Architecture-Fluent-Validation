using System.Runtime.CompilerServices;

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
