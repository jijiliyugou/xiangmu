using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System.Linq.Expressions;
using System;

namespace Yaeher
{
    /// <summary>
    /// 订单交易记录
    /// </summary>
    public class OrderTradeRecordService : IOrderTradeRecordService
    {
        private readonly IRepository<OrderTradeRecord> _repository;
        private readonly IRepository<OrderManage> _orderrepository;
        private readonly IRepository<YaeherConsultation> _consulrepository;
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="orderrepository"></param>
        /// <param name="consulrepository"></param>
        public OrderTradeRecordService(IRepository<OrderTradeRecord> repository, IRepository<OrderManage> orderrepository, IRepository<YaeherConsultation> consulrepository)
        {
            _repository = repository;
            _orderrepository = orderrepository;
            _consulrepository = consulrepository;
        }
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<OrderTradeRecord>> OrderTradePayRecordList(OrderTradeRecordIn OrderTradeRecordInfo)
        {
            var OrderTradeRecords = _repository.GetAll().Where(OrderTradeRecordInfo.Expression);
            var ordermanage = _orderrepository.GetAll().Where(t => t.IsDelete == false);
            var consul = _consulrepository.GetAll().Where(t => t.IsDelete == false&&t.ConsultState!="return");
            OrderTradeRecords = from a in OrderTradeRecords
                                join b in ordermanage on a.OrderID equals b.Id
                                join c in consul on b.ConsultNumber equals c.ConsultNumber
                                select a;
            return await OrderTradeRecords.OrderByDescending(t => t.CreatedOn).ToListAsync();
        }

        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<OrderTradeRecord>> OrderTradeRecordList(OrderTradeRecordIn OrderTradeRecordInfo)
        {
            var OrderTradeRecords = _repository.GetAll().Where(OrderTradeRecordInfo.Expression);
            if (OrderTradeRecordInfo.DoctorId > 0)
            {

                var ordermanage = _orderrepository.GetAll().Where(t => t.IsDelete == false && t.DoctorID == OrderTradeRecordInfo.DoctorId);
                if (OrderTradeRecordInfo.ServiceID > 0)
                {
                    ordermanage = ordermanage.Where(t => t.ServiceID == OrderTradeRecordInfo.ServiceID);
                }
                OrderTradeRecords = from a in OrderTradeRecords
                                    join b in ordermanage on a.OrderID equals b.Id
                                    select a;
            }
            return await OrderTradeRecords.OrderByDescending(t => t.CreatedOn).ToListAsync();
        }

        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<OrderTradeRecord>> DoctorOrderTradeRecordList(OrderTradeRecordIn OrderTradeRecordInfo)
        {
            var OrderTradeRecords = _repository.GetAll().Where(OrderTradeRecordInfo.Expression);
            var order = _orderrepository.GetAll().Where(t => t.IsDelete == false);
            if (OrderTradeRecordInfo.ServiceID > 0)
            {
                order = order.Where(t => t.ServiceID == OrderTradeRecordInfo.ServiceID);
            }
            var consul = _consulrepository.GetAll().Where(t => t.IsDelete == false);
            var record = from a in OrderTradeRecords
                         join b in order on a.OrderNumber equals b.OrderNumber
                         join c in consul on b.ConsultNumber equals c.ConsultNumber
                         where c.DoctorID == OrderTradeRecordInfo.DoctorId
                         select a;

            return await record.ToListAsync();
        }
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<OrderTradeRecord>> PatientOrderTradeRecordList(OrderTradeRecordIn OrderTradeRecordInfo)
        {
            var OrderTradeRecords = _repository.GetAll().Where(OrderTradeRecordInfo.Expression);
            var order = _orderrepository.GetAll().Where(t => t.IsDelete == false && t.ServiceID == OrderTradeRecordInfo.ServiceID);
            var consul = _consulrepository.GetAll().Where(t => t.IsDelete == false && (t.RefundNumber == "" || t.RefundNumber == null));
            var record = from a in OrderTradeRecords
                         join b in order on a.OrderNumber equals b.OrderNumber
                         join c in consul on b.ConsultNumber equals c.ConsultNumber
                         where c.DoctorID == OrderTradeRecordInfo.DoctorId
                         select a;
            return await record.ToListAsync();
        }
        /// <summary>
        /// 查询订单交易记录 List
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<OrderTradeRecord>> OrderTradeRecordReportList(OrderTradeRecordIn OrderTradeRecordInfo)
        {
            var list = _repository.GetAll().Where(t => !t.IsDelete);
            var order = _orderrepository.GetAll().Where(t => !t.IsDelete);
            var consul = _consulrepository.GetAll().Where(t => !t.IsDelete);
            var query = from a in list
                        join c in order on a.OrderNumber equals c.OrderNumber
                        join d in consul on c.ConsultNumber equals d.ConsultNumber
                        where a.PaymentState == "paid" && a.PaymentSourceCode == "order"
                        select new OrderTradeRecord
                        {
                            OrderID = c.Id,
                            OrderCurrency = c.OrderCurrency,
                            VoucherNumber = a.VoucherNumber,
                            PayMoney = a.PayMoney,
                            CreatedBy = a.CreatedBy,
                            CreatedOn = a.CreatedOn,
                        };

            var OrderTradeRecords = await _repository.GetAllListAsync(OrderTradeRecordInfo.Expression);
            return OrderTradeRecords.ToList();
        }
        /// <summary>
        /// 用户基础表 Expression查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTradeRecord> OrderTradeRecordExpress(Expression<Func<OrderTradeRecord, bool>> whereExpression)
        {
            return await _repository.FirstOrDefaultAsync(whereExpression);
        }
        /// <summary>
        /// 查询订单交易记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTradeRecord> OrderTradeRecordByID(int Id)
        {
            var OrderTradeRecords = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return OrderTradeRecords;
        }
        /// <summary>
        /// 查询订单交易记录 page
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<OrderTradeRecord>> OrderTradeRecordPage(OrderTradeRecordIn OrderTradeRecordInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(OrderTradeRecordInfo.Expression);


            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / OrderTradeRecordInfo.MaxResultCount;
            var OrderTradeRecordList = await query.PageBy(OrderTradeRecordInfo.SkipTotal, OrderTradeRecordInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<OrderTradeRecord>(tasksCount, OrderTradeRecordList.MapTo<List<OrderTradeRecord>>());
        }
        /// <summary>
        /// 查询订单交易记录 PCOrderTradeRecordPage
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<OrderTradeRecordPCModule>> PCOrderTradeRecordPage(OrderTradeRecordIn OrderTradeRecordInfo)
        {
            //初步过滤
            var querylist = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(OrderTradeRecordInfo.Expression);
            var order = _orderrepository.GetAll().Where(t => t.IsDelete == false);
            var consul = _consulrepository.GetAll().Where(t => t.IsDelete == false);
            var query = from a in querylist
                        join b in order on a.OrderNumber equals b.OrderNumber
                        join c in consul on b.ConsultNumber equals c.ConsultNumber
                        select new OrderTradeRecordPCModule
                        {
                            SequenceNo = a.SequenceNo,
                            OrderID = a.OrderID,
                            OrderNumber = a.OrderNumber,
                            PayType = a.PayType,
                            OrderCurrency = a.OrderCurrency,
                            TenpayNumber = a.TenpayNumber,
                            VoucherNumber = a.VoucherNumber,
                            VoucherJSON = a.VoucherJSON,
                            PayMoney = a.PayMoney,
                            PayAchiveTime = a.PayAchiveTime,
                            PaySerialNumber = a.PaySerialNumber,
                            PaymentState = a.PaymentState,
                            PaymentSource = a.PaymentSource,
                            PaymentSourceCode = a.PaymentSourceCode,
                            WXPayBillno = a.WXPayBillno,
                            WXTransactionId = a.WXTransactionId,
                            WXOrderQuery = a.WXOrderQuery,
                            CreatedOn = a.CreatedOn,
                            Id = a.Id,
                            ConsultId = c.Id,
                            ConsultNumber = c.ConsultNumber,
                        };
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.KeyWord))
            {
                //(a => a.OrderNumber.Contains(OrderTradeRecordInfo.KeyWord)||a.WXPayBillno.Contains(OrderTradeRecordInfo.KeyWord)||a.WXTransactionId.Contains(OrderTradeRecordInfo.KeyWord));

                query = query.Where(a => a.OrderNumber.Contains(OrderTradeRecordInfo.KeyWord) ||
                                         a.WXPayBillno.Contains(OrderTradeRecordInfo.KeyWord) ||
                                         a.WXTransactionId.Contains(OrderTradeRecordInfo.KeyWord) ||
                                         a.ConsultNumber.Contains(OrderTradeRecordInfo.KeyWord));
            }
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / OrderTradeRecordInfo.MaxResultCount;
            var OrderTradeRecordList = await query.PageBy(OrderTradeRecordInfo.SkipTotal, OrderTradeRecordInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<OrderTradeRecordPCModule>(tasksCount, OrderTradeRecordList.MapTo<List<OrderTradeRecordPCModule>>());
        }

