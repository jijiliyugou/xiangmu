using Abp.Application.Services;
using Abp.Domain.Repositories;
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
    /// 记录标签与医生 科室 文章 问答之间的关系
    /// </summary>
    public class RelationLabelListService : IRelationLabelListService
    {
        private readonly IRepository<RelationLabelList> _repository;
        /// <summary>
        /// 标签关系表 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public RelationLabelListService(IRepository<RelationLabelList> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 标签关系表 List
        /// </summary>
        /// <param name="RelationLabelListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<RelationLabelList>> RelationLabelListList(RelationLabelListIn RelationLabelListInfo)
        {
            //初步过滤
            var RelationLabelLists = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(RelationLabelListInfo.Expression);
            return await RelationLabelLists.ToListAsync();
        }
        /// <summary>
        /// 新建 标签关系表
        /// </summary>
        /// <param name="RelationLabelListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RelationLabelList> CreateRelationLabelList(RelationLabelList RelationLabelListInfo)
        {
            RelationLabelList relationLabelList = new RelationLabelList();
            // 按照类型、业务ID查询是否存在数据
            var RelationLabelLists = _repository.GetAll().Where(a=>a.IsDelete==false && a.BusinessID==RelationLabelListInfo.BusinessID && a.RelationCode==RelationLabelListInfo.RelationCode&&a.LableID==RelationLabelListInfo.LableID);
            // 先删除所有业务关联标签
            if (RelationLabelLists.Count() == 0)
            {
                relationLabelList.Id = await _repository.InsertAndGetIdAsync(RelationLabelListInfo);
            }
            return relationLabelList;
        }
        /// <summary>
        /// 新建标签与业务之间的关系
        /// </summary>
        /// <param name="RelationLabelListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<string> CreateRelationLabe(RelationLabel RelationLabelListInfo)
        {
            try
            {
                // 按照类型、业务ID查询是否存在数据
                RelationLabelListInfo.AndAlso(a => a.IsDelete == false);
                RelationLabelListInfo.AndAlso(a => a.RelationCode == RelationLabelListInfo.RelationCode);
                RelationLabelListInfo.AndAlso(a => a.BusinessID == RelationLabelListInfo.BusinessID);
                var RelationLabelLists = _repository.GetAll().Where(RelationLabelListInfo.Expression);
                // 先删除所有业务关联标签
                if (RelationLabelLists.Count() > 0)
                {
                    foreach (var Item in RelationLabelLists)
                    {
                        Item.IsDelete = true;
                        Item.DeleteBy = 0;
                        Item.DeleteTime = DateTime.Now;
                        await _repository.UpdateAsync(Item);
                    }
                }
                // 再新增相关业务关联标签
                var LableIDList = RelationLabelListInfo.LableID.Split(',');
                var LableNameList = RelationLabelListInfo.LableName.Split(',');
                if (LableIDList.Length > 0)
                {
                    for(int a=0;a<LableIDList.Length;a++)
                    {
                        RelationLabelList relationLabelList = new RelationLabelList();
                        relationLabelList.RelationCode = RelationLabelListInfo.RelationCode;
                        relationLabelList.BusinessID = RelationLabelListInfo.BusinessID;
                        relationLabelList.BusinessName = RelationLabelListInfo.BusinessName;
                        relationLabelList.LableID = int.Parse(LableIDList[a].ToString());
                        relationLabelList.LableName = LableNameList[a].ToString();
                        await _repository.InsertAndGetIdAsync(relationLabelList);
                    }
                }
                return "success";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 删除 标签关系表
        /// </summary>
        /// <param name="RelationLabelListInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<RelationLabelList> DeleteRelationLabelList(RelationLabelList RelationLabelListInfo)
        {
           return await _repository.UpdateAsync(RelationLabelListInfo);
        }
    }
}
