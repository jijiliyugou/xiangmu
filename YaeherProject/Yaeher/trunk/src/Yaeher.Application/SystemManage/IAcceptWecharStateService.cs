using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IAcceptWecharStateService: IApplicationService
    {
        Task<AcceptWecharState> AcceptWecharStateByID(int Id);
        Task<IList<WecharState>> AcceptWecharStateList(AcceptWecharStateIn AcceptWecharStateInfo);
        Task<IList<AcceptWecharState>> WecharStateList(AcceptWecharStateIn AcceptWecharStateInfo);
        Task<PagedResultDto<AcceptWecharState>> AcceptWecharStatePage(AcceptWecharStateIn AcceptWecharStateInfo);
        Task<AcceptWecharState> CreateAcceptWecharState(AcceptWecharState AcceptWecharStateInfo);
        Task<AcceptWecharState> UpdateAcceptWecharState(AcceptWecharState AcceptWecharStateInfo);
    }
}