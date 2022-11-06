using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Entity.OA_Manage;
using AIStudio.IBusiness;
using AIStudio.Util.Common;

namespace AIStudio.Business.OA_Manage
{
    public interface IOA_UserFormBusiness : IBaseBusiness<OA_UserForm>
    {
        Task QueueWork(string id);
        Task<string> DequeueWork(string id);
        Task<PageResult<OA_UserFormDTO>> GetDataListAsync(PageInput<OA_UserFormInputDTO> input);
        Task<OA_UserFormDTO> GetTheDataAsync(string id);
        int GetDataListCount(List<string> jsonids, OAStatus status);


        //#region 历史数据查询
        //Task<int> GetHistoryDataCountAsync(Input<OA_UserFormInputDTO> input);
        //Task<List<OA_UserFormDTO>> GetHistoryDataListAsync(Input<OA_UserFormInputDTO> input);
        //Task<PageResult<OA_UserFormDTO>> GetPageHistoryDataListAsync(PageInput<OA_UserFormInputDTO> input);
        //#endregion

    }


}