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
    /// OrderManageOut
    /// </summary>
    public class OrderManageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="OrderManageDto"></param>
        /// <param name="OrderManageInfo"></param>
        public OrderManageOut(PagedResultDto<OrderManage> OrderManageDto, OrderManageIn OrderManageInfo)
        {
            Items = OrderManageDto.Items.Select(t=>new OrderManageDetail(t)).ToList();
            TotalCount = OrderManageDto.TotalCount;
            TotalPage = OrderManageDto.TotalCount / OrderManageInfo.MaxResultCount;
            SkipCount = OrderManageInfo.SkipCount;
            MaxResultCount = OrderManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<OrderManage> Items { get; set; }
    }
    /// <summary>
    /// OrderManageTotalOut 统计中明细
    /// </summary>
    public class OrderManageTotalOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="OrderManageDto"></param>
        /// <param name="OrderTradeRecordInfo"></param>
        public OrderManageTotalOut(PagedResultDto<OrderManageDetail> OrderManageDto, ConsultationIn OrderTradeRecordInfo)
        {
            Items = OrderManageDto.Items;
            TotalCount = OrderManageDto.TotalCount;
            TotalPage = OrderManageDto.TotalCount / OrderTradeRecordInfo.MaxResultCount;
            SkipCount = OrderTradeRecordInfo.SkipCount;
            MaxResultCount = OrderTradeRecordInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<OrderManageDetail> Items { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class OrderManageDetail: OrderManage
    {
        public OrderManageDetail() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderManage"></param>
        public OrderManageDetail(OrderManage orderManage)
        {
            OrderNumber = orderManage.OrderNumber;
            ConsultNumber = orderManage.ConsultNumber;
            ConsultID = orderManage.ConsultID;
            ConsultType = orderManage.ConsultType;
            ConsultantID = orderManage.ConsultantID;
            ConsultantName = orderManage.ConsultantName;
            PatientID = orderManage.PatientID;
            PatientName = orderManage.PatientName;
            DoctorName = orderManage.DoctorName;
            DoctorID = orderManage.DoctorID;
            OrderCurrency = orderManage.OrderCurrency;
            OrderMoney = orderManage.OrderMoney;
            ReceivablesType = orderManage.ReceivablesType;
            ReceivablesNumber = orderManage.ReceivablesNumber;
            ServiceID = orderManage.ServiceID;
            ServiceName = orderManage.ServiceName;
            SellerMoneyID = orderManage.SellerMoneyID;
            TradeType = orderManage.TradeType;
            CreatedOn = orderManage.CreatedOn;
            Id = orderManage.Id;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set;}
    }
}
