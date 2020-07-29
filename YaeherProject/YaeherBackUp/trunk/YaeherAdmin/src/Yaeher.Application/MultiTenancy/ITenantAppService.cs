using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.MultiTenancy.Dto;

namespace Yaeher.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
