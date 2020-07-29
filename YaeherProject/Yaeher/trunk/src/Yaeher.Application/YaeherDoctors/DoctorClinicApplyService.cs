using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生申请门诊
    /// </summary>
    public class DoctorClinicApplyService : IDoctorClinicApplyService
    {
        private readonly IRepository<DoctorClinicApply> _repository;
        private readonly IRepository<SystemParameter> _pararepository;
        private readonly IRepository<YaeherUser> _userrepository;
        private readonly IRepository<ClinicInfomation> _clinicrepository;
        private readonly IRepository<YaeherDoctor> _doctorrepository;
      /// <summary>
      /// 医生申请门诊 构造函数
      /// </summary>
      /// <param name="repository"></param>
      /// <param name="pararepository"></param>
      /// <param name="userrepository"></param>
      /// <param name="doctorrepository"></param>
      /// <param name="clinicrepository"></param>
        public DoctorClinicApplyService(
            IRepository<DoctorClinicApply> repository,
            IRepository<SystemParameter> pararepository, 
            IRepository<YaeherUser> userrepository, 
            IRepository<YaeherDoctor> doctorrepository,
            IRepository<ClinicInfomation> clinicrepository)
        {
            _repository = repository;
            _pararepository = pararepository;
            _userrepository=userrepository;
            _doctorrepository=doctorrepository;
           _clinicrepository= clinicrepository;
        }
        /// <summary>
        /// 医生申请门诊 List
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<DoctorClinicApply>> DoctorClinicApplyList(DoctorClinicApplyIn DoctorClinicApplyInfo)
        {
            var DoctorClinicApplys = await _repository.GetAllListAsync(DoctorClinicApplyInfo.Expression);
            return DoctorClinicApplys.ToList();
        }

        /// <summary>
        /// 医生申请门诊byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorClinicApply> DoctorClinicApplyByID(int Id)
        {
            var DoctorClinicApplys = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorClinicApplys;
        }
        /// <summary>
        /// 医生申请门诊 page
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<DoctorClinicApplyOutDetail>> DoctorClinicApplyOutDetailList(DoctorClinicApplyIn DoctorClinicApplyInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().Where(DoctorClinicApplyInfo.Expression).OrderByDescending(t => t.CreatedOn);
            var syspara = _pararepository.GetAll().Where(t=>!t.IsDelete&&t.SystemCode== "CheckType");
            var clinicquery = _clinicrepository.GetAll().Where(t => !t.IsDelete);
            var query = from a in query1
                        join b in syspara on a.CheckRes equals b.Code
                        join c in clinicquery on a.ClinicID equals c.Id
                        select new DoctorClinicApplyOutDetail
                        {
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            ApplyType = a.ApplyType,
                            ClinicID = a.ClinicID,
                            ClinicName = a.ClinicName,
                            ApplyRemark = a.ApplyRemark,
                            CheckTime = a.CheckTime,
                            CheckRemark = a.CheckRemark,
                            ClinicType=c.ClinicType,
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                            CheckResCode = a.CheckRes,
                            CheckRes = b.Name,
                           
                        };
            return await query.ToListAsync();
        }
        /// <summary>
        /// 医生申请门诊 page
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorClinicApplyOutDetail>> DoctorClinicApplyOutDetailPage(DoctorClinicApplyIn DoctorClinicApplyInfo)
        {
            //初步过滤
            var query1 = _repository.GetAll().Where(DoctorClinicApplyInfo.Expression).OrderByDescending(t => t.CreatedOn);
            var syspara = _pararepository.GetAll().Where(t => !t.IsDelete && t.SystemCode == "CheckType");
            var user = _userrepository.GetAll().Where(t=>!t.IsDelete);
            var doctor = _doctorrepository.GetAll().Where(t => !t.IsDelete);
            var query = from a in query1
                        join b in syspara on a.CheckRes equals b.Code
                        join c in doctor on a.DoctorID equals c.Id
                        join d in user on c.UserID equals d.Id
                        select new DoctorClinicApplyOutDetail
                        {
                            DoctorName = a.DoctorName,
                            DoctorID = a.DoctorID,
                            ApplyType = a.ApplyType,
                            ClinicID = a.ClinicID,
                            ClinicName = a.ClinicName,
                            ApplyRemark = a.ApplyRemark,
                            CheckTime = a.CheckTime,
                            CheckRemark = a.CheckRemark,
                            Id = a.Id,
                            CreatedOn = a.CreatedOn,
                            CheckResCode = a.CheckRes,
                            CheckRes = b.Name,
                            UserImage=d.UserImage,
                        };
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorClinicApplyInfo.MaxResultCount;
            var DoctorClinicApplyList = await query.PageBy(DoctorClinicApplyInfo.SkipTotal, DoctorClinicApplyInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorClinicApplyOutDetail>(tasksCount, DoctorClinicApplyList.MapTo<List<DoctorClinicApplyOutDetail>>());
        }
        
        /// <summary>
        /// 新建医生申请门诊
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorClinicApply> CreateDoctorClinicApply(DoctorClinicApply DoctorClinicApplyInfo)
        {
            DoctorClinicApplyInfo.Id = await _repository.InsertAndGetIdAsync(DoctorClinicApplyInfo);
            return DoctorClinicApplyInfo;
        }

        /// <summary>
        /// 修改医生申请门诊
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorClinicApply> UpdateDoctorClinicApply(DoctorClinicApply DoctorClinicApplyInfo)
        {
            return await _repository.UpdateAsync(DoctorClinicApplyInfo);
        }

        /// <summary>
        /// 删除医生申请门诊
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorClinicApply> DeleteDoctorClinicApply(DoctorClinicApply DoctorClinicApplyInfo)
        {
            return await _repository.UpdateAsync(DoctorClinicApplyInfo);
        }
    }
}
