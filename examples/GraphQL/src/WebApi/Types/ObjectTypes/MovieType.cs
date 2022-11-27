using MoviesExample.Domain.Entities;
using WebApi.DataLoaders;

namespace WebApi.Types.ObjectTypes;

public class MovieType : ObjectType<Movie>
{
    protected override void Configure(IObjectTypeDescriptor<Movie> descriptor)
    {
        descriptor
            .Field(f => f.Genre)
            .Resolve(async (context, ct) =>
            {                
                var dataLoader = context.DataLoader<GenreByIdDataLoader>();

                return await dataLoader.LoadAsync(context.Parent<Movie>().GenreId, ct);
            });
    }
}