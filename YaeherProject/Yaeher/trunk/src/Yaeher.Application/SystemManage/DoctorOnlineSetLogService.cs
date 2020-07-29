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

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生上下线设置log
    /// </summary>
    public class DoctorOnlineSetLogService : IDoctorOnlineSetLogService
    {
        private readonly IRepository<DoctorOnlineSetLog> _repository;
        /// <summary>
        /// 医生上下线设置log 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorOnlineSetLogService(IRepository<DoctorOnlineSetLog> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生上下线设置log List
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorOnlineSetLog>> DoctorOnlineSetLogList(DoctorOnlineSetLogIn DoctorOnlineSetLogInfo)
        {
            var DoctorOnlineSetLogs = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorOnlineSetLogInfo.Expression);
            return await  DoctorOnlineSetLogs.ToListAsync();
        }

        /// <summary>
        /// 查询医生上下线设置logbyId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineSetLog> DoctorOnlineSetLogByID(int Id)
        {
            var DoctorOnlineSetLogs = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorOnlineSetLogs;
        }
        /// <summary>
        /// 查询医生上下线设置log page
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorOnlineSetLog>> DoctorOnlineSetLogPage(DoctorOnlineSetLogIn DoctorOnlineSetLogInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorOnlineSetLogInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorOnlineSetLogInfo.MaxResultCount;
            var DoctorOnlineSetLogList = await query.PageBy(DoctorOnlineSetLogInfo.SkipTotal, DoctorOnlineSetLogInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorOnlineSetLog>(tasksCount, DoctorOnlineSetLogList.MapTo<List<DoctorOnlineSetLog>>());
        }
        /// <summary>
        /// 新建医生上下线设置log
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineSetLog> CreateDoctorOnlineSetLog(DoctorOnlineSetLog DoctorOnlineSetLogInfo)
        {
            DoctorOnlineSetLogInfo.Id= await _repository.InsertAndGetIdAsync(DoctorOnlineSetLogInfo);
            return DoctorOnlineSetLogInfo;
        }

        /// <summary>
        /// 修改医生上下线设置log
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineSetLog> UpdateDoctorOnlineSetLog(DoctorOnlineSetLog DoctorOnlineSetLogInfo)
        {
            return await _repository.UpdateAsync(DoctorOnlineSetLogInfo);
        }

        /// <summary>
        /// 删除医生上下线设置log
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineSetLog> DeleteDoctorOnlineSetLog(DoctorOnlineSetLog DoctorOnlineSetLogInfo)
        {
            return await _repository.UpdateAsync(DoctorOnlineSetLogInfo);
        }
    }
}
