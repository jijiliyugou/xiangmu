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
    /// 医生上下线设置
    /// </summary>
    public class DoctorOnlineRecordService : IDoctorOnlineRecordService
    {
        private readonly IRepository<DoctorOnlineRecord> _repository;
        /// <summary>
        /// 医生上下线设置 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorOnlineRecordService(IRepository<DoctorOnlineRecord> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生上下线设置 List
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorOnlineRecord>> DoctorOnlineRecordList(DoctorOnlineRecordIn DoctorOnlineRecordInfo)
        {
            var DoctorOnlineRecords = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorOnlineRecordInfo.Expression);
            return await DoctorOnlineRecords.ToListAsync();
        }

        /// <summary>
        /// 查询医生上下线设置byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineRecord> DoctorOnlineRecordByID(int Id)
        {
            var DoctorOnlineRecords = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorOnlineRecords;
        }
        /// <summary>
        /// 查询医生上下线设置byDoctorId
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineRecord> DoctorOnlineRecordByDoctorID(int DoctorId)
        {
            var DoctorOnlineRecords = await _repository.FirstOrDefaultAsync(t => t.DoctorID == DoctorId && !t.IsDelete);
            return DoctorOnlineRecords;
        }
        
        /// <summary>
        /// 查询医生上下线设置byId
        /// </summary>
        /// <param name="DoctorID"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineRecord> DoctorOnlineRecordDoctorID(int DoctorID)
        {
            var DoctorOnlineRecords = await _repository.FirstOrDefaultAsync(t => t.DoctorID == DoctorID && !t.IsDelete);
            return DoctorOnlineRecords;
        }

        

        /// <summary>
        /// 查询医生上下线设置 page
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorOnlineRecord>> DoctorOnlineRecordPage(DoctorOnlineRecordIn DoctorOnlineRecordInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorOnlineRecordInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorOnlineRecordInfo.MaxResultCount;
            var DoctorOnlineRecordList = await query.PageBy(DoctorOnlineRecordInfo.SkipTotal, DoctorOnlineRecordInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorOnlineRecord>(tasksCount, DoctorOnlineRecordList.MapTo<List<DoctorOnlineRecord>>());
        }
        /// <summary>
        /// 新建医生上下线设置
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineRecord> CreateDoctorOnlineRecord(DoctorOnlineRecord DoctorOnlineRecordInfo)
        {
            DoctorOnlineRecordInfo.Id= await _repository.InsertAndGetIdAsync(DoctorOnlineRecordInfo);
            return DoctorOnlineRecordInfo;
        }

        /// <summary>
        /// 修改医生上下线设置
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineRecord> UpdateDoctorOnlineRecord(DoctorOnlineRecord DoctorOnlineRecordInfo)
        {
            return await _repository.UpdateAsync(DoctorOnlineRecordInfo);
        }

        /// <summary>
        /// 删除医生上下线设置
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorOnlineRecord> DeleteDoctorOnlineRecord(DoctorOnlineRecord DoctorOnlineRecordInfo)
        {
            return await _repository.UpdateAsync(DoctorOnlineRecordInfo);
        }
    }
}
