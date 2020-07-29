using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生提供服务费用表
    /// </summary>
    public class ServiceMoneyListOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ServiceMoneyListDto"></param>
        /// <param name="ServiceMoneyListInfo"></param>
        public ServiceMoneyListOut(PagedResultDto<ServiceMoneyList> ServiceMoneyListDto, ServiceMoneyListIn ServiceMoneyListInfo)
        {
            Items = ServiceMoneyListDto.Items;
            TotalCount = ServiceMoneyListDto.TotalCount;
            TotalPage = ServiceMoneyListDto.TotalCount / ServiceMoneyListInfo.MaxResultCount;
            SkipCount = ServiceMoneyListInfo.SkipCount;
            MaxResultCount = ServiceMoneyListInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ServiceMoneyList> Items { get; set; }

    }
    /// <summary>
    /// 医生提供服务
    /// </summary>
    public class ServiceMoneyStateList: ServiceMoneyList
    {
        /// <summary>
        /// 接单状态
        /// </summary>
        public bool ReceiptState { get; set; }
    }
}
