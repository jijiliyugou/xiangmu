using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 订单收入分配_医生
    /// </summary>
    public class IncomeDevideService : IIncomeDevideService
    {
        private readonly IRepository<IncomeDevide> _repository;
        private readonly IRepository<YaeherConsultation> _consulrepository;
        private readonly IRepository<OrderManage> _orderrepository;
        private readonly IRepository<OrderTradeRecord> _OrderTradeRecordrepository;
        private readonly IRepository<YaeherDoctor> _doctorrepository;
        /// <summary>
        /// 订单收入分配_医生构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="consulrepository"></param>
        /// <param name="orderrepository"></param>
        /// <param name="OrderTradeRecordrepository"></param>
        /// <param name="Doctorrepository"></param>
        public IncomeDevideService(IRepository<IncomeDevide> repository,
            IRepository<YaeherConsultation> consulrepository,
             IRepository<OrderManage> orderrepository,
             IRepository<OrderTradeRecord> OrderTradeRecordrepository,
             IRepository<YaeherDoctor> Doctorrepository)
        {
            _repository = repository;
            _consulrepository = consulrepository;
            _orderrepository = orderrepository;
            _OrderTradeRecordrepository = OrderTradeRecordrepository;
            _doctorrepository = Doctorrepository;
        }
        /// <summary>
        /// 查询订单收入分配_医生 List
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<IncomeDevide>> IncomeDevideList(IncomeDevideIn IncomeDevideInfo)
        {
            var YaeherDoctors = await _repository.GetAllListAsync(IncomeDevideInfo.Expression);
            return YaeherDoctors.ToList();
        }
        /// <summary>
        /// 查询订单收入分配_医生 List
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<IncomeDevide>> IncomeForeignDevideList(IncomeDevideIn IncomeDevideInfo)
        {
            var query = _repository.GetAll().Where(IncomeDevideInfo.Expression);
            var doctor= _doctorrepository.GetAll().Where(t=>t.IsDelete==false&&t.IsSharing==false);
            var incomelist = from a in query
                             join b in doctor on a.DoctorID equals b.Id
                             select a;
            return await incomelist.ToListAsync();
        }
        

        /// <summary>
        /// 统计分账订单收入分配_医生 List
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeTotalModel> IncomeTotalModelList(IncomeDevideIn IncomeDevideInfo)
        {
            IncomeTotalModel model = new IncomeTotalModel();
            var Income = _repository.GetAll().Where(IncomeDevideInfo.Expression);
            var consulres = _consulrepository.GetAll().Where(t => t.IsDelete == false&&t.ConsultState == "success");
            var orderres = _orderrepository.GetAll().Where(t => t.IsDelete == false);
            var ordertraderes = _OrderTradeRecordrepository.GetAll().Where(t => t.IsDelete == false&&t.PayType == "wxpay" && t.PaymentSourceCode == "order" && t.PaymentState == "paid");

            var consuls = from a in consulres
                          join b in Income on a.ConsultNumber equals b.ConsultNumber
                          select a;

            var orders = from c in orderres
                         join a in consulres on c.ConsultNumber equals a.ConsultNumber
                         join b in Income on a.ConsultNumber equals b.ConsultNumber
                         select c;
            var ordertrades = from d in ordertraderes
                              join c in orderres on d.OrderNumber equals c.OrderNumber
                              join a in consulres on c.ConsultNumber equals a.ConsultNumber
                              join b in Income on a.ConsultNumber equals b.ConsultNumber
                              select d;
            model.incomeDevides = await Income.ToListAsync();
            model.yaeherConsultations = await consuls.ToListAsync();
            model.orderManages = await orders.ToListAsync();
            model.orderTradeRecords = await ordertrades.ToListAsync();

            return model;
        }
        /// <summary>
        /// 查询订单收入分配_医生byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDevide> IncomeDevideByID(int Id)
        {
            var YaeherDoctors = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherDoctors;
        }
        /// <summary>
        /// 查询订单收入分配_医生 page
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<IncomeDevide>> IncomeDevidePage(IncomeDevideIn IncomeDevideInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(IncomeDevideInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / IncomeDevideInfo.MaxResultCount;
            var YaeherDoctorList = await query.PageBy(IncomeDevideInfo.SkipCount, IncomeDevideInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<IncomeDevide>(tasksCount, YaeherDoctorList.MapTo<List<IncomeDevide>>());
        }
        /// <summary>
        /// 新建订单收入分配_医生
        /// </summary>
        /// <param name="IncomeDevide"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDevide> CreateIncomeDevide(IncomeDevide IncomeDevide)
        {
            IncomeDevide.Id = await _repository.InsertAndGetIdAsync(IncomeDevide);
            return IncomeDevide;
        }

        /// <summary>
        /// 修改订单收入分配_医生
        /// </summary>
        /// <param name="IncomeDevide"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDevide> UpdateIncomeDevide(IncomeDevide IncomeDevide)
        {
            return await _repository.UpdateAsync(IncomeDevide);
        }

        /// <summary>
        /// 订单收入分配_医生
        /// </summary>
        /// <param name="IncomeDevide"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDevide> DeleteIncomeDevide(IncomeDevide IncomeDevide)
        {
            return await _repository.UpdateAsync(IncomeDevide);
        }
    }
}
