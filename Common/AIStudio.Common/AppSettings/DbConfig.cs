using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.AppSettings
{
    /// <summary>
    /// 数据库参数
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public string DbNumber { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbString { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DbName { get; set; }
    }
}
