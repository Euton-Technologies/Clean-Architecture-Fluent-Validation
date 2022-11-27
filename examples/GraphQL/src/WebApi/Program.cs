using HotChocolate.Execution.Options;
using MoviesExample.Infrastructure.Persistence;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddWebApiServices(builder.Configuration)
        .AddCors(o =>
            o.AddDefaultPolicy(b =>
                b.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()))

        .AddInMemorySubscriptions()
        .AddGraphQLServer()
        .AddApolloTracing(TracingPreference.Always)
        .AddProjections()
        .AddQueryType()
        .AddMutationType()
        .AddWebApiTypes()
        .AddFiltering()
        .AddSorting()
        .AddGlobalObjectIdentification()
        .AddFileSystemQueryStorage("./persisted_queries")
        .UsePersistedQueryPipeline();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

app.UseCors();

app.UseWebSockets();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // We will be using the new routing API to host our GraphQL middleware.
    endpoints.MapGraphQL();

    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/graphql", true);
        return Task.CompletedTask;
    });
});
app.Run();