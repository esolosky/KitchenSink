using Microsoft.OpenApi;
using RiderDevTest.Application;
using RiderDevTest.Application.Infrastructure.Persistence;

namespace RiderDevTest.Extensions;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Book API",
                Version = "v1",
                Description = """
                              ## Sample Data (Development)

                              The following sample data is seeded automatically in development mode.

                              ### Books
                              | ID | Title | Author |
                              |----|------|-------|
                              | `11111111-1111-1111-1111-111111111111` | Kafka On The Shore | Haruki Murakami |
                              | `22222222-2222-2222-2222-222222222222` | Grapes of Wrath | John Steinbeck |
                              | `33333333-3333-3333-3333-333333333333` | Ship of Magic | Robin Hobb |
                              """,
            });

            c.CustomSchemaIds(type => (type.FullName ?? type.Name ?? type.ToString()).Replace("+", "."));
        });
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


        builder.Services.AddProblemDetails();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddHealthChecks();
        builder.Services.AddHttpContextAccessor();
        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipeline(this WebApplication app)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        app.UseCors();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error-development");
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseAuthorization();
        app.MapControllers();

        // Map Minimal API endpoints
        app.MapApplicationEndpoints();

        // Seed the database in development only
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await ApplicationDbContextSeed.SeedSampleDataAsync(context);
        }
        return app;
    }
}