using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 用户支付表
    /// </summary>
    public class YaeherUserPaymentOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherUserPaymentDto"></param>
        /// <param name="YaeherUserPaymentInfo"></param>
        public YaeherUserPaymentOut(PagedResultDto<YaeherUserPayment> YaeherUserPaymentDto, YaeherUserPaymentIn YaeherUserPaymentInfo)
        {
            Items = YaeherUserPaymentDto.Items;
            TotalCount = YaeherUserPaymentDto.TotalCount;
            TotalPage = YaeherUserPaymentDto.TotalCount / YaeherUserPaymentInfo.MaxResultCount;
            SkipCount = YaeherUserPaymentInfo.SkipCount;
            MaxResultCount = YaeherUserPaymentInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherUserPayment> Items { get; set; }

    }
}
