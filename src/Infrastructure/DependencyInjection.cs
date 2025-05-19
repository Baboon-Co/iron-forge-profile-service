using Application.Features.Profiles.Abstractions;
using FluentValidation;
using Infrastructure.Databases;
using Infrastructure.Features.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Utility.Configuration.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddGrpc();
        services.AddDatabase();
        services.AddServices();
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddOptionsWithFluentValidation<DatabaseOptions>();
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var dbOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseNpgsql(dbOptions.ConnectionString);
        });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        return services;
    }
}