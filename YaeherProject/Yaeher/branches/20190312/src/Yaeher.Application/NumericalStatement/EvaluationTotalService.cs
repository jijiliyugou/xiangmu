using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.NumericalStatement.Dto;

namespace Yaeher.NumericalStatement
{
    /// <summary>
    /// 评分汇总表
    /// </summary>
    public class EvaluationTotalService : IEvaluationTotalService
    {
        private readonly IRepository<EvaluationTotal> _repository;
        private readonly IRepository<YaeherConsultation> _YaeherConsultationrepository;
        private readonly IRepository<OrderManage> _OrderManagerepository;
        private readonly IRepository<ConsultationEvaluation> _Evaluationrepository;
        private readonly IRepository<ConsultationReply> _Replyrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="YaeherConsultationrepository"></param>
        /// <param name="OrderManagerepository"></param>
        /// <param name="Evaluationrepository"></param>
        /// <param name="Replyrepository"></param>
        /// <param name="unitOfWorkManager"></param>
        public EvaluationTotalService(IRepository<EvaluationTotal> repository,
                                      IRepository<YaeherConsultation> YaeherConsultationrepository,
                                      IRepository<OrderManage> OrderManagerepository,
                                      IRepository<ConsultationEvaluation> Evaluationrepository,
                                      IRepository<ConsultationReply> Replyrepository,
                                      IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _YaeherConsultationrepository = YaeherConsultationrepository;
            _OrderManagerepository = OrderManagerepository;
            _Evaluationrepository = Evaluationrepository;
            _Replyrepository = Replyrepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 评分汇总 List
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<EvaluationTotal>> EvaluationTotalList(EvaluationTotalIn EvaluationTotalInfo)
        {
            var EvaluationTotals = _repository.GetAll().OrderByDescending(t => t.CreatedOn).Where(EvaluationTotalInfo.Expression);
            return await EvaluationTotals.ToListAsync();
        }

        /// <summary>
        /// 评分汇总byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<EvaluationTotal> EvaluationTotalByID(int Id)
        {
            var EvaluationTotals = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return EvaluationTotals;
        }
        /// <summary>
        /// 评分汇总 page
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<EvaluationTotal>> EvaluationTotalPage(EvaluationTotalIn EvaluationTotalInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(EvaluationTotalInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / EvaluationTotalInfo.MaxResultCount;
            var EvaluationTotalList = await query.PageBy(EvaluationTotalInfo.SkipTotal, EvaluationTotalInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<EvaluationTotal>(tasksCount, EvaluationTotalList.MapTo<List<EvaluationTotal>>());
        }
        /// <summary>
        /// 新建评分汇总
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<EvaluationTotal> CreateEvaluationTotal(EvaluationTotal EvaluationTotalInfo)
        {
            EvaluationTotalInfo.Id = await _repository.InsertAndGetIdAsync(EvaluationTotalInfo);
            return EvaluationTotalInfo;
        }

        /// <summary>
        /// 修改评分汇总
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<EvaluationTotal> UpdateEvaluationTotal(EvaluationTotal EvaluationTotalInfo)
        {
            return await _repository.UpdateAsync(EvaluationTotalInfo);
        }

        /// <summary>
        /// 删除评分汇总
        /// </summary>
        /// <param name="EvaluationTotalInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<EvaluationTotal> DeleteEvaluationTotal(EvaluationTotal EvaluationTotalInfo)
        {
            return await _repository.UpdateAsync(EvaluationTotalInfo);
        }

        /// <summary>
        /// EvaluationTotalAll 汇总统计当前评分汇总
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<string> EvaluationTotalAll()
        {
            var ConsultationList = _YaeherConsultationrepository.GetAll().Where(a => a.IsDelete == false);
            var OrderList = _OrderManagerepository.GetAll().Where(a => a.IsDelete == false);
            var EvaluationList = _Evaluationrepository.GetAll().Where(a => a.IsDelete == false);
            //var ReplyList = await _Replyrepository.GetAll().Where(a => a.ReplyType == "answer"&&a.IsDelete == false).ToListAsync();

            DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime EndTime = StartTime.AddDays(+1);
            var evatotal = await _repository.GetAll().Where(a => a.CreatedOn >= StartTime && a.CreatedOn < EndTime).ToListAsync();

            var EvaluationTotalInfoquery = from a in ConsultationList
                                           join b in OrderList on a.ConsultNumber equals b.ConsultNumber
                                           join c in EvaluationList on a.ConsultNumber equals c.ConsultNumber into temp
                                           from tt in temp.DefaultIfEmpty()
                                           select new EvaluationTotalInfo
                                           {
                                               ConsultNumber = a.ConsultNumber,
                                               DoctorName = a.DoctorName,
                                               DoctorID = a.DoctorID,
                                         //      DoctorJSON = "",
                                               Evaluate = tt == null ? 0 : tt.QualityLevel,
                                               AnswerTime = 0,
                                               RefundNumber = a.RefundNumber,
                                               CreatedOn = a.CreatedOn,
                                               RefundTime = a.RefundTime,
                                               Completetime = a.Completetime,
                                               OrderCurrency = b.OrderCurrency,
                                               OrderMoney = b.OrderMoney,
                                           };
            // EvaluationTotalInfo = await EvaluationTotalInfoquery.ToListAsync();
            var EvaluationTotalInfo = await EvaluationTotalInfoquery.ToListAsync();
            if (EvaluationTotalInfo.Count() > 0)
            {
                //foreach (var EvaluationInfo in EvaluationTotalInfo)
                //for (int i = 0; i < EvaluationTotalInfo.Count; i++)
                //{
                //    var EvaluationInfo = EvaluationTotalInfo[i];
                //    var ReplyInfo = ReplyList.Where(a => a.ConsultNumber == EvaluationInfo.ConsultNumber).OrderBy(a => a.CreatedOn).FirstOrDefault();
                //    if (ReplyInfo != null)
                //    {
                //        EvaluationInfo.AnswerTime = (ReplyInfo.CreatedOn - EvaluationInfo.CreatedOn).TotalSeconds;
                //    }
                //    else
                //    {
                //        if (EvaluationInfo.RefundTime.Year < 1900)
                //        {
                //            EvaluationInfo.AnswerTime = (DateTime.Now - EvaluationInfo.CreatedOn).TotalSeconds;
                //        }
                //        else
                //        {
                //            EvaluationInfo.AnswerTime = (EvaluationInfo.RefundTime - EvaluationInfo.CreatedOn).TotalSeconds;
                //        }
                //    }
                //    //EvaluationInfo.Evaluate = 0;
                //    //if (EvaluationList.Count > 0)
                //    //{
                //    //    var Evaluate = EvaluationList.Where(a => a.ConsultNumber == EvaluationInfo.ConsultNumber).FirstOrDefault();
                //    //    if (Evaluate != null)
                //    //    {
                //    //        EvaluationInfo.Evaluate = Evaluate.QualityLevel;
                //    //    }
                //    //}
                //}
                var EvaliationList = from a in EvaluationTotalInfo
                                     group a by new { a.DoctorID, a.DoctorName } into b
                                     select new
                                     {
                                         b.Key,
                                         EvaluateTotal = b.Sum(a => a.Evaluate),
                                         AnswerTime = b.Sum(a => a.AnswerTime),
                                         RevenueTotal = b.Sum(a => a.OrderMoney),
                                         OrderTotal = b.Count(),
                                     };
                #region 星级统计
                var EvaliationOneStar = from a in EvaluationTotalInfo
                                        where a.Evaluate == 1
                                        group a by new { a.DoctorID, a.DoctorName } into b
                                        select new
                                        {
                                            b.Key,
                                            OneStar = b.Count(),
                                        };
                var EvaliationTwoStar = from a in EvaluationTotalInfo
                                        where a.Evaluate == 2
                                        group a by new { a.DoctorID, a.DoctorName } into b
                                        select new
                                        {
                                            b.Key,
                                            TwoStar = b.Count(),
                                        };
                var EvaliationThreeStar = from a in EvaluationTotalInfo
                                          where a.Evaluate == 3
                                          group a by new { a.DoctorID, a.DoctorName } into b
                                          select new
                                          {
                                              b.Key,
                                              ThreeStar = b.Count(),
                                          };
                var EvaliationFourStar = from a in EvaluationTotalInfo
                                         where a.Evaluate == 4
                                         group a by new { a.DoctorID, a.DoctorName } into b
                                         select new
                                         {
                                             b.Key,
                                             FourStar = b.Count(),
                                         };
                var EvaliationFiveStar = from a in EvaluationTotalInfo
                                         where a.Evaluate == 5
                                         group a by new { a.DoctorID, a.DoctorName } into b
                                         select new
                                         {
                                             b.Key,
                                             FiveStar = b.Count(),
                                         };
                #endregion

                #region 退单数 完成数
                var RefundTotal = from p in EvaluationTotalInfo
                                  where p.RefundTime.Year > 1900
                                  group p by new { p.DoctorID, p.DoctorName } into g
                                  select new
                                  {
                                      g.Key,
                                      RefundTotal = g.Count()
                                  };
                var CompleteTotal = from p in EvaluationTotalInfo
                                    where p.Completetime.Year > 1900
                                    group p by new { p.DoctorID, p.DoctorName } into g
                                    select new
                                    {
                                        g.Key,
                                        CompleteTotal = g.Count()
                                    };
                #endregion

                EvaliationList = EvaliationList.ToList();
                //  foreach (var EvaliationInfo in EvaliationList)
                var evalist = EvaliationList.ToList();
                for (int i = 0; i < evalist.Count; i++)
                {
                    var EvaliationInfo = evalist[i];
                    EvaluationTotal evaluationTotal = new EvaluationTotal();
                    evaluationTotal.DoctorName = EvaliationInfo.Key.DoctorName;
                    evaluationTotal.DoctorID = EvaliationInfo.Key.DoctorID;
                    //evaluationTotal.DoctorJSON = "";
                    evaluationTotal.EvaluateTotal = EvaliationInfo.EvaluateTotal;
                    #region 1星总单数
                    if (EvaliationOneStar.Count() > 0)
                    {
                        var OneStar = EvaliationOneStar.Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).ToList().FirstOrDefault();
                        if (OneStar != null)
                        {
                            evaluationTotal.OneStar = OneStar.OneStar;
                        }
                        else
                        {
                            evaluationTotal.OneStar = 0;
                        }
                    }
                    else
                    {
                        evaluationTotal.OneStar = 0;
                    }
                    #endregion

                    #region 2星总单数
                    if (EvaliationTwoStar.Count() > 0)
                    {
                        var TwoStar = EvaliationTwoStar.Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).ToList().FirstOrDefault();
                        if (TwoStar != null)
                        {
                            evaluationTotal.TwoStar = TwoStar.TwoStar;
                        }
                        else
                        {
                            evaluationTotal.TwoStar = 0;
                        }
                    }
                    else
                    {
                        evaluationTotal.TwoStar = 0;
                    }
                    #endregion

                    #region 3星总单数
                    if (EvaliationThreeStar.Count() > 0)
                    {
                        var ThreeStar = EvaliationThreeStar.Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).ToList().FirstOrDefault();
                        if (ThreeStar != null)
                        {
                            evaluationTotal.ThreeStar = ThreeStar.ThreeStar;
                        }
                        else
                        {
                            evaluationTotal.ThreeStar = 0;
                        }
                    }
                    else
                    {
                        evaluationTotal.ThreeStar = 0;
                    }
                    #endregion

                    #region 4星总单数
                    if (EvaliationFourStar.Count() > 0)
                    {
                        var FourStar = EvaliationFourStar.Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).ToList().FirstOrDefault();
                        if (FourStar != null)
                        {
                            evaluationTotal.FourStar = FourStar.FourStar;
                        }
                        else
                        {
                            evaluationTotal.FourStar = 0;
                        }
                    }
                    else
                    {
                        evaluationTotal.FourStar = 0;
                    }
                    #endregion

                    #region 5星总单数
                    if (EvaliationFiveStar.Count() > 0)
                    {
                        var FiveStar = EvaliationFiveStar.Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).ToList().FirstOrDefault();
                        if (FiveStar != null)
                        {
                            evaluationTotal.FiveStar = FiveStar.FiveStar;
                        }
                        else
                        {
                            evaluationTotal.FiveStar = 0;
                        }
                    }
                    else
                    {
                        evaluationTotal.FiveStar = 0;
                    }
                    #endregion
                    //evaluationTotal.AverageEvaluate = EvaliationInfo.EvaluateTotal / EvaliationInfo.OrderTotal;
                    evaluationTotal.OrderTotal = EvaliationInfo.OrderTotal;
                    // 平均时长
                    evaluationTotal.AverageAnswer = (EvaliationInfo.AnswerTime / EvaliationInfo.OrderTotal) / 3600;
                    // 平均分
                    var totalstar = (evaluationTotal.FiveStar + evaluationTotal.FourStar + evaluationTotal.ThreeStar + evaluationTotal.TwoStar + evaluationTotal.OneStar);
                    evaluationTotal.AverageEvaluate = totalstar < 1 ? 0.00 : Convert.ToDouble(evaluationTotal.EvaluateTotal) / Convert.ToDouble(totalstar);
                    evaluationTotal.RevenueTotal = Double.Parse(EvaliationInfo.RevenueTotal.ToString());
                    #region 退单数  完成数
                    if (RefundTotal.Count() > 0)
                    {
                        if (RefundTotal.ToList().Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).Count() > 0)
                        {
                            evaluationTotal.RefundTotal = RefundTotal.ToList().Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).FirstOrDefault().RefundTotal;
                        }
                        else
                        {
                            evaluationTotal.RefundTotal = 0;
                        }
                    }
                    else
                    {
                        evaluationTotal.RefundTotal = 0;
                    }
                    if (CompleteTotal.Count() > 0)
                    {
                        if (CompleteTotal.ToList().Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).Count() > 0)
                        {
                            evaluationTotal.CompleteTotal = CompleteTotal.ToList().Where(a => a.Key.DoctorID == EvaliationInfo.Key.DoctorID).FirstOrDefault().CompleteTotal;
                        }
                        else
                        {
                            evaluationTotal.CompleteTotal = 0;
                        }
                    }
                    else
                    {
                        evaluationTotal.CompleteTotal = 0;
                    }
                    #endregion



                    #region  将平均分 回复时长取1位小数
                    evaluationTotal.AverageEvaluate = Math.Round(evaluationTotal.AverageEvaluate, 1);
                    evaluationTotal.AverageAnswer = Math.Round(evaluationTotal.AverageAnswer, 1);
                    evaluationTotal.RevenueTotal = Math.Round(evaluationTotal.RevenueTotal, 1);
                    #endregion

                    var Evaluations = evatotal.Where(a => a.DoctorID == evaluationTotal.DoctorID && a.CreatedOn >= StartTime && a.CreatedOn < EndTime).ToList();

                    if (Evaluations.Count > 0)
                    {
                        var EvaluationsInfo = Evaluations.FirstOrDefault();
                        EvaluationsInfo.DoctorName = evaluationTotal.DoctorName;
                        EvaluationsInfo.DoctorID = evaluationTotal.DoctorID;
                      //  EvaluationsInfo.DoctorJSON = "";
                        EvaluationsInfo.EvaluateTotal = evaluationTotal.EvaluateTotal;
                        EvaluationsInfo.FiveStar = evaluationTotal.FiveStar;
                        EvaluationsInfo.FourStar = evaluationTotal.FourStar;
                        EvaluationsInfo.ThreeStar = evaluationTotal.ThreeStar;
                        EvaluationsInfo.TwoStar = evaluationTotal.TwoStar;
                        EvaluationsInfo.OneStar = evaluationTotal.OneStar;
                        EvaluationsInfo.AverageEvaluate = evaluationTotal.AverageEvaluate;
                        EvaluationsInfo.OrderTotal = evaluationTotal.OrderTotal;
                        EvaluationsInfo.AverageAnswer = evaluationTotal.AverageAnswer;
                        EvaluationsInfo.RevenueTotal = evaluationTotal.RevenueTotal;
                        EvaluationsInfo.RefundTotal = evaluationTotal.RefundTotal;
                        EvaluationsInfo.CompleteTotal = evaluationTotal.CompleteTotal;
                        EvaluationsInfo.ModifyOn = DateTime.Now;
                        var re = await _repository.UpdateAsync(EvaluationsInfo);
                    }
                    else
                    {
                        evaluationTotal.CreatedOn = DateTime.Now;
                        evaluationTotal.Id = await _repository.InsertAndGetIdAsync(evaluationTotal);
                    }

                }
            }
            return "success";
        }
    }
}
