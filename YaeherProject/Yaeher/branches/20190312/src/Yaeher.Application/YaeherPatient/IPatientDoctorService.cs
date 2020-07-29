using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 我的医生
    /// </summary>
    public interface IPatientDoctorService : IApplicationService
    {
        /// <summary>
        /// 新建我的医生
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        Task<YaeherPatientDoctor> CreateYaeherPatientDoctor(YaeherPatientDoctor YaeherPatientDoctorInfo);
        /// <summary>
        /// 删除我的医生
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        Task<YaeherPatientDoctor> DeleteYaeherPatientDoctor(YaeherPatientDoctor YaeherPatientDoctorInfo);
        /// <summary>
        /// 修改我的医生
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        Task<YaeherPatientDoctor> UpdateYaeherPatientDoctor(YaeherPatientDoctor YaeherPatientDoctorInfo);
        /// <summary>
        /// 我的医生byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherPatientDoctor> YaeherPatientDoctorByID(int Id);
        /// <summary>
        /// 我的医生byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<YaeherPatientDoctor> YaeherPatientDoctorByExpression(Expression<Func<YaeherPatientDoctor, bool>> whereExpression);
        /// <summary>
        /// 我的医生 List
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherPatientDoctor>> YaeherPatientDoctorList(YaeherPatientDoctorIn YaeherPatientDoctorInfo);
        /// <summary>
        /// 我的医生 page
        /// </summary>
        /// <param name="YaeherPatientDoctorInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherPatientDoctor>> YaeherPatientDoctorPage(YaeherPatientDoctorIn YaeherPatientDoctorInfo);
    }
}