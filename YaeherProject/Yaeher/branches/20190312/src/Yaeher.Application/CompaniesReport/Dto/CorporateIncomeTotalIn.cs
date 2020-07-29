using Abp.Application.Services.Dto;

namespace Yaeher.CompaniesReport.Dto
{
    /// <summary>
    /// 公司收入汇总
    /// </summary>
    public class CorporateIncomeTotalIn : ListParameters<CorporateIncomeTotal>, IPagedResultRequest
    {
        /// <summary>
        /// 收入时间类型
        /// </summary>
        public string IncomeType { get; set;}
    }
}
