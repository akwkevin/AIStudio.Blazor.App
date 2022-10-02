using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.Common.Authorization;

public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorization_(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddTransient<IAuthorizationPolicyProvider, SimpleAuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, SimpleAuthorizationHandler>();
        services.AddTransient<IPermissionChecker, DefaultPermissionChecker>();

        return services;
    }
}
