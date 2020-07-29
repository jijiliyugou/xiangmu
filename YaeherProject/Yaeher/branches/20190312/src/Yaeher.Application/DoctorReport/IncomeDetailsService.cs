using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.DoctorReport.Dto;
using System;
using Yaeher.Common;
using Yaeher.Common.TencentCustom;
using Yaeher.SystemConfig;

namespace Yaeher.DoctorReport
{
    /// <summary>
    /// 医生收入明细
    /// </summary>
    public class IncomeDetailsService : IIncomeDetailsService
    {
        private readonly IRepository<IncomeDetails> _repository;
        private readonly IRepository<IncomeDevide> _deviderepository;
        private readonly IRepository<YaeherDoctor> _doctorrepository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<DoctorOnlineRecord> _onlinerepository;
        private readonly IRepository<YaeherConsultation> _consultationrepository;
        private readonly IRepository<OrderManage> _OrderManagerepository;
        private readonly IRepository<DoctorIncome> _DoctorIncomerepository;
        private readonly IRepository<CorporateIncomeTotal> _CorporateIncomeTotalrepository;
        private readonly IRepository<RefundManage> _RefundManagerepository;
        private readonly IRepository<SystemConfigs> _SystemConfigsrepository;
        private readonly IRepository<OrderTradeRecord> _OrderTradeRecordrepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="deviderepository"></param>
        /// <param name="doctorrepository"></param>
        /// <param name="userrepository"></param>
        public IncomeDetailsService(IRepository<IncomeDetails> repository,
            IRepository<IncomeDevide> deviderepository,
            IRepository<YaeherDoctor> doctorrepository,
            IRepository<YaeherUser> userrepository,
             IRepository<DoctorOnlineRecord> onlinerepository,
             IRepository<YaeherConsultation> consultationrepository,
              IRepository<OrderManage> ordermanagerepository,
               IRepository<DoctorIncome> DoctorIncomerepository,
               IRepository<CorporateIncomeTotal> CorporateIncomeTotalrepository,
               IRepository<RefundManage> RefundManagerepository,
                IRepository<SystemConfigs> SystemConfigsrepository,
                IRepository<OrderTradeRecord> OrderTradeRecordrepository)
        {
            _repository = repository;
            _deviderepository = deviderepository;
            _doctorrepository = doctorrepository;
            _userrepository = userrepository;
            _onlinerepository = onlinerepository;
            _consultationrepository = consultationrepository;
            _OrderManagerepository = ordermanagerepository;
            _DoctorIncomerepository = DoctorIncomerepository;
            _CorporateIncomeTotalrepository = CorporateIncomeTotalrepository;
            _RefundManagerepository = RefundManagerepository;
            _SystemConfigsrepository = SystemConfigsrepository;
            _OrderTradeRecordrepository = OrderTradeRecordrepository;
        }

        /// <summary>
        /// 查询医生收入明细 List
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<IncomeDetails>> IncomeDetailsList(IncomeDetailsIn IncomeDetailsInfo)
        {
            var IncomeDetailss = await _repository.GetAllListAsync(IncomeDetailsInfo.Expression);
            return IncomeDetailss.ToList();
        }

        /// <summary>
        /// 查询医生收入明细byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDetails> IncomeDetailsByID(int Id)
        {
            var IncomeDetailss = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return IncomeDetailss;
        }
        /// <summary>
        /// 查询医生收入明细 Page
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<IncomeDetails>> IncomeDetailsPage(IncomeDetailsIn IncomeDetailsInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(IncomeDetailsInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / IncomeDetailsInfo.MaxResultCount;
            var IncomeDetailsList = await query.PageBy(IncomeDetailsInfo.SkipTotal, IncomeDetailsInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<IncomeDetails>(tasksCount, IncomeDetailsList.MapTo<List<IncomeDetails>>());
        }
        /// <summary>
        /// 新建医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDetails> CreateIncomeDetails(IncomeDetails IncomeDetailsInfo)
        {
            IncomeDetailsInfo.Id = await _repository.InsertAndGetIdAsync(IncomeDetailsInfo);
            return IncomeDetailsInfo;
        }
        /// <summary>
        /// 新建医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task TotalIncomeDetails(IncomeDetails IncomeDetailsInfo)
        {
            await _repository.InsertAsync(IncomeDetailsInfo);
        }
        
        /// <summary>
        /// 修改医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDetails> UpdateIncomeDetails(IncomeDetails IncomeDetailsInfo)
        {
            var updater = await _repository.UpdateAsync(IncomeDetailsInfo);
            return updater;
        }

        /// <summary>
        /// 删除医生收入明细
        /// </summary>
        /// <param name="IncomeDetailsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IncomeDetails> DeleteIncomeDetails(IncomeDetails IncomeDetailsInfo)
        {
            var deleter = await _repository.UpdateAsync(IncomeDetailsInfo);
            return deleter;
        }


        /// <summary>
        /// EvaluationTotalAll 汇总统计当前评分汇总
        /// </summary>
        /// <param name="startdateTime">统计开始时间</param>
        /// <param name="enddateTime">统计结束时间</param>
        /// <param name="Ipaddress">ip</param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<string> IncomeTotalAll(DateTime startdateTime, DateTime enddateTime, string Ipaddress)
        {
            var devideTotal = await _deviderepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var devide = devideTotal.Where(t => t.WXSharing == "Open").ToList();
            var doctor = await _doctorrepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var userlist = await _userrepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var online = await _onlinerepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var ConsultationList = await _consultationrepository.GetAll().Where(t => t.IsDelete == false && t.ConsultState == "success").ToListAsync();
            var OrderList = await _OrderManagerepository.GetAll().Where(t => t.IsDelete == false).ToListAsync();
            var OrderTradeList = await _OrderTradeRecordrepository.GetAll().Where(t => t.IsDelete == false).ToListAsync();
            var doctorIncome = await _DoctorIncomerepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var sysIncome = await _CorporateIncomeTotalrepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            //日起止时间
            var enddate = enddateTime;
            var startdate = startdateTime;
            // 检查今日是否执行过job 日起止时间
            //var docincomestartdate = Convert.ToDateTime("2018-12-19");
            //var docincomeenddate = Convert.ToDateTime("2018-12-20").AddSeconds(-1);
            var docincomestartdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var docincomeenddate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")).AddSeconds(-1);

            //月起止时间
            DateTime MonthStartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:01")).AddDays(1 - DateTime.Now.Day);
            DateTime MonthEndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 11:59:59")).AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1);
            //年起止时间
            DateTime YearStartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01 00:00:01"));
            DateTime YearEndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-12-31 11:59:59"));

            var sysdaytotal = 0.00;//公司收入
            var sysdayordertotal = 0.00;//公司订单总金额

            var totaldate = enddateTime;
            //分账配置信息
            var tencentparam = await _SystemConfigsrepository.GetAll().Where(t => !t.IsDelete && t.SystemType == "TencentWechar").FirstOrDefaultAsync();

            //医生收入
            foreach (var item in doctor)
            {
                var userid = item.UserID;var doctorid = item.Id;
                var user = userlist.Where(t => t.Id == userid).FirstOrDefault();
                var doctoronline = online.FirstOrDefault(t => t.DoctorID == doctorid);
                var daydocincomelist = doctorIncome.Where(t => t.IsDelete == false && t.DoctorID == doctorid && t.IncomeTimeType == "day" && t.TotalDate >= docincomestartdate && t.TotalDate <= docincomeenddate).ToList();
                var doctordaytotal = 0.00;
                var devidelist = devide.Where(t => t.DoctorID == doctorid && t.DevideTime >= startdate && t.DevideTime < enddate).ToList();
                if (daydocincomelist == null || daydocincomelist.Count < 1)//新增
                {
                    if (devidelist != null && devidelist.Count > 0)
                    {
                        IncomeDevide devid = new IncomeDevide();
                        for (int j = 0; j < devidelist.Count; j++)
                        {
                            devid = devidelist[j];
                            var yaeherConsultation = ConsultationList.Where(t => t.ConsultNumber == devid.ConsultNumber).FirstOrDefault();
                            if (yaeherConsultation == null) { continue; }//必须为完成状态,没找到不分账
                            var order = OrderList.Where(t => t.ConsultNumber == devid.ConsultNumber).FirstOrDefault();
                            var ordertrade = OrderTradeList.Where(t => t.OrderNumber == order.OrderNumber && t.PayType == "wxpay" && t.PaymentState == "paid" && t.PaymentSourceCode == "order").FirstOrDefault();
                            if (ordertrade != null && ordertrade.Id > 0)
                            {
                                //分账开启
                                var Sahring = "false";
                                SharingResult shareresult = new SharingResult();
                                if (Ipaddress != "http://192.168.2.3:5002/" && item.IsSharing)
                                {
                                    TencentWXPay tencentWXPay = new TencentWXPay();
                                    List<receivershare> receivers = new List<receivershare>();
                                    var receivershare = new receivershare();
                                    receivershare.name = item.DoctorName;
                                    receivershare.type = "PERSONAL_OPENID";
                                    receivershare.account = user.WecharOpenID;
                                    receivershare.amount = Convert.ToInt32(devid.DevideMoney * 100);
                                    receivers.Add(receivershare);
                                    shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, ordertrade.WXTransactionId, ordertrade.WXPayBillno);
                                    Sahring = shareresult.result_code;
                                }
                                else
                                { Sahring = "SUCCESS"; }
                                if (Sahring == "SUCCESS")
                                {
                                    devid.WXSharing = "Complete";
                                    //医生收入
                                    IncomeDetails incomeDetails = new IncomeDetails();
                                    incomeDetails.DoctorName = yaeherConsultation.DoctorName;
                                    incomeDetails.DoctorID = yaeherConsultation.DoctorID;//有doctorid为医生收入
                                    incomeDetails.ConsultID = yaeherConsultation.Id;
                                    incomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    incomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    incomeDetails.OrderNumber = order.OrderNumber;
                                    incomeDetails.OrderCurrency = order.OrderCurrency;
                                    incomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    incomeDetails.ProportionMoney = Convert.ToDouble(devid.DevideMoney);//医生收款
                                    incomeDetails.CreatedOn = DateTime.Now;
                                    incomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAsync(incomeDetails);
                                    doctordaytotal += incomeDetails.ProportionMoney;

                                    sysdayordertotal += incomeDetails.OrderMoney;

                                    //系统收入(未过滤微信手续费,购物券等)没有doctorid为系统收入
                                    IncomeDetails sysincomeDetails = new IncomeDetails();
                                    sysincomeDetails.DoctorName = item.DoctorName;
                                    sysincomeDetails.ConsultID = yaeherConsultation.Id;
                                    sysincomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    sysincomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    sysincomeDetails.OrderNumber = order.OrderNumber;
                                    sysincomeDetails.OrderCurrency = order.OrderCurrency;
                                    sysincomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    var propo = order.OrderMoney - devid.DevideMoney;
                                    var propdouble = Convert.ToDouble(propo.ToString());
                                    sysincomeDetails.ProportionMoney = propdouble;//公司收入
                                    sysdaytotal += sysincomeDetails.ProportionMoney;
                                    sysincomeDetails.CreatedOn = DateTime.Now;
                                    sysincomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAsync(sysincomeDetails);
                                }
                                devid.WXSharingJson = JsonHelper.ToJson(shareresult);
                                devid.ModifyOn = DateTime.Now;
                                await _deviderepository.UpdateAsync(devid);
                            }
                        }
                    }
                    //新增医生统计日总表
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = item.Id;
                    DoctorIncome.DoctorName = item.DoctorName;
                    DoctorIncome.IncomeTimeType = "day";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _DoctorIncomerepository.InsertAsync(DoctorIncome);

                }
                else //修改
                {
                    var daydocincome = daydocincomelist.FirstOrDefault();
                    if (devidelist != null && devidelist.Count > 0)
                    {
                        var devid = new IncomeDevide();
                        for (int j = 0; j < devidelist.Count; j++)
                        {
                             devid = devidelist[j];
                            var yaeherConsultation = ConsultationList.Where(t => t.ConsultNumber == devid.ConsultNumber).FirstOrDefault();
                            if (yaeherConsultation == null) { continue; }//必须为完成状态,没找到不分账
                            var order = OrderList.Where(t => t.ConsultNumber == devid.ConsultNumber).FirstOrDefault();
                            var ordertrade = OrderTradeList.Where(t => t.OrderNumber == order.OrderNumber && t.PayType == "wxpay" && t.PaymentState == "paid" && t.PaymentSourceCode == "order").FirstOrDefault();
                            if (ordertrade != null && ordertrade.Id > 0)
                            {
                                //分账开启
                                var Sahring = "false";
                                SharingResult shareresult = new SharingResult();
                                if (Ipaddress != "http://192.168.2.3:5002/")
                                {
                                    TencentWXPay tencentWXPay = new TencentWXPay();
                                    List<receivershare> receivers = new List<receivershare>();
                                    var receivershare = new receivershare();
                                    receivershare.name = item.DoctorName;
                                    receivershare.type = "PERSONAL_OPENID";
                                    receivershare.account = user.WecharOpenID;
                                    receivershare.amount = Convert.ToInt32(devid.DevideMoney * 100);
                                    receivers.Add(receivershare);
                                    shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, ordertrade.WXTransactionId, ordertrade.WXPayBillno);
                                    Sahring = shareresult.result_code;
                                }
                                else
                                { Sahring = "SUCCESS"; }
                                if (Sahring == "SUCCESS")
                                {
                                    devid.WXSharing = "Complete";
                                    //医生收入
                                    IncomeDetails incomeDetails = new IncomeDetails();
                                    incomeDetails.DoctorName = yaeherConsultation.DoctorName;
                                    incomeDetails.DoctorID = yaeherConsultation.DoctorID;//有doctorid为医生收入
                                    incomeDetails.ConsultID = yaeherConsultation.Id;
                                    incomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    incomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    incomeDetails.OrderNumber = order.OrderNumber;
                                    incomeDetails.OrderCurrency = order.OrderCurrency;
                                    incomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    incomeDetails.ProportionMoney = Convert.ToDouble(devid.DevideMoney);//医生收款
                                    incomeDetails.CreatedOn = DateTime.Now;
                                    incomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAsync(incomeDetails);
                                    doctordaytotal += incomeDetails.ProportionMoney;

                                    sysdayordertotal += incomeDetails.OrderMoney;

                                    //系统收入(未过滤微信手续费,购物券等)没有doctorid为系统收入
                                    IncomeDetails sysincomeDetails = new IncomeDetails();
                                    sysincomeDetails.DoctorName = item.DoctorName;
                                    sysincomeDetails.ConsultID = yaeherConsultation.Id;
                                    sysincomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    sysincomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    sysincomeDetails.OrderNumber = order.OrderNumber;
                                    sysincomeDetails.OrderCurrency = order.OrderCurrency;
                                    sysincomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    var propo = order.OrderMoney - devid.DevideMoney;
                                    var propdouble = Convert.ToDouble(propo.ToString());
                                    sysincomeDetails.ProportionMoney = propdouble;//公司收入
                                    sysdaytotal += sysincomeDetails.ProportionMoney;
                                    sysincomeDetails.CreatedOn = DateTime.Now;
                                    sysincomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAsync(sysincomeDetails);
                                }
                                devid.WXSharingJson = JsonHelper.ToJson(shareresult);
                                await _deviderepository.UpdateAsync(devid);
                            }
                        }
                    }
                    //新增医生统计日总表
                    daydocincome.Total += doctordaytotal;//金额
                    daydocincome.ModifyOn = DateTime.Now;
                    daydocincome.TotalDate = totaldate;
                    await _DoctorIncomerepository.UpdateAsync(daydocincome);
                }

                //月 医生收入
                var monthdocincomelist = doctorIncome.Where(t => t.DoctorID == item.Id && t.IncomeTimeType == "month" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();
                if (monthdocincomelist.Count > 0)
                {
                    var monthincome = monthdocincomelist.FirstOrDefault();
                    var monthdaydocincomelist = doctorIncome.Where(t => t.DoctorID == item.Id && t.IncomeTimeType == "day" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();

                    var monthtotalIenum = from a in monthdaydocincomelist
                                          group a by a.DoctorID into g
                                          select new
                                          {
                                              g.Key,
                                              total = g.Sum(a => a.Total)
                                          };
                    var month = monthtotalIenum.FirstOrDefault();
                    monthincome.Total = month.total;
                    monthincome.TotalDate = totaldate;
                    monthincome.ModifyOn = DateTime.Now;
                    await _DoctorIncomerepository.UpdateAsync(monthincome);
                }
                else
                {
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = item.Id;
                    DoctorIncome.DoctorName = item.DoctorName;
                    DoctorIncome.IncomeTimeType = "month";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _DoctorIncomerepository.InsertAsync(DoctorIncome);
                }
                //年 医生收入
                var yeardocincomelist = doctorIncome.Where(t => t.IsDelete == false && t.DoctorID == item.Id && t.IncomeTimeType == "year" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();
                if (yeardocincomelist.Count > 0)
                {
                    var yearincome = yeardocincomelist.FirstOrDefault();
                    var monthcountdocincomelist = doctorIncome.Where(t => t.DoctorID == item.Id && t.IncomeTimeType == "month" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();

                    var monthtotalIenum = from a in monthcountdocincomelist
                                          group a by a.DoctorID into g
                                          select new
                                          {
                                              g.Key,
                                              total = g.Sum(a => a.Total)
                                          };
                    var month = monthtotalIenum.FirstOrDefault();
                    yearincome.Total = month.total;
                    yearincome.TotalDate = totaldate;
                    yearincome.ModifyOn = DateTime.Now;
                    await _DoctorIncomerepository.UpdateAsync(yearincome);
                }
                else
                {
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = item.Id;
                    DoctorIncome.DoctorName = item.DoctorName;
                    DoctorIncome.IncomeTimeType = "year";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _DoctorIncomerepository.InsertAsync(DoctorIncome);
                }

            }
            //公司收入

            var sysdaydevide = devideTotal.Where(t => !t.IsDelete && t.DevideTime >= startdate && t.DevideTime < enddate).ToList();

            var daysysIncomeList = sysIncome.Where(t => t.IncomeType == "day" && t.TotalDate >= docincomestartdate && t.TotalDate <= docincomeenddate).ToList();
            if (daysysIncomeList == null || daysysIncomeList.Count < 1)
            {
                ////新增公司统计总表
                var CorporateIncomeTotal = new CorporateIncomeTotal();
                CorporateIncomeTotal.IncomeType = "day";
                CorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                CorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                CorporateIncomeTotal.RefundTotalMoney = 0;
                CorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                CorporateIncomeTotal.CreatedOn = DateTime.Now;
                CorporateIncomeTotal.TotalDate = totaldate;
                await _CorporateIncomeTotalrepository.InsertAsync(CorporateIncomeTotal);
            }
            else
            {
                var daysysincome = daysysIncomeList.FirstOrDefault();
                daysysincome.IncomeTotal += Convert.ToDecimal(sysdaytotal);
                daysysincome.OrderTotalMoney += Convert.ToDecimal(sysdayordertotal);
                daysysincome.OrderTotal = sysdaydevide.Count;
                daysysincome.ModifyOn = DateTime.Now;
                await _CorporateIncomeTotalrepository.UpdateAsync(daysysincome);
            }
            #region 新增月数据

            ////月 公司收入

            var monthsysIncomeList = sysIncome.Where(t => t.IncomeType == "month" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();
            if (monthsysIncomeList.Count > 0)
            {
                var sysConsultationmonthlist = ConsultationList.Where(t => t.CreatedOn >= MonthStartTime && t.CreatedOn <= MonthEndTime).ToList();

                var monthsysIncome = monthsysIncomeList.FirstOrDefault();

                var daydocincomelist = sysIncome.Where(t => t.IncomeType == "day" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();

                var daytotalIenum = from a in daydocincomelist
                                    group a by a.IncomeType into g
                                    select new
                                    {
                                        g.Key,
                                        totalmoney = g.Sum(a => a.OrderTotalMoney),
                                        incometotal = g.Sum(a => a.IncomeTotal),
                                        total = g.Sum(a => a.OrderTotal)
                                    };
                var daytotal = daytotalIenum.FirstOrDefault();

                monthsysIncome.IncomeTotal = Convert.ToDecimal(daytotal.incometotal);
                monthsysIncome.OrderTotalMoney = Convert.ToDecimal(daytotal.totalmoney);
                monthsysIncome.OrderTotal = daytotal.total;
                monthsysIncome.TotalDate = totaldate;
                monthsysIncome.ModifyOn = DateTime.Now;

                await _CorporateIncomeTotalrepository.UpdateAsync(monthsysIncome);
            }
            else
            {
                var monthCorporateIncomeTotal = new CorporateIncomeTotal();
                monthCorporateIncomeTotal.IncomeType = "month";
                monthCorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                monthCorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                monthCorporateIncomeTotal.RefundTotalMoney = 0;
                monthCorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                monthCorporateIncomeTotal.CreatedOn = DateTime.Now;
                monthCorporateIncomeTotal.TotalDate = totaldate;
                await _CorporateIncomeTotalrepository.InsertAsync(monthCorporateIncomeTotal);

            }
            //年 公司收入
            var yaersysIncomeList = sysIncome.Where(t => t.IncomeType == "year" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();
            if (yaersysIncomeList.Count > 0)
            {
                var sysConsultationmonthlist = ConsultationList.Where(t => t.CreatedOn >= MonthStartTime && t.CreatedOn <= MonthEndTime).ToList();

                var yearsysIncome = yaersysIncomeList.FirstOrDefault();

                var daydocincomelist = sysIncome.Where(t => t.IncomeType == "month" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();

                var monthtotalIenum = from a in daydocincomelist
                                      group a by a.IncomeType into g
                                      select new
                                      {
                                          g.Key,
                                          totalmoney = g.Sum(a => a.OrderTotalMoney),
                                          incometotal = g.Sum(a => a.IncomeTotal),
                                          total = g.Sum(a => a.OrderTotal)
                                      };
                var month = monthtotalIenum.FirstOrDefault();

                yearsysIncome.IncomeTotal = Convert.ToDecimal(month.incometotal);
                yearsysIncome.OrderTotalMoney = Convert.ToDecimal(month.totalmoney);
                yearsysIncome.OrderTotal = month.total;
                yearsysIncome.TotalDate = totaldate;
                yearsysIncome.ModifyOn = DateTime.Now;

                await _CorporateIncomeTotalrepository.UpdateAsync(yearsysIncome);
            }
            else
            {
                var monthCorporateIncomeTotal = new CorporateIncomeTotal();
                monthCorporateIncomeTotal.IncomeType = "year";
                monthCorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                monthCorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                monthCorporateIncomeTotal.RefundTotalMoney = 0;
                monthCorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                monthCorporateIncomeTotal.CreatedOn = DateTime.Now;
                monthCorporateIncomeTotal.TotalDate = totaldate;
                await _CorporateIncomeTotalrepository.InsertAsync(monthCorporateIncomeTotal);

            }

            #endregion

            return "SUCCESS";
        }
        /// <summary>
        /// EvaluationTotalAll 汇总统计当前评分汇总
        /// </summary>
        /// <param name="startdateTime">统计开始时间</param>
        /// <param name="enddateTime">统计结束时间</param>
        /// <param name="Ipaddress">ip</param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<string> IncomeTotalTestServerAll(DateTime startdateTime, DateTime enddateTime, string Ipaddress)
        {
            var devideTotal = await _deviderepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            //var devide = devideTotal.ToList();
            var devide = devideTotal.Where(t => t.WXSharing == "Open").ToList();
            var doctor = await _doctorrepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var userlist = await _userrepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var online = await _onlinerepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var ConsultationList = await _consultationrepository.GetAll().Where(a => a.IsDelete == false && a.ConsultState == "success").ToListAsync();
            var OrderList = await _OrderManagerepository.GetAll().Where(a => a.IsDelete == false).ToListAsync();
            var OrderTradeList = await _OrderTradeRecordrepository.GetAll().Where(a => a.IsDelete == false).ToListAsync();
            var doctorIncome = await _DoctorIncomerepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var sysIncome = await _CorporateIncomeTotalrepository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            //日起止时间
            var enddate = enddateTime;
            var startdate = startdateTime;
            // 检查今日是否执行过job 日起止时间
            var docincomestartdate = Convert.ToDateTime(enddateTime.ToString("yyyy-MM-dd"));
            var docincomeenddate = Convert.ToDateTime(enddateTime.AddDays(1).ToString("yyyy-MM-dd")).AddSeconds(-1);
            //月起止时间
            DateTime MonthStartTime = DateTime.Parse(enddateTime.ToString("yyyy-MM-dd 00:00:01")).AddDays(1 - enddateTime.Day);
            DateTime MonthEndTime = DateTime.Parse(enddateTime.ToString("yyyy-MM-dd 11:59:59")).AddDays(1 - enddateTime.Day).AddMonths(1).AddDays(-1);
            //年起止时间
            DateTime YearStartTime = DateTime.Parse(enddateTime.ToString("yyyy-01-01 00:00:01"));
            DateTime YearEndTime = DateTime.Parse(enddateTime.ToString("yyyy-12-31 11:59:59"));

            var sysdaytotal = 0.00;//公司收入
            var sysdayordertotal = 0.00;//公司订单总金额

            var totaldate = enddateTime;
            //分账配置信息
            var tencentparam = await _SystemConfigsrepository.GetAll().Where(t => !t.IsDelete && t.SystemType == "TencentWechar").FirstOrDefaultAsync();

            //医生收入
            //foreach (var item in doctor)
            for (int i = 0; i < doctor.Count; i++)
            {
                var item = doctor[i];
                var user = userlist.Where(t => t.Id == item.UserID).FirstOrDefault();
                var doctoronline = online.FirstOrDefault(t => t.DoctorID == item.Id);
                var daydocincomelist = doctorIncome.Where(t => t.IsDelete == false && t.DoctorID == item.Id && t.IncomeTimeType == "day" && t.TotalDate >= docincomestartdate && t.TotalDate <= docincomeenddate).ToList();
                var doctordaytotal = 0.00;
                var devidelist = devide.Where(t => t.DoctorID == item.Id && t.DevideTime >= startdate && t.DevideTime < enddate).ToList();
                if (daydocincomelist == null || daydocincomelist.Count < 1)//新增
                {
                    if (devidelist != null && devidelist.Count > 0)
                    {
                        //  foreach (var devid in devidelist)
                        for (int j = 0; j < devidelist.Count; j++)
                        {
                            var devid = devidelist[j];
                            var yaeherConsultation = ConsultationList.Where(t => t.ConsultNumber == devid.ConsultNumber).FirstOrDefault();
                            var order = OrderList.Where(t => t.ConsultNumber == yaeherConsultation.ConsultNumber).FirstOrDefault();
                            var ordertrade = OrderTradeList.Where(t => t.OrderNumber == order.OrderNumber && t.PayType == "wxpay" && t.PaymentState == "paid" && t.PaymentSourceCode == "order").FirstOrDefault();
                            if (ordertrade != null && ordertrade.Id > 0)
                            {
                                //分账开启
                                var Sahring = "Fail";
                                SharingResult shareresult = new SharingResult();
                                if (Ipaddress != "http://192.168.2.3:5002/")
                                {
                                    TencentWXPay tencentWXPay = new TencentWXPay();
                                    List<receivershare> receivers = new List<receivershare>();
                                    var receivershare = new receivershare();
                                    receivershare.name = item.DoctorName;
                                    receivershare.type = "PERSONAL_OPENID";
                                    receivershare.account = user.WecharOpenID;
                                    receivershare.amount = Convert.ToInt32(devid.DevideMoney * 100);
                                    receivers.Add(receivershare);
                                    shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, ordertrade.WXTransactionId, ordertrade.WXPayBillno);
                                    Sahring = shareresult.result_code;
                                }
                                else
                                { Sahring = "SUCCESS"; }
                                if (Sahring == "SUCCESS")
                                {
                                    devid.WXSharing = "Complete";
                                    //医生收入
                                    IncomeDetails incomeDetails = new IncomeDetails();
                                    incomeDetails.DoctorName = yaeherConsultation.DoctorName;
                                    incomeDetails.DoctorID = yaeherConsultation.DoctorID;//有doctorid为医生收入
                                    incomeDetails.ConsultID = yaeherConsultation.Id;
                                    incomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    incomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    incomeDetails.OrderNumber = order.OrderNumber;
                                    incomeDetails.OrderCurrency = order.OrderCurrency;
                                    incomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    incomeDetails.ProportionMoney = Convert.ToDouble(devid.DevideMoney);//医生收款
                                    incomeDetails.CreatedOn = DateTime.Now;
                                    incomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAndGetIdAsync(incomeDetails);
                                    doctordaytotal += incomeDetails.ProportionMoney;

                                    sysdayordertotal += incomeDetails.OrderMoney;

                                    //系统收入(未过滤微信手续费,购物券等)没有doctorid为系统收入
                                    IncomeDetails sysincomeDetails = new IncomeDetails();
                                    sysincomeDetails.DoctorName = item.DoctorName;
                                    sysincomeDetails.ConsultID = yaeherConsultation.Id;
                                    sysincomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    sysincomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    sysincomeDetails.OrderNumber = order.OrderNumber;
                                    sysincomeDetails.OrderCurrency = order.OrderCurrency;
                                    sysincomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    var propo = order.OrderMoney - devid.DevideMoney;
                                    var propdouble = Convert.ToDouble(propo.ToString());
                                    sysincomeDetails.ProportionMoney = propdouble;//公司收入
                                    sysdaytotal += sysincomeDetails.ProportionMoney;
                                    sysincomeDetails.CreatedOn = DateTime.Now;
                                    sysincomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAndGetIdAsync(sysincomeDetails);
                                }
                                devid.WXSharingJson = JsonHelper.ToJson(shareresult);
                                devid.ModifyOn = enddate;
                                await _deviderepository.UpdateAsync(devid);
                            }
                        }
                    }
                    //新增医生统计日总表
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = item.Id;
                    DoctorIncome.DoctorName = item.DoctorName;
                    DoctorIncome.IncomeTimeType = "day";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _DoctorIncomerepository.InsertAndGetIdAsync(DoctorIncome);

                }
                else //修改
                {
                    var daydocincome = daydocincomelist.FirstOrDefault();
                    if (devidelist != null && devidelist.Count > 0)
                    {
                        // foreach (var devid in devidelist)
                        for (int j = 0; j < devidelist.Count; j++)
                        {
                            var devid = devidelist[j];
                            var yaeherConsultation = ConsultationList.Where(t => t.ConsultNumber == devid.ConsultNumber).FirstOrDefault();
                            var order = OrderList.Where(t => t.ConsultNumber == yaeherConsultation.ConsultNumber).FirstOrDefault();
                            var ordertrade = OrderTradeList.Where(t => t.OrderNumber == order.OrderNumber && t.PayType == "wxpay" && t.PaymentState == "paid" && t.PaymentSourceCode == "order").FirstOrDefault();
                            if (ordertrade != null && ordertrade.Id > 0)
                            {
                                //分账开启
                                var Sahring = "Fail";
                                SharingResult shareresult = new SharingResult();
                                if (Ipaddress != "http://192.168.2.3:5002/")
                                {
                                    TencentWXPay tencentWXPay = new TencentWXPay();
                                    List<receivershare> receivers = new List<receivershare>();
                                    var receivershare = new receivershare();
                                    receivershare.name = item.DoctorName;
                                    receivershare.type = "PERSONAL_OPENID";
                                    receivershare.account = user.WecharOpenID;
                                    receivershare.amount = Convert.ToInt32(devid.DevideMoney * 100);
                                    receivers.Add(receivershare);
                                    shareresult = await tencentWXPay.ProfitSharing(receivers, tencentparam, ordertrade.WXTransactionId, ordertrade.WXPayBillno);
                                    Sahring = shareresult.result_code;
                                }
                                else
                                { Sahring = "SUCCESS"; }
                                if (Sahring == "SUCCESS")
                                {
                                    devid.WXSharing = "Complete";
                                    //医生收入
                                    IncomeDetails incomeDetails = new IncomeDetails();
                                    incomeDetails.DoctorName = yaeherConsultation.DoctorName;
                                    incomeDetails.DoctorID = yaeherConsultation.DoctorID;//有doctorid为医生收入
                                    incomeDetails.ConsultID = yaeherConsultation.Id;
                                    incomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    incomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    incomeDetails.OrderNumber = order.OrderNumber;
                                    incomeDetails.OrderCurrency = order.OrderCurrency;
                                    incomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    incomeDetails.ProportionMoney = Convert.ToDouble(devid.DevideMoney);//医生收款
                                    incomeDetails.CreatedOn = DateTime.Now;
                                    incomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAndGetIdAsync(incomeDetails);
                                    doctordaytotal += incomeDetails.ProportionMoney;

                                    sysdayordertotal += incomeDetails.OrderMoney;

                                    //系统收入(未过滤微信手续费,购物券等)没有doctorid为系统收入
                                    IncomeDetails sysincomeDetails = new IncomeDetails();
                                    sysincomeDetails.DoctorName = item.DoctorName;
                                    sysincomeDetails.ConsultID = yaeherConsultation.Id;
                                    sysincomeDetails.ConsultNumber = yaeherConsultation.ConsultNumber;
                                    sysincomeDetails.ConsultJSON = JsonHelper.ToJson(yaeherConsultation);
                                    sysincomeDetails.OrderNumber = order.OrderNumber;
                                    sysincomeDetails.OrderCurrency = order.OrderCurrency;
                                    sysincomeDetails.OrderMoney = Convert.ToDouble(order.OrderMoney);
                                    var propo = order.OrderMoney - devid.DevideMoney;
                                    var propdouble = Convert.ToDouble(propo.ToString());
                                    sysincomeDetails.ProportionMoney = propdouble;//公司收入
                                    sysdaytotal += sysincomeDetails.ProportionMoney;
                                    sysincomeDetails.CreatedOn = DateTime.Now;
                                    sysincomeDetails.TotalDate = totaldate;
                                    await _repository.InsertAndGetIdAsync(sysincomeDetails);
                                }
                                devid.WXSharingJson = JsonHelper.ToJson(shareresult);
                                devid.ModifyOn = enddate;
                                await _deviderepository.UpdateAsync(devid);
                            }
                        }
                    }
                    //新增医生统计日总表
                    daydocincome.Total += doctordaytotal;//金额
                    daydocincome.ModifyOn = DateTime.Now;
                    daydocincome.TotalDate = totaldate;
                    await _DoctorIncomerepository.UpdateAsync(daydocincome);
                }

                //月 医生收入
                var monthdocincomelist = doctorIncome.Where(t => t.DoctorID == item.Id && t.IncomeTimeType == "month" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();
                if (monthdocincomelist.Count > 0)
                {
                    var monthincome = monthdocincomelist.FirstOrDefault();
                    var monthdaydocincomelist = doctorIncome.Where(t => t.DoctorID == item.Id && t.IncomeTimeType == "day" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();

                    var monthtotalIenum = from a in monthdaydocincomelist
                                          group a by a.DoctorID into g
                                          select new
                                          {
                                              g.Key,
                                              total = g.Sum(a => a.Total)
                                          };
                    var month = monthtotalIenum.FirstOrDefault();
                    monthincome.Total = month.total;
                    monthincome.TotalDate = totaldate;
                    monthincome.ModifyOn = DateTime.Now;
                    await _DoctorIncomerepository.UpdateAsync(monthincome);
                }
                else
                {
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = item.Id;
                    DoctorIncome.DoctorName = item.DoctorName;
                    DoctorIncome.IncomeTimeType = "month";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _DoctorIncomerepository.InsertAndGetIdAsync(DoctorIncome);
                }
                //年 医生收入
                var yeardocincomelist = doctorIncome.Where(t => t.IsDelete == false && t.DoctorID == item.Id && t.IncomeTimeType == "year" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();
                if (yeardocincomelist.Count > 0)
                {
                    var yearincome = yeardocincomelist.FirstOrDefault();
                    var monthcountdocincomelist = doctorIncome.Where(t => t.DoctorID == item.Id && t.IncomeTimeType == "month" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();

                    var monthtotalIenum = from a in monthcountdocincomelist
                                          group a by a.DoctorID into g
                                          select new
                                          {
                                              g.Key,
                                              total = g.Sum(a => a.Total)
                                          };
                    var month = monthtotalIenum.FirstOrDefault();
                    yearincome.Total = month.total;
                    yearincome.TotalDate = totaldate;
                    yearincome.ModifyOn = DateTime.Now;
                    await _DoctorIncomerepository.UpdateAsync(yearincome);
                }
                else
                {
                    var DoctorIncome = new DoctorIncome();
                    DoctorIncome.DoctorID = item.Id;
                    DoctorIncome.DoctorName = item.DoctorName;
                    DoctorIncome.IncomeTimeType = "year";
                    DoctorIncome.Total = doctordaytotal;
                    DoctorIncome.CreatedOn = DateTime.Now;
                    DoctorIncome.TotalDate = totaldate;
                    await _DoctorIncomerepository.InsertAndGetIdAsync(DoctorIncome);
                }

            }
            //公司收入

            var sysdaydevide = devideTotal.Where(t => !t.IsDelete && t.DevideTime >= startdate && t.DevideTime < enddate).ToList();

            var daysysIncomeList = sysIncome.Where(t => t.IncomeType == "day" && t.TotalDate >= docincomestartdate && t.TotalDate <= docincomeenddate).ToList();
            if (daysysIncomeList == null || daysysIncomeList.Count < 1)
            {
                ////新增公司统计总表
                var CorporateIncomeTotal = new CorporateIncomeTotal();
                CorporateIncomeTotal.IncomeType = "day";
                CorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                CorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                CorporateIncomeTotal.RefundTotalMoney = 0;
                CorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                CorporateIncomeTotal.CreatedOn = DateTime.Now;
                CorporateIncomeTotal.TotalDate = totaldate;
                await _CorporateIncomeTotalrepository.InsertAndGetIdAsync(CorporateIncomeTotal);
            }
            else
            {
                var daysysincome = daysysIncomeList.FirstOrDefault();
                daysysincome.IncomeTotal += Convert.ToDecimal(sysdaytotal);
                daysysincome.OrderTotalMoney += Convert.ToDecimal(sysdayordertotal);
                daysysincome.OrderTotal = sysdaydevide.Count;
                daysysincome.ModifyOn = DateTime.Now;
                await _CorporateIncomeTotalrepository.UpdateAsync(daysysincome);
            }
            #region 新增月数据

            ////月 公司收入

            var monthsysIncomeList = sysIncome.Where(t => t.IncomeType == "month" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();
            if (monthsysIncomeList.Count > 0)
            {
                var sysConsultationmonthlist = ConsultationList.Where(t => t.CreatedOn >= MonthStartTime && t.CreatedOn <= MonthEndTime).ToList();

                var monthsysIncome = monthsysIncomeList.FirstOrDefault();

                var daydocincomelist = sysIncome.Where(t => t.IncomeType == "day" && t.TotalDate >= MonthStartTime && t.TotalDate <= MonthEndTime).ToList();

                var daytotalIenum = from a in daydocincomelist
                                    group a by a.IncomeType into g
                                    select new
                                    {
                                        g.Key,
                                        totalmoney = g.Sum(a => a.OrderTotalMoney),
                                        incometotal = g.Sum(a => a.IncomeTotal),
                                        total = g.Sum(a => a.OrderTotal)
                                    };
                var daytotal = daytotalIenum.FirstOrDefault();

                monthsysIncome.IncomeTotal = Convert.ToDecimal(daytotal.incometotal);
                monthsysIncome.OrderTotalMoney = Convert.ToDecimal(daytotal.totalmoney);
                monthsysIncome.OrderTotal = daytotal.total;
                monthsysIncome.TotalDate = totaldate;
                monthsysIncome.ModifyOn = DateTime.Now;

                await _CorporateIncomeTotalrepository.UpdateAsync(monthsysIncome);
            }
            else
            {
                var monthCorporateIncomeTotal = new CorporateIncomeTotal();
                monthCorporateIncomeTotal.IncomeType = "month";
                monthCorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                monthCorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                monthCorporateIncomeTotal.RefundTotalMoney = 0;
                monthCorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                monthCorporateIncomeTotal.CreatedOn = DateTime.Now;
                monthCorporateIncomeTotal.TotalDate = totaldate;
                await _CorporateIncomeTotalrepository.InsertAndGetIdAsync(monthCorporateIncomeTotal);

            }
            //年 公司收入
            var yaersysIncomeList = sysIncome.Where(t => t.IncomeType == "year" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();
            if (yaersysIncomeList.Count > 0)
            {
                var sysConsultationmonthlist = ConsultationList.Where(t => t.CreatedOn >= MonthStartTime && t.CreatedOn <= MonthEndTime).ToList();

                var yearsysIncome = yaersysIncomeList.FirstOrDefault();

                var daydocincomelist = sysIncome.Where(t => t.IncomeType == "month" && t.TotalDate >= YearStartTime && t.TotalDate <= YearEndTime).ToList();

                var monthtotalIenum = from a in daydocincomelist
                                      group a by a.IncomeType into g
                                      select new
                                      {
                                          g.Key,
                                          totalmoney = g.Sum(a => a.OrderTotalMoney),
                                          incometotal = g.Sum(a => a.IncomeTotal),
                                          total = g.Sum(a => a.OrderTotal)
                                      };
                var month = monthtotalIenum.FirstOrDefault();

                yearsysIncome.IncomeTotal = Convert.ToDecimal(month.incometotal);
                yearsysIncome.OrderTotalMoney = Convert.ToDecimal(month.totalmoney);
                yearsysIncome.OrderTotal = month.total;
                yearsysIncome.TotalDate = totaldate;
                yearsysIncome.ModifyOn = DateTime.Now;

                await _CorporateIncomeTotalrepository.UpdateAsync(yearsysIncome);
            }
            else
            {
                var monthCorporateIncomeTotal = new CorporateIncomeTotal();
                monthCorporateIncomeTotal.IncomeType = "year";
                monthCorporateIncomeTotal.IncomeTotal = Convert.ToDecimal(sysdaytotal);
                monthCorporateIncomeTotal.OrderTotalMoney = Convert.ToDecimal(sysdayordertotal);
                monthCorporateIncomeTotal.RefundTotalMoney = 0;
                monthCorporateIncomeTotal.OrderTotal = sysdaydevide.Count;
                monthCorporateIncomeTotal.CreatedOn = DateTime.Now;
                monthCorporateIncomeTotal.TotalDate = totaldate;
                await _CorporateIncomeTotalrepository.InsertAndGetIdAsync(monthCorporateIncomeTotal);

            }

            #endregion

            return "SUCCESS";
        }
    }
}
