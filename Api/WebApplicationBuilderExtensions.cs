using CWM.CleanArchitecture.Application;
using CWM.CleanArchitecture.Application.Interfaces;
using CWM.CleanArchitecture.Infrastructure;
using CWM.CleanArchitecture.Infrastructure.Implementation;
using CWM.CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;
using Wolverine.FluentValidation.Internals;
using Wolverine.SqlServer;

namespace CWM.CleanArchitecture.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {
        builder.AddSerilog();
        builder.AddDatabase();
        builder.AddApplicationServices();
        builder.AddApiInfrastructure();
        builder.AddOpenApiDocs();
        builder.AddHealthChecks();
        builder.AddWolverine();

        return builder;
    }

    private static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)); 
    }
    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    private static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
    }

    private static void AddApiInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
    }

    private static void AddOpenApiDocs(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new Microsoft.OpenApi.OpenApiInfo
                {
                    Title = "Clean Architecture API",
                    Description = "A production-ready Clean Architecture template for .NET 10"
                };

                var components = document.Components ?? new Microsoft.OpenApi.OpenApiComponents();
                components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>();
                components.SecuritySchemes["Bearer"] = new Microsoft.OpenApi.OpenApiSecurityScheme
                {
                    Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token"
                };

                document.Components = components;

                var schemeReference = new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer");
                document.Security ??= [];
                document.Security.Add(new Microsoft.OpenApi.OpenApiSecurityRequirement
                {
                    [schemeReference] = new List<string>()
                });

                return Task.CompletedTask;
            });
        });
    }

    private static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
    }

    private static void AddWolverine(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Host.UseWolverine(opts =>
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                opts.PersistMessagesWithSqlServer(connectionString);
                opts.Policies.UseDurableLocalQueues();
            }
            opts.Policies.AutoApplyTransactions();
            opts.UseEntityFrameworkCoreTransactions();

            opts.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
            opts.Discovery.IncludeAssembly(typeof(Application.DependencyInjection).Assembly);
        });
    }
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Clean Architecture API");
                options.WithTheme(ScalarTheme.BluePlanet);
                options.WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl);
            });
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSerilogRequestLogging();
        app.MapHealthChecks("/health");
        app.MapControllers(); 


        return app;
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            await AppDbSeeder.SeedAsync(app.Services);
        }
    }
}


