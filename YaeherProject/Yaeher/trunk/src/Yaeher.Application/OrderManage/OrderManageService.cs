using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;

namespace Yaeher
{

    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderManageService : IOrderManageService
    {
        private readonly IRepository<OrderManage> _repository;
        private readonly IRepository<OrderTradeRecord> _orderdetailrepository;
        private readonly IRepository<YaeherConsultation> _yaeherConsultationrepository;
       /// <summary>
       /// 构造函数
       /// </summary>
       /// <param name="repository"></param>
       /// <param name="orderdetailrepository"></param>
       /// <param name="yaeherConsultationrepository"></param>
        public OrderManageService(IRepository<OrderManage> repository, 
            IRepository<OrderTradeRecord> orderdetailrepository,
            IRepository<YaeherConsultation> yaeherConsultationrepository)
        {
            _repository = repository;
            _orderdetailrepository = orderdetailrepository;
            _yaeherConsultationrepository = yaeherConsultationrepository;
        }

        /// <summary>
        /// 查询订单管理 List
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<OrderManage>> OrderManageList(OrderManageIn OrderManageInfo)
        {
            var OrderManages = await _repository.GetAllListAsync(OrderManageInfo.Expression);
            return OrderManages.ToList();
        }

        /// <summary>
        /// 查询订单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderManage> OrderManageByID(int Id)
        {
            var OrderManages = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return OrderManages;
        }

        /// <summary>
        /// 查询订单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderManage> OrderManageByConsultationID(int Id)
        {
            var OrderManages = await _repository.FirstOrDefaultAsync(t => t.ConsultID == Id && !t.IsDelete);
            return OrderManages;
        }
        /// <summary>
        /// 查询订单管理byconsultNumber
        /// </summary>
        /// <param name="consultNumber"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderManage> OrderManageByconsultNumber(string consultNumber)
        {
            var OrderManages = await _repository.FirstOrDefaultAsync(t => t.ConsultNumber == consultNumber && !t.IsDelete);
            return OrderManages;
        }

        /// <summary>
        /// 查询订单管理 page
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<OrderManage>> OrderManagePage(OrderManageIn OrderManageInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(OrderManageInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / OrderManageInfo.MaxResultCount;
            var OrderManageList = await query.PageBy(OrderManageInfo.SkipTotal, OrderManageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<OrderManage>(tasksCount, OrderManageList.MapTo<List<OrderManage>>());
        }

        /// <summary>
        /// 查询订单管理 page
        /// </summary>
        /// <param name="consultationIn"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<OrderManageDetail>> TotalOrderManagePage(ConsultationIn consultationIn)
        {
            //初步过滤
            var consul = _yaeherConsultationrepository.GetAll().Where(consultationIn.Expression);
            var query = _repository.GetAll().Where(t=>t.IsDelete==false&&t.DoctorID==consultationIn.DoctorID);
            var orderdetail = _orderdetailrepository.GetAll().Where(t=>t.IsDelete==false&&t.PayMoney>0);
            var querydetail = from c in consul
                              join b in query on c.ConsultNumber equals b.ConsultNumber
                              join a in orderdetail on b.OrderNumber equals a.OrderNumber
                              select new OrderManageDetail
                              {
                                  OrderNumber = b.OrderNumber,
                                  ConsultNumber = b.ConsultNumber,
                                  ConsultID = b.ConsultID,
                                  ConsultType = b.ConsultType,
                                  ConsultantID = b.ConsultantID,
                                  ConsultantName = b.ConsultantName,
                                  PatientID = b.PatientID,
                                  PatientName = b.PatientName,
                                  DoctorName = b.DoctorName,
                                  DoctorID = b.DoctorID,
                                  OrderCurrency = b.OrderCurrency,
                                  OrderMoney = a.PayMoney,
                                  ReceivablesType = b.ReceivablesType,
                                  ReceivablesNumber = b.ReceivablesNumber,
                                  ServiceID = b.ServiceID,
                                  ServiceName = b.ServiceName,
                                  SellerMoneyID = b.SellerMoneyID,
                                  TradeType = b.TradeType,
                                  CreatedOn = a.CreatedOn,
                                  Id = b.Id,
                              };
            //获取总数
            var tasksCount = querydetail.Count();
            //获取总数
            var totalpage = tasksCount / consultationIn.MaxResultCount;
            var OrderManageList = await querydetail.OrderByDescending(t=>t.CreatedBy).PageBy(consultationIn.SkipTotal, consultationIn.MaxResultCount).ToListAsync();
            return new PagedResultDto<OrderManageDetail>(tasksCount, OrderManageList.MapTo<List<OrderManageDetail>>());
        
        }
        /// <summary>
        /// 新建订单管理
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderManage> CreateOrderManage(OrderManage OrderManageInfo)
        {
            OrderManageInfo.Id = await _repository.InsertAndGetIdAsync(OrderManageInfo);
            return OrderManageInfo;
        }

        /// <summary>
        /// 修改订单管理
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderManage> UpdateOrderManage(OrderManage OrderManageInfo)
        {
            return await _repository.UpdateAsync(OrderManageInfo);
        }

        /// <summary>
        /// 删除订单管理
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderManage> DeleteOrderManage(OrderManage OrderManageInfo)
        {
            return await _repository.UpdateAsync(OrderManageInfo);
        }

    }
}
