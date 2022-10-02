using Microsoft.AspNetCore.Mvc;
using Simple.Common;

namespace AIStudio.Common.Result;

public class AjaxResultOptions
{
    private Func<AjaxResultException, IActionResult> _resultFactory = default!;

    public Func<AjaxResultException, IActionResult> ResultFactory
    {
        get => _resultFactory;
        set => _resultFactory = value ?? throw new ArgumentNullException(nameof(value));
    }
}
