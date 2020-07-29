using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemManage.Dto;
using System.Linq;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生参数接口
    /// </summary>
    public class DoctorParaSetService : IDoctorParaSetService
    {
        private readonly IRepository<DoctorParaSet> _repository;

        /// <summary>
        /// 医生参数接口 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorParaSetService(IRepository<DoctorParaSet> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 医生参数接口 List
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<DoctorParaSet>> DoctorParaSetList(DoctorParaSetIn DoctorParaSetInfo)
        {
            var DoctorParaSets = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorParaSetInfo.Expression);
            return await DoctorParaSets.ToListAsync();
        }

        /// <summary>
        /// 医生参数接口byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorParaSet> DoctorParaSetByID(int Id)
        {
            var DoctorParaSets = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorParaSets;
        }
        /// <summary>
        /// 医生参数接口 page
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorParaSet>> DoctorParaSetPage(DoctorParaSetIn DoctorParaSetInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorParaSetInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorParaSetInfo.MaxResultCount;
            var DoctorParaSetList = await query.PageBy(DoctorParaSetInfo.SkipTotal, DoctorParaSetInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorParaSet>(tasksCount, DoctorParaSetList.MapTo<List<DoctorParaSet>>());
        }
        /// <summary>
        /// 新建医生参数接口
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorParaSet> CreateDoctorParaSet(DoctorParaSet DoctorParaSetInfo)
        {
            DoctorParaSetInfo.Id = await _repository.InsertAndGetIdAsync(DoctorParaSetInfo);
            return DoctorParaSetInfo;
        }

        /// <summary>
        /// 修改医生参数接口
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorParaSet> UpdateDoctorParaSet(DoctorParaSet DoctorParaSetInfo)
        {
            return await _repository.UpdateAsync(DoctorParaSetInfo);
        }

        /// <summary>
        /// 删除医生参数接口
        /// </summary>
        /// <param name="DoctorParaSetInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorParaSet> DeleteDoctorParaSet(DoctorParaSet DoctorParaSetInfo)
        {
            return await _repository.UpdateAsync(DoctorParaSetInfo);
        }
    }
}
