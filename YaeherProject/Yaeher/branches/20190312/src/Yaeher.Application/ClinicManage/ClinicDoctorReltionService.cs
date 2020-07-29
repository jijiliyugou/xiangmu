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

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 门诊与医生关系
    /// </summary>
    public class ClinicDoctorReltionService : IClinicDoctorReltionService
    {
        private readonly IRepository<ClinicDoctorReltion> _repository;
        private readonly IRepository<ClinicInfomation> _clinicrepository;
        /// <summary>
        /// 门诊与医生关系构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="clinicrepository"></param>
        public ClinicDoctorReltionService(IRepository<ClinicDoctorReltion> repository, IRepository<ClinicInfomation>  clinicrepository)
        {
            _repository = repository;
            _clinicrepository = clinicrepository;
        }
        /// <summary>
        /// 查询门诊与医生关系 List
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ClinicDoctorReltion>> ClinicDoctorReltionList(ClinicDoctorReltionIn ClinicDoctorReltionInfo)
        {
            var query = _repository.GetAll().Where(ClinicDoctorReltionInfo.Expression).OrderByDescending(a => a.CreatedOn);
            return await query.ToListAsync();
        }
        /// <summary>
        /// 查询医生所有门诊 List
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<DoctorClinicApplyOutDetail>> ClinicDoctorReltionApplyList(ClinicDoctorReltionIn ClinicDoctorReltionInfo)
        {
            var query1 = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ClinicDoctorReltionInfo.Expression);
            var clinicquery = _clinicrepository.GetAll().OrderByDescending(a => a.CreatedOn).Where(t=>t.IsDelete==false);

            var query = from item in query1
                        join b in clinicquery on item.ClinicID equals b.Id
                        select new DoctorClinicApplyOutDetail
                        {
                            DoctorName = item.DoctorName,
                            DoctorID = item.DoctorID,
                            ApplyType = "",
                            ClinicID = item.ClinicID,
                            ClinicName = item.ClinicName,
                            ApplyRemark = "",
                            CheckRemark = "",
                            Id = item.Id,
                            CreatedOn = item.CreatedOn,
                            ClinicType=b.ClinicType,
                            CheckResCode = "",
                            CheckRes = "",
                        };
            return await query.ToListAsync();
        }


        /// <summary>
        /// 查询门诊与医生关系byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicDoctorReltion> ClinicDoctorReltionByID(int Id)
        {
            var ClinicDoctorReltions = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ClinicDoctorReltions;
        }
        /// <summary>
        /// 查询门诊与医生关系 page
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicDoctorReltion>> ClinicDoctorReltionPage(ClinicDoctorReltionIn ClinicDoctorReltionInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ClinicDoctorReltionInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ClinicDoctorReltionInfo.MaxResultCount;
            var ClinicDoctorReltionList = await query.PageBy(ClinicDoctorReltionInfo.SkipTotal, ClinicDoctorReltionInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ClinicDoctorReltion>(tasksCount, ClinicDoctorReltionList.MapTo<List<ClinicDoctorReltion>>());
        }
        /// <summary>
        /// 新建门诊与医生关系
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicDoctorReltion> CreateClinicDoctorReltion(ClinicDoctorReltion ClinicDoctorReltionInfo)
        {
            ClinicDoctorReltionInfo.Id = await _repository.InsertAndGetIdAsync(ClinicDoctorReltionInfo);
            return ClinicDoctorReltionInfo;
        }

        /// <summary>
        /// 修改门诊与医生关系
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicDoctorReltion> UpdateClinicDoctorReltion(ClinicDoctorReltion ClinicDoctorReltionInfo)
        {
            return await _repository.UpdateAsync(ClinicDoctorReltionInfo);
        }

        /// <summary>
        /// 删除门诊与医生关系
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicDoctorReltion> DeleteClinicDoctorReltion(ClinicDoctorReltion ClinicDoctorReltionInfo)
        {
            return await _repository.UpdateAsync(ClinicDoctorReltionInfo);
        }
    }
}
