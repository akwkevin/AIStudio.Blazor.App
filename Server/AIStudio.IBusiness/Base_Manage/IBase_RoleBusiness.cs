using AIStudio.Common.Mapper;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.IBusiness.IBaseBusiness&lt;AIStudio.Entity.Base_Manage.Base_Role&gt;" />
    public interface IBase_RoleBusiness : IBaseBusiness<Base_Role>
    {
        /// <summary>
        /// Gets the data list asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Task<PageResult<Base_RoleEditInputDTO>> GetDataListAsync(PageInput input);
        /// <summary>
        /// Gets the data asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        new Task<Base_RoleEditInputDTO> GetTheDataAsync(string id);
        /// <summary>
        /// Adds the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Task AddDataAsync(Base_RoleEditInputDTO input);
        /// <summary>
        /// Updates the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Task UpdateDataAsync(Base_RoleEditInputDTO input);
        /// <summary>
        /// Saves the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Task SaveDataAsync(Base_RoleEditInputDTO input);
    }

 
}