using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IHomeBusiness
    {
        Task<string> SubmitLoginAsync(LoginInputDTO input);
        Task<string> RefreshTokenAsync(RefreshTokenInputDTO input);
        Task SubmitLogoutAsync();
        Task ChangePwdAsync(ChangePwdInputDTO input);
        Task<Base_UserDTO> GetOperatorInfoAsync();
        Task<List<Base_ActionTree>> GetOperatorMenuListAsync();
    }




}
