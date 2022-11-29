using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.Common.Authorization;

/// <summary>
/// 
/// </summary>
public static class AuthorizationServiceCollectionExtensions
{
    /// <summary>
    /// Adds the authorization.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization_(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddTransient<IAuthorizationPolicyProvider, SimpleAuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, SimpleAuthorizationHandler>();
        services.AddTransient<IPermissionChecker, DefaultPermissionChecker>();

        return services;
    }
}
