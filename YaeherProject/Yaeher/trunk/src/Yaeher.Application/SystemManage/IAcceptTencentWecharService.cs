using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IAcceptTencentWecharService: IApplicationService
    {
        Task<AcceptTencentWechar> AcceptTencentByID(int Id);
        Task<IList<AcceptTencentWechar>> AcceptTencentList(AcceptTencentWecharIn AcceptTencentWecharInfo);
        Task<PagedResultDto<AcceptTencentWechar>> AcceptTencentPage(AcceptTencentWecharIn AcceptTencentWecharInfo);
        Task<AcceptTencentWechar> CreateAcceptTencent(AcceptTencentWechar AcceptTencentWecharInfo);
        Task<AcceptTencentWechar> DeleteAcceptTencent(AcceptTencentWechar AcceptTencentWecharInfo);
        Task<AcceptTencentWechar> UpdateAcceptTencent(AcceptTencentWechar AcceptTencentWecharInfo);
        Task<string> SendWecharMesaage(string KeyWord);
    }
}