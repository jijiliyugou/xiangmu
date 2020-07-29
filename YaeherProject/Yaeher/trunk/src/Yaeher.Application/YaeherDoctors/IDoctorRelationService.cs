using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.LableManages.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生与标签关系
    /// </summary>
    public interface IDoctorRelationService : IApplicationService
    {
        /// <summary>
        /// 新建医生与标签关系
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        Task<DoctorRelation> CreateDoctorRelation(DoctorRelation DoctorRelationInfo);
        /// <summary>
        /// 删除医生与标签关系
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        Task<DoctorRelation> DeleteDoctorRelation(DoctorRelation DoctorRelationInfo);
        /// <summary>
        /// 查询医生与标签关系byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorRelation> DoctorRelationByID(int Id);
       
        /// <summary>
        /// 查询医生与标签关系 List
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        Task<List<DoctorRelation>> DoctorRelationList(DoctorRelationIn DoctorRelationInfo);
        /// <summary>
        /// 查询医生与标签关系 List
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorRelation>> DoctorClinicRelationList(DoctorRelationIn DoctorRelationInfo);
        /// <summary>
        /// 查询医生与标签关系 page
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorRelation>> DoctorRelationPage(DoctorRelationIn DoctorRelationInfo);
        /// <summary>
        /// 查询医生与标签关系 page
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorRelation>> LabelDoctorRelationPage(DoctorRelationIn DoctorRelationInfo);

        /// <summary>
        /// 查询医生与标签关系 page
        /// </summary>
        /// <param name="LableManageIn"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorLale>> LabelDoctorRelationPage(LableManageIn LableManageIn);

        /// <summary>
        /// 修改医生与标签关系
        /// </summary>
        /// <param name="DoctorRelationInfo"></param>
        /// <returns></returns>
        Task<DoctorRelation> UpdateDoctorRelation(DoctorRelation DoctorRelationInfo);
    }
}