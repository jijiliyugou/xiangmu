using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 订单交易记录
    /// </summary>
    public interface IOrderTradeRecordService : IApplicationService
    {
        /// <summary>
        /// 新建订单交易记录
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<OrderTradeRecord> CreateOrderTradeRecord(OrderTradeRecord OrderTradeRecordInfo);
        /// <summary>
        /// 删除订单交易记录
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<OrderTradeRecord> DeleteOrderTradeRecord(OrderTradeRecord OrderTradeRecordInfo);
        /// <summary>
        /// 查询订单交易记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OrderTradeRecord> OrderTradeRecordByID(int Id);
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<List<OrderTradeRecord>> OrderTradeRecordList(OrderTradeRecordIn OrderTradeRecordInfo);
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<List<OrderTradeRecord>> OrderTradePayRecordList(OrderTradeRecordIn OrderTradeRecordInfo);
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<List<OrderTradeRecord>> DoctorOrderTradeRecordList(OrderTradeRecordIn OrderTradeRecordInfo);
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<List<OrderTradeRecord>> PatientOrderTradeRecordList(OrderTradeRecordIn OrderTradeRecordInfo);
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<IList<OrderTradeRecord>> OrderTradeRecordReportList(OrderTradeRecordIn OrderTradeRecordInfo);

        
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<OrderTradeRecord> OrderTradeRecordExpress(Expression<Func<OrderTradeRecord, bool>> whereExpression);
        /// <summary>
        /// 查询订单交易记录 page
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderTradeRecord>> OrderTradeRecordPage(OrderTradeRecordIn OrderTradeRecordInfo);
        /// <summary>
        /// 查询订单交易记录 page
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderTradeRecordPCModule>> PCOrderTradeRecordPage(OrderTradeRecordIn OrderTradeRecordInfo);
        /// <summary>
        /// 修改订单交易记录
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        Task<OrderTradeRecord> UpdateOrderTradeRecord(OrderTradeRecord OrderTradeRecordInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        Task<OrderTradeRecord> OrderTradeRecordByOrderNumber(string OrderNumber);


        /// <summary>
        /// 查询订单交易记录byConsultNumber
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <returns></returns>
        Task<OrderTradeRecord> OrderTradeRecordByConsultNumber(string ConsultNumber);
    }
}