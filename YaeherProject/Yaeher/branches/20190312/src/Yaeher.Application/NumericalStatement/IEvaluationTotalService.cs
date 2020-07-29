using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.NumericalStatement.Dto;

namespace Yaeher.NumericalStatement
{
    public interface IEvaluationTotalService : IApplicationService
    {
        Task<EvaluationTotal> CreateEvaluationTotal(EvaluationTotal EvaluationTotalInfo);
        Task<EvaluationTotal> DeleteEvaluationTotal(EvaluationTotal EvaluationTotalInfo);
        Task<EvaluationTotal> EvaluationTotalByID(int Id);
        Task<List<EvaluationTotal>> EvaluationTotalList(EvaluationTotalIn EvaluationTotalInfo);
        Task<PagedResultDto<EvaluationTotal>> EvaluationTotalPage(EvaluationTotalIn EvaluationTotalInfo);
        Task<EvaluationTotal> UpdateEvaluationTotal(EvaluationTotal EvaluationTotalInfo);
        Task<string> EvaluationTotalAll();
    }
}