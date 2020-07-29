using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.ClinicManage.Dto;

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClinicManageService: IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        Task<List<ClinicInfo>> ClinicInfoList(ClinicInfomationIn ClinicInfomationInfo);
    }
}