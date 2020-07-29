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
using Yaeher.Consultation;
using System;
using Yaeher.DoctorReport.Dto;
using Yaeher.NumericalStatement.Dto;

namespace Yaeher
{
    /// <summary>
    /// 咨询管理
    /// </summary>
    public class ConsultationService : IConsultationService
    {
        private readonly IRepository<YaeherConsultation> _repository;
        private readonly IRepository<YaeherDoctor> _doctorrepository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<QualityControlManage> _qualitycon;
        private readonly IRepository<ConsultationEvaluation> _evarepository;
        private readonly IRepository<RefundManage> _refundrepository;
        private readonly IRepository<IncomeDetails> _incomerepository;

        /// <summary>
        /// 咨询管理构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="qualitycon"></param>
        /// <param name="userrepository"></param>
        /// <param name="evarepository"></param>
        /// <param name="refundrepository"></param>
        /// <param name="incomerepository"></param>
        /// <param name="doctorrepository"></param>
        public ConsultationService(IRepository<YaeherConsultation> repository,
                                    IRepository<YaeherDoctor> doctorrepository,
                                    IRepository<QualityControlManage> qualitycon,
                                    IRepository<YaeherUser> userrepository,
                                    IRepository<ConsultationEvaluation> evarepository,
                                    IRepository<RefundManage> refundrepository,
                                    IRepository<IncomeDetails> incomerepository)
        {
            _repository = repository;
            _doctorrepository= doctorrepository;
            _qualitycon = qualitycon;
            _userrepository = userrepository;
            _evarepository = evarepository;
            _refundrepository=refundrepository;
            _incomerepository=incomerepository;
        }

        /// <summary>
        /// 查询咨询管理 List
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherConsultation>> YaeherConsultationList(ConsultationIn YaeherConsultationInfo)
        {
            var YaeherConsultations = await _repository.GetAllListAsync(YaeherConsultationInfo.Expression);
            return YaeherConsultations.ToList();
        }
        /// <summary>
        /// 查询咨询管理 List
        /// </summary>
        /// <param name="DoctorNewin"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorNew>> DoctorNewList(DoctorNewIn DoctorNewin)
        {
            var YaeherConsultations =  await _repository.GetAll().Where(t=>!t.IsDelete).ToListAsync();
            var query = from a in YaeherConsultations
                        where a.ConsultState == "success"
                        group a by a.DoctorID into g
                         where g.Count() <= DoctorNewin.RecordCount
                        select new DoctorNew
                        {
                            DoctorId = g.Key,
                            ConsultationCount=g.Count(),
                        };
            var result =  query.ToList();
            return result;
        }
        /// <summary>
        /// 查询咨询管理 List
        /// </summary>
        /// <param name="DoctorNewin"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorNew>> DoctorOldList(DoctorNewIn DoctorNewin)
        {
            var YaeherConsultations = await _repository.GetAll().Where(t => !t.IsDelete).ToListAsync();
            var query = from a in YaeherConsultations
                        where a.ConsultState == "success"
                        group a by a.DoctorID into g
                        where g.Count() > DoctorNewin.RecordCount
                        select new DoctorNew
                        {
                            DoctorId = g.Key,
                            ConsultationCount = g.Count(),
                        };
            var result = query.ToList();
            return result;
        }

        /// <summary>
        /// 查询咨询管理byConsultNumber
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConsultation> YaeherConsultationByNumber(string ConsultNumber)
        {
            var YaeherConsultations = await _repository.FirstOrDefaultAsync(t => t.ConsultNumber== ConsultNumber && !t.IsDelete);
            return YaeherConsultations;
        }
        /// <summary>
        /// 查询咨询管理byConsultNumber
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConsultation> ConsultationByNumber(string ConsultNumber)
        {
            var YaeherConsultations = await _repository.FirstOrDefaultAsync(t => t.ConsultNumber == ConsultNumber);
            return YaeherConsultations;
        }
        /// <summary>
        /// 查询咨询管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConsultation> YaeherConsultationByID(int Id)
        {
            var YaeherConsultations = await _repository.FirstOrDefaultAsync(t => t.Id == Id&&!t.IsDelete);
            return YaeherConsultations;
        }
        /// <summary>
        /// 查询咨询管理 page
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherConsultation>> YaeherConsultationPage(ConsultationIn YaeherConsultationInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(YaeherConsultationInfo.Expression).OrderByDescending(a => a.CreatedOn);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherConsultationInfo.MaxResultCount;
            var YaeherConsultationList = await query.OrderByDescending(t => t.CreatedOn).PageBy(YaeherConsultationInfo.SkipTotal, YaeherConsultationInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherConsultation>(tasksCount, YaeherConsultationList.MapTo<List<YaeherConsultation>>());
        }
        /// <summary>
        /// 质控处理列表
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QualityConsultationPage>>QualityYaeherConsultationPage(ConsultationIn YaeherConsultationInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherConsultationInfo.Expression);
            var qualitycon = _qualitycon.GetAll().Where(t=>!t.IsDelete&&t.CreatedBy== YaeherConsultationInfo.CreatedBy);
            var user = _userrepository.GetAll().Where(t=>!t.IsDelete);
            var doctor = _doctorrepository.GetAll().Where(t=>!t.IsDelete);
            var eva= _evarepository.GetAll();
            var query = from a in query1
                        join b in qualitycon on a.Id equals b.ConsultID
                        join doc in doctor on a.DoctorID equals doc.Id
                        join c in user on doc.UserID equals c.Id
                        join d in eva on a.Id equals d.ConsultID
                        select new QualityConsultationPage
                        {
                            ConsultNumber=a.ConsultNumber,
                            ConsultantID = a.ConsultantID,
                            ConsultantName = a.ConsultantName,
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            PatientID = a.PatientID,
                            PatientName = a.PatientName,
                            ConsultType = a.ConsultType,
                            IIInessType = a.IIInessType,
                            PhoneNumber = a.PhoneNumber,
                            PatientCity = a.PatientCity,
                            IIInessDescription = a.IIInessDescription,
                            InquiryTimes = a.InquiryTimes,
                            ConsultState = a.ConsultState,
                            OvertimeRemindTimes = a.OvertimeRemindTimes,
                            Overtime = a.Overtime,
                            RefundBy = a.RefundBy,
                            RefundTime = a.RefundTime,
                            RefundType = a.RefundType,
                            RefundNumber = a.RefundNumber,
                            RefundState = a.RefundState,
                            RefundReason = a.RefundReason,
                            RefundRemarks = a.RefundRemarks,
                            RecommendDoctorID = a.RecommendDoctorID,
                            RecommendDoctorName = a.RecommendDoctorName,
                            HasReply = a.HasReply,
                            Age = a.Age,
                            ServiceMoneyListId = a.ServiceMoneyListId,
                            HasInquiryTimes = a.HasInquiryTimes,
                            IsReturnVisit = a.IsReturnVisit,
                            IsEvaluate = a.IsEvaluate,
                            ReturnVisit = a.ReturnVisit,
                            ReturnVisitTime = a.ReturnVisitTime,
                            Completetime = a.Completetime,
                            PatientJSON=a.PatientJSON,
                            Id = a.Id,
                            UserImage=c.UserImage,
                            CreatedOn=a.CreatedOn,
                            ReplyState=b.ReplyState,
                            IsQuality=d.IsQuality,
                            QualityReason    = d.QualityReason,
                            QualityLevel = d.QualityLevel,
                            EvaluateLevel=d.EvaluateLevel,
                            EvaluateReason = d.EvaluateReason,
                            EvaluateContent = d.EvaluateContent,
                        };
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherConsultationInfo.MaxResultCount;
            var YaeherConsultationList = await query.OrderByDescending(t => t.CreatedOn).PageBy(YaeherConsultationInfo.SkipTotal, YaeherConsultationInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<QualityConsultationPage>(tasksCount, YaeherConsultationList.MapTo<List<QualityConsultationPage>>());
        }
        /// <summary>
        /// 质控退单列表
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QualityConsultationPage>> QualityRefundYaeherConsultationPage(ConsultationIn YaeherConsultationInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherConsultationInfo.Expression);
            var doctor = _doctorrepository.GetAll().OrderByDescending(a => a.CreatedOn).Where(t=>!t.IsDelete);
            var refund = _refundrepository.GetAll().Where(t=>!t.IsDelete);
            var user = _userrepository.GetAll().Where(t => !t.IsDelete);
            var eva = _evarepository.GetAll();
            var query = from a in query1
                        join doc in doctor on a.DoctorID equals doc.Id
                        join c in user on doc.UserID equals c.Id
                        join  b in refund on a.ConsultNumber equals b.ConsultNumber
                        join d in eva on a.Id equals d.ConsultID
                        where b.CreatedBy== YaeherConsultationInfo.CreatedBy
                        select new QualityConsultationPage
                        {
                            ConsultNumber = a.ConsultNumber,
                            ConsultantID = a.ConsultantID,
                            ConsultantName = a.ConsultantName,
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            PatientID = a.PatientID,
                            PatientName = a.PatientName,
                            ConsultType = a.ConsultType,
                            IIInessType = a.IIInessType,
                            PhoneNumber = a.PhoneNumber,
                            PatientCity = a.PatientCity,
                            IIInessDescription = a.IIInessDescription,
                            InquiryTimes = a.InquiryTimes,
                            ConsultState = a.ConsultState,
                            OvertimeRemindTimes = a.OvertimeRemindTimes,
                            Overtime = a.Overtime,
                            RefundBy = a.RefundBy,
                            RefundTime = a.RefundTime,
                            RefundType = a.RefundType,
                            RefundNumber = a.RefundNumber,
                            RefundState = a.RefundState,
                            RefundReason = b.RefundReason,
                            RefundRemarks = b.RefundRemarks,
                            RecommendDoctorID = a.RecommendDoctorID,
                            RecommendDoctorName = a.RecommendDoctorName,
                            HasReply = a.HasReply,
                            Age = a.Age,
                            ServiceMoneyListId = a.ServiceMoneyListId,
                            HasInquiryTimes = a.HasInquiryTimes,
                            IsReturnVisit = a.IsReturnVisit,
                            IsEvaluate = a.IsEvaluate,
                            ReturnVisit = a.ReturnVisit,
                            ReturnVisitTime = a.ReturnVisitTime,
                            Completetime = a.Completetime,
                            PatientJSON = a.PatientJSON,
                            Id = a.Id,
                            UserImage = c.UserImage,
                            CreatedOn = a.CreatedOn,
                           // IsQuality = d.IsQuality,
                         //   QualityReason = d.QualityReason,
                            QualityLevel = d.QualityLevel,
                         //   EvaluateLevel = d.EvaluateLevel,
                         //   EvaluateReason = d.EvaluateReason,
                         //   EvaluateContent = d.EvaluateContent,
                            RefundCheckState=b.CheckState
                        };
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherConsultationInfo.MaxResultCount;
            var YaeherConsultationList = await query.OrderByDescending(t => t.CreatedOn).PageBy(YaeherConsultationInfo.SkipTotal, YaeherConsultationInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<QualityConsultationPage>(tasksCount, YaeherConsultationList.MapTo<List<QualityConsultationPage>>());
        }
        
        /// <summary>
        /// 新建咨询管理
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConsultation> CreateYaeherConsultation(YaeherConsultation YaeherConsultationInfo)
        {
            YaeherConsultationInfo.Id =await _repository.InsertAndGetIdAsync(YaeherConsultationInfo);
            return YaeherConsultationInfo;
        }

        /// <summary>
        /// 新建咨询管理
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConsultation> InsertAndGetIdAsync(YaeherConsultation YaeherConsultationInfo)
        {
            YaeherConsultationInfo.Id= await _repository.InsertAndGetIdAsync(YaeherConsultationInfo);
            return YaeherConsultationInfo;
        }

        /// <summary>
        /// 修改咨询管理
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConsultation> UpdateYaeherConsultation(YaeherConsultation YaeherConsultationInfo)
        {
            return await _repository.UpdateAsync(YaeherConsultationInfo);
        }

        /// <summary>
        /// 删除咨询管理
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConsultation> DeleteYaeherConsultation(YaeherConsultation YaeherConsultationInfo)
        {
            return await _repository.UpdateAsync(YaeherConsultationInfo);
        }
        /// <summary>
        /// 删除咨询管理
        /// </summary>
        /// <param name="detailin"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<AdminIncomeDetail>> IncomeConsultationDetail(IncomeDetailsIn detailin)
        {
            var con = _repository.GetAll().Where(t=>!t.IsDelete);
            var incom=_incomerepository.GetAll().Where(detailin.Expression);
            var query = from a in incom
                        join b in con on a.ConsultID equals b.Id
                        select new AdminIncomeDetail
                        {
                            ConsultType = b.ConsultType,//咨询类型
                            ConsultID=b.Id,
                            ConsultantName=b.ConsultantName,//咨询人姓名
                            PatientName=b.PatientName,//患者姓名
                            CreatedOn=a.CreatedOn,
                            OrderMoney=a.OrderMoney,//流水金额
                            ProportionMoney=Math.Round(a.OrderMoney-a.ProportionMoney,2),//公司进账
                            DoctorName=a.DoctorName,
                            DoctorID=a.DoctorID,
                        };
            return await query.ToListAsync();

        }
    }
}
