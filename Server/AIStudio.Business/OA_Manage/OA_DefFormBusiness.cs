using AIStudio.Business;
using AIStudio.Business.OA_Manage;
using AIStudio.Common.DI;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Entity.OA_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.Helper;
using AutoMapper;
using LinqKit;
using SqlSugar;
using System.Linq.Dynamic.Core;

namespace AIStudio.Business.OA_Manage
{
    public class OA_DefFormBusiness : BaseBusiness<OA_DefForm>, IOA_DefFormBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public OA_DefFormBusiness(ISqlSugarClient db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<List<OA_DefFormTree>> GetTreeDataListAsync(string type, List<string> roleidlist)
        {
            if (roleidlist == null)
            {
                roleidlist = new List<string>();
            }

            var where = LinqHelper.True<OA_DefForm>();
            if (!type.IsNullOrEmpty())
                where = where.And(x => x.Type == type);

            where = where.And(x => x.Status == 1);

            var list = await GetIQueryable().Where(where).Select<OA_DefFormDTO>().ToListAsync();
            list = list.Where(p => string.IsNullOrEmpty(p.Value) || roleidlist.Intersect(p.ValueRoles).Count() > 0).ToList();

            List<OA_DefFormTree> treeList = new List<OA_DefFormTree>();
            foreach (var data in list.GroupBy(p => p.Type))
            {
                OA_DefFormTree node = new OA_DefFormTree()
                {
                    Id = data.Key,
                    Text = data.Key,
                    Value = data.Key,
                    scopedSlots = new { title = "title" },
                };

                node.Children = data.Select(x => new OA_DefFormTree
                {
                    Id = x.Id,
                    Text = x.Name,
                    Value = x.Id,
                    type = data.Key,
                    jsonId = x.JSONId,
                    jsonVersion = x.JSONVersion,
                    json = x.WorkflowJSON,
                    scopedSlots = new { title = "titleExtend" },
                }).ToList();

                treeList.Add(node);
            }

            return treeList;
        }



        public async Task<PageResult<OA_DefFormDTO>> GetDataListAsync(PageInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);     

            return await q.Select<OA_DefFormDTO>().GetPageResultAsync(input);
        }

        public async Task<OA_DefFormDTO> GetTheDataAsync(string id)
        {
            return _mapper.Map<OA_DefFormDTO>(await GetEntityAsync(id));
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }




}