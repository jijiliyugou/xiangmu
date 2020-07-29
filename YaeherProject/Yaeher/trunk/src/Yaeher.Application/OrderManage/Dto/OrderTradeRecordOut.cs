using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// OrderTradeRecordOut
    /// </summary>
    public class OrderTradeRecordOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="OrderTradeRecordDto"></param>
        /// <param name="OrderTradeRecordInfo"></param>
        public OrderTradeRecordOut(PagedResultDto<OrderTradeRecord> OrderTradeRecordDto, OrderTradeRecordIn OrderTradeRecordInfo)
        {
            Items = OrderTradeRecordDto.Items;
            TotalCount = OrderTradeRecordDto.TotalCount;
            TotalPage = OrderTradeRecordDto.TotalCount / OrderTradeRecordInfo.MaxResultCount;
            SkipCount = OrderTradeRecordInfo.SkipCount;
            MaxResultCount = OrderTradeRecordInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<OrderTradeRecord> Items { get; set; }
    }

    /// <summary>
    /// OrderTradeRecordOut
    /// </summary>
    public class OrderTradeRecordPCModuleOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="OrderTradeRecordDto"></param>
        /// <param name="OrderTradeRecordInfo"></param>
        public OrderTradeRecordPCModuleOut(PagedResultDto<OrderTradeRecordPCModule> OrderTradeRecordDto, OrderTradeRecordIn OrderTradeRecordInfo)
        {
            Items = OrderTradeRecordDto.Items;
            TotalCount = OrderTradeRecordDto.TotalCount;
            TotalPage = OrderTradeRecordDto.TotalCount / OrderTradeRecordInfo.MaxResultCount;
            SkipCount = OrderTradeRecordInfo.SkipCount;
            MaxResultCount = OrderTradeRecordInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<OrderTradeRecordPCModule> Items { get; set; }
    }
    public class OrderTradeRecordPCModule : OrderTradeRecord
    {
        public int ConsultId { get; set; }
        public string ConsultNumber { get; set; }
    }


}
