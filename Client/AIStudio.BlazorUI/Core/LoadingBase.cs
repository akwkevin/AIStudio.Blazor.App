using Microsoft.AspNetCore.Components;

namespace AIStudio.BlazorUI.Core
{
    public class LoadingBase : ComponentBase, IDisposable
    {  
        protected bool Loading { get; set; }
        protected void ShowWait()
        {
            Loading = true;
        }

        protected void HideWait()
        {
            Loading = false;
        }
        public virtual void Dispose()
        {
          
        }
    }
}
