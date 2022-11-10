using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.Common
{
    public class SearchInput
    {
        /// <summary>
        /// 排序列
        /// </summary>
        [DefaultValue("Id")]
        public string SortField { get; set; } = "Id";

        private string _sortType { get; set; } = "asc";
        /// <summary>
        /// 排序类型
        /// </summary>
        [DefaultValue("desc")]
        public string SortType { get => _sortType; set => _sortType = (value ?? string.Empty).ToLower().Contains("desc") ? "desc" : "asc"; }
        public Dictionary<string, object> SearchKeyValues { get; set; } = new Dictionary<string, object>();
    }
}
