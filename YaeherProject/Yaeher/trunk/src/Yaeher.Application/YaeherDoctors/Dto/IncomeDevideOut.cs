using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 订单收入分配_医生
    /// </summary>
    public class IncomeDevideOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="IncomeDevideDto"></param>
        /// <param name="IncomeDevideInfo"></param>
        public IncomeDevideOut(PagedResultDto<IncomeDevide> IncomeDevideDto, IncomeDevideIn IncomeDevideInfo)
        {
            Items = IncomeDevideDto.Items;
            TotalCount = IncomeDevideDto.TotalCount;
            TotalPage = IncomeDevideDto.TotalCount / IncomeDevideInfo.MaxResultCount;
            SkipCount = IncomeDevideInfo.SkipCount;
            MaxResultCount = IncomeDevideInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<IncomeDevide> Items { get; set; }

        
    }
    /// <summary>
    /// 
    /// </summary>
    public class IncomeTotalModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<IncomeDevide> incomeDevides { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<YaeherConsultation> yaeherConsultations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OrderManage> orderManages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OrderTradeRecord> orderTradeRecords { get; set; }
    }
}
