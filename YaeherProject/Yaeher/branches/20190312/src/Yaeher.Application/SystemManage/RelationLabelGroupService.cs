using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 标签组群维护
    /// </summary>
    public class RelationLabelGroupService : IRelationLabelGroupService
    {
        private readonly IRepository<RelationLabelGroup> _repository;
        /// <summary>
        /// 标签组群 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public RelationLabelGroupService(IRepository<RelationLabelGroup> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 标签组群 List
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<RelationLabelGroup>> RelationLabelGroupList(RelationLabelGroupIn RelationLabelGroupInfo)
        {
            //初步过滤
            var RelationLabelGroups = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(RelationLabelGroupInfo.Expression);
            return await RelationLabelGroups.ToListAsync();
        }

        /// <summary>
        /// 标签组群 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RelationLabelGroup> RelationLabelGroupByID(int Id)
        {
            var RelationLabelGroups = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return RelationLabelGroups;
        }
        /// <summary>
        /// 标签组群 page
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<RelationLabelGroup>> RelationLabelGroupPage(RelationLabelGroupIn RelationLabelGroupInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(RelationLabelGroupInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / RelationLabelGroupInfo.MaxResultCount;
            var RelationLabelGroupList = await query.PageBy(RelationLabelGroupInfo.SkipTotal, RelationLabelGroupInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<RelationLabelGroup>(tasksCount, RelationLabelGroupList.MapTo<List<RelationLabelGroup>>());
        }
        /// <summary>
        /// 新建 发送消息模板
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RelationLabelGroup> CreateRelationLabelGroup(RelationLabelGroup RelationLabelGroupInfo)
        {
            RelationLabelGroupInfo.Id = await _repository.InsertAndGetIdAsync(RelationLabelGroupInfo);
            return RelationLabelGroupInfo;
        }

        /// <summary>
        /// 修改 标签组群
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RelationLabelGroup> UpdateRelationLabelGroup(RelationLabelGroup RelationLabelGroupInfo)
        {
            return await _repository.UpdateAsync(RelationLabelGroupInfo);
        }

        /// <summary>
        /// 删除 标签组群
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RelationLabelGroup> DeleteRelationLabelGroup(RelationLabelGroup RelationLabelGroupInfo)
        {
            return await _repository.UpdateAsync(RelationLabelGroupInfo);
        }

        /// <summary>
        /// 标签组群 List
        /// </summary>
        /// <param name="RelationLabelGroupInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<LabelGroup>> LabelGroupList(RelationLabelGroupIn RelationLabelGroupInfo)
        {
            //初步过滤
            var RelationLabelGroups = _repository.GetAll().Where(RelationLabelGroupInfo.Expression);
            List<LabelGroup> labelGroups = new List<LabelGroup>();
            if (RelationLabelGroups!=null)
            {
                var RelationLabel = await RelationLabelGroups.ToListAsync();
                foreach (var Item in RelationLabel)
                {
                    string[] LableID = Item.LableID.Split(',');
                    string[] LableName = Item.LableName.Split(',');
                    if (LableID.Length > 0)
                    {
                        for(int a=0;a<LableID.Length;a++)
                        {
                            LabelGroup labelGroup = new LabelGroup();
                            labelGroup.GroupName = Item.GroupName;
                            labelGroup.LableID = int.Parse(LableID[a]);
                            labelGroup.LableName = LableName[a];
                            labelGroups.Add(labelGroup);
                        }
                    }
                }
            }
            return labelGroups.Distinct().ToList();
        }
    }
}
