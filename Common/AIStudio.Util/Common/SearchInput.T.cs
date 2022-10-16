using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.Common
{
    public class SearchInput<T> : SearchInput where T : new()
    {
        public T Search { get; set; } = new T();

    }
}
