using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Doctor;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生收藏夹
    /// </summary>
    public class CollectConsultationService : ICollectConsultationService
    {
        private readonly IRepository<CollectConsultation> _repository;
        private readonly IRepository<YaeherConsultation> _consrepository;
        /// <summary>
        /// 医生上传附件 构造函数
        /// </summary>
        /// <param name="repository"></param>
        ///  <param name="consrepository"></param>
        public CollectConsultationService(IRepository<CollectConsultation> repository, IRepository<YaeherConsultation> consrepository)
        {
            _repository = repository;
            _consrepository = consrepository;
        }

        /// <summary>
        /// 查询医生收藏夹byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CollectConsultation> CollectConsultationByID(int Id)
        {
            var DoctorFileApplys = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorFileApplys;
        }
        /// <summary>
        /// 新建医生收藏夹
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CollectConsultation> CreateCollectConsultation(CollectConsultation DoctorFileApplyInfo)
        {
            DoctorFileApplyInfo.Id = await _repository.InsertAndGetIdAsync(DoctorFileApplyInfo);
            return DoctorFileApplyInfo;
        }

        /// <summary>
        /// 修改医生收藏夹
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CollectConsultation> UpdateCollectConsultation(CollectConsultation DoctorFileApplyInfo)
        {
            return await _repository.UpdateAsync(DoctorFileApplyInfo);
        }
        /// <summary>
        /// 医生收藏夹
        /// </summary>
        /// <param name="collectConsultation"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherConsultation>> CollectConsultationPage(CollectConsultationIn collectConsultation)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(collectConsultation.Expression);
            
              var   consquery = _consrepository.GetAll().Where(t => !t.IsDelete );
           
            var querylist = from a in query
                            join b in consquery on a.ConsultID equals b.Id
                            where !a.IsDelete && !b.IsDelete
                            select new YaeherConsultation
                            {
                                ConsultNumber = b.ConsultNumber,
                                ConsultantID = b.ConsultantID,
                                ConsultantName = b.ConsultantName,
                                ConsultantJSON = b.ConsultantJSON,
                                DoctorName = b.DoctorName,
                                DoctorID = b.DoctorID,
                                DoctorJSON = b.DoctorJSON,
                                PatientID = b.PatientID,
                                PatientName = b.PatientName,
                                PatientJSON = b.PatientJSON,
                                ConsultType = b.ConsultType,
                                IIInessType = b.IIInessType,
                                IIInessJSON = b.IIInessJSON,
                                PhoneNumber = b.PhoneNumber,
                                PatientCity = b.PatientCity,
                                IIInessDescription = b.IIInessDescription,
                                InquiryTimes = b.InquiryTimes,
                                ConsultState = b.ConsultState,
                                OvertimeRemindTimes = b.OvertimeRemindTimes,
                                Overtime = b.Overtime,
                                RefundBy = b.RefundBy,
                                RefundTime = b.RefundTime,
                                RefundType = b.RefundType,
                                RefundNumber = b.RefundNumber,
                                RefundState = b.RefundState,
                                RefundReason = b.RefundReason,
                                RefundRemarks = b.RefundRemarks,
                                RecommendDoctorID = b.RecommendDoctorID,
                                RecommendDoctorName = b.RecommendDoctorName,
                                HasReply = b.HasReply,
                                Age = b.Age,
                                ServiceMoneyListId = b.ServiceMoneyListId,
                                HasInquiryTimes = b.HasInquiryTimes,
                                IsReturnVisit = b.IsReturnVisit,
                                IsEvaluate = b.IsEvaluate,
                                ReturnVisit = b.ReturnVisit,
                                ReturnVisitTime = b.ReturnVisitTime,
                                CreatedOn = b.CreatedOn,
                                Id=b.Id,
                            };

            if (!string.IsNullOrEmpty(collectConsultation.KeyWord))
            {
                querylist = querylist.Where(t=>(t.ConsultNumber.Contains(collectConsultation.KeyWord) ||
                                                                    t.DoctorName.Contains(collectConsultation.KeyWord) ||
                                                                    t.PatientName.Contains(collectConsultation.KeyWord) ||
                                                                    t.ConsultType.Contains(collectConsultation.KeyWord) ||
                                                                    t.ConsultantName.Contains(collectConsultation.KeyWord)));
            }
            //获取总数
            var tasksCount = querylist.Count();
            //获取总数
            var totalpage = tasksCount / collectConsultation.MaxResultCount;
            var YaeherConsultationList = await querylist.OrderByDescending(t => t.CreatedOn).PageBy(collectConsultation.SkipTotal, collectConsultation.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherConsultation>(tasksCount, YaeherConsultationList.MapTo<List<YaeherConsultation>>());
        }

        /// <summary>
        /// 医生收藏夹 List
        /// </summary>
        /// <param name="collectConsultation"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<CollectConsultation>> CollectConsultationList(CollectConsultationIn collectConsultation)
        {
            var CollectConsultation = await _repository.GetAllListAsync(t=>!t.IsDelete);
            return CollectConsultation.ToList();
        }
        /// <summary>
        /// 医生收藏夹 List
        /// </summary>
        /// <param name="collectConsultation"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<CollectConsultation>> CollectConsultationListAsync(CollectConsultationIn collectConsultation)
        {
            var CollectConsultation =  _repository.GetAll();
            return await CollectConsultation.ToListAsync();
        }

        /// <summary>
        /// 医生收藏夹byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CollectConsultation> CollectConsultationByExpression(Expression<Func<CollectConsultation, bool>> whereExpression)
        {
            var CollectConsultation = await _repository.FirstOrDefaultAsync(whereExpression);
            return CollectConsultation;
        }
        /// <summary>
        /// 删除收藏夹
        /// </summary>
        /// <param name="Consultation"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<CollectConsultation> DeleteCollectConsultation(CollectConsultation Consultation)
        {
            return await _repository.UpdateAsync(Consultation);
        }

    }
}
