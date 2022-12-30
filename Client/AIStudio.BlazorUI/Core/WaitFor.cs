using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorUI.Core
{
    public class WaitFor : IDisposable
    {
        private int Counter;
        private ILoading LoadingComponent;

        public static ConcurrentDictionary<ILoading, WaitFor> WaitForList = new ConcurrentDictionary<ILoading, WaitFor>();

        public static WaitFor GetWaitFor(ILoading loading)
        {
            WaitFor waitFor;
            if (!WaitForList.TryGetValue(loading, out waitFor))
            {
                waitFor = new WaitFor(loading);
                WaitForList.TryAdd(loading, waitFor);
            }
            Interlocked.Increment(ref waitFor.Counter);
            return waitFor;
        }

        public WaitFor(ILoading loading)
        {
            LoadingComponent = loading;
            LoadingComponent.Loading = true;
        }

        public void Dispose()
        {
            if (Interlocked.Decrement(ref Counter) == 0)
            {
                LoadingComponent.Loading = false;
                WaitForList.TryRemove(LoadingComponent, out var waitFor);
            }
        }
    }
}
