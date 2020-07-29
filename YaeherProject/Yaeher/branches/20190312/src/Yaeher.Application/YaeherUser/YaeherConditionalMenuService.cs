
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.SystemConfig;

namespace Yaeher
{
    /// <summary>
    /// 微信个性化菜单
    /// </summary>
   public class YaeherConditionalMenuService : IYaeherConditionalMenuService
    {
        private readonly IRepository<YaeherConditionalMenu> _menurepository;
        /// <summary>
        /// 微信个性化菜单 构造函数
        /// </summary>
        /// <param name="menurepository"></param>
        public YaeherConditionalMenuService(IRepository<YaeherConditionalMenu> menurepository)
        {
            _menurepository = menurepository;
        }
        /// <summary>
        /// 微信个性化菜单 List
        /// </summary>
        /// <param name="YaeherConditionalMenuInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<YaeherConditionalMenu>> YaeherConditionalMenuList(YaeherConditionalMenuIn YaeherConditionalMenuInfo)
        {
            var query = _menurepository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherConditionalMenuInfo.Expression);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 查询菜单管理 List
        /// </summary>
        /// <param name="YaeherConditionalMenuInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<WecharMenu>> YaeherModuleList(YaeherConditionalMenuIn YaeherConditionalMenuInfo)
        {
            var query = await _menurepository.GetAll().Where(YaeherConditionalMenuInfo.Expression).ToListAsync();
            List<WecharMenu> wecharMenus = new List<WecharMenu>();
            List<YaeherConditionalMenu> yaeherConditionalMenus = query.Where(a => a.ParentID == 0).ToList();
            foreach (var item in yaeherConditionalMenus)
            {
                WecharMenu wecharMenu = new WecharMenu();
                wecharMenu.Id = item.Id;
                wecharMenu.RoleCode = item.RoleCode;
                wecharMenu.RoleName = item.RoleName;
                wecharMenu.TagId = item.TagId;
                wecharMenu.ConditionalName = item.ConditionalName;
                wecharMenu.ConditionalType = item.ConditionalType;
                wecharMenu.ConditionalTypeName = item.ConditionalTypeName;
                wecharMenu.ConditionalUrl = item.ConditionalUrl;
                wecharMenu.AppID = item.AppID;
                wecharMenu.Pagepath = item.Pagepath;
                wecharMenu.ParentID = item.ParentID;
                wecharMenu.MenuID = item.MenuID;
                wecharMenu.CreatedOn = item.CreatedOn;
                wecharMenu.children = GetChild(query.ToList(), item.Id);
                wecharMenus.Add(wecharMenu);
            }
            return wecharMenus;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MenuList"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public List<WecharMenu> GetChild(List<YaeherConditionalMenu> MenuList, int Id)
        {
            List<WecharMenu> child = new List<WecharMenu>();
            var ChildList = MenuList.Where(a => a.ParentID == Id).ToList();
            if (ChildList.Count > 0)
            {
                foreach (var item in ChildList)
                {
                    WecharMenu wecharMenu = new WecharMenu();
                    wecharMenu.Id = item.Id;
                    wecharMenu.RoleCode = item.RoleCode;
                    wecharMenu.RoleName = item.RoleName;
                    wecharMenu.TagId = item.TagId;
                    wecharMenu.ConditionalName = item.ConditionalName;
                    wecharMenu.ConditionalType = item.ConditionalType;
                    wecharMenu.ConditionalTypeName = item.ConditionalTypeName;
                    wecharMenu.ConditionalUrl = item.ConditionalUrl;
                    wecharMenu.AppID = item.AppID;
                    wecharMenu.Pagepath = item.Pagepath;
                    wecharMenu.ParentID = item.ParentID;
                    wecharMenu.MenuID = item.MenuID;
                    wecharMenu.CreatedOn = item.CreatedOn;
                    if (MenuList.Where(a => a.ParentID == item.Id).ToList().Count > 0)
                    {
                        wecharMenu.children = GetChild(MenuList, item.Id);
                    }
                    child.Add(wecharMenu);
                }
            }
            return child;
        }
        /// <summary>
        /// 微信个性化菜单byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConditionalMenu> YaeherConditionalMenuByID(int Id)
        {
            var query = await _menurepository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return query;
        }
        /// <summary>
        /// 微信个性化菜单 page
        /// </summary>
        /// <param name="YaeherConditionalMenuInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<YaeherConditionalMenu>> YaeherConditionalMenuPage(YaeherConditionalMenuIn YaeherConditionalMenuInfo)
        {
            //初步过滤
            var query = _menurepository.GetAll().OrderByDescending(a => a.CreatedOn).Where(YaeherConditionalMenuInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / YaeherConditionalMenuInfo.MaxResultCount;
            var YaeherConditionalMenuList = await query.PageBy(YaeherConditionalMenuInfo.SkipTotal, YaeherConditionalMenuInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<YaeherConditionalMenu>(tasksCount, YaeherConditionalMenuList.MapTo<List<YaeherConditionalMenu>>());
        }
        /// <summary>
        /// 新建微信个性化菜单
        /// </summary>
        /// <param name="YaeherConditionalMenufo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConditionalMenu> CreateYaeherConditionalMenu(YaeherConditionalMenu YaeherConditionalMenufo)
        {
            YaeherConditionalMenufo.Id = await _menurepository.InsertAndGetIdAsync(YaeherConditionalMenufo);
            return YaeherConditionalMenufo;
        }

        /// <summary>
        /// 修改微信个性化菜单
        /// </summary>
        /// <param name="YaeherConditionalMenufo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConditionalMenu> UpdateYaeherConditionalMenu(YaeherConditionalMenu YaeherConditionalMenufo)
        {
            return await _menurepository.UpdateAsync(YaeherConditionalMenufo);
        }

        /// <summary>
        /// 删除微信个性化菜单
        /// </summary>
        /// <param name="YaeherConditionalMenufo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<YaeherConditionalMenu> DeleteYaeherConditionalMenu(YaeherConditionalMenu YaeherConditionalMenufo)
        {
            return await _menurepository.UpdateAsync(YaeherConditionalMenufo);
        }
    }
}