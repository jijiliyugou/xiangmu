using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.Quality.Dto;

namespace Yaeher.Quality
{
    /// <summary>
    /// 质控委员会
    /// </summary>
    public class QualityCommitteeService : IQualityCommitteeService
    {
        private readonly IRepository<QualityCommittee> _repository;
        /// <summary>
        /// 质控委员会 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public QualityCommitteeService(IRepository<QualityCommittee> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询质控委员会 List
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<QualityCommittee>> QualityCommitteeList(QualityCommitteeIn QualityCommitteeInfo)
        {
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.ClinicName))
            {
                query = query.Where(a => a.ClinicName.Contains(QualityCommitteeInfo.ClinicName));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.DoctorName))
            {
                query = query.Where(a => a.DoctorName.Contains(QualityCommitteeInfo.DoctorName));
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// 查询质控委员会byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommittee> QualityCommitteeByID(int Id)
        {
            var QualityCommittees = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return QualityCommittees;
        }
        /// <summary>
        /// 查询质控委员会byId
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommittee> QualityCommitteeByDoctorID(int DoctorId)
        {
            var QualityCommittees = await _repository.FirstOrDefaultAsync(t => t.DoctorID == DoctorId && !t.IsDelete);
            return QualityCommittees;
        }
        

        /// <summary>
        /// 查询质控委员会 page
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<QualityCommittee>> QualityCommitteePage(QualityCommitteeIn QualityCommitteeInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(a=>a.IsDelete==false);
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.ClinicName))
            {
                query = query.Where(a => a.ClinicName.Contains(QualityCommitteeInfo.ClinicName));
            }
            if (!string.IsNullOrEmpty(QualityCommitteeInfo.DoctorName))
            {
                query = query.Where(a => a.DoctorName.Contains(QualityCommitteeInfo.DoctorName));
            }
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / QualityCommitteeInfo.MaxResultCount;
            var QualityCommitteeList = await query.PageBy(QualityCommitteeInfo.SkipTotal, QualityCommitteeInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<QualityCommittee>(tasksCount, QualityCommitteeList.MapTo<List<QualityCommittee>>());
        }
        /// <summary>
        /// 新建质控委员会
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommittee> CreateQualityCommittee(QualityCommittee QualityCommitteeInfo)
        {
            QualityCommitteeInfo.Id= await _repository.InsertAndGetIdAsync(QualityCommitteeInfo);
            return QualityCommitteeInfo;
        }

        /// <summary>
        /// 修改质控委员会
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommittee> UpdateQualityCommittee(QualityCommittee QualityCommitteeInfo)
        {
            return await _repository.UpdateAsync(QualityCommitteeInfo);
        }

        /// <summary>
        /// 删除质控委员会
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<QualityCommittee> DeleteQualityCommittee(QualityCommittee QualityCommitteeInfo)
        {
            return await _repository.UpdateAsync(QualityCommitteeInfo);
        }
    }
}
