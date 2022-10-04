using System.Collections.Generic;

namespace AIStudio.Util.Common
{
    public class PageInput<T> : PageInput where T : new()
    {
        public T Search { get; set; } = new T();
       
    }


}
