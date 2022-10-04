using AIStudio.Util.Common;
using System.Threading.Tasks;

namespace AIStudio.Client.Business
{
    public interface IAuthService
    {
        Task<AjaxResult> Login(string userName, string password);
        Task<AjaxResult> Logout();
        Task<IOperator> CurrentUserInfo();
    }
}
