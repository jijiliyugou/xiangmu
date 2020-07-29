using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.ClinicManage.Dto;
using Yaeher.DoctorQuality.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 门诊信息
    /// </summary>
    public interface IClinicInfomationService : IApplicationService
    {
        /// <summary>
        /// 查询门诊信息byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ClinicInfomation> ClinicInfomationByID(int Id);
        /// <summary>
        /// 查询门诊信息 List
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        Task<IList<ClinicInfomation>> ClinicInfomationList(ClinicInfomationIn ClinicInfomationInfo);
        /// <summary>
        /// 查询门诊信息 List
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <param name="IDArr"></param>
        /// <returns></returns>
        Task<IList<ClinicInfomation>> ClinicInfomationListByArrId(ClinicInfomationIn ClinicInfomationInfo,List<int>IDArr);
        /// <summary>
        /// 查询门诊信息 List
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        Task<List<ClinicInfomation>> ClinicInfomationList(ClinicInfomationIn ClinicInfomationInfo, List<ClinicDoctorReltion> rel);
        
        /// <summary>
        /// 根据门诊Id获取医生信息
        /// </summary>
        /// <param name="clinic"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicDoctorsView>> DoctorInformation(ClinicInfomationIn clinic);
        /// <summary>
        /// 医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicDoctorsView>> ClinicDoctorInformation(YaeherDoctorSearch clinic,IList<DoctorRelation> rel);
        /// <summary>
        /// 医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicDoctorsView>> DoctorInformation(YaeherDoctorSearch clinic, IList<DoctorRelation> rel);
        /// <summary>
        /// 医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicDoctorsView>> QualityDoctorInfor(YaeherDoctorSearch clinic, IList<DoctorRelation> rel);
        /// <summary>
        /// 质控查看新医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        ///   <param name="doctorNews"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicDoctorsView>> QualityDoctorInformation(YaeherDoctorSearch clinic, IList<DoctorRelation> rel,IList<DoctorNew> doctorNews);
        
        /// <summary>
        /// 根据门诊Id获取医生信息
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns></returns>
        Task<List<ClinicDoctorsView>> PatientCollectDoctorInformation(YaeherPatientDoctorSearch doctor);
        /// <summary>
        /// 根据门诊Id获取医生信息
        /// </summary>
        /// <param name="clinic"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicDoctorsView>> DoctorInformation(YaeherDoctorSearch clinic);
        /// <summary>
        /// 查询门诊信息 page
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicInfomation>> ClinicInfomationPage(ClinicInfomationIn ClinicInfomationInfo);
        /// <summary>
        /// 新建门诊信息
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        Task<ClinicInfomation> CreateClinicInfomation(ClinicInfomation ClinicInfomationInfo);
        /// <summary>
        /// 删除门诊信息
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        Task<ClinicInfomation> DeleteClinicInfomation(ClinicInfomation ClinicInfomationInfo);
        /// <summary>
        /// 修改门诊信息
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        Task<ClinicInfomation> UpdateClinicInfomation(ClinicInfomation ClinicInfomationInfo);
    }
}