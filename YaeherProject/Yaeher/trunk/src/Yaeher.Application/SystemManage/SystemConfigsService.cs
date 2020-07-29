using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.AutoMapper;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    public class SystemConfigsService : ISystemConfigsService
    {
        private readonly IRepository<SystemConfigs> _repository;
        /// <summary>
        /// 系统配置表 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public SystemConfigsService(IRepository<SystemConfigs> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 系统配置表 List
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<SystemConfigs>> SystemConfigsList(SystemConfigsIn SystemConfigsInfo)
        {
            var AreaManages = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(SystemConfigsInfo.Expression);
            return await AreaManages.ToListAsync();
        }
        /// <summary>
        /// 系统配置表 page
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<SystemConfigs>> SystemConfigsPage(SystemConfigsIn SystemConfigsInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(SystemConfigsInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / SystemConfigsInfo.MaxResultCount;
            var AreaManageList = await query.PageBy(SystemConfigsInfo.SkipTotal, SystemConfigsInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<SystemConfigs>(tasksCount, AreaManageList.MapTo<List<SystemConfigs>>());
        }
        /// <summary>
        /// 系统配置表byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemConfigs> SystemConfigsByID(int Id)
        {
            var SystemConfigss = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return SystemConfigss;
        }
        /// <summary>
        /// 新建 系统配置表
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemConfigs> CreateSystemConfigs(SystemConfigs SystemConfigsInfo)
        {
            SystemConfigsInfo.Id = await _repository.InsertAndGetIdAsync(SystemConfigsInfo);
            return SystemConfigsInfo;
        }

        /// <summary>
        /// 修改系统配置表
        /// </summary>
        /// <param name="SystemConfigsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<SystemConfigs> UpdateSystemConfigs(SystemConfigs SystemConfigsInfo)
        {
            return await _repository.UpdateAsync(SystemConfigsInfo);
        }
    }
}
