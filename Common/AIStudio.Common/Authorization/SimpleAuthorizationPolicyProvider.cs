using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AIStudio.Common.Authorization;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider" />
/// <seealso cref="Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider" />
public class SimpleAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider, IAuthorizationPolicyProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleAuthorizationPolicyProvider"/> class.
    /// </summary>
    /// <param name="options">The options used to configure this instance.</param>
    public SimpleAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    { }

    /// <summary>
    /// Gets the default authorization policy.
    /// </summary>
    /// <returns>
    /// The default authorization policy.
    /// </returns>
    public new Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => base.GetDefaultPolicyAsync();

    /// <summary>
    /// Gets the fallback authorization policy.
    /// </summary>
    /// <returns>
    /// The fallback authorization policy.
    /// </returns>
    public new Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => base.GetFallbackPolicyAsync();

    /// <summary>
    /// Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given <paramref name="policyName" />
    /// </summary>
    /// <param name="policyName">The policy name to retrieve.</param>
    /// <returns>
    /// The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.
    /// </returns>
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }

        if (!string.IsNullOrEmpty(policyName))
        {
            var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
            policyBuilder.AddRequirements(new SimpleAuthorizationRequirement(policyName));
            return policyBuilder.Build();
        }

        return null;
    }
}
