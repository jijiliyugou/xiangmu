using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IDoctorParaSetService : IApplicationService
    {
        Task<DoctorParaSet> CreateDoctorParaSet(DoctorParaSet DoctorParaSetInfo);
        Task<DoctorParaSet> DeleteDoctorParaSet(DoctorParaSet DoctorParaSetInfo);
        Task<DoctorParaSet> DoctorParaSetByID(int Id);
        Task<List<DoctorParaSet>> DoctorParaSetList(DoctorParaSetIn DoctorParaSetInfo);
        Task<PagedResultDto<DoctorParaSet>> DoctorParaSetPage(DoctorParaSetIn DoctorParaSetInfo);
        Task<DoctorParaSet> UpdateDoctorParaSet(DoctorParaSet DoctorParaSetInfo);
    }
}