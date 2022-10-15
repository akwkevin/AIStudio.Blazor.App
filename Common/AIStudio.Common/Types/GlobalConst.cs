using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Types
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public class GlobalConst
    {
        #region 数据库相关
        /// <summary>
        /// 自然主键
        /// </summary>
        public const string Id = "Id";

        /// <summary>
        /// 否已删除
        /// </summary>
        public const string Deleted = "Deleted";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string CreateTime = "CreateTime";

        /// <summary>
        /// 创建人Id
        /// </summary>
        [MaxLength(50)]
        public const string CreatorId = "CreatorId";

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(255)]
        public const string CreatorName = "CreatorName";

        /// <summary>
        /// 修改时间
        /// </summary>
        public const string ModifyTime = "ModifyTime";

        /// <summary>
        /// 修改人Id
        /// </summary>
        [MaxLength(50)]
        public const string ModifyId = "ModifyId";

        /// <summary>
        /// 修改人
        /// </summary>
        [MaxLength(255)]
        public const string ModifyName = "ModifyName";

        /// <summary>
        /// 租户Id
        /// </summary>
        [MaxLength(50)]
        public const string TenantId = "TenantId";
        #endregion
    }
}
