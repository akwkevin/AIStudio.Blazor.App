using Microsoft.AspNetCore.Authorization;

namespace AIStudio.Common.Authorization;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Authorization.AuthorizationHandler&lt;AIStudio.Common.Authorization.SimpleAuthorizationRequirement&gt;" />
public class SimpleAuthorizationHandler : AuthorizationHandler<SimpleAuthorizationRequirement>
{
    /// <summary>
    /// The checker
    /// </summary>
    private readonly IPermissionChecker _checker;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleAuthorizationHandler"/> class.
    /// </summary>
    /// <param name="checker">The checker.</param>
    public SimpleAuthorizationHandler(IPermissionChecker checker)
    {
        _checker = checker;
    }

    /// <summary>
    /// Makes a decision if authorization is allowed based on a specific requirement.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement to evaluate.</param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SimpleAuthorizationRequirement requirement)
    {
        if (await _checker.IsGrantedAsync(context.User, requirement.Name))
        {
            context.Succeed(requirement);
        }
    }
}
