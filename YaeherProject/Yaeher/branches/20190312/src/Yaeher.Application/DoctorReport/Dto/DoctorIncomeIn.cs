using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Yaeher.DoctorReport.Dto
{
    /// <summary>
    /// 医生收入汇总
    /// </summary>
    public class DoctorIncomeIn : ListParameters<DoctorIncome>, IPagedResultRequest
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(20)]
        public string DoctorName { get; set; }
    }
  
}
