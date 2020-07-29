using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.SystemConfig;

namespace Yaeher
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserManagerService: IApplicationService
    {
        /// <summary>
        /// 用户基本信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        UserManager UserManager(int UserID);
        /// <summary>
        /// 医生基本信息
        /// </summary>
        /// <param name="DoctorID"></param>
        /// <returns></returns>
        YaeherDoctorInfo DoctorInformation(int DoctorID);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<YaeherDoctorInfo>> DoctorInfoList();
    }
}