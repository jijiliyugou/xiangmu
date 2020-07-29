using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.NumericalStatement.Dto;
using System.Linq;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Abp.Domain.Uow;

namespace Yaeher.NumericalStatement
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsultationOrderTotalService : IConsultationOrderTotalService
    {
        private readonly IRepository<ConsultationOrderTotal> _repository;
        private readonly IRepository<YaeherConsultation> _YaeherConsultationrepository;
        private readonly IRepository<OrderManage> _OrderManagerepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="YaeherConsultationrepository"></param>
        /// <param name="OrderManagerepository"></param>
        public ConsultationOrderTotalService(IRepository<ConsultationOrderTotal> repository,
                                             IRepository<YaeherConsultation> YaeherConsultationrepository,
                                             IRepository<OrderManage> OrderManagerepository
                                           , IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _YaeherConsultationrepository = YaeherConsultationrepository;
            _OrderManagerepository = OrderManagerepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 订单汇总统计 List
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ConsultationOrderTotal>> ConsultationOrderTotalList(ConsultationOrderTotalIn ConsultationOrderTotalInfo)
        {
            var ConsultationOrderTotals = await _repository.GetAll().Where(ConsultationOrderTotalInfo.Expression).OrderByDescending(t => t.TotalDate).ToListAsync();
            return ConsultationOrderTotals;
        }

        /// <summary>
        /// 订单汇总统计byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationOrderTotal> ConsultationOrderTotalByID(int Id)
        {
            var ConsultationOrderTotals = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ConsultationOrderTotals;
        }

        /// <summary>
        /// 订单汇总统计 page
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ConsultationOrderTotal>> ConsultationOrderTotalPage(ConsultationOrderTotalIn ConsultationOrderTotalInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ConsultationOrderTotalInfo.Expression).OrderByDescending(a => a.RevenueTotal);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ConsultationOrderTotalInfo.MaxResultCount;
            var ConsultationOrderTotalList = await query.PageBy(ConsultationOrderTotalInfo.SkipTotal, ConsultationOrderTotalInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ConsultationOrderTotal>(tasksCount, ConsultationOrderTotalList.MapTo<List<ConsultationOrderTotal>>());
        }
        /// <summary>
        /// 新建 订单汇总统计
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationOrderTotal> CreateConsultationOrderTotal(ConsultationOrderTotal ConsultationOrderTotalInfo)
        {
            ConsultationOrderTotalInfo.Id = await _repository.InsertAndGetIdAsync(ConsultationOrderTotalInfo);
            return ConsultationOrderTotalInfo;
        }

        /// <summary>
        /// 修改 订单汇总统计
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationOrderTotal> UpdateConsultationOrderTotal(ConsultationOrderTotal ConsultationOrderTotalInfo)
        {
            return await _repository.UpdateAsync(ConsultationOrderTotalInfo);
        }

        /// <summary>
        /// 删除 订单汇总统计
        /// </summary>
        /// <param name="ConsultationOrderTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationOrderTotal> DeleteConsultationOrderTotal(ConsultationOrderTotal ConsultationOrderTotalInfo)
        {
            return await _repository.UpdateAsync(ConsultationOrderTotalInfo);
        }

        /// <summary>
        /// OrderTotalByDay  汇总统计当天订单数据 
        /// </summary>
        /// <param name="orderTotalIn"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<string> OrderTotal(OrderTotalIn orderTotalIn)
        {

            var ConsultationList = _YaeherConsultationrepository.GetAll().Where(a => a.IsDelete == false);
            var OrderList = _OrderManagerepository.GetAll().Where(a => a.IsDelete == false);
            var ConsultationOrder = from a in ConsultationList
                                    join b in OrderList on a.ConsultNumber equals b.ConsultNumber
                                    select new ConsultationOrder
                                    {
                                        ConsultNumber = a.ConsultNumber,
                                        DoctorID = a.DoctorID,
                                        DoctorName = a.DoctorName,
                                        //   DoctorJSON = a.DoctorJSON,
                                        RefundNumber = a.RefundNumber,
                                        CreatedOn = a.CreatedOn,
                                        RefundTime = a.RefundTime,
                                        Completetime = a.Completetime,
                                        OrderCurrency = b.OrderCurrency,
                                        OrderMoney = b.OrderMoney,
                                        ConsultState = a.ConsultState,
                                    };
            // 当天总单数
            var OrderTotal = from p in ConsultationOrder
                             where p.CreatedOn >= orderTotalIn.StartTime && p.CreatedOn < orderTotalIn.EndTime
                             group p by new { p.DoctorID, p.DoctorName } into g
                             select new
                             {
                                 g.Key,
                                 OrderTotal = g.Count(),
                                 RevenueTotal = g.Sum(p => p.OrderMoney)
                             };
            var RefundTotal = from p in ConsultationOrder
                              where p.ConsultState == "return" && p.RefundTime >= orderTotalIn.StartTime && p.RefundTime < orderTotalIn.EndTime
                              group p by new { p.DoctorID, p.DoctorName } into g
                              select new
                              {
                                  g.Key,
                                  RefundTotal = g.Count(),
                                  RefundMoney = g.Sum(p => p.OrderMoney)
                              };
            var CompleteTotal = from p in ConsultationOrder
                                where p.ConsultState == "success" && p.Completetime >= orderTotalIn.StartTime && p.Completetime < orderTotalIn.EndTime
                                group p by new { p.DoctorID, p.DoctorName } into g
                                select new
                                {
                                    g.Key,
                                    CompleteTotal = g.Count(),
                                    CompleteMoney = g.Sum(p => p.OrderMoney)
                                };

            var ConsultationOrders = (from a in OrderTotal select a.Key).Union
                                     (from b in RefundTotal select b.Key).Union
                                     (from c in CompleteTotal select c.Key);
            if (ConsultationOrders.Count() > 0)
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var consulorderlist = ConsultationOrders.ToList();
                    var OrderTotallist = OrderTotal.ToList();
                    var RefundTotallist = RefundTotal.ToList();
                    var CompleteTotallist = CompleteTotal.ToList();
                    #region 新增每天数据
                    foreach (var OrderInfo in consulorderlist)
                    {
                        ConsultationOrderTotal consultationOrderTotals = new ConsultationOrderTotal();
                        consultationOrderTotals.DoctorID = OrderInfo.DoctorID;
                        consultationOrderTotals.DoctorName = OrderInfo.DoctorName;
                        //   consultationOrderTotals.DoctorJSON = "";
                        if (OrderTotal.Count() > 0)
                        {
                            if (OrderTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).Count() > 0)
                            {
                                consultationOrderTotals.OrderTotal = OrderTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).FirstOrDefault().OrderTotal;
                                consultationOrderTotals.RevenueTotal = double.Parse(OrderTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).FirstOrDefault().RevenueTotal.ToString());
                            }
                            else
                            {
                                consultationOrderTotals.OrderTotal = 0;
                                consultationOrderTotals.RevenueTotal = 0;
                            }
                        }
                        else
                        {
                            consultationOrderTotals.OrderTotal = 0;
                            consultationOrderTotals.RevenueTotal = 0;
                        }
                        if (RefundTotal.Count() > 0)
                        {
                            if (RefundTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).Count() > 0)
                            {
                                consultationOrderTotals.RefundTotal = RefundTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).FirstOrDefault().RefundTotal;
                                consultationOrderTotals.RefundMoney = double.Parse(RefundTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).FirstOrDefault().RefundMoney.ToString());

                                //RefundTotal.ToList().Where(a => a.Key.DoctorID == OrderInfo.DoctorID).FirstOrDefault().RefundMoney;
                            }
                            else
                            {
                                consultationOrderTotals.RefundTotal = 0;
                                consultationOrderTotals.RefundMoney = 0;
                            }
                        }
                        else
                        {
                            consultationOrderTotals.RefundTotal = 0;
                            consultationOrderTotals.RefundMoney = 0;
                        }
                        if (CompleteTotal.Count() > 0)
                        {
                            if (CompleteTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).Count() > 0)
                            {
                                consultationOrderTotals.CompleteTotal = CompleteTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).FirstOrDefault().CompleteTotal;
                                consultationOrderTotals.CompleteMoney = double.Parse(CompleteTotallist.Where(a => a.Key.DoctorID == OrderInfo.DoctorID).FirstOrDefault().CompleteMoney.ToString());
                            }
                            else
                            {
                                consultationOrderTotals.CompleteTotal = 0;
                                consultationOrderTotals.CompleteMoney = 0;
                            }
                        }
                        else
                        {
                            consultationOrderTotals.CompleteTotal = 0;
                            consultationOrderTotals.CompleteMoney = 0;
                        }
                        consultationOrderTotals.TotalDate = orderTotalIn.StartTime;
                        consultationOrderTotals.TotalType = "day";
                        var ConsultationDays = await _repository.GetAll().Where(a => a.DoctorID == consultationOrderTotals.DoctorID &&
                                                                        a.TotalType == "day" &&
                                                                        a.TotalDate == consultationOrderTotals.TotalDate).ToListAsync();

                        if (ConsultationDays.Count > 0)
                        {
                            var ConsultationOrderDays = ConsultationDays.FirstOrDefault();
                            ConsultationOrderDays.DoctorName = consultationOrderTotals.DoctorName;
                            ConsultationOrderDays.DoctorID = consultationOrderTotals.DoctorID;
                            //  ConsultationOrderDays.DoctorJSON = consultationOrderTotals.DoctorJSON;
                            ConsultationOrderDays.OrderTotal = consultationOrderTotals.OrderTotal;
                            ConsultationOrderDays.RevenueTotal = Math.Round(consultationOrderTotals.RevenueTotal, 1);  // 取数据的1位小数
                            ConsultationOrderDays.RefundMoney = Math.Round(consultationOrderTotals.RefundMoney, 1);  // 取数据的1位小数
                            ConsultationOrderDays.CompleteMoney = Math.Round(consultationOrderTotals.CompleteMoney, 1);  // 取数据的1位小数
                            ConsultationOrderDays.TotalType = consultationOrderTotals.TotalType;
                            ConsultationOrderDays.RefundTotal = consultationOrderTotals.RefundTotal;
                            ConsultationOrderDays.CompleteTotal = consultationOrderTotals.CompleteTotal;
                            ConsultationOrderDays.TotalDate = consultationOrderTotals.TotalDate;
                            ConsultationOrderDays.ModifyOn = DateTime.Now;
                            var reday = await _repository.UpdateAsync(ConsultationOrderDays);
                        }
                        else
                        {
                            consultationOrderTotals.RevenueTotal = Math.Round(consultationOrderTotals.RevenueTotal, 1);  // 取数据的1位小数
                                                                                                                         //wait _repository.InsertAsync(consultationOrderTotals);
                            consultationOrderTotals.Id = await _repository.InsertAndGetIdAsync(consultationOrderTotals);
                        }
                    }
                    #endregion
                    #region 新增每月数据
                    //DateTime MonthStartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:01")).AddDays(1 - DateTime.Now.Day);
                    //DateTime MonthEndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 11:59:59")).AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1);
                    DateTime MonthStartTime = DateTime.Parse(orderTotalIn.StartTime.ToString("yyyy-MM-dd 00:00:01")).AddDays(1 - orderTotalIn.StartTime.Day);
                    DateTime dayStartTime = DateTime.Parse(orderTotalIn.StartTime.ToString("yyyy-MM-dd 00:00:00")).AddDays(1 - orderTotalIn.StartTime.Day);
                    DateTime MonthEndTime = DateTime.Parse(orderTotalIn.StartTime.ToString("yyyy-MM-dd 11:59:59")).AddDays(1 - orderTotalIn.StartTime.Day).AddMonths(1).AddDays(-1);
                    var MonthTotals = await _repository.GetAll().Where(a => a.IsDelete == false && a.TotalDate >= dayStartTime && a.TotalDate <= MonthEndTime && a.TotalType == "day").ToListAsync();
                    var ConsultationMonthTotals = from p in MonthTotals
                                                  group p by new { p.DoctorID, p.DoctorName } into g
                                                  select new
                                                  {
                                                      g.Key,
                                                      OrderTotal = g.Sum(p => p.OrderTotal),
                                                      RevenueTotal = g.Sum(p => p.RevenueTotal),
                                                      RefundMoney = g.Sum(p => p.RefundMoney),
                                                      CompleteMoney = g.Sum(p => p.CompleteMoney),
                                                      RefundTotal = g.Sum(p => p.RefundTotal),
                                                      CompleteTotal = g.Sum(p => p.CompleteTotal),
                                                      TotalType = "month",
                                                      TotalDate = MonthStartTime,
                                                  };
                    if (ConsultationMonthTotals.Count() > 0)
                    {
                        var consulmonthtotal = ConsultationMonthTotals.ToList();
                        foreach (var ConsultationMonthInfo in consulmonthtotal)
                        {
                            var ConsultationMonths = await _repository.GetAll().Where(a => a.DoctorID == ConsultationMonthInfo.Key.DoctorID &&
                                                                     a.TotalType == "month" &&
                                                                     a.TotalDate == ConsultationMonthInfo.TotalDate).ToListAsync();
                            if (ConsultationMonths.Count() > 0)
                            {
                                var ConsultationOrderMonths = ConsultationMonths.FirstOrDefault();
                                ConsultationOrderMonths.DoctorName = ConsultationMonthInfo.Key.DoctorName;
                                ConsultationOrderMonths.DoctorID = ConsultationMonthInfo.Key.DoctorID;
                                //   ConsultationOrderMonths.DoctorJSON = "";
                                ConsultationOrderMonths.OrderTotal = ConsultationMonthInfo.OrderTotal;
                                ConsultationOrderMonths.RevenueTotal = ConsultationMonthInfo.RevenueTotal;
                                ConsultationOrderMonths.RefundMoney = ConsultationMonthInfo.RefundMoney;
                                ConsultationOrderMonths.CompleteMoney = ConsultationMonthInfo.CompleteMoney;
                                ConsultationOrderMonths.TotalType = ConsultationMonthInfo.TotalType;
                                ConsultationOrderMonths.RefundTotal = ConsultationMonthInfo.RefundTotal;
                                ConsultationOrderMonths.CompleteTotal = ConsultationMonthInfo.CompleteTotal;
                                ConsultationOrderMonths.TotalDate = ConsultationMonthInfo.TotalDate;
                                ConsultationOrderMonths.ModifyOn = DateTime.Now;
                                var remonth = await _repository.UpdateAsync(ConsultationOrderMonths);
                            }
                            else
                            {
                                ConsultationOrderTotal ConsultationOrderMonths = new ConsultationOrderTotal();
                                ConsultationOrderMonths.DoctorName = ConsultationMonthInfo.Key.DoctorName;
                                ConsultationOrderMonths.DoctorID = ConsultationMonthInfo.Key.DoctorID;
                                //  ConsultationOrderMonths.DoctorJSON = "";
                                ConsultationOrderMonths.OrderTotal = ConsultationMonthInfo.OrderTotal;
                                ConsultationOrderMonths.RevenueTotal = ConsultationMonthInfo.RevenueTotal;
                                ConsultationOrderMonths.RefundMoney = ConsultationMonthInfo.RefundMoney;
                                ConsultationOrderMonths.CompleteMoney = ConsultationMonthInfo.CompleteMoney;
                                ConsultationOrderMonths.TotalType = ConsultationMonthInfo.TotalType;
                                ConsultationOrderMonths.RefundTotal = ConsultationMonthInfo.RefundTotal;
                                ConsultationOrderMonths.CompleteTotal = ConsultationMonthInfo.CompleteTotal;
                                ConsultationOrderMonths.TotalDate = ConsultationMonthInfo.TotalDate;
                                //await _repository.InsertAsync(ConsultationOrderMonths);
                                ConsultationOrderMonths.Id = await _repository.InsertAndGetIdAsync(ConsultationOrderMonths);
                            }
                        }
                    }
                    #endregion
                    #region 新增每年数据
                    DateTime YearStartTime = DateTime.Parse(orderTotalIn.StartTime.ToString("yyyy-01-01 00:00:01"));
                    DateTime monthStartTime = DateTime.Parse(orderTotalIn.StartTime.ToString("yyyy-01-01 00:00:00"));
                    DateTime YearEndTime = DateTime.Parse(orderTotalIn.StartTime.ToString("yyyy-12-31 11:59:59"));
                    var YearTotals = await _repository.GetAll().Where(a => a.IsDelete == false && a.TotalDate >= monthStartTime && a.TotalDate <= YearEndTime && a.TotalType == "month").ToListAsync();
                    var ConsultationYearTotals = from p in YearTotals
                                                 group p by new { p.DoctorID, p.DoctorName } into g
                                                 select new
                                                 {
                                                     g.Key,
                                                     OrderTotal = g.Sum(p => p.OrderTotal),
                                                     RevenueTotal = g.Sum(p => p.RevenueTotal),
                                                     RefundMoney = g.Sum(p => p.RefundMoney),
                                                     CompleteMoney = g.Sum(p => p.CompleteMoney),
                                                     RefundTotal = g.Sum(p => p.RefundTotal),
                                                     CompleteTotal = g.Sum(p => p.CompleteTotal),
                                                     TotalType = "year",
                                                     TotalDate = YearStartTime,
                                                 };
                    if (ConsultationYearTotals.Count() > 0)
                    {
                        var consulyaehertollist = ConsultationYearTotals.ToList();
                        foreach (var ConsultationYearInfo in consulyaehertollist)
                        {
                            var ConsultationYears = await _repository.GetAll().Where(a => a.DoctorID == ConsultationYearInfo.Key.DoctorID &&
                                                                     a.TotalType == "year" &&
                                                                     a.TotalDate == ConsultationYearInfo.TotalDate).ToListAsync();
                            if (ConsultationYears.Count() > 0)
                            {
                                var ConsultationOrderYears = ConsultationYears.FirstOrDefault();
                                ConsultationOrderYears.DoctorName = ConsultationYearInfo.Key.DoctorName;
                                ConsultationOrderYears.DoctorID = ConsultationYearInfo.Key.DoctorID;
                                //    ConsultationOrderYears.DoctorJSON = "";
                                ConsultationOrderYears.OrderTotal = ConsultationYearInfo.OrderTotal;
                                ConsultationOrderYears.RevenueTotal = ConsultationYearInfo.RevenueTotal;
                                ConsultationOrderYears.RefundMoney = ConsultationYearInfo.RefundMoney;
                                ConsultationOrderYears.CompleteMoney = ConsultationYearInfo.CompleteMoney;
                                ConsultationOrderYears.TotalType = ConsultationYearInfo.TotalType;
                                ConsultationOrderYears.RefundTotal = ConsultationYearInfo.RefundTotal;
                                ConsultationOrderYears.CompleteTotal = ConsultationYearInfo.CompleteTotal;
                                ConsultationOrderYears.TotalDate = ConsultationYearInfo.TotalDate;
                                ConsultationOrderYears.ModifyOn = DateTime.Now;
                                var reyear = await _repository.UpdateAsync(ConsultationOrderYears);
                            }
                            else
                            {
                                ConsultationOrderTotal ConsultationOrderYears = new ConsultationOrderTotal();
                                ConsultationOrderYears.DoctorName = ConsultationYearInfo.Key.DoctorName;
                                ConsultationOrderYears.DoctorID = ConsultationYearInfo.Key.DoctorID;
                                //   ConsultationOrderYears.DoctorJSON = "";
                                ConsultationOrderYears.OrderTotal = ConsultationYearInfo.OrderTotal;
                                ConsultationOrderYears.RevenueTotal = ConsultationYearInfo.RevenueTotal;
                                ConsultationOrderYears.RefundMoney = ConsultationYearInfo.RefundMoney;
                                ConsultationOrderYears.CompleteMoney = ConsultationYearInfo.CompleteMoney;
                                ConsultationOrderYears.TotalType = ConsultationYearInfo.TotalType;
                                ConsultationOrderYears.RefundTotal = ConsultationYearInfo.RefundTotal;
                                ConsultationOrderYears.CompleteTotal = ConsultationYearInfo.CompleteTotal;
                                ConsultationOrderYears.TotalDate = YearStartTime;
                                // await _repository.InsertAsync(ConsultationOrderYears);
                                ConsultationOrderYears.Id = await _repository.InsertAndGetIdAsync(ConsultationOrderYears);
                            }
                        }
                    }
                    #endregion
                    unitOfWork.Complete();
                }
            }
            return "success";
        }
    }
}
