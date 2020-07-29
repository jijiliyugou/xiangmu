using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface ISystemTokenService:IApplicationService
    {
       Task<SystemToken> SystemTokenList(string SystemTokenInfo);
        
       Task<SystemToken> JSWecharTokenList(string SystemTokenInfo,SystemTokenIn accesstoken);
    }
}