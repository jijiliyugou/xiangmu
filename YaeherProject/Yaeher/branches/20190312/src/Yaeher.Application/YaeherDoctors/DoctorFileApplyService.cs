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
    /// 医生上传附件
    /// </summary>
    public class DoctorFileApplyService : IDoctorFileApplyService
    {
        private readonly IRepository<DoctorFileApply> _repository;
        /// <summary>
        /// 医生上传附件 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorFileApplyService(IRepository<DoctorFileApply> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生上传附件 List
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<DoctorFileApply>> DoctorFileApplyList(DoctorFileApplyIn DoctorFileApplyInfo)
        {
            var DoctorFileApplys = await _repository.GetAllListAsync(DoctorFileApplyInfo.Expression);
            return DoctorFileApplys;
        }

        /// <summary>
        /// 查询医生上传附件byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorFileApply> DoctorFileApplyByID(int Id)
        {
            var DoctorFileApplys = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorFileApplys;
        }
        /// <summary>
        /// 查询医生上传附件 page
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorFileApply>> DoctorFileApplyPage(DoctorFileApplyIn DoctorFileApplyInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorFileApplyInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorFileApplyInfo.MaxResultCount;
            var DoctorFileApplyList = await query.PageBy(DoctorFileApplyInfo.SkipTotal, DoctorFileApplyInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorFileApply>(tasksCount, DoctorFileApplyList.MapTo<List<DoctorFileApply>>());
        }
        /// <summary>
        /// 新建医生上传附件
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorFileApply> CreateDoctorFileApply(DoctorFileApply DoctorFileApplyInfo)
        {
            DoctorFileApplyInfo.Id= await _repository.InsertAndGetIdAsync(DoctorFileApplyInfo);
            return DoctorFileApplyInfo;
        }

        /// <summary>
        /// 修改医生上传附件
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorFileApply> UpdateDoctorFileApply(DoctorFileApply DoctorFileApplyInfo)
        {
            return await _repository.UpdateAsync(DoctorFileApplyInfo);
        }

        /// <summary>
        /// 删除医生上传附件
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorFileApply> DeleteDoctorFileApply(DoctorFileApply DoctorFileApplyInfo)
        {
            return await _repository.UpdateAsync(DoctorFileApplyInfo);
        }
    }
}
