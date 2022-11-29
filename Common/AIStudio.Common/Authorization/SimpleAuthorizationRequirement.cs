using Microsoft.AspNetCore.Authorization;

namespace AIStudio.Common.Authorization;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" />
public class SimpleAuthorizationRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleAuthorizationRequirement"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    public SimpleAuthorizationRequirement(string name)
    {
        Name = name;
    }
}
