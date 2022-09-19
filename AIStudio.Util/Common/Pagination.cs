namespace AIStudio.Util.Common
{
    public class Pagination
    {
        #region 默认方案

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }  = 1;


        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageRows
        {
            get;
            set;
        } = int.MaxValue;


        /// <summary>
        /// 排序列
        /// </summary>
        public string SortField { get; set; } = "Id";

        private string _sortType { get; set; } = "desc";
        /// <summary>
        /// 排序类型
        /// </summary>
        public string SortType { get => _sortType; set => _sortType = (value ?? string.Empty).Contains("desc") ? "desc" : "asc"; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
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
