using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IRelationLabelListService:IApplicationService
    {
        Task<RelationLabelList> CreateRelationLabelList(RelationLabelList RelationLabelListInfo);
        Task<IList<RelationLabelList>> RelationLabelListList(RelationLabelListIn RelationLabelListInfo);
        Task<string> CreateRelationLabe(RelationLabel RelationLabelListInfo);
        Task<RelationLabelList> DeleteRelationLabelList(RelationLabelList RelationLabelListInfo);
    }
}