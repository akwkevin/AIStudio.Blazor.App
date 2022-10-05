using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_UserLogBusiness :BaseBusiness<Base_UserLog>, IBase_UserLogBusiness, ITransientDependency
    {

        public Base_UserLogBusiness(ISqlSugarClient db) : base(db)
        {
            
        }

        public async Task<PageResult<Base_UserLog>> GetLogListAsync(PageInput<Base_UserLogsInputDTO> input)
        {
            var search = input.Search;
            RefAsync<int> total = 0;
            var P = await Db.Queryable<Base_UserLog>()
                .WhereIF(!search.logContent.IsNullOrEmpty(), x => x.LogContent.Contains(search.logContent))
                .WhereIF(!search.logType.IsNullOrEmpty(), x => x.LogType == search.logType)
                .WhereIF(!search.opUserName.IsNullOrEmpty(), x => x.CreatorName.Contains(search.opUserName))
                .WhereIF(!search.startTime.IsNullOrEmpty(), x => x.CreateTime >= search.startTime)
                .WhereIF(!search.endTime.IsNullOrEmpty(), x => x.CreateTime <= search.endTime)
                 .ToPageListAsync(input.PageIndex, input.PageRows, total);
            return new PageResult<Base_UserLog> { Data = P, Total = total }; ;
        }
        public async Task<PageResult<Base_UserLog>> GetLogList(PageInput input)
        {
            RefAsync<int> total = 0;
            var P = await Db.Queryable<Base_UserLog>()
                 .ToPageListAsync(input.PageIndex, input.PageRows, total);
            return new PageResult<Base_UserLog> { Data = P, Total = total }; ;
        }
    }
}