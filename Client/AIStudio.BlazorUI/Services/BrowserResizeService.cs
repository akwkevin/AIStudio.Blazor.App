using AIStudio.BlazorUI.Core;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorUI.Services
{
    public class BrowserResizeService
    {
        IJSRuntime JSRuntime;
        public BrowserResizeService(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
            JSRuntime.InvokeVoidAsync($"window.AddResize", "OnReSize", DotNetObjectReference.Create(this));
        }
        public event Func<Task> OnResize;

        [JSInvokable("OnReSize")]
        public void OnReSize()
        {
            Console.WriteLine("窗口触发ReSize事件");
        }


        public async Task<BrowserDimension> GetInnerDimension()
        {
            return await JSRuntime.InvokeAsync<BrowserDimension>("getDimensions");
        }
    }
}
