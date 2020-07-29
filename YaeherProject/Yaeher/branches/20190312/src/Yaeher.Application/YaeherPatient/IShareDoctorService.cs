using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 分享医生
    /// </summary>
    public interface IShareDoctorService : IApplicationService
    {
        /// <summary>
        /// 新建分享医生
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        Task<ShareDoctor> CreateShareDoctor(ShareDoctor ShareDoctorInfo);
        /// <summary>
        /// 删除分享医生
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        Task<ShareDoctor> DeleteShareDoctor(ShareDoctor ShareDoctorInfo);
        /// <summary>
        /// 查询分享医生byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ShareDoctor> ShareDoctorByID(int Id);
        /// <summary>
        /// 查询分享医生 List
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        Task<IList<ShareDoctor>> ShareDoctorList(ShareDoctorIn ShareDoctorInfo);
        /// <summary>
        /// 查询分享医生 page
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ShareDoctor>> ShareDoctorPage(ShareDoctorIn ShareDoctorInfo);
        /// <summary>
        /// 修改分享医生
        /// </summary>
        /// <param name="ShareDoctorInfo"></param>
        /// <returns></returns>
        Task<ShareDoctor> UpdateShareDoctor(ShareDoctor ShareDoctorInfo);
    }
}