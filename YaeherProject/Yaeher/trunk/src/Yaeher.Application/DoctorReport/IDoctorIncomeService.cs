using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.DoctorReport.Dto;

namespace Yaeher.DoctorReport
{
    /// <summary>
    /// 医生收入汇总
    /// </summary>
    public interface IDoctorIncomeService : IApplicationService
    {
        /// <summary>
        /// 新建医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        Task<DoctorIncome> CreateDoctorIncome(DoctorIncome DoctorIncomeInfo);
        /// <summary>
        /// 新建医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        Task ToTalDoctorIncome(DoctorIncome DoctorIncomeInfo);
        /// <summary>
        /// 删除医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        Task<DoctorIncome> DeleteDoctorIncome(DoctorIncome DoctorIncomeInfo);
        /// <summary>
        /// 查询医生收入汇总byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorIncome> DoctorIncomeByID(int Id);
        /// <summary>
        /// 查询医生收入汇总 List
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorIncome>> DoctorIncomeList(DoctorIncomeIn DoctorIncomeInfo);
        /// <summary>
        /// 查询医生收入汇总 page
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorIncome>> DoctorIncomePage(DoctorIncomeIn DoctorIncomeInfo);
        /// <summary>
        /// 修改医生收入汇总
        /// </summary>
        /// <param name="DoctorIncomeInfo"></param>
        /// <returns></returns>
        Task<DoctorIncome> UpdateDoctorIncome(DoctorIncome DoctorIncomeInfo);
    }
}