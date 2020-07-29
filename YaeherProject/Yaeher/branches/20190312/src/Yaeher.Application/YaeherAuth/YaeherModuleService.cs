using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.YaeherAuth.Dto;

namespace Yaeher.YaeherAuth
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class YaeherModuleService : IYaeherModuleService
    {
        private readonly IRepository<YaeherModule> _repository;
        /// <summary>
        /// 菜单管理 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public YaeherModuleService(IRepository<YaeherModule> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询菜单管理 List
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherModule>> YaeherModule(YaeherModuleIn YaeherModulefo)
        {
            var query = _repository.GetAll().Where(YaeherModulefo.Expression);
            return await query.ToListAsync();
        }
        /// <summary>
        /// 查询菜单管理 List
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherModuleNode>> YaeherModuleList(YaeherModuleIn YaeherModulefo)
        {
            #region
            //var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherModulefo.Expression);
            //// 根据主菜单查询所有子菜单
            //if (YaeherModulefo.UpperLevel > -1 && YaeherModulefo.UpperLevel != 0)
            //{
            //    query = query.Where(a => a.ParentId == YaeherModulefo.UpperLevel || a.Id == YaeherModulefo.UpperLevel);
            //}
            ////  查询所有主菜单
            //if (YaeherModulefo.UpperLevel == -1)
            //{
            //    query = query.Where(a => a.ParentId ==0);
            //}
            //// 查询所有菜单
            //if (YaeherModulefo.UpperLevel == -2)
            //{
            //    query = query.Where(a => a.ParentId >= 0);
            //}
            #endregion
            var query =await _repository.GetAll().Where(YaeherModulefo.Expression).ToListAsync();
            List<YaeherModuleNode> yaeherModuleNodes = new List<YaeherModuleNode>();
            List<YaeherModule> ModuleNode = query.Where(a => a.ParentId == 0).ToList();
            foreach (var item in ModuleNode)
            {
                YaeherModuleNode yaeherModuleNode = new YaeherModuleNode();
                yaeherModuleNode.Id = item.Id;
                yaeherModuleNode.ParentId = item.ParentId;
                yaeherModuleNode.Names = item.Names;
                yaeherModuleNode.LinkUrls = item.LinkUrls;
                yaeherModuleNode.Areas = item.Areas;
                yaeherModuleNode.Controllers = item.Controllers;
                yaeherModuleNode.Actionss = item.Actionss;
                yaeherModuleNode.Icons = item.Icons;
                yaeherModuleNode.Codes = item.Codes;
                yaeherModuleNode.OrderSort = item.OrderSort;
                yaeherModuleNode.Description = item.Description;
                yaeherModuleNode.IsMenu = item.IsMenu;
                yaeherModuleNode.Enabled = item.Enabled;
                yaeherModuleNode.children = GetChild(query.ToList(),item.Id);
                yaeherModuleNodes.Add(yaeherModuleNode);
            }
            return yaeherModuleNodes.OrderBy(a=>a.OrderSort).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ModuleList"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public List<YaeherModuleNode> GetChild(List<YaeherModule> ModuleList, int Id)
        {
            List<YaeherModuleNode> child = new List<YaeherModuleNode>();
            var  ChildModuleList = ModuleList.Where(a => a.ParentId == Id).ToList();
            if (ChildModuleList.Count > 0)
            {
                foreach (var item in ChildModuleList)
                {
                    YaeherModuleNode yaeherModuleNode = new YaeherModuleNode();
                    yaeherModuleNode.Id = item.Id;
                    yaeherModuleNode.ParentId = item.ParentId;
                    yaeherModuleNode.Names = item.Names;
                    yaeherModuleNode.LinkUrls = item.LinkUrls;
                    yaeherModuleNode.Areas = item.Areas;
                    yaeherModuleNode.Controllers = item.Controllers;
                    yaeherModuleNode.Actionss = item.Actionss;
                    yaeherModuleNode.Icons = item.Icons;
                    yaeherModuleNode.Codes = item.Codes;
                    yaeherModuleNode.OrderSort = item.OrderSort;
                    yaeherModuleNode.Description = item.Description;
                    yaeherModuleNode.IsMenu = item.IsMenu;
                    yaeherModuleNode.Enabled = item.Enabled;
                    if (ModuleList.Where(a => a.ParentId == item.Id).ToList().Count > 0)
                    {
                        yaeherModuleNode.children = GetChild(ModuleList, item.Id);
                    }
                    child.Add(yaeherModuleNode);
                }
            }
            return child.OrderBy(a=>a.OrderSort).ToList();
        }
        /// <summary>
        /// 查询菜单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherModule> YaeherModuleByID(int Id)
        {
            var YaeherModules = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return YaeherModules;
        }
        /// <summary>
        /// 查询菜单管理 page
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherModule>> YaeherModulePage(YaeherModuleIn YaeherModulefo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherModulefo.Expression);
            // 根据主菜单查询所有子菜单
            if (YaeherModulefo.UpperLevel > -1&& YaeherModulefo.UpperLevel!=0)
            {
                query = query.Where(a => a.ParentId == YaeherModulefo.UpperLevel||a.Id== YaeherModulefo.UpperLevel);
            }
            //  查询所有主菜单
            if (YaeherModulefo.UpperLevel == -1)
            {
                query = query.Where(a => a.ParentId == 0);
            }
            // 查询所有菜单
            if (YaeherModulefo.UpperLevel == -2)
            {
                query = query.Where(a => a.ParentId >= 0);
            }
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherModulefo.MaxResultCount;
            var YaeherModuleList = await query.PageBy(YaeherModulefo.SkipTotal, YaeherModulefo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherModule>(tasksCount, YaeherModuleList.MapTo<List<YaeherModule>>());
        }
        /// <summary>
        /// 新建菜单管理
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherModule> CreateYaeherModule(YaeherModule YaeherModulefo)
        {
            YaeherModulefo.Id= await _repository.InsertAndGetIdAsync(YaeherModulefo);
            return YaeherModulefo;
        }

        /// <summary>
        /// 修改菜单管理
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherModule> UpdateYaeherModule(YaeherModule YaeherModulefo)
        {
            return await _repository.UpdateAsync(YaeherModulefo);
        }

        /// <summary>
        /// 删除菜单管理
        /// </summary>
        /// <param name="YaeherModulefo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherModule> DeleteYaeherModule(YaeherModule YaeherModulefo)
        {
            return await _repository.UpdateAsync(YaeherModulefo);
        }
    }
}
