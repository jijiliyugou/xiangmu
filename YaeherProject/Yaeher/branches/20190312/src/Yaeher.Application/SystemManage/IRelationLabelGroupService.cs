using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IRelationLabelGroupService:IApplicationService
    {
        Task<RelationLabelGroup> CreateRelationLabelGroup(RelationLabelGroup RelationLabelGroupInfo);
        Task<RelationLabelGroup> DeleteRelationLabelGroup(RelationLabelGroup RelationLabelGroupInfo);
        Task<RelationLabelGroup> RelationLabelGroupByID(int Id);
        Task<IList<RelationLabelGroup>> RelationLabelGroupList(RelationLabelGroupIn RelationLabelGroupInfo);
        Task<PagedResultDto<RelationLabelGroup>> RelationLabelGroupPage(RelationLabelGroupIn RelationLabelGroupInfo);
        Task<RelationLabelGroup> UpdateRelationLabelGroup(RelationLabelGroup RelationLabelGroupInfo);
        Task<IList<LabelGroup>> LabelGroupList(RelationLabelGroupIn RelationLabelGroupInfo);
    }
}