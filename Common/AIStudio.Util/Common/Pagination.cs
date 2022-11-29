namespace AIStudio.Util.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Pagination
    {
        #region 默认方案

        /// <summary>
        /// 当前页码
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex
        {
            get;
            set;
        }  = 1;


        /// <summary>
        /// 每页行数
        /// </summary>
        /// <value>
        /// The page rows.
        /// </value>
        public int PageRows
        {
            get;
            set;
        } = int.MaxValue;


        /// <summary>
        /// 排序列
        /// </summary>
        /// <value>
        /// The sort field.
        /// </value>
        public string SortField { get; set; } = "Id";

        /// <summary>
        /// Gets or sets the type of the sort.
        /// </summary>
        /// <value>
        /// The type of the sort.
        /// </value>
        private string _sortType { get; set; } = "desc";
        /// <summary>
        /// 排序类型
        /// </summary>
        /// <value>
        /// The type of the sort.
        /// </value>
        public string SortType { get => _sortType; set => _sortType = (value ?? string.Empty).Contains("desc") ? "desc" : "asc"; }

        /// <summary>
        /// 总记录数
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        /// <value>
        /// The page count.
        /// </value>
        public int PageCount
        {
            get
            {
                if (PageRows == 0)
                    return 1;
                int pages = Total / PageRows;
                pages = Total % PageRows == 0 ? pages : pages + 1;
                return pages;
            }
        }


        #endregion
    }
}
