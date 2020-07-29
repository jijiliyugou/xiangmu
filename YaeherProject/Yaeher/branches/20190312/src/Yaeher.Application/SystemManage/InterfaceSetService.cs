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
    /// 接口管理
    /// </summary>
    public class InterfaceSetService : IInterfaceSetService
    {
        private readonly IRepository<InterfaceSet> _repository;
        /// <summary>
        /// 接口管理 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public InterfaceSetService(IRepository<InterfaceSet> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询接口管理 List
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<InterfaceSet>> InterfaceSetList(InterfaceSetIn InterfaceSetInfo)
        {
            //初步过滤
            var InterfaceSets = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(InterfaceSetInfo.Expression);
            return await InterfaceSets.ToListAsync();
        }

        /// <summary>
        /// 查询接口管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<InterfaceSet> InterfaceSetByID(int Id)
        {
            var InterfaceSets = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return InterfaceSets;
        }
        /// <summary>
        /// 查询接口管理 page
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<InterfaceSet>> InterfaceSetPage(InterfaceSetIn InterfaceSetInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(InterfaceSetInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / InterfaceSetInfo.MaxResultCount;
            var InterfaceSetList = await query.PageBy(InterfaceSetInfo.SkipTotal, InterfaceSetInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<InterfaceSet>(tasksCount, InterfaceSetList.MapTo<List<InterfaceSet>>());
        }
        /// <summary>
        /// 新建接口管理
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<InterfaceSet> CreateInterfaceSet(InterfaceSet InterfaceSetInfo)
        {
            InterfaceSetInfo.Id= await _repository.InsertAndGetIdAsync(InterfaceSetInfo);
            return InterfaceSetInfo;
        }

        /// <summary>
        /// 修改接口管理
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<InterfaceSet> UpdateInterfaceSet(InterfaceSet InterfaceSetInfo)
        {
            return await _repository.UpdateAsync(InterfaceSetInfo);
        }

        /// <summary>
        /// 删除接口管理
        /// </summary>
        /// <param name="InterfaceSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<InterfaceSet> DeleteInterfaceSet(InterfaceSet InterfaceSetInfo)
        {
            return await _repository.UpdateAsync(InterfaceSetInfo);
        }
    }
}
