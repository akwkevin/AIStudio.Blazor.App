using AIStudio.Common.DI;
using AIStudio.Entity.OA_Manage;
using AIStudio.Util.Common;
using AutoMapper;
using SqlSugar;

namespace AIStudio.Business.OA_Manage
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Business.BaseBusiness&lt;AIStudio.Entity.OA_Manage.OA_UserFormStep&gt;" />
    /// <seealso cref="AIStudio.Business.OA_Manage.IOA_UserFormStepBusiness" />
    /// <seealso cref="AIStudio.Common.DI.ITransientDependency" />
    public class OA_UserFormStepBusiness : BaseBusiness<OA_UserFormStep>, IOA_UserFormStepBusiness, ITransientDependency
    {
        /// <summary>
        /// The mapper
        /// </summary>
        readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="OA_UserFormStepBusiness"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="mapper">The mapper.</param>
        public OA_UserFormStepBusiness(ISqlSugarClient db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public async Task<PageResult<OA_UserFormStep>> GetDataListAsync(PageInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);

            return await q.GetPageResultAsync(input);
        }
        #endregion

        #region 私有成员

        #endregion
    }


}