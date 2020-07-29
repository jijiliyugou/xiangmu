using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.SystemManage.Dto;
using System;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生审核
    /// </summary>
    public class DoctorCheckService : IDoctorCheckService
    {
        private readonly IRepository<DoctorCheck> _repository;
        /// <summary>
        /// 医生审核 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorCheckService(IRepository<DoctorCheck> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生审核 List
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorCheck>> DoctorCheckList(DoctorCheckIn DoctorCheckInfo)
        {
            //初步过滤
            var DoctorChecks = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorCheckInfo.Expression);
            return await DoctorChecks.ToListAsync();
        }

        /// <summary>
        /// 查询医生审核byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorCheck> DoctorCheckByID(int Id)
        {
            var DoctorChecks = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorChecks;
        }
        /// <summary>
        /// 查询医生审核 page
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorCheck>> DoctorCheckPage(DoctorCheckIn DoctorCheckInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorCheckInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorCheckInfo.MaxResultCount;
            var DoctorCheckList = await query.PageBy(DoctorCheckInfo.SkipTotal, DoctorCheckInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorCheck>(tasksCount, DoctorCheckList.MapTo<List<DoctorCheck>>());
        }
        /// <summary>
        /// 新建医生审核
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorCheck> CreateDoctorCheck(DoctorCheck DoctorCheckInfo)
        {
            DoctorCheckInfo.Id= await _repository.InsertAndGetIdAsync(DoctorCheckInfo);
            return DoctorCheckInfo;
        }

        /// <summary>
        /// 修改医生审核
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorCheck> UpdateDoctorCheck(DoctorCheck DoctorCheckInfo)
        {
            return await _repository.UpdateAsync(DoctorCheckInfo);
        }

        /// <summary>
        /// 删除医生审核
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorCheck> DeleteDoctorCheck(DoctorCheck DoctorCheckInfo)
        {
            return await _repository.UpdateAsync(DoctorCheckInfo);
        }
    }
}
