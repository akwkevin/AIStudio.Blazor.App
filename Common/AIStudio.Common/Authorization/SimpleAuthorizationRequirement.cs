using Microsoft.AspNetCore.Authorization;

namespace AIStudio.Common.Authorization;

public class SimpleAuthorizationRequirement : IAuthorizationRequirement
{
    public string Name { get; set; }

    public SimpleAuthorizationRequirement(string name)
    {
        Name = name;
    }
}
