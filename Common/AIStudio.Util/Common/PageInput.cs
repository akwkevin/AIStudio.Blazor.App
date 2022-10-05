namespace AIStudio.Util.Common
{
    /// <summary>
    /// 分页查询基类
    /// </summary>
    public class PageInput: SearchInput
    {


        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageRows { get; set; } = int.MaxValue;

  

    }
}
