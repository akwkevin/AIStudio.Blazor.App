using System.Collections.Generic;

namespace AIStudio.Util.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="AIStudio.Util.Common.PageInput" />
    public class PageInput<T> : PageInput where T : new()
    {
        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        public T Search { get; set; } = new T();
       
    }


}
