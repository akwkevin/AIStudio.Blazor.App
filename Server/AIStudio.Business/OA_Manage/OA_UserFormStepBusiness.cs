using AIStudio.Common.DI;
using AIStudio.Entity.OA_Manage;
using AIStudio.Util.Common;
using AutoMapper;
using SqlSugar;

namespace AIStudio.Business.OA_Manage
{
    public class OA_UserFormStepBusiness : BaseBusiness<OA_UserFormStep>, IOA_UserFormStepBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public OA_UserFormStepBusiness(ISqlSugarClient db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

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