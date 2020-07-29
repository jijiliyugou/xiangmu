using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yaeher.NumericalStatement.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsultationOrderTotalOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ConsultationOrderTotalDto"></param>
        /// <param name="ConsultationOrderTotalInfo"></param>
        public ConsultationOrderTotalOut(PagedResultDto<ConsultationOrderTotal> ConsultationOrderTotalDto, ConsultationOrderTotalIn ConsultationOrderTotalInfo)
        {
            Items = ConsultationOrderTotalDto.Items;
            TotalCount = ConsultationOrderTotalDto.TotalCount;
            TotalPage = ConsultationOrderTotalDto.TotalCount / ConsultationOrderTotalInfo.MaxResultCount;
            SkipCount = ConsultationOrderTotalInfo.SkipCount;
            MaxResultCount = ConsultationOrderTotalInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ConsultationOrderTotal> Items { get; set; }
    }
    /// <summary>
    /// 我的收入
    /// </summary>
    public class ConsultationOrderTotalOutDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public ConsultationOrderTotalOutDetail(ConsultationOrderTotal total,double _refundsum, int revenueRanking, int orderRanking, string revenuePoint, string orderPoint, List<ConsultationOrderTotal> ordertotallist,List<OrderTradeRecord> orderTradeRecords)
        {
           // aRevenueTotal = total.RevenueTotal;
           // bRevenueTotal = _refundsum;
            CompleteMoney = (Math.Round((total.CompleteMoney+_refundsum),1)).ToString();
            CompleteTotal  = (total.CompleteTotal-total.RefundTotal).ToString();
            RevenueRanking = revenueRanking;
            OrderRanking = orderRanking;
            RevenuePoint = revenuePoint;
            OrderPoint = orderPoint;
            OrderTradeRecords = orderTradeRecords;
            Ordertotallist = ordertotallist.Select(t => new ConsultationOrderTotalModel(t,orderTradeRecords)).ToList();
        }
        public ConsultationOrderTotalOutDetail(ConsultationOrderTotal total,  int revenueRanking, int orderRanking, string revenuePoint, string orderPoint, List<ConsultationOrderTotal> ordertotallist )
        {
            // aRevenueTotal = total.RevenueTotal;
            // bRevenueTotal = _refundsum;
            CompleteMoney= (Math.Round((total.CompleteMoney), 1)).ToString();
            CompleteTotal  = total.CompleteTotal.ToString();
            RevenueRanking = revenueRanking;
            OrderRanking = orderRanking;
            RevenuePoint = revenuePoint;
            OrderPoint = orderPoint;
            Ordertotallist = ordertotallist.Select(t => new ConsultationOrderTotalModel(t)).ToList();

        }
        //public double aRevenueTotal { get; set; }
        //public double bRevenueTotal { get; set; }
        public List<OrderTradeRecord> OrderTradeRecords { get; set; }
        /// <summary>
        /// list结果集
        /// </summary>
        public List<ConsultationOrderTotalModel> Ordertotallist { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public string CompleteMoney { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public string CompleteTotal  { get; set; }
        /// <summary>
        /// 收入排名
        /// </summary>
        public int RevenueRanking { get; set; }
        /// <summary>
        /// 订单排名
        /// </summary>
        public int OrderRanking { get; set; }
        /// <summary>
        /// 收入百分比
        /// </summary>
        public string RevenuePoint { get; set; }
        /// <summary>
        /// 订单百分比
        /// </summary>
        public string OrderPoint { get; set; }
    }
    /// <summary>
    /// 详情
    /// </summary>
    public class ConsultationOrderTotalModel: ConsultationOrderTotal
    {
        public ConsultationOrderTotalModel(ConsultationOrderTotal total)
        {
            TotalDate = total.TotalDate;

            //var doctorrefund = orderTradeRecords.Where(t => t.CreatedOn >= TotalDate && t.CreatedOn < TotalDate.AddDays(1));
            Id = total.Id;
            DoctorName = total.DoctorName;
            DoctorID = total.DoctorID;
            DoctorJSON = total.DoctorJSON;
            CompleteTotal = total.CompleteTotal ;
            //   aRevenueTotal = total.RevenueTotal;
            //   bPayMoney =Convert.ToDouble(doctorrefund.Sum(t=>t.PayMoney));
            //   cRevenueTotal = aRevenueTotal + bPayMoney;
            CompleteMoney = Math.Round(total.CompleteMoney,1);
            TotalType = total.TotalType;
            TotalDateUtc = total.TotalDate.ToString("yyyy-MM-ddTHH:mm:ss");
        }
        /// <summary>
        /// 
        /// </summary>
        public ConsultationOrderTotalModel(ConsultationOrderTotal total,List<OrderTradeRecord> orderTradeRecords)
        {
            TotalDate = total.TotalDate;

            var doctorrefund = orderTradeRecords.Where(t=>t.CreatedOn>=TotalDate&&t.CreatedOn<TotalDate.AddDays(1));
            Id = total.Id;
            DoctorName = total.DoctorName;
            DoctorID = total.DoctorID;
            DoctorJSON = total.DoctorJSON;
            OrderTotal = total.OrderTotal-doctorrefund.Count();
         //   aRevenueTotal = total.RevenueTotal;
         //   bPayMoney =Convert.ToDouble(doctorrefund.Sum(t=>t.PayMoney));
         //   cRevenueTotal = aRevenueTotal + bPayMoney;
            RevenueTotal = Math.Round((total.RevenueTotal+Convert.ToDouble(doctorrefund.Sum(t=>t.PayMoney))),1);
            TotalType = total.TotalType;
            TotalDateUtc = total.TotalDate.ToString("yyyy-MM-ddTHH:mm:ss");
        }
        /// <summary>
        /// 时间
        /// </summary>
        public string TotalDateUtc { get; set; }
        //public double aRevenueTotal { get; set; }
        //public double bPayMoney { get; set; }
        // public double cRevenueTotal { get; set; }
    }
    /// <summary>
    /// 管理端订单统计
    /// </summary>
    public class OrderTotalReport
    {
        /// <summary>
        /// 管理端订单统计
        /// </summary>
        public OrderTotalReport(int a, double b, int c, double d, string e,string f)
        {
            RefundTotal = a;
            OrderTotal = b;
            CompleteTotal = c;
            NotCompleteTotal = d;
            RevenueTotal = e;
            AverageMoney = f;
        }
        /// <summary>
        /// 退单数
        /// </summary>
        public int RefundTotal { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public double OrderTotal { get; set; }
        /// <summary>
        /// 完成数
        /// </summary>
        public int CompleteTotal { get; set; }
        /// <summary>
        /// 未完成数
        /// </summary>
        public double NotCompleteTotal { get; set; }
        /// <summary>
        /// 订单总价格
        /// </summary>
        public string RevenueTotal { get; set; }
        /// <summary>
        /// 平均价
        /// </summary>
        public string AverageMoney { get; set; }

    }
    /// <summary>
    /// 管理端流量统计
    /// </summary>
    public class UserReport
    {
        /// <summary>
        /// 管理端流量统计
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <param name="h"></param>
        public UserReport(int a, int b, int c, int d , int e, int g, string h)
        {
            TotalUser = a;
            NewUser = b;
            PaidUser = c;
            NewPaidUser = d;
            NewDoctor = e;
            NewRepurchaseCount = g;
            NewRepurchaserate = h;
        }
        /// <summary>
        /// 总用户数
        /// </summary>
        public int TotalUser  { get; set; }
        /// <summary>
        /// 新增用户数
        /// </summary>
        public int NewUser { get; set; }
        /// <summary>
        /// 付费用户数
        /// </summary>
        public int PaidUser { get; set; }
        /// <summary>
        /// 新增付费用户数
        /// </summary>
        public int NewPaidUser { get; set; }
        /// <summary>
        /// 医生
        /// </summary>
        public int NewDoctor { get; set; }
        /// <summary>
        /// 复购数
        /// </summary>
        public int NewRepurchaseCount { get; set; }
        /// <summary>
        /// 复购率
        /// </summary>
        public string NewRepurchaserate { get; set; }
    }
    /// <summary>
    ///管理 收入头部
    /// </summary>
    public class IncomeTotal: CorporateIncomeTotal
    {
        public IncomeTotal()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="total"></param>
        public IncomeTotal(CorporateIncomeTotal total)
        {
            IncomeType = total.IncomeType;//类型
            IncomeTotal = total.IncomeTotal;//公司进账
            OrderTotalMoney = total.OrderTotalMoney;//订单完成金额
            RefundTotalMoney = total.RefundTotalMoney;//退单金额
            OrderTotal = total.OrderTotal;//订单数
            CreatedOn = total.CreatedOn;//时间
            IncomeWater = Math.Round(total.OrderTotalMoney + total.RefundTotalMoney,2);//流水 可能为负数
        }
        /// <summary>
        /// 收入流水
        /// </summary>
        public decimal IncomeWater { get; set;}

    }
    /// <summary>
    /// 管理 收入明细
    /// </summary>
    public class AdminIncomeDetail:YaeherConsultation
    {
        /// <summary>
        /// 订单单号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 订单币别
        /// </summary>
        public string OrderCurrency { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public double OrderMoney { get; set; }
        /// <summary>
        /// 分成金额
        /// </summary>
        public double ProportionMoney { get; set; }
        /// <summary>
        /// 咨询ID
        /// </summary>
        public int ConsultID { get; set; }
    }
}
