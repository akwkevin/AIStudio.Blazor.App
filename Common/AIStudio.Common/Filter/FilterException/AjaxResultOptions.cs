using Microsoft.AspNetCore.Mvc;
using Simple.Common;

namespace AIStudio.Common.Filter.FilterException;

/// <summary>
/// 
/// </summary>
public class AjaxResultOptions
{
    /// <summary>
    /// The result factory
    /// </summary>
    private Func<AjaxResultException, IActionResult> _resultFactory = default!;

    /// <summary>
    /// Gets or sets the result factory.
    /// </summary>
    /// <value>
    /// The result factory.
    /// </value>
    /// <exception cref="System.ArgumentNullException">value</exception>
    public Func<AjaxResultException, IActionResult> ResultFactory
    {
        get => _resultFactory;
        set => _resultFactory = value ?? throw new ArgumentNullException(nameof(value));
    }
}
