using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.ClinicManage.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生基本信息
    /// </summary>
    public interface IYaeherDoctorService : IApplicationService
    {
        /// <summary>
        /// 新建医生基本信息
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<YaeherDoctor> CreateYaeherDoctor(YaeherDoctor YaeherDoctorInfo);
        /// <summary>
        /// 删除医生基本信息
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<YaeherDoctor> DeleteYaeherDoctor(YaeherDoctor YaeherDoctorInfo);
        /// <summary>
        /// 修改医生基本信息
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<YaeherDoctor>  UpdateYaeherDoctor(YaeherDoctor YaeherDoctorInfo);
        /// <summary>
        /// 查询医生基本信息byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherDoctor> YaeherDoctorByID(int Id);
        /// <summary>
        /// 查询医生基本信息byUserId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<YaeherDoctor> YaeherDoctorByUserID(int UserId);
        /// <summary>
        /// 查询医生基本信息 List
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<List<YaeherDoctor>> YaeherDoctorList(YaeherDoctorIn YaeherDoctorInfo);
        /// <summary>
        /// 查询医生基本信息 List
        /// </summary>
        /// <returns></returns>
        Task<List<YaeherClinicDoctor>> YaeherClinicDoctorList();
        /// <summary>
        /// 查询老医生基本信息 List
        /// </summary>
        /// <returns></returns>
        Task<List<YaeherClinicDoctor>> YaeherClinicOldDoctorList(IList<DoctorNew> doctorold);
        /// <summary>
        /// 查询医生基本信息 List
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherClinicDoctor>> YaeherClinicDoctorPage(ClinicInfomationIn input);
        /// <summary>
        /// 查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherDoctor>> YaeherDoctorPage(YaeherDoctorIn YaeherDoctorInfo);
        /// <summary>
        /// 查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherDoctorUser>> YaeherDoctorUserPage(YaeherDoctorIn YaeherDoctorInfo);
        /// <summary>
        /// 查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<YaeherDoctorUser> YaeherDoctorUser(YaeherDoctorIn YaeherDoctorInfo);
        /// <summary>
        /// 质控查询医生基本信息 page
        /// </summary>
        /// <param name="YaeherDoctorInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherDoctor>> QualityYaeherDoctorPage(YaeherDoctorIn YaeherDoctorInfo);
        /// <summary>
        /// 更新医生接单状态
        /// </summary>
        /// <param name="doctorInfo"></param>
        /// <returns></returns>
        Task<bool> UpdateDoctorService(DoctorInfo doctorInfo);
    }
}