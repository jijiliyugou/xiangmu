using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.Scheduling.Dto;

namespace Yaeher.Scheduling
{
    /// <summary>
    /// 医生排班
    /// </summary>
    public class DoctorSchedulingService : IDoctorSchedulingService
    {
        private readonly IRepository<DoctorScheduling> _repository;
        /// <summary>
        /// 医生排班 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorSchedulingService(IRepository<DoctorScheduling> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生排班 List
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorScheduling>> DoctorSchedulingList(DoctorSchedulingIn DoctorSchedulingInfo)
        {
            var DoctorSchedulings = await _repository.GetAllListAsync(DoctorSchedulingInfo.Expression);
            return DoctorSchedulings.ToList();
        }

        /// <summary>
        /// 查询医生排班byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorScheduling> DoctorSchedulingByID(int Id)
        {
            var DoctorSchedulings = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorSchedulings;
        }
        /// <summary>
        /// 查询医生排班 page
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorScheduling>> DoctorSchedulingPage(DoctorSchedulingIn DoctorSchedulingInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorSchedulingInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorSchedulingInfo.MaxResultCount;
            var DoctorSchedulingList = await query.PageBy(DoctorSchedulingInfo.SkipTotal, DoctorSchedulingInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorScheduling>(tasksCount, DoctorSchedulingList.MapTo<List<DoctorScheduling>>());
        }
        /// <summary>
        /// 新建医生排班
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorScheduling> CreateDoctorScheduling(DoctorScheduling DoctorSchedulingInfo)
        {
            DoctorSchedulingInfo.Id= await _repository.InsertAndGetIdAsync(DoctorSchedulingInfo);
            return DoctorSchedulingInfo;
        }

        /// <summary>
        /// 修改医生排班
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorScheduling> UpdateDoctorScheduling(DoctorScheduling DoctorSchedulingInfo)
        {
            return await _repository.UpdateAsync(DoctorSchedulingInfo);
        }

        /// <summary>
        /// 删除医生排班
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorScheduling> DeleteDoctorScheduling(DoctorScheduling DoctorSchedulingInfo)
        {
            return await _repository.UpdateAsync(DoctorSchedulingInfo);
        }
    }
}
