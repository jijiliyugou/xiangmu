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
    /// 医生服务log日志
    /// </summary>
    public class DoctorServiceLogService : IDoctorServiceLogService
    {
        private readonly IRepository<DoctorServiceLog> _repository;
        /// <summary>
        /// 医生服务log日志 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorServiceLogService(IRepository<DoctorServiceLog> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生服务log日志 List
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorServiceLog>> DoctorServiceLogList(DoctorServiceLogIn DoctorServiceLogInfo)
        {
            var DoctorServiceLogs = await _repository.GetAllListAsync(DoctorServiceLogInfo.Expression);
            return DoctorServiceLogs.ToList();
        }

        /// <summary>
        /// 查询医生服务log日志byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorServiceLog> DoctorServiceLogByID(int Id)
        {
            var DoctorServiceLogs = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorServiceLogs;
        }
        /// <summary>
        /// 查询医生服务log日志 page
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorServiceLog>> DoctorServiceLogPage(DoctorServiceLogIn DoctorServiceLogInfo)
        {
            //初步过滤
            var query = _repository.GetAll().Where(DoctorServiceLogInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorServiceLogInfo.MaxResultCount;
            var DoctorServiceLogList = await query.PageBy(DoctorServiceLogInfo.SkipTotal, DoctorServiceLogInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorServiceLog>(tasksCount, DoctorServiceLogList.MapTo<List<DoctorServiceLog>>());
        }
        /// <summary>
        /// 新建医生服务log日志
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorServiceLog> CreateDoctorServiceLog(DoctorServiceLog DoctorServiceLogInfo)
        {
            DoctorServiceLogInfo.Id= await _repository.InsertAndGetIdAsync(DoctorServiceLogInfo);
            return DoctorServiceLogInfo;
        }

        /// <summary>
        /// 修改医生服务log日志
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorServiceLog> UpdateDoctorServiceLog(DoctorServiceLog DoctorServiceLogInfo)
        {
            return await _repository.UpdateAsync(DoctorServiceLogInfo);
        }

        /// <summary>
        /// 删除医生服务log日志
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorServiceLog> DeleteDoctorServiceLog(DoctorServiceLog DoctorServiceLogInfo)
        {
            return await _repository.UpdateAsync(DoctorServiceLogInfo);
        }
    }
}
