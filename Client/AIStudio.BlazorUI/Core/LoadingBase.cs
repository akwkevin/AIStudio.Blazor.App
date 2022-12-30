using Microsoft.AspNetCore.Components;

namespace AIStudio.BlazorUI.Core
{
    public class LoadingBase : ComponentBase, IDisposable, ILoading
    {  
        public bool Loading { get; set; }

        public virtual void Dispose()
        {
          
        }
    }

    public interface ILoading
    {
        bool Loading { get; set; }
    }
}
