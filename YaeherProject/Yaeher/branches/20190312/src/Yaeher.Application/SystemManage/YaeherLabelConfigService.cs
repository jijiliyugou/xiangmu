using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.SystemManage.Dto;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 标签配置
    /// </summary>
    public class YaeherLabelConfigService : IYaeherLabelConfigService
    {
        private readonly IRepository<YaeherLabelConfig> _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public YaeherLabelConfigService(IRepository<YaeherLabelConfig> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 标签配置 List
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherLabelConfig>> YaeherLabelConfigList(YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            //初步过滤
            var YaeherLabels = _repository.GetAll().OrderBy(a => a.OrderSort).Where(YaeherLabelConfigInfo.Expression);
            return await YaeherLabels.ToListAsync();
        }

        /// <summary>
        /// 查询菜单管理 List
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherLabelList>> YaeherModuleList(YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            var query = await _repository.GetAll().Where(YaeherLabelConfigInfo.Expression).ToListAsync();
            List<YaeherLabelList> yaeherLabelList = new List<YaeherLabelList>();
            List<YaeherLabelConfig> ModuleNode = query.Where(a => a.ParentId == 0).ToList();
            foreach (var item in ModuleNode)
            {
                YaeherLabelList yaeherLabel = new YaeherLabelList();
                yaeherLabel.Id = item.Id;
                yaeherLabel.LabelTypeCode = item.LabelTypeCode;
                yaeherLabel.LabelTypeName = item.LabelTypeName;
                yaeherLabel.LabelCode = item.LabelCode;
                yaeherLabel.LabelName = item.LabelName;
                yaeherLabel.ParentId = item.ParentId;
                yaeherLabel.CreatedOn = item.CreatedOn;
                yaeherLabel.OrderSort = item.OrderSort;
                yaeherLabel.children = GetChild(query.ToList(), item.Id);
                yaeherLabelList.Add(yaeherLabel);
            }
            return yaeherLabelList.OrderBy(a => a.OrderSort).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LabelList"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public List<YaeherLabelList> GetChild(List<YaeherLabelConfig> LabelList, int Id)
        {
            List<YaeherLabelList> child = new List<YaeherLabelList>();
            var ChildLabelList = LabelList.Where(a => a.ParentId == Id).ToList();
            if (ChildLabelList.Count > 0)
            {
                foreach (var item in ChildLabelList)
                {
                    YaeherLabelList yaeherLabel = new YaeherLabelList();
                    yaeherLabel.Id = item.Id;
                    yaeherLabel.LabelTypeCode = item.LabelTypeCode;
                    yaeherLabel.LabelTypeName = item.LabelTypeName;
                    yaeherLabel.LabelCode = item.LabelCode;
                    yaeherLabel.LabelName = item.LabelName;
                    yaeherLabel.ParentId = item.ParentId;
                    yaeherLabel.CreatedOn = item.CreatedOn;
                    yaeherLabel.OrderSort = item.OrderSort;
                    if (LabelList.Where(a => a.ParentId == item.Id).ToList().Count > 0)
                    {
                        yaeherLabel.children = GetChild(LabelList, item.Id);
                    }
                    child.Add(yaeherLabel);
                }
            }
            return child.OrderBy(a => a.OrderSort).ToList() ;
        }

        /// <summary>
        /// 标签配置 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherLabelConfig> YaeherLabelConfigByID(int Id)
        {
            var YaeherLabels = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherLabels;
        }
        /// <summary>
        /// 标签配置 page
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherLabelConfig>> YaeherLabelConfigPage(YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderBy(a => a.OrderSort).Where(YaeherLabelConfigInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherLabelConfigInfo.MaxResultCount;
            var YaeherLabelList = await query.PageBy(YaeherLabelConfigInfo.SkipTotal, YaeherLabelConfigInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherLabelConfig>(tasksCount, YaeherLabelList.MapTo<List<YaeherLabelConfig>>());
        }
        /// <summary>
        /// 新建 标签配置
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherLabelConfig> CreateYaeherLabelConfig(YaeherLabelConfig YaeherLabelConfigInfo)
        {
            YaeherLabelConfigInfo.Id = await _repository.InsertAndGetIdAsync(YaeherLabelConfigInfo);
            return YaeherLabelConfigInfo;
        }

        /// <summary>
        /// 修改 标签配置
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherLabelConfig> UpdateYaeherLabelConfig(YaeherLabelConfig YaeherLabelConfigInfo)
        {
            return await _repository.UpdateAsync(YaeherLabelConfigInfo);
        }

        /// <summary>
        /// 删除 标签配置
        /// </summary>
        /// <param name="YaeherLabelConfigInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherLabelConfig> DeleteYaeherLabelConfig(YaeherLabelConfig YaeherLabelConfigInfo)
        {
            return await _repository.UpdateAsync(YaeherLabelConfigInfo);
        }
    }
}
