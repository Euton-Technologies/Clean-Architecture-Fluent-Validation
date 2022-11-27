using System.Reflection;

namespace WebApi.Mappings;


public class MappingProfile : MoviesExample.Application.Common.Mappings.MappingProfile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }
}