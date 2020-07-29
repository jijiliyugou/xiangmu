using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.DoctorQuality.Dto;

namespace Yaeher.DoctorQuality
{
    /// <summary>
    /// 处理质控
    /// </summary>
    public class QualityControlManageService : IQualityControlManageService
    {
        private readonly IRepository<QualityControlManage> _repository;
        private readonly IRepository<SystemParameter> _sysrepository;
        private readonly IRepository<YaeherConsultation> _consrepository;
        /// <summary>
        /// 处理质控 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="consrepository"></param>
        /// <param name="sysrepository"></param>
        public QualityControlManageService(IRepository<QualityControlManage> repository, IRepository<YaeherConsultation> consrepository, IRepository<SystemParameter> sysrepository)
        {
            _repository = repository;
            _sysrepository = sysrepository;
            _consrepository = consrepository;
        }

        /// <summary>
        /// 查询处理质控 List
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<QualityControlManage>> QualityControlManageList(QualityControlManageIn QualityControlManageInfo)
        {
            var QualityControlManages = await _repository.GetAll().Where(QualityControlManageInfo.Expression).OrderByDescending(t=>t.CreatedOn).ToListAsync();
            return QualityControlManages;
        }

        /// <summary>
        /// 查询处理质控byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityControlManage> QualityControlManageByID(int Id)
        {
            var QualityControlManages = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return QualityControlManages;
        }
        /// <summary>
        /// 查询处理质控byConsultId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityControlManage> QualityControlManageByConsultStateID(int Id)
        {
            var QualityControlManages = await _repository.FirstOrDefaultAsync(t => !t.IsDelete && t.ConsultID == Id  &&t.ReplyState== "untreated");
            return QualityControlManages;
        }
        
        /// <summary>
        /// 查询处理质控 page
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QualityControlManagePage>> QualityControlManagePage(QualityControlManageIn QualityControlManageInfo)
        {
            //初步过滤
            var consquery = _consrepository.GetAll().Where(a => a.IsDelete == false);

            //初步过滤
            var query = _repository.GetAll().Where(QualityControlManageInfo.Expression).OrderByDescending(a => a.CreatedOn);

            var sysquery = _sysrepository.GetAll().Where(a=>!a.IsDelete && a.SystemCode== "QualityControlManageState");

            var querylist = from a in query
                            join b in consquery on a.ConsultID equals b.Id
                            join c in sysquery on a.ReplyState equals c.Code
                            select new QualityControlManagePage
                            {
                                Id=a.Id,
                                ConsultantJSON = b.ConsultantJSON,
                                PatientID = b.PatientID,
                                PatientName = b.PatientName,
                                PatientJSON=b.PatientJSON,
                                IIInessType = b.IIInessType,
                                PatientCity = b.PatientCity,
                                IIInessDescription = b.IIInessDescription,
                                InquiryTimes = b.InquiryTimes,
                                ConsultState = b.ConsultState,
                                OvertimeRemindTimes = b.OvertimeRemindTimes,
                                Overtime = b.Overtime,
                                RefundBy = b.RefundBy,
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
                                Completetime = b.Completetime,
                                DoctorID = a.DoctorID,
                                DoctorName = a.DoctorName,
                                CreatedOn = a.CreatedOn,
                                ConsultNumber = a.ConsultNumber,
                                ConsultID = a.ConsultID,
                                ConsultantID = a.ConsultantID,
                                ConsultantName = a.ConsultantName,
                                ConsultType = a.ConsultType,
                                QualityLevel = a.QualityLevel,
                                RepayIllnessDescription = a.RepayIllnessDescription,
                                ReplyState = a.ReplyState,
                                QualityControlManageState=c.Name,
                                QualityControlManageStateCode=c.Code,
                                QualityDoctor=a.DoctorName,
                            };
           //获取总数
           var tasksCount = querylist.Count();
            //获取总数
            var totalpage = tasksCount / QualityControlManageInfo.MaxResultCount;
            var QualityControlManageList = await querylist.PageBy(QualityControlManageInfo.SkipTotal, QualityControlManageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<QualityControlManagePage>(tasksCount, QualityControlManageList.MapTo<List<QualityControlManagePage>>());
        }
        /// <summary>
        /// 新建处理质控
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityControlManage> CreateQualityControlManage(QualityControlManage QualityControlManageInfo)
        {
            QualityControlManageInfo.Id = await _repository.InsertAndGetIdAsync(QualityControlManageInfo);
            return QualityControlManageInfo;
        }

        /// <summary>
        /// 修改处理质控
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityControlManage> UpdateQualityControlManage(QualityControlManage QualityControlManageInfo)
        {
            return await _repository.UpdateAsync(QualityControlManageInfo);
        }

        /// <summary>
        /// 删除处理质控
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityControlManage> DeleteQualityControlManage(QualityControlManage QualityControlManageInfo)
        {
            return await _repository.UpdateAsync(QualityControlManageInfo);
        }
    }
}
