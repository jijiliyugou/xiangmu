using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.ClinicManage.Dto;
using Yaeher.YaeherDoctors.Dto;
using Yaeher.Quality.Dto;
using Yaeher.DoctorQuality.Dto;
using Yaeher.NumericalStatement;
using System;

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 门诊信息
    /// </summary>
    public class ClinicInfomationService : IClinicInfomationService
    {
        private readonly IRepository<ClinicInfomation> _repository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<YaeherDoctor> _docrepository;
        private readonly IRepository<ClinicDoctorReltion> _clinicdocrepository;
        private readonly IRepository<DoctorOnlineRecord> _doctoronlinerepository;

        // 评分汇总
        private readonly IRepository<EvaluationTotal> _EvaluationTotalrepository;
        // 医生提供服务状态
        private readonly IRepository<ServiceMoneyList> _ServiceMoneyListrepository;
        // 质控委员
        private readonly IRepository<QualityCommittee> _QualityCommitteerepository;
        // 咨询订单表
        private readonly IRepository<YaeherConsultation> _YaeherConsultationrepository;


         /// <summary>
         /// 门诊信息构造函数
         /// </summary>
         /// <param name="repository"></param>
         /// <param name="userrepository"></param>
         /// <param name="docrepository"></param>
         /// <param name="clinicdocrepository"></param>
         /// <param name="doctoronlinerepository"></param>
         /// <param name="evaluationTotalrepository"></param>
         /// <param name="serviceMoneyListrepository"></param>
         /// <param name="qualityCommitteerepository"></param>
         /// <param name="YaeherConsultationrepository"></param>
        public ClinicInfomationService(IRepository<ClinicInfomation> repository,
                                       IRepository<YaeherUser> userrepository,
                                       IRepository<YaeherDoctor> docrepository,
                                       IRepository<ClinicDoctorReltion> clinicdocrepository,
                                       IRepository<DoctorOnlineRecord> doctoronlinerepository,
                                       IRepository<EvaluationTotal> evaluationTotalrepository,
                                       IRepository<ServiceMoneyList> serviceMoneyListrepository,
                                       IRepository<QualityCommittee> qualityCommitteerepository,
                                       IRepository<YaeherConsultation> YaeherConsultationrepository)
        {
            _repository = repository;
            _userrepository = userrepository;
            _docrepository = docrepository;
            _clinicdocrepository = clinicdocrepository;
            _doctoronlinerepository = doctoronlinerepository;
            _EvaluationTotalrepository = evaluationTotalrepository;
            _ServiceMoneyListrepository = serviceMoneyListrepository;
            _QualityCommitteerepository = qualityCommitteerepository;
            _YaeherConsultationrepository = YaeherConsultationrepository;
        }
        /// <summary>
        /// 查询门诊信息 List
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ClinicInfomation>> ClinicInfomationList(ClinicInfomationIn ClinicInfomationInfo)
        {
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ClinicInfomationInfo.Expression);
            return await query.ToListAsync();
        }
        /// <summary>
        /// 查询门诊信息 List
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <param name="IDArr"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ClinicInfomation>> ClinicInfomationListByArrId(ClinicInfomationIn ClinicInfomationInfo,List<int>IDArr)
        {
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ClinicInfomationInfo.Expression);
            if (IDArr.Count>0)
            {
                query = query.Where(t=>IDArr.Contains(t.Id));
            }
            return await query.ToListAsync();
        }
        
        /// <summary>
        /// 查询某某医生剩余的门诊信息 List
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<ClinicInfomation>> ClinicInfomationList(ClinicInfomationIn ClinicInfomationInfo, List<ClinicDoctorReltion> rel)
        {
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ClinicInfomationInfo.Expression);
            if (rel.Count() > 0)
            {
                var labelrel = new int[rel.Count];
                for (var i = 0; i < rel.Count(); i++)
                {
                    labelrel[i] = rel[i].ClinicID;
                }
                query = query.Where(t => !labelrel.Contains(t.Id));
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// 获取医生信息
        /// </summary>
        /// <param name="clinic"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicDoctorsView>> DoctorInformation(ClinicInfomationIn clinic)
        {
            var doc = _docrepository.GetAll().Where(t => !t.IsDelete).Where(t => !t.IsDelete&& t.CheckRes=="success" && t.AuthCheckRes=="success");
            var user = _userrepository.GetAll().Where(t => !t.IsDelete && t.RoleName=="doctor");
            var clinicdoc = _clinicdocrepository.GetAll().Where(t => !t.IsDelete);
            var doctorline = _doctoronlinerepository.GetAll().Where(t => !t.IsDelete&& t.OnlineState=="online");
            var clinicinfo = _repository.GetAll().Where(clinic.Expression);
            var query = from a in doc
                        join b in user on a.UserID equals b.Id
                        join c in clinicdoc on a.Id equals c.DoctorID
                        join d in doctorline on a.Id equals d.DoctorID
                        join e in clinicinfo on c.ClinicID equals e.Id
                        select new ClinicDoctorsView
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Title = a.Title,
                            Status = "1",
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                        };
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / clinic.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(clinic.SkipTotal, clinic.MaxResultCount).OrderBy(a => a.DoctorLevel).ToListAsync();
            return new PagedResultDto<ClinicDoctorsView>(tasksCount, ClinicInfomationList.MapTo<List<ClinicDoctorsView>>());
        }
        /// <summary>
        /// 根据科室Id获取医生信息
        /// </summary>
        /// <param name="clinic"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicDoctorsView>> ClinicDoctorInformation(YaeherDoctorSearch clinic, IList<DoctorRelation> rel)
        {
            var doc = _docrepository.GetAll().OrderByDescending(t=>t.CreatedOn).Where(t => !t.IsDelete&& t.CheckRes=="success" && t.AuthCheckRes=="success");;
            var user = _userrepository.GetAll().Where(t => !t.IsDelete && t.RoleName=="doctor");
            var clinicdoc = _clinicdocrepository.GetAll().Where(t => !t.IsDelete);
            var doctorline = _doctoronlinerepository.GetAll().Where(t => !t.IsDelete);
            var clinicinfo = _repository.GetAll().Where(t => !t.IsDelete);
            var online = _doctoronlinerepository.GetAll().Where(t => !t.IsDelete);
            //DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            //DateTime EndTime = StartTime.AddDays(1);
            //var evaluationTotalList = _EvaluationTotalrepository.GetAll().Where(a => a.IsDelete == false && a.CreatedOn >= StartTime && a.CreatedOn < EndTime);
            //var DoctorServiceList = _ServiceMoneyListrepository.GetAll().Where(a => a.IsDelete == false);
            //var qualitycontrol = _QualityCommitteerepository.GetAll().Where(a => a.IsDelete == false);
            var query = from cd in clinicdoc
                        join ci in clinicinfo on cd.ClinicID equals ci.Id
                        join a in doc on cd.DoctorID equals a.Id
                        join c in doctorline on a.Id equals c.DoctorID
                        join b in user on a.UserID equals b.Id
                        join d in online on a.Id equals d.DoctorID
                        where ci.Id == clinic.ClinicID && cd.ClinicID == clinic.ClinicID
                        select new ClinicDoctorsView
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Title = a.Title,
                            Status = "1",
                            OnlineState = d.OnlineState,
                            DoctorOnlineRecordId = d.Id,
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                        };
            if (!string.IsNullOrEmpty(clinic.KeyWord))
            {
                if (rel.Count > 0)
                {
                    var labelrel = new int[rel.Count];
                    for (var i = 0; i < rel.Count(); i++)
                    {
                        labelrel[i] = rel[i].DoctorID;
                    }
                    query = query.Distinct().Where(t => labelrel.Contains(t.Id));
                }
                else
                {
                    query = query.Distinct().Where(t => t.DoctorName.Contains(clinic.KeyWord));
                }
            }
            if (!string.IsNullOrEmpty(clinic.OnlineState))
            {
                query = query.Distinct().Where(t => t.OnlineState == clinic.OnlineState);
            }
            #region
            //List<ClinicDoctorsView> clinicDoctorsViews = new List<ClinicDoctorsView>(); 
            //if (query.Count() > 0)
            //{
            //    foreach (var DoctorInfo in query)
            //    {
            //        if (evaluationTotalList.Count() > 0)
            //        {
            //            var evaluationTotal = evaluationTotalList.FirstOrDefault(t => t.DoctorID == DoctorInfo.Id);
            //            if (evaluationTotal != null)
            //            {
            //                DoctorInfo.ReceiptNumBer = evaluationTotal == null ? 0 : evaluationTotal.CompleteTotal;//接单数
            //                DoctorInfo.AverageTime = evaluationTotal == null ? "0" : evaluationTotal.AverageAnswer.ToString();//平均时长

            //                DoctorInfo.EvaluateTotal = evaluationTotal.EvaluateTotal;
            //                DoctorInfo.CompleteTotal = evaluationTotal.CompleteTotal;
            //                DoctorInfo.EvaluationCount = evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;
            //                if (DoctorInfo.EvaluationCount >= 15)
            //                {
            //                    DoctorInfo.AverageEvaluate = evaluationTotal.AverageEvaluate;//星级
            //                }
            //            }
            //        }
            //        else
            //        {
            //            DoctorInfo.ReceiptNumBer = 0;//接单数
            //            DoctorInfo.AverageTime = "0";//平均时长
            //            DoctorInfo.AverageEvaluate = 0;
            //        }
            //        DoctorInfo.serviceMoneyList = DoctorServiceList.ToList();
            //        // 接单状态
            //        bool ImageState = false;   // 图文咨询关闭
            //        bool PhoneState = false;   // 电话咨询关闭
            //        DoctorInfo.ServiceState = false;   // 停止服务
            //        DoctorInfo.ReceiptState = false;   // 不可接单
            //        DoctorInfo.ImageServiceFrequency = 0;  // 默认0
            //        DoctorInfo.PhoneServiceFrequency = 0;  // 默认0
            //        #region 统计当天订单
            //        var yaeherConsultations = _YaeherConsultationrepository.GetAll().Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.Id && a.CreatedOn>=StartTime && a.RefundNumber==null).ToList();
            //        // 当天图文接单总数
            //        var ImageNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "ImageText").Count();
            //        // 当天电话接单总数
            //        var PhoneNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "Phone").Count();
            //        #endregion
            //        var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.Id && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
            //        if (ImageTextItem != null)
            //        {
            //            ImageState = ImageTextItem.ServiceState;
            //            DoctorInfo.ImageServiceExpense = ImageTextItem.ServiceExpense;
            //            if (ImageState)  // 开启
            //            {
            //                DoctorInfo.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
            //            }
            //        }
            //        var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.Id && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
            //        if (PhoneItem != null)
            //        {
            //            PhoneState = PhoneItem.ServiceState;
            //            DoctorInfo.PhoneServiceExpense = PhoneItem.ServiceExpense;
            //            if (PhoneState)  // 开启
            //            {
            //                DoctorInfo.PhoneServiceFrequency = PhoneItem.ServiceFrequency;
            //            }
            //        }
            //        if (ImageState || PhoneState)
            //        {
            //            DoctorInfo.ServiceState = true;  //可咨询
            //        }
            //        if (DoctorInfo.PhoneServiceFrequency - PhoneNumberTotal > 0 || DoctorInfo.ImageServiceFrequency - ImageNumberTotal > 0)
            //        {
            //            DoctorInfo.ReceiptState = true;  // 可接单,默认不满额
            //        }
            //        clinicDoctorsViews.Add(DoctorInfo);
            //    }
            //}
            //query = from a in query
            //        join b in clinicDoctorsViews on a.Id equals b.Id
            //        select b;
            #endregion
            query = query.OrderBy(a=>a.OnlineState).OrderByDescending(a=>a.ReceiptState).OrderByDescending(a=>a.ServiceState);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / clinic.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(clinic.SkipTotal, clinic.MaxResultCount).ToListAsync();
            return new PagedResultDto<ClinicDoctorsView>(tasksCount, ClinicInfomationList.MapTo<List<ClinicDoctorsView>>());

        }

        /// <summary>
        /// 根据科室Id获取医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicDoctorsView>> DoctorInformation(YaeherDoctorSearch clinic, IList<DoctorRelation> rel)
        {
            // 医生评分
            //DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            //DateTime EndTime = StartTime.AddDays(1);
            //var evaluationTotalList = _EvaluationTotalrepository.GetAll().Where(a=>a.IsDelete==false && a.CreatedOn >= StartTime && a.CreatedOn < EndTime);
            //var DoctorServiceList =  _ServiceMoneyListrepository.GetAll().Where(a=>a.IsDelete==false);
            //var qualitycontrol = _QualityCommitteerepository.GetAll().Where(a=>a.IsDelete==false);
            var doc = _docrepository.GetAll().OrderByDescending(t=>t.CreatedOn).Where(t => !t.IsDelete&& t.CheckRes=="success" && t.AuthCheckRes=="success");
            var user = _userrepository.GetAll().Where(t => !t.IsDelete && t.RoleName=="doctor");
            var doctorline = _doctoronlinerepository.GetAll().Where(t => !t.IsDelete);
            var query = from a in doc
                        join c in doctorline on a.Id equals c.DoctorID
                        join b in user on a.UserID equals b.Id
                        select new ClinicDoctorsView
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Title = a.Title,
                            Status = "1",
                            OnlineState = c.OnlineState,
                            DoctorOnlineRecordId = c.Id,
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                        };
            if (!string.IsNullOrEmpty(clinic.KeyWord))
            {
                if (rel.Count > 0)
                {
                    var labelrel = new int[rel.Count];
                    for (var i = 0; i < rel.Count(); i++)
                    {
                        labelrel[i] = rel[i].DoctorID;
                    }
                    query = query.Distinct().Where(t => labelrel.Contains(t.Id));
                }
                else
                {
                    query = query.Distinct().Where(t => t.DoctorName.Contains(clinic.KeyWord));
                }
            }
            if (!string.IsNullOrEmpty(clinic.OnlineState))
            {
                query = query.Distinct().Where(t => t.OnlineState == clinic.OnlineState);
            }
            #region
            //List<ClinicDoctorsView> clinicDoctorsViews = new List<ClinicDoctorsView>(); 
            //if (query.Count() > 0)
            //{
            //    foreach (var DoctorInfo in query)
            //    {
            //        if (evaluationTotalList.Count() > 0)
            //        {
            //            var evaluationTotal = evaluationTotalList.FirstOrDefault(t => t.DoctorID == DoctorInfo.Id);
            //            if (evaluationTotal != null)
            //            {
            //                DoctorInfo.ReceiptNumBer = evaluationTotal == null ? 0 : evaluationTotal.CompleteTotal;//接单数
            //                DoctorInfo.AverageTime = evaluationTotal == null ? "0" : evaluationTotal.AverageAnswer.ToString();//平均时长

            //                DoctorInfo.EvaluateTotal = evaluationTotal.EvaluateTotal;
            //                DoctorInfo.CompleteTotal = evaluationTotal.CompleteTotal;
            //                DoctorInfo.EvaluationCount = evaluationTotal.OneStar + evaluationTotal.TwoStar + evaluationTotal.ThreeStar + evaluationTotal.FourStar + evaluationTotal.FiveStar;
            //                if (DoctorInfo.EvaluationCount >= 15)
            //                {
            //                    DoctorInfo.AverageEvaluate = evaluationTotal.AverageEvaluate;//星级
            //                }
            //            }
            //        }
            //        else
            //        {
            //            DoctorInfo.ReceiptNumBer = 0;//接单数
            //            DoctorInfo.AverageTime = "0";//平均时长
            //            DoctorInfo.AverageEvaluate = 0;
            //        }
            //        DoctorInfo.serviceMoneyList = DoctorServiceList.ToList();
            //        // 接单状态
            //        bool ImageState = false;   // 图文咨询关闭
            //        bool PhoneState = false;   // 电话咨询关闭
            //        DoctorInfo.ServiceState = false;   // 停止服务
            //        DoctorInfo.ReceiptState = false;   // 不可接单
            //        DoctorInfo.ImageServiceFrequency = 0;  // 默认0
            //        DoctorInfo.PhoneServiceFrequency = 0;  // 默认0
            //        #region 统计当天订单
            //        var yaeherConsultations = _YaeherConsultationrepository.GetAll().Where(a => a.IsDelete == false && a.DoctorID == DoctorInfo.Id && a.CreatedOn>=StartTime && a.RefundNumber==null).ToList();
            //        // 当天图文接单总数
            //        var ImageNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "ImageText").Count();
            //        // 当天电话接单总数
            //        var PhoneNumberTotal = yaeherConsultations.Where(a => a.ConsultType == "Phone").Count();
            //        #endregion
            //        var ImageTextItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.Id && a.IsDelete == false && a.ServiceType == "ImageText").FirstOrDefault();
            //        if (ImageTextItem != null)
            //        {
            //            ImageState = ImageTextItem.ServiceState;
            //            DoctorInfo.ImageServiceExpense = ImageTextItem.ServiceExpense;
            //            if (ImageState)  // 开启
            //            {
            //                DoctorInfo.ImageServiceFrequency = ImageTextItem.ServiceFrequency;
            //            }
            //        }
            //        var PhoneItem = DoctorServiceList.Where(a => a.DoctorID == DoctorInfo.Id && a.IsDelete == false && a.ServiceType == "Phone").FirstOrDefault();
            //        if (PhoneItem != null)
            //        {
            //            PhoneState = PhoneItem.ServiceState;
            //            DoctorInfo.PhoneServiceExpense = PhoneItem.ServiceExpense;
            //            if (PhoneState)  // 开启
            //            {
            //                DoctorInfo.PhoneServiceFrequency = PhoneItem.ServiceFrequency;
            //            }
            //        }
            //        if (ImageState || PhoneState)
            //        {
            //            DoctorInfo.ServiceState = true;  //可咨询
            //        }
            //        if (DoctorInfo.PhoneServiceFrequency - PhoneNumberTotal > 0 || DoctorInfo.ImageServiceFrequency - ImageNumberTotal > 0)
            //        {
            //            DoctorInfo.ReceiptState = true;  // 可接单,默认不满额
            //        }
            //        clinicDoctorsViews.Add(DoctorInfo);
            //    }
            //}
            //query = from a in query
            //        join b in clinicDoctorsViews on a.Id equals b.Id
            //        select b;
            #endregion
            query = query.OrderByDescending(a => a.ServiceState).ThenBy(a => a.ReceiptState);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / clinic.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(clinic.SkipTotal, clinic.MaxResultCount).ToListAsync();
            return new PagedResultDto<ClinicDoctorsView>(tasksCount, ClinicInfomationList.MapTo<List<ClinicDoctorsView>>());
        }
        /// <summary>
        /// 医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicDoctorsView>> QualityDoctorInfor(YaeherDoctorSearch clinic, IList<DoctorRelation> rel)
        {
            var doc = _docrepository.GetAll().Where(t => !t.IsDelete&& t.CheckRes=="success" && t.AuthCheckRes=="success");
            var user = _userrepository.GetAll().Where(t => !t.IsDelete && t.RoleName=="doctor");
            var clinicdoc = _clinicdocrepository.GetAll().Where(t => !t.IsDelete);
            var doctorline = _doctoronlinerepository.GetAll().Where(t => !t.IsDelete&& t.OnlineState=="Online");
            var clinicinfo = _repository.GetAll().Where(t => !t.IsDelete);

            var query = from cd in clinicdoc
                        join ci in clinicinfo on cd.ClinicID equals ci.Id
                        join a in doc on cd.DoctorID equals a.Id
                        join c in doctorline on a.Id equals c.DoctorID
                        join b in user on a.UserID equals b.Id
                        select new ClinicDoctorsView
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Title = a.Title,
                            Status = "1",
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                        };
            if (!string.IsNullOrEmpty(clinic.KeyWord))
            {
                var labelrel = new int[rel.Count];
                for (var i = 0; i < rel.Count(); i++)
                {
                    labelrel[i] = rel[i].DoctorID;
                }
                query = query.Where(t => labelrel.Contains(t.Id));
            }
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / clinic.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(clinic.SkipTotal, clinic.MaxResultCount).OrderBy(a => a.DoctorLevel).ToListAsync();
            return new PagedResultDto<ClinicDoctorsView>(tasksCount, ClinicInfomationList.MapTo<List<ClinicDoctorsView>>());
        }

        /// <summary>
        /// 根据科室Id获取医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        ///  <param name="doctorNews"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicDoctorsView>> QualityDoctorInformation(YaeherDoctorSearch clinic, IList<DoctorRelation> rel, IList<DoctorNew> doctorNews)
        {
            var doc = _docrepository.GetAll().Where(t => !t.IsDelete&& t.CheckRes=="success" && t.AuthCheckRes=="success");
            var user = _userrepository.GetAll().Where(t => !t.IsDelete && t.RoleName=="doctor");
            var clinicdoc = _clinicdocrepository.GetAll().Where(t => !t.IsDelete);
            var doctorline = _doctoronlinerepository.GetAll().Where(t => !t.IsDelete);
            var clinicinfo = _repository.GetAll().Where(t => !t.IsDelete);

            var query = from a in doc 
                        join c in doctorline on a.Id equals c.DoctorID
                        join b in user on a.UserID equals b.Id
                        where !a.IsDelete && !b.IsDelete  && !c.IsDelete && a.CheckRes == "success" && a.AuthCheckRes == "success"
                        select new ClinicDoctorsView
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Title = a.Title,
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                        };

            if (rel.Count() > 0)
            {
                var labelrel = new int[rel.Count];
                for (var i = 0; i < rel.Count(); i++)
                {
                    labelrel[i] = rel[i].DoctorID;
                }
                query = query.Where(t => labelrel.Contains(t.Id));
            }
            var newdoctorrel = new int[doctorNews.Count];
            for (var i = 0; i < doctorNews.Count(); i++)
            {
                newdoctorrel[i] = doctorNews[i].DoctorId;
            }
            if (newdoctorrel.Count() > 0)
            {
                query = query.Where(t => newdoctorrel.Contains(t.Id));
            }

            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / clinic.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(clinic.SkipTotal, clinic.MaxResultCount).OrderBy(a => a.DoctorLevel).ToListAsync();
            return new PagedResultDto<ClinicDoctorsView>(tasksCount, ClinicInfomationList.MapTo<List<ClinicDoctorsView>>());
        }
        /// <summary>
        /// 根据科室Id获取医生信息
        /// </summary>
        /// <param name="docarr"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<ClinicDoctorsView>> PatientCollectDoctorInformation(YaeherPatientDoctorSearch docarr)
        {
            var doc = _docrepository.GetAll().Where(t => !t.IsDelete&& t.CheckRes=="success" && t.AuthCheckRes=="success");
            var user = _userrepository.GetAll().Where(t => !t.IsDelete && t.RoleName=="doctor");
            var query = from a in doc 
                        join b in user on a.UserID equals b.Id
                        select new ClinicDoctorsView
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Title = a.Title,
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                            CreatedBy = a.CreatedBy,
                            ModifyOn = a.ModifyOn,
                            ModifyBy = a.ModifyBy,
                            DeleteBy = a.DeleteBy,
                            DeleteTime = a.DeleteTime,
                            IsDelete = a.IsDelete,
                            KeyWord = a.DoctorName,
                        };
            if (!string.IsNullOrEmpty(docarr.IdArr))
            {
                var sourceIdList = docarr.IdArr.Split(',');
                var idList = new int[sourceIdList.Count()+1];
                for (var i = 0; i < sourceIdList.Count(); i++)
                {
                    idList[i] = int.Parse(sourceIdList[i]);
                }
                query = query.Where(t => idList.Contains(t.Id));
            }
            return await query.ToListAsync();

        }
        /// <summary>
        /// 根据科室Id获取医生信息
        /// </summary>
        /// <param name="docotr"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicDoctorsView>> DoctorInformation(YaeherDoctorSearch docotr)
        {
            var doc = _docrepository.GetAll().Where(t => !t.IsDelete&& t.CheckRes=="success" && t.AuthCheckRes=="success");
            var user = _userrepository.GetAll().Where(t => !t.IsDelete && t.RoleName=="doctor");
            var clinicdoc = _clinicdocrepository.GetAll().Where(t => !t.IsDelete);
            var clinicinfo = _repository.GetAll().Where(t => !t.IsDelete );
            var query = from cd in clinicdoc
                        join ci in clinicinfo on cd.ClinicID equals ci.Id
                        join a in doc on cd.DoctorID equals a.Id
                        join b in user on a.UserID equals b.Id
                        select new ClinicDoctorsView
                        {
                            UserImage = b.UserImage,
                            UserID = b.Id,
                            DoctorName = a.DoctorName,
                            HospitalName = a.HospitalName,
                            Title = a.Title,
                            DoctorLevel = "4.1",
                            Status = "1",
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                        };
            if (!string.IsNullOrEmpty(docotr.KeyWord))
            {
                query = query.Where(t => t.DoctorName.Contains(docotr.KeyWord));
            }
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / docotr.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(docotr.SkipTotal, docotr.MaxResultCount).OrderBy(a => a.DoctorLevel).ToListAsync();
            return new PagedResultDto<ClinicDoctorsView>(tasksCount, ClinicInfomationList.MapTo<List<ClinicDoctorsView>>());

        }
        /// <summary>
        /// 查询门诊信息byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicInfomation> ClinicInfomationByID(int Id)
        {
            var ClinicInfomations = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ClinicInfomations;
        }
        /// <summary>
        /// 查询门诊信息 page
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicInfomation>> ClinicInfomationPage(ClinicInfomationIn ClinicInfomationInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderBy(a => a.OrderSort).ThenByDescending(a=>a.CreatedOn).Where(ClinicInfomationInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ClinicInfomationInfo.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(ClinicInfomationInfo.SkipTotal, ClinicInfomationInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ClinicInfomation>(tasksCount, ClinicInfomationList.MapTo<List<ClinicInfomation>>());
        }
        /// <summary>
        /// 新建门诊信息
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicInfomation> CreateClinicInfomation(ClinicInfomation ClinicInfomationInfo)
        {
            ClinicInfomationInfo.Id = await _repository.InsertAndGetIdAsync(ClinicInfomationInfo);
            return ClinicInfomationInfo;
        }

        /// <summary>
        /// 修改门诊信息
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicInfomation> UpdateClinicInfomation(ClinicInfomation ClinicInfomationInfo)
        {
            return await _repository.UpdateAsync(ClinicInfomationInfo);
        }

        /// <summary>
        /// 删除门诊信息
        /// </summary>
        /// <param name="ClinicInfomationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicInfomation> DeleteClinicInfomation(ClinicInfomation ClinicInfomationInfo)
        {
            return await _repository.UpdateAsync(ClinicInfomationInfo);
        }


    }
}
