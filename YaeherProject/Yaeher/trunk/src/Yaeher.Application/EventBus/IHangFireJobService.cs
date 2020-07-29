using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.EventBus.Dto;
using Yaeher.HangFire;

namespace Yaeher.EventBus
{
    public interface IHangFireJobService: IApplicationService
    {
        Task<HangFireJob> CreateHangFireJob(HangFireJob HangFireJobInfo);
        void CreateSingleHangFireJob(HangFireJob HangFireJobInfo);
        Task<IList<HangFireJob>> HangFireJobList();
        Task<List<HangFireJob>> HangFireJobListAsync(HangFireJobIn hangFireJobIn);
        Task<HangFireJob> UpdateHangFireJob(HangFireJob HangFireJobInfo);
        Task<HangFireJob> HangFireJobByID(int Id);
        HangFireJob HangFireJobSingleByID (int Id);
        HangFireJob HangFireJobSingleByJobID (string Id);
        HangFireJob HangFireJobSingleFirstOrDefault ();
        Task<HangFireJob> HangFireJobByBusiness(string BusinessCode, int BusinessID);
    }
}