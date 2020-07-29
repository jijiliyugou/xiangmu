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
    /// 医生提现记录
    /// </summary>
    public class DoctorWithdrawRecordService : IDoctorWithdrawRecordService
    {
        private readonly IRepository<DoctorWithdrawRecord> _repository;
        /// <summary>
        /// 医生提现记录 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public DoctorWithdrawRecordService(IRepository<DoctorWithdrawRecord> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询医生提现记录 List
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<DoctorWithdrawRecord>> DoctorWithdrawRecordList(DoctorWithdrawRecordIn DoctorWithdrawRecordInfo)
        {
            //初步过滤
            var DoctorWithdrawRecords = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorWithdrawRecordInfo.Expression);
            return await DoctorWithdrawRecords.ToListAsync();
        }

        /// <summary>
        /// 查询医生提现记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorWithdrawRecord> DoctorWithdrawRecordByID(int Id)
        {
            var DoctorWithdrawRecords = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return DoctorWithdrawRecords;
        }
        /// <summary>
        /// 查询医生提现记录 page
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<DoctorWithdrawRecord>> DoctorWithdrawRecordPage(DoctorWithdrawRecordIn DoctorWithdrawRecordInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(DoctorWithdrawRecordInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / DoctorWithdrawRecordInfo.MaxResultCount;
            var DoctorWithdrawRecordList = await query.PageBy(DoctorWithdrawRecordInfo.SkipTotal, DoctorWithdrawRecordInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<DoctorWithdrawRecord>(tasksCount, DoctorWithdrawRecordList.MapTo<List<DoctorWithdrawRecord>>());
        }
        /// <summary>
        /// 新建医生提现记录
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorWithdrawRecord> CreateDoctorWithdrawRecord(DoctorWithdrawRecord DoctorWithdrawRecordInfo)
        {
            DoctorWithdrawRecordInfo.Id= await _repository.InsertAndGetIdAsync(DoctorWithdrawRecordInfo);
            return DoctorWithdrawRecordInfo;
        }

        /// <summary>
        /// 修改医生提现记录
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorWithdrawRecord> UpdateDoctorWithdrawRecord(DoctorWithdrawRecord DoctorWithdrawRecordInfo)
        {
            return await _repository.UpdateAsync(DoctorWithdrawRecordInfo);
        }

        /// <summary>
        /// 删除医生提现记录
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<DoctorWithdrawRecord> DeleteDoctorWithdrawRecord(DoctorWithdrawRecord DoctorWithdrawRecordInfo)
        {
            return await _repository.UpdateAsync(DoctorWithdrawRecordInfo);
        }
    }
}
