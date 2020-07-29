using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.Quality.Dto;
using Yaeher.DoctorQuality.Dto;
using Yaeher.ClinicManage.Dto;
using Yaeher.NumericalStatement;
using System;

namespace Yaeher.Quality
{
    /// <summary>
    /// 质控委员注册
    /// </summary>
    public class QualityCommitteeRegisterService : IQualityCommitteeRegisterService
    {
        private readonly IRepository<QualityCommitteeRegister> _repository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<YaeherDoctor> _docrepository;
        private readonly IRepository<QualityCommittee> _qualitycomrepository;
        private readonly IRepository<EvaluationTotal> _EvaluationTotalrepository;

        /// <summary>
        /// 质控委员注册构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userrepository"></param>
        /// <param name="docrepository"></param>
        /// <param name="qualitycomrepository"></param>
        /// <param name="EvaluationTotalrepository"></param>
        public QualityCommitteeRegisterService(IRepository<QualityCommitteeRegister> repository,
            IRepository<YaeherUser> userrepository,
            IRepository<YaeherDoctor> docrepository,
             IRepository<QualityCommittee> qualitycomrepository,
             IRepository<EvaluationTotal> EvaluationTotalrepository)
        {
            _repository = repository;
            _userrepository = userrepository;
            _docrepository = docrepository;
            _qualitycomrepository = qualitycomrepository;
            _EvaluationTotalrepository = EvaluationTotalrepository;
        }

