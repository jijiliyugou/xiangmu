using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Yaeher.DoctorReport.Dto
{
    /// <summary>
    /// 医生收入明细
    /// </summary>
    public class IncomeDetailsIn : ListParameters<IncomeDetails>, IPagedResultRequest
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(20)]
        public string DoctorName { get; set; }
    }
   
}
