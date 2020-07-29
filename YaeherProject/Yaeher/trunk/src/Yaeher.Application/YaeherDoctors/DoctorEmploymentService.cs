using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Doctor;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生执业记录
    /// </summary>
    public class DoctorEmploymentService : IDoctorEmploymentService
    {
        private readonly IRepository<DoctorEmployment> _repository;
        /// <summary>
        /// 医生上传附件 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorEmploymentService(IRepository<DoctorEmployment> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生执业记录 List
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorEmployment>> DoctorEmploymentList(DoctorEmploymentIn DoctorFileApplyInfo)
        {
            var DoctorFileApplys =  _repository.GetAll().Where(DoctorFileApplyInfo.Expression).OrderByDescending(t => t.CreatedOn);
            return await DoctorFileApplys.ToListAsync();
        }

        /// <summary>
        /// 查询医生执业记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorEmployment> DoctorEmploymentByID(int Id)
        {
            var DoctorFileApplys = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorFileApplys;
        }
        /// <summary>
        /// 查询医生执业记录 page
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorEmployment>> DoctorEmploymentPage(DoctorEmploymentIn DoctorFileApplyInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorFileApplyInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorFileApplyInfo.MaxResultCount;
            var DoctorFileApplyList = await query.PageBy(DoctorFileApplyInfo.SkipCount, DoctorFileApplyInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorEmployment>(tasksCount, DoctorFileApplyList.MapTo<List<DoctorEmployment>>());
        }
        /// <summary>
        /// 新建医生执业记录
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorEmployment> CreateDoctorEmployment(DoctorEmployment DoctorFileApplyInfo)
        {
            DoctorFileApplyInfo.Id = await _repository.InsertAndGetIdAsync(DoctorFileApplyInfo);
            return DoctorFileApplyInfo;
        }

        /// <summary>
        /// 修改医生执业记录
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorEmployment> UpdateDoctorEmployment(DoctorEmployment DoctorFileApplyInfo)
        {
            return await _repository.UpdateAsync(DoctorFileApplyInfo);
        }

        /// <summary>
        /// 删除医生执业记录
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorEmployment> DeleteDoctorEmployment(DoctorEmployment DoctorFileApplyInfo)
        {
            return await _repository.UpdateAsync(DoctorFileApplyInfo);
        }
    }
}
