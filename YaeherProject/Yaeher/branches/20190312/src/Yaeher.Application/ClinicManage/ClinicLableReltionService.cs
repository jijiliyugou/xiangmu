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

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 门诊与标签关系
    /// </summary>
    public class ClinicLableReltionService : IClinicLableReltionService
    {
        private readonly IRepository<ClinicLableReltion> _repository;
        /// <summary>
        /// 门诊与标签关系构造函数
        /// </summary>
        /// <param name="repository"></param>
        public ClinicLableReltionService(IRepository<ClinicLableReltion> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询门诊与标签关系 List
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ClinicLableReltion>> ClinicDoctorReltionList(ClinicLableReltionIn ClinicLableReltionInfo)
        {
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ClinicLableReltionInfo.Expression);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 查询门诊与标签关系 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicLableReltion> ClinicDoctorReltionByID(int Id)
        {
            var ClinicLableReltions = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ClinicLableReltions;
        }
        /// <summary>
        /// 查询门诊与标签关系 Page
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ClinicLableReltion>> ClinicDoctorReltionPage(ClinicLableReltionIn ClinicLableReltionInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ClinicLableReltionInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ClinicLableReltionInfo.MaxResultCount;
            var ClinicLableReltionList = await query.PageBy(ClinicLableReltionInfo.SkipTotal, ClinicLableReltionInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ClinicLableReltion>(tasksCount, ClinicLableReltionList.MapTo<List<ClinicLableReltion>>());
        }
        /// <summary>
        /// 新建门诊与标签关系
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicLableReltion> CreateClinicDoctorReltion(ClinicLableReltion ClinicLableReltionInfo)
        {
            ClinicLableReltionInfo.Id= await _repository.InsertAndGetIdAsync(ClinicLableReltionInfo);
            return ClinicLableReltionInfo;
        }

        /// <summary>
        /// 修改门诊与标签关系
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicLableReltion> UpdateClinicDoctorReltion(ClinicLableReltion ClinicLableReltionInfo)
        {
            return await _repository.UpdateAsync(ClinicLableReltionInfo);
        }

        /// <summary>
        /// 删除门诊与标签关系
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ClinicLableReltion> DeleteClinicDoctorReltion(ClinicLableReltion ClinicLableReltionInfo)
        {
            return await _repository.UpdateAsync(ClinicLableReltionInfo);
        }
    }
}
