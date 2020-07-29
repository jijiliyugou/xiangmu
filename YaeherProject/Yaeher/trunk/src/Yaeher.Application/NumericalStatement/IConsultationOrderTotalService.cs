using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.NumericalStatement.Dto;

namespace Yaeher.NumericalStatement
{
    public interface IConsultationOrderTotalService : IApplicationService
    {
        Task<ConsultationOrderTotal> ConsultationOrderTotalByID(int Id);
        Task<IList<ConsultationOrderTotal>> ConsultationOrderTotalList(ConsultationOrderTotalIn ConsultationOrderTotalInfo);
        Task<PagedResultDto<ConsultationOrderTotal>> ConsultationOrderTotalPage(ConsultationOrderTotalIn ConsultationOrderTotalInfo);
        Task<ConsultationOrderTotal> CreateConsultationOrderTotal(ConsultationOrderTotal ConsultationOrderTotalInfo);
        Task<ConsultationOrderTotal> DeleteConsultationOrderTotal(ConsultationOrderTotal ConsultationOrderTotalInfo);
        Task<ConsultationOrderTotal> UpdateConsultationOrderTotal(ConsultationOrderTotal ConsultationOrderTotalInfo);
        Task<string> OrderTotal(OrderTotalIn orderTotalIn);
    }
}