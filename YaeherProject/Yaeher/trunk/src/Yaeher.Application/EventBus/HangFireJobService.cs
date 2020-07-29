using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.HangFire;
using System.Linq;
using Yaeher.EventBus.Dto;

namespace Yaeher.EventBus
{
    /// <summary>
    /// 定时服务执行
    /// </summary>
    public class HangFireJobService : IHangFireJobService
    {
        private readonly IRepository<HangFireJob> _repository;
        /// <summary>
        /// 定时服务执行
        /// </summary>
        /// <param name="repository"></param>
        public HangFireJobService(IRepository<HangFireJob> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增 事件服务处理结果
        /// </summary>
        /// <param name="HangFireJobInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<HangFireJob> CreateHangFireJob(HangFireJob HangFireJobInfo)
        {
            HangFireJobInfo.Id = await _repository.InsertAndGetIdAsync(HangFireJobInfo);
            return HangFireJobInfo;
        }
        /// <summary>
        /// 新增 事件服务处理结果
        /// </summary>
        /// <param name="HangFireJobInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public  void CreateSingleHangFireJob(HangFireJob HangFireJobInfo)
        {
             _repository.Insert(HangFireJobInfo);
        }
        

        /// <summary>
        /// 修改 事件服务处理结果
        /// </summary>
        /// <param name="HangFireJobInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<HangFireJob> UpdateHangFireJob(HangFireJob HangFireJobInfo)
        {
            return await _repository.UpdateAsync(HangFireJobInfo);
        }
        /// <summary>
        /// 查询订阅 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<HangFireJob>> HangFireJobList()
        {
            var HangFireJobs = await _repository.GetAllListAsync();
            return HangFireJobs.Where(a => a.IsDelete == false).ToList();
        }
        /// <summary>
        /// 查询订阅 List
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<List<HangFireJob>> HangFireJobListAsync(HangFireJobIn hangFireJobIn)
        {
            var HangFireJobs = await _repository.GetAllListAsync(hangFireJobIn.Expression);
            return HangFireJobs.ToList();

        }
        
        /// <summary>
        /// 查询门诊信息byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<HangFireJob> HangFireJobByID(int Id)
        {
            var HangFireJobs = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return HangFireJobs;
        }

        /// <summary>
        /// 查询门诊信息byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public HangFireJob HangFireJobSingleByID(int Id)
        {
            var HangFireJobs =  _repository.FirstOrDefault(t => t.Id == Id && !t.IsDelete);
            return HangFireJobs;
        }
        /// <summary>
        /// 查询门诊信息byJoBId
        /// </summary>
        /// <param name="JoBId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public HangFireJob HangFireJobSingleByJobID(string JoBId)
        {
            var HangFireJobs = _repository.FirstOrDefault(t => t.JobRunID == JoBId && !t.IsDelete);
            return HangFireJobs;
        }
        /// <summary>
        /// 查询门诊信息byFirstOrDefault
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public HangFireJob HangFireJobSingleFirstOrDefault()
        {
            var HangFireJobs = _repository.GetAll().Where(t =>t.JobCode=="IncomeTotal"&& t.JobSates=="Open"&& !t.IsDelete).OrderByDescending(t=>t.CreatedOn);
            return HangFireJobs.FirstOrDefault();
        }

         /// <summary>
         /// 查询hangfire
         /// </summary>
         /// <param name="BusinessCode"></param>
         /// <param name="BusinessID"></param>
         /// <returns></returns>
        [RemoteService(false)]
        public async Task<HangFireJob> HangFireJobByBusiness(string BusinessCode,int BusinessID)
        {
            var HangFireJobs =await _repository.FirstOrDefaultAsync(t => t.BusinessID == BusinessID && t.BusinessCode==BusinessCode && !t.IsDelete);
            return HangFireJobs;
        }
    }
}