        /// <summary>
        /// 查询质控委员注册 List
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<QualityCommitteeRegister>> QualityCommitteeRegisterList(QualityCommitteeRegisterIn QualityCommitteeRegisterInfo)
        {
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(QualityCommitteeRegisterInfo.Expression);
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.ClinicName))
            {
                query = query.Where(a => a.ClinicName.Contains(QualityCommitteeRegisterInfo.ClinicName));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.DoctorName))
            {
                query = query.Where(a => a.DoctorName.Contains(QualityCommitteeRegisterInfo.DoctorName));
            }
            var QualityCommitteeRegisters = await _repository.GetAllListAsync(QualityCommitteeRegisterInfo.Expression);
            return await query.OrderByDescending(t=>t.CreatedOn).ToListAsync();
        }

        /// <summary>
        /// 查询质控委员注册byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommitteeRegister> QualityCommitteeRegisterByID(int Id)
        {
            var QualityCommitteeRegisters = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return QualityCommitteeRegisters;
        }
        /// <summary>
        /// 查询质控委员注册byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityManager> QualityCommitteeRegisterDoctorMsgByID(int Id)
        {
            var doc = _docrepository.GetAll().Where(t => !t.IsDelete);
            var user = _userrepository.GetAll().Where(t => !t.IsDelete);
            var register = _repository.GetAll().Where(t => !t.IsDelete);
            DateTime EndTime = DateTime.Now;
            var Evaluation = _EvaluationTotalrepository.GetAll().Where(a => a.CreatedOn >= EndTime.AddDays(-1) && a.CreatedOn < EndTime);
            var query = from a in doc
                        join b in user on a.UserID equals b.Id
                        join d in register on a.Id equals d.DoctorID
                        where a.CheckRes == "success" && d.Id == Id && a.AuthCheckRes == "success"
                        select new QualityManager
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Department = a.Department,
                            Title = a.Title,
                            Status = "1",
                            Id = a.Id,
                            QualityControlId = d.Id,
                            ApplyState = d.ApplyState,
                            CreatedOn = d.CreatedOn,
                            CheckState = d.CheckState,
                            QualityCommitteeRegisterId = d.Id,
                            QualityApplyRemark = d.ApplyRemark,
                        };
            // 计算医生评分
            var QualityCommitteeRegisters =await  query.FirstOrDefaultAsync();
            if (QualityCommitteeRegisters != null)
            {
                QualityCommitteeRegisters.AverageEvaluate = 0;
                var DoctorEvaluation = Evaluation.Where(a => a.DoctorID == QualityCommitteeRegisters.Id).FirstOrDefault();
                if (DoctorEvaluation != null)
                {
                    var EvaluationCount = DoctorEvaluation.OneStar + DoctorEvaluation.TwoStar + DoctorEvaluation.ThreeStar + DoctorEvaluation.FourStar + DoctorEvaluation.FiveStar;
                    if (EvaluationCount >= 15)
                    {
                        QualityCommitteeRegisters.AverageEvaluate = DoctorEvaluation.AverageEvaluate;
                    }
                } 
            }
            return QualityCommitteeRegisters;
        }
        /// <summary>
        /// 查询质控委员注册 page
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QualityCommitteeRegister>> QualityCommitteeRegisterPage(QualityCommitteeRegisterIn QualityCommitteeRegisterInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(a=>a.IsDelete==false);
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.ClinicName))
            {
                query = query.Where(a => a.ClinicName.Contains(QualityCommitteeRegisterInfo.ClinicName));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeRegisterInfo.DoctorName))
            {
                query = query.Where(a => a.DoctorName.Contains(QualityCommitteeRegisterInfo.DoctorName));
            }
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / QualityCommitteeRegisterInfo.MaxResultCount;
            var QualityCommitteeRegisterList = await query.PageBy(QualityCommitteeRegisterInfo.SkipTotal, QualityCommitteeRegisterInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<QualityCommitteeRegister>(tasksCount, QualityCommitteeRegisterList.MapTo<List<QualityCommitteeRegister>>());
        }
        /// <summary>
        /// 新建质控委员注册
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommitteeRegister> CreateQualityCommitteeRegister(QualityCommitteeRegister QualityCommitteeRegisterInfo)
        {
            QualityCommitteeRegisterInfo.Id= await _repository.InsertAndGetIdAsync(QualityCommitteeRegisterInfo);
            return QualityCommitteeRegisterInfo;
        }
        /// <summary>
        /// 质控端质控委员申请列表
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QualityManager>> QualityDoctorRegisterInformation(QualityCommitteeRegisterIn clinic, IList<DoctorRelation> rel)
        {
            var doc = _docrepository.GetAll().Where(t=>!t.IsDelete);
            var user = _userrepository.GetAll().Where(t => !t.IsDelete);
            var register = _repository.GetAll().Where(clinic.Expression);

            var query = from a in doc 
                        join b in user on a.UserID equals b.Id
                       join d in register on a.Id equals d.DoctorID
                        where  a.CheckRes == "success" && a.AuthCheckRes == "success"
                        select new QualityManager
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Department = a.Department,
                            Title = a.Title,
                            Status = "1",
                            Id = a.Id,
                            QualityControlId = d.Id,
                            CreatedOn = d.CreatedOn,
                            CheckState = d.CheckState,
                            QualityCommitteeRegisterId = d.Id
                        };

            query = query.OrderBy(a=>a.CheckState).ThenByDescending(a=>a.QualityControlId);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / clinic.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(clinic.SkipTotal, clinic.MaxResultCount).ToListAsync();
            return new PagedResultDto<QualityManager>(tasksCount, ClinicInfomationList.MapTo<List<QualityManager>>());
        }
        /// <summary>
        /// 根据指控查询质控委员列表
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QualityManager>> QualityDoctorInformation(QualityCommitteeIn clinic, IList<DoctorRelation> rel)
        {
            var doc = _docrepository.GetAll();
            var user = _userrepository.GetAll();
            var qualitycom = _qualitycomrepository.GetAll().Where(clinic.Expression);

            var query =from a in doc 
                        join b in user on a.UserID equals b.Id
                        join d in qualitycom on a.Id equals d.DoctorID
                        where !a.IsDelete && !b.IsDelete  && !d.IsDelete && a.CheckRes == "success" && d.QualityState == "success" && a.AuthCheckRes == "success"
                        select new QualityManager
                        {
                            UserImage = b.UserImage,
                            DoctorName = a.DoctorName,
                            DoctorLevel = "4.1",
                            UserID = b.Id,
                            HospitalName = a.HospitalName,
                            Department = a.Department,
                            Title = a.Title,
                            Status = "1",
                            Id = a.Id,
                            QualityControlId = d.Id,
                            CreatedOn = d.CreatedOn
                        };
            if (!string.IsNullOrEmpty(clinic.KeyWord))
            {
                var labelrel = new int[rel.Count];
                for (var i = 0; i < rel.Count(); i++)
                {
                    labelrel[i] = rel[i].DoctorID;
                }
                if (labelrel.Count() > 0)
                {
                    query = query.OrderByDescending(a => a.CreatedOn).Where(t => labelrel.Contains(t.Id));
                }
                else
                {
                    query = query.OrderByDescending(a => a.CreatedOn).Where(t => t.DoctorName.Contains(clinic.KeyWord));
                }
            }
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / clinic.MaxResultCount;
            var ClinicInfomationList = await query.PageBy(clinic.SkipTotal, clinic.MaxResultCount).ToListAsync();
            return new PagedResultDto<QualityManager>(tasksCount, ClinicInfomationList.MapTo<List<QualityManager>>());

        }
        /// <summary>
        /// 修改质控委员注册
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommitteeRegister> UpdateQualityCommitteeRegister(QualityCommitteeRegister QualityCommitteeRegisterInfo)
        {
            return await _repository.UpdateAsync(QualityCommitteeRegisterInfo);
        }

        /// <summary>
        /// 删除质控委员注册
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommitteeRegister> DeleteQualityCommitteeRegister(QualityCommitteeRegister QualityCommitteeRegisterInfo)
        {
            return await _repository.UpdateAsync(QualityCommitteeRegisterInfo);
        }
    }
}
