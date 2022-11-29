using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchInput
    {
        /// <summary>
        /// 排序列
        /// </summary>
        /// <value>
        /// The sort field.
        /// </value>
        [DefaultValue("Id")]
        public string SortField { get; set; } = "Id";

        /// <summary>
        /// Gets or sets the type of the sort.
        /// </summary>
        /// <value>
        /// The type of the sort.
        /// </value>
        private string _sortType { get; set; } = "asc";
        /// <summary>
        /// 排序类型
        /// </summary>
        /// <value>
        /// The type of the sort.
        /// </value>
        [DefaultValue("desc")]
        public string SortType { get => _sortType; set => _sortType = (value ?? string.Empty).ToLower().Contains("desc") ? "desc" : "asc"; }
        /// <summary>
        /// Gets or sets the search key values.
        /// </summary>
        /// <value>
        /// The search key values.
        /// </value>
        public Dictionary<string, object> SearchKeyValues { get; set; } = new Dictionary<string, object>();
    }
}
