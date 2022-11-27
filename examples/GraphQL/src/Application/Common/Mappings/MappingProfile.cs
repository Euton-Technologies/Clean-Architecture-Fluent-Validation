using System.Reflection;
using AutoMapper;

namespace MoviesExample.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        if (Assembly.GetCallingAssembly().GetName().Name == "AutoMapper")
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }
        ShouldMapMethod = _ShouldMapMethod;
    }
    internal bool _ShouldMapMethod(MethodInfo method)
    {
        return !method.DeclaringType.FullName.StartsWith("System.");
    }
    public void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        
        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
        
        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
        
        var argumentTypes = new Type[] { typeof(Profile) };

        foreach (var type in types)
        {
            var isRecord = type.GetMethods().Any(m => m.Name == "<Clone>$");
            var instance = isRecord ? null : Activator.CreateInstance(type);
            
            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                if (interfaces.Count > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);
                        if (isRecord)
                        {
                            CreateMap(type, @interface.GenericTypeArguments.First());
                        }
                        else
                        {
                            interfaceMethodInfo?.Invoke(instance, new object[] { this });
                        }
                    }
                }
            }
        }
    }
}