        /// <summary>
        /// 新建订单交易记录
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTradeRecord> CreateOrderTradeRecord(OrderTradeRecord OrderTradeRecordInfo)
        {
            OrderTradeRecordInfo.Id = await _repository.InsertAndGetIdAsync(OrderTradeRecordInfo);
            return OrderTradeRecordInfo;


        }

        /// <summary>
        /// 修改订单交易记录
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTradeRecord> UpdateOrderTradeRecord(OrderTradeRecord OrderTradeRecordInfo)
        {
            return await _repository.UpdateAsync(OrderTradeRecordInfo);
        }

        /// <summary>
        /// 删除订单交易记录
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTradeRecord> DeleteOrderTradeRecord(OrderTradeRecord OrderTradeRecordInfo)
        {
            return await _repository.UpdateAsync(OrderTradeRecordInfo);
        }

        /// <summary>
        /// 查询订单交易记录byOrderNumber
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTradeRecord> OrderTradeRecordByOrderNumber(string OrderNumber)
        {
            var OrderTradeRecords = await _repository.FirstOrDefaultAsync(t => t.OrderNumber == OrderNumber && !t.IsDelete);
            return OrderTradeRecords;
        }
        /// <summary>
        /// 查询订单交易记录byConsultNumber
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTradeRecord> OrderTradeRecordByConsultNumber(string ConsultNumber)
        {
            var list = _repository.GetAll().Where(t => t.PayMoney>0&&!t.IsDelete);
            var order = _orderrepository.GetAll().Where(t => t.ConsultNumber==ConsultNumber&&!t.IsDelete);
            var consul = _consulrepository.GetAll().Where(t => t.ConsultNumber==ConsultNumber&& !t.IsDelete);
            var query = from a in list
                        join c in order on a.OrderNumber equals c.OrderNumber
                        join d in consul on c.ConsultNumber equals d.ConsultNumber
                        where a.PaymentState == "paid" && a.PaymentSourceCode == "order"
                        select a;

            var OrderTradeRecords = await query.FirstOrDefaultAsync();
            return OrderTradeRecords;
        }
    }
}
