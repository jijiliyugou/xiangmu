using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.LableManages.Dto;

namespace Yaeher.LableManages
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public interface ILableManageService : IApplicationService
    {
        /// <summary>
        /// 新建标签管理
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        Task<LableManage> CreateLableManage(LableManage LableManageInfo);
        /// <summary>
        /// 查询医生标签管理 List
        /// </summary>
        /// <returns></returns>
        Task<IList<LabelDoctorManage>> DoctorLableManageList();
        /// <summary>
        /// 删除标签管理
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        Task<LableManage> DeleteLableManage(LableManage LableManageInfo);
        /// <summary>
        /// 查询标签管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<LableManage> LableManageByID(int Id);
        /// <summary>
        /// 查询标签管理byName
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        Task<LableManage> LableManageByName(LableManageIn LableManageInfo);
        /// <summary>
        /// 查询标签管理 List
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        Task<IList<LableManage>> LableManageList(LableManageIn LableManageInfo);
        /// <summary>
        /// 查询科室标签管理 List
        /// </summary>
        /// <returns></returns>
        Task<IList<LabelClinicManage>> ClinicLableManageList();
        /// <summary>
        /// 查询科室标签管理 List
        /// </summary>
        /// <returns></returns>
        Task<IList<LabelClinicManage>> LableClinicManageInList(LabelClinicManageIn input);
        /// <summary>
        /// 查询医生标签管理 List
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        Task<IList<LabelDoctorManage>> LableDoctorManageInList(LabelDoctorManageIn LableManageInfo);
        /// <summary>
        /// 查询标签管理 page
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<LableManage>> LableManagePage(LableManageIn LableManageInfo);
        /// <summary>
        /// 修改标签管理
        /// </summary>
        /// <param name="LableManageInfo"></param>
        /// <returns></returns>
        Task<LableManage> UpdateLableManage(LableManage LableManageInfo);
    }
}