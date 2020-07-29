using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.YaeherAuth;
using Yaeher.YaeherAuth.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 用户权限管理
    /// </summary>
    public class YaeherAuthController : YaeherAppServiceBase
    {
        private readonly IYaeherModuleService _yaeherModuleService;
        private readonly IYaeherRoleModuleService _yaeherRoleModuleService;
        private readonly IYaeherRoleService _yaeherRoleService;
        private readonly IYaeherUserRoleService _yaeherUserRoleService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherOperListService _yaeherOperListService;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yaeherModuleService"></param>
        /// <param name="yaeherRoleModuleService"></param>
        /// <param name="yaeherRoleService"></param>
        /// <param name="yaeherUserRoleService"></param>
        /// <param name="session"></param>
        /// <param name="yaeherOperListService"></param>
        public YaeherAuthController(IYaeherModuleService yaeherModuleService,
                                    IYaeherRoleModuleService yaeherRoleModuleService,
                                    IYaeherRoleService yaeherRoleService,
                                    IYaeherUserRoleService yaeherUserRoleService,
                                    IAbpSession session,
                                    IYaeherOperListService yaeherOperListService)
        {
            _yaeherModuleService = yaeherModuleService;
            _yaeherRoleModuleService = yaeherRoleModuleService;
            _yaeherRoleService = yaeherRoleService;
            _yaeherUserRoleService = yaeherUserRoleService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
        }

        #region 角色管理
        /// <summary>
        /// 角色管理 新增
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherRole")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherRole([FromBody] YaeherRole YaeherRoleInfo)
        {
            if (!Commons.CheckSecret(YaeherRoleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherRoleIn yaeherRoleIn = new YaeherRoleIn();
            yaeherRoleIn.AndAlso(a => a.IsDelete == false);
            yaeherRoleIn.AndAlso(a => a.RoleCode == YaeherRoleInfo.RoleCode && a.RoleName == YaeherRoleInfo.RoleName);
            var RoleList = await _yaeherRoleService.YaeherRoleList(yaeherRoleIn);
            if (RoleList.Count() > 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "角色重复！";
                return ObjectResultModule;
            }
            var CreateYaeherRole = new YaeherRole()
            {
                RoleName = YaeherRoleInfo.RoleName,
                Description = YaeherRoleInfo.Description,
                Enabled = YaeherRoleInfo.Enabled,
                IsAdmin = YaeherRoleInfo.IsAdmin,
                RoleCode= YaeherRoleInfo.RoleCode,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _yaeherRoleService.CreateYaeherRole(CreateYaeherRole);
            if (result.Id > 0)
            {
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateYaeherRole",
                OperContent = JsonHelper.ToJson(YaeherRoleInfo),
                OperType = "CreateYaeherRole",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 角色管理 修改
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherRole")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherRole([FromBody]  YaeherRole YaeherRoleInfo)
        {
            if (!Commons.CheckSecret(YaeherRoleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateYaeherRole = await _yaeherRoleService.YaeherRoleByID(YaeherRoleInfo.Id);
            if (UpdateYaeherRole != null)
            {
                UpdateYaeherRole.RoleName = YaeherRoleInfo.RoleName;
                UpdateYaeherRole.Description = YaeherRoleInfo.Description;
                UpdateYaeherRole.Enabled = YaeherRoleInfo.Enabled;
                UpdateYaeherRole.IsAdmin = YaeherRoleInfo.IsAdmin;
                UpdateYaeherRole.RoleCode = YaeherRoleInfo.RoleCode;
                UpdateYaeherRole.ModifyOn = DateTime.Now;
                UpdateYaeherRole.ModifyBy = userid;
                var result = await _yaeherRoleService.UpdateYaeherRole(UpdateYaeherRole);

                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateYaeherRole",
                OperContent = JsonHelper.ToJson(YaeherRoleInfo),
                OperType = "UpdateYaeherRole",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 角色管理 删除
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherRole")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherRole([FromBody]  YaeherRole YaeherRoleInfo)
        {
            if (!Commons.CheckSecret(YaeherRoleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _yaeherRoleService.YaeherRoleByID(YaeherRoleInfo.Id);
            YaeherUserRoleIn yaeherUserRoleIn = new YaeherUserRoleIn();
            yaeherUserRoleIn.AndAlso(a => a.IsDelete == false);
            yaeherUserRoleIn.AndAlso(a => a.RoleID == YaeherRoleInfo.Id);
            var RoleUserList = await _yaeherUserRoleService.YaeherUserRoleList(yaeherUserRoleIn);
            if (RoleUserList.Count() > 0)
            {
                this.ObjectResultModule.Message = "用户使用角色不可删除！";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _yaeherRoleService.DeleteYaeherRole(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteYaeherRole",
                OperContent = JsonHelper.ToJson(YaeherRoleInfo),
                OperType = "DeleteYaeherRole",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 角色管理 Page
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherRolePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherRolePage([FromBody] YaeherRoleIn YaeherRoleInfo)
        {
            if (!Commons.CheckSecret(YaeherRoleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherRoleInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherRoleInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherRoleInfo.EndTime))
                {
                    YaeherRoleInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherRoleInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherRoleInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherRoleInfo.StartTime))
            {
                YaeherRoleInfo.AndAlso(t => t.CreatedOn >= StartTime);
                YaeherRoleInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherRoleInfo.KeyWord))
            {
                YaeherRoleInfo.AndAlso(t => t.RoleCode.Contains(YaeherRoleInfo.KeyWord)
                                                  || t.RoleName.Contains(YaeherRoleInfo.KeyWord));
            }
            YaeherRoleInfo.AndAlso(a => a.IsDelete == false);
            var values = await _yaeherRoleService.YaeherRolePage(YaeherRoleInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new YaeherRoleOut(values, YaeherRoleInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherRolePage",
                OperContent = JsonHelper.ToJson(YaeherRoleInfo),
                OperType = "YaeherRolePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 角色管理 List 
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherRoleList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherRoleList([FromBody]YaeherRoleIn YaeherRoleInfo)
        {
            if (!Commons.CheckSecret(YaeherRoleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherRoleInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherRoleInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherRoleInfo.EndTime))
                {
                    YaeherRoleInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherRoleInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherRoleInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherRoleInfo.StartTime))
            {
                YaeherRoleInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherRoleInfo.KeyWord))
            {
                YaeherRoleInfo.AndAlso(t => t.RoleCode.Contains(YaeherRoleInfo.KeyWord)
                                                  || t.RoleName.Contains(YaeherRoleInfo.KeyWord));
            }
            YaeherRoleInfo.AndAlso(a => a.IsDelete == false);
            var values = await _yaeherRoleService.YaeherRoleList(YaeherRoleInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object ="";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherRoleList",
                OperContent = JsonHelper.ToJson(YaeherRoleInfo),
                OperType = "YaeherRoleList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 角色管理 Byid
        /// </summary>
        /// <param name="YaeherRoleInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherRoleById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherRoleById([FromBody]YaeherRoleIn YaeherRoleInfo)
        {
            if (!Commons.CheckSecret(YaeherRoleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherRoleService.YaeherRoleByID(YaeherRoleInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherRoleById",
                OperContent = JsonHelper.ToJson(YaeherRoleInfo),
                OperType = "YaeherRoleById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 角色与用户关联表
        /// <summary>
        /// 角色与用户关联表 新增
        /// </summary>
        /// <param name="YaeherUserRoleJSON"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherUserRole")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherUserRole([FromBody]YaeherUserRoleJSON YaeherUserRoleJSON)
        {
            if (!Commons.CheckSecret(YaeherUserRoleJSON.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            try
            {
                var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
                var resultId = 0;
                var UserID = YaeherUserRoleJSON.UserID;
                string[] Roles = null;
                if (!string.IsNullOrEmpty(YaeherUserRoleJSON.RoleID))
                {
                    Roles = YaeherUserRoleJSON.RoleID.Split(',');
                }
                #region 根据用户删除现有数据
                YaeherUserRoleIn yaeherUserRoleIn = new YaeherUserRoleIn();
                yaeherUserRoleIn.UserID = UserID;
                var query = await _yaeherUserRoleService.YaeherUserRoleList(yaeherUserRoleIn);
                if (query.Count() > 0)
                {
                    foreach (var UserRoleInfo in query)
                    {
                        UserRoleInfo.DeleteBy = UserID;
                        UserRoleInfo.DeleteTime = DateTime.Now;
                        UserRoleInfo.IsDelete = true;
                        var res = await _yaeherUserRoleService.DeleteYaeherUserRole(UserRoleInfo);
                    }
                }
                #endregion
                #region 新增用户权限
                if (Roles != null && Roles.Length > 0)
                {
                    for (int a = 0; a < Roles.Length; a++)
                    {
                        var CreateYaeherUserRole = new YaeherUserRole()
                        {
                            RoleID = int.Parse(Roles[a]),
                            UserID = UserID,
                            CreatedBy = userid,
                            CreatedOn = DateTime.Now
                        };
                        var result = await _yaeherUserRoleService.CreateYaeherUserRole(CreateYaeherUserRole);
                        resultId = +result.Id;
                    }
                }
                #endregion
                #region 操作日志
                var CreateYaeherOperList = new YaeherOperList()
                {
                    OperExplain = "CreateYaeherUserRole",
                    OperContent = JsonHelper.ToJson(YaeherUserRoleJSON),
                    OperType = "CreateYaeherUserRole",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now
                };
                var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
                #endregion
                this.ObjectResultModule.Object = resultId;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
                return this.ObjectResultModule;
            }
            catch (Exception ex)
            {
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
                this.ObjectResultModule.Object = ex.ToString();
                return this.ObjectResultModule;
            }
        }
        
        /// <summary>
        /// 用户与角色关系List
        /// </summary>
        /// <param name="YaeherUserRoleInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherUserRoleByUserID")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherUserRoleByUserID([FromBody]YaeherUserRoleIn YaeherUserRoleInfo)
        {
            if (!Commons.CheckSecret(YaeherUserRoleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherUserRoleService.YaeherUserRoleList(YaeherUserRoleInfo);
            YaeherRoleIn yaeherRoleIn = new YaeherRoleIn();
            yaeherRoleIn.AndAlso(a => a.IsDelete == false);
            var RoleList = await _yaeherRoleService.YaeherRoleList(yaeherRoleIn);
            var UserRoleList = from Role in RoleList join UserAndRole in values
                               on Role.Id equals UserAndRole.RoleID
                               select new {Role.Id,Role.RoleName,Role.Description,UserAndRole.UserID};
            if(UserRoleList.Count()==0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                YaeherUserRoleJSON yaeherUserRoleJSON = new YaeherUserRoleJSON();
                foreach (var UserRoles in UserRoleList)
                {
                    yaeherUserRoleJSON.RoleID += UserRoles.Id + ",";
                    yaeherUserRoleJSON.UserID = UserRoles.UserID;
                }
                this.ObjectResultModule.Object = yaeherUserRoleJSON;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherUserRoleByUserID",
                OperContent = JsonHelper.ToJson(YaeherUserRoleInfo),
                OperType = "YaeherUserRoleByUserID",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 菜单表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="YaeherModuleInfo"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherModule")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherModule([FromBody]Yaeher.YaeherAuth.Dto.YaeherModuleNode YaeherModuleInfo)
        {
            if (!Commons.CheckSecret(YaeherModuleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherModuleIn yaeherModuleIn = new YaeherModuleIn();
            yaeherModuleIn.AndAlso(a => a.IsDelete == false);
            yaeherModuleIn.AndAlso(a => a.LinkUrls == YaeherModuleInfo.LinkUrls);
            yaeherModuleIn.AndAlso(a => a.Names == YaeherModuleInfo.Names);
            yaeherModuleIn.AndAlso(a => a.Codes == YaeherModuleInfo.Codes);
            yaeherModuleIn.AndAlso(a => a.ParentId == YaeherModuleInfo.ParentId);
            yaeherModuleIn.AndAlso(a => a.Areas == YaeherModuleInfo.Areas);
            yaeherModuleIn.AndAlso(a => a.Controllers == YaeherModuleInfo.Controllers);
            yaeherModuleIn.AndAlso(a => a.IsMenu == YaeherModuleInfo.IsMenu);
            var ModuleList = await _yaeherModuleService.YaeherModule(yaeherModuleIn);
            if (ModuleList.Count() > 0)
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 100;
                this.ObjectResultModule.Message = "菜单已存在不可新增！";
                return ObjectResultModule;
            }
            var CreateYaeherModule = new YaeherModule()
            {
                ParentId = YaeherModuleInfo.ParentId,
                Names = YaeherModuleInfo.Names,
                LinkUrls = YaeherModuleInfo.LinkUrls,
                Areas = YaeherModuleInfo.Areas,
                Controllers = YaeherModuleInfo.Controllers,
                Actionss = YaeherModuleInfo.Actionss,
                Icons = YaeherModuleInfo.Icons,
                Codes = YaeherModuleInfo.Codes,
                OrderSort = YaeherModuleInfo.OrderSort,
                Description = YaeherModuleInfo.Description,
                IsMenu = YaeherModuleInfo.IsMenu,
                Enabled = YaeherModuleInfo.Enabled,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _yaeherModuleService.CreateYaeherModule(CreateYaeherModule);
            if (result.Id > 0)
            {
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateYaeherModule",
                OperContent = JsonHelper.ToJson(YaeherModuleInfo),
                OperType = "CreateYaeherModule",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 菜单表 修改
        /// </summary>
        /// <param name="YaeherModuleInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateYaeherModule")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateYaeherModule([FromBody] Yaeher.YaeherAuth.Dto.YaeherModuleNode YaeherModuleInfo)
        {
            if (!Commons.CheckSecret(YaeherModuleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateYaeherModule = await _yaeherModuleService.YaeherModuleByID(YaeherModuleInfo.Id);
            if (UpdateYaeherModule != null)
            {
                UpdateYaeherModule.ParentId = YaeherModuleInfo.ParentId;
                UpdateYaeherModule.Names = YaeherModuleInfo.Names;
                UpdateYaeherModule.LinkUrls = YaeherModuleInfo.LinkUrls;
                UpdateYaeherModule.Areas = YaeherModuleInfo.Areas;
                UpdateYaeherModule.Controllers = YaeherModuleInfo.Controllers;
                UpdateYaeherModule.Actionss = YaeherModuleInfo.Actionss;
                UpdateYaeherModule.Icons = YaeherModuleInfo.Icons;
                UpdateYaeherModule.Codes = YaeherModuleInfo.Codes;
                UpdateYaeherModule.OrderSort = YaeherModuleInfo.OrderSort;
                UpdateYaeherModule.Description = YaeherModuleInfo.Description;
                UpdateYaeherModule.IsMenu = YaeherModuleInfo.IsMenu;
                UpdateYaeherModule.Enabled = YaeherModuleInfo.Enabled;
                UpdateYaeherModule.ModifyOn =DateTime.Now;
                UpdateYaeherModule.ModifyBy = userid;
                var result = await _yaeherModuleService.UpdateYaeherModule(UpdateYaeherModule);

                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateYaeherModule",
                OperContent = JsonHelper.ToJson(YaeherModuleInfo),
                OperType = "UpdateYaeherModule",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 菜单表 删除
        /// </summary>
        /// <param name="YaeherModuleInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteYaeherModule")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteYaeherModule([FromBody] Yaeher.YaeherAuth.Dto.YaeherModuleNode YaeherModuleInfo)
        {
            if (!Commons.CheckSecret(YaeherModuleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _yaeherModuleService.YaeherModuleByID(YaeherModuleInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _yaeherModuleService.DeleteYaeherModule(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteYaeherModule",
                OperContent = JsonHelper.ToJson(YaeherModuleInfo),
                OperType = "DeleteYaeherModule",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }
        
        /// <summary>
        /// 菜单表 List 
        /// </summary>
        /// <param name="YaeherModuleInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherModuleList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherModuleList([FromBody]YaeherModuleIn YaeherModuleInfo)
        {
            if (!Commons.CheckSecret(YaeherModuleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(YaeherModuleInfo.StartTime))
            {
                StartTime = DateTime.Parse(YaeherModuleInfo.StartTime);
                if (string.IsNullOrEmpty(YaeherModuleInfo.EndTime))
                {
                    YaeherModuleInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(YaeherModuleInfo.EndTime))
            {
                EndTime = DateTime.Parse(YaeherModuleInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(YaeherModuleInfo.StartTime))
            {
                YaeherModuleInfo.AndAlso(t => t.CreatedOn >= StartTime);
                YaeherModuleInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(YaeherModuleInfo.KeyWord))
            {
                YaeherModuleInfo.AndAlso(t => t.Names.Contains(YaeherModuleInfo.KeyWord)
                                           || t.Actionss.Contains(YaeherModuleInfo.KeyWord)
                                           || t.Areas.Contains(YaeherModuleInfo.KeyWord)
                                           || t.Codes.Contains(YaeherModuleInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(YaeherModuleInfo.Enabled))
            {
                YaeherModuleInfo.AndAlso(t => t.Enabled == bool.Parse(YaeherModuleInfo.Enabled));
            }
            YaeherModuleInfo.AndAlso(a => a.IsDelete == false);
            var values = await _yaeherModuleService.YaeherModuleList(YaeherModuleInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherModuleList",
                OperContent = JsonHelper.ToJson(YaeherModuleInfo),
                OperType = "YaeherModuleList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 菜单上级菜单 List 
        /// </summary>
        /// <param name="YaeherModuleInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherModuleUpperList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherModuleUpperList([FromBody]YaeherModuleIn YaeherModuleInfo)
        {
            if (!Commons.CheckSecret(YaeherModuleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherModuleInfo.AndAlso(a => a.IsDelete == false);
            YaeherModuleInfo.AndAlso(a => a.Enabled == true);
            var values = await _yaeherModuleService.YaeherModuleList(YaeherModuleInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherModuleUpperList",
                OperContent = JsonHelper.ToJson(YaeherModuleInfo),
                OperType = "YaeherModuleUpperList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 菜单表 Byid
        /// </summary>
        /// <param name="YaeherModuleInfo"></param>
        /// <returns></returns>
        [Route("api/ReleaseById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherModuleById([FromBody]YaeherModuleIn YaeherModuleInfo)
        {
            if (!Commons.CheckSecret(YaeherModuleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _yaeherModuleService.YaeherModuleByID(YaeherModuleInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherModuleById",
                OperContent = JsonHelper.ToJson(YaeherModuleInfo),
                OperType = "YaeherModuleById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 菜单与角色关联表
        /// <summary>
        /// 菜单与角色关联表 新增
        /// </summary>
        /// <param name="YaeherRoleModuleJSon"></param>
        /// <returns></returns>
        [Route("api/CreateYaeherRoleModule")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateYaeherRoleModule([FromBody]YaeherRoleModuleJSon YaeherRoleModuleJSon)
        {
            if (!Commons.CheckSecret(YaeherRoleModuleJSon.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            int RoleModuleId = 0;
            int RoleID = YaeherRoleModuleJSon.RoleId;
            String Secret = YaeherRoleModuleJSon.Secret;
            string[] ModuleId = null;
            if (!string.IsNullOrEmpty(YaeherRoleModuleJSon.ModuleId))
            {
                ModuleId = YaeherRoleModuleJSon.ModuleId.Split(',');
            }
            if (!Commons.CheckSecret(Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            #region 删除现有所有数据
            YaeherRoleModuleIn yaeherRoleModuleIn = new YaeherRoleModuleIn();
            yaeherRoleModuleIn.RoleId = RoleID;
            var values = await _yaeherRoleModuleService.YaeherRoleModuleList(yaeherRoleModuleIn);
            if (values.Count() > 0)
            {
                foreach (var RoleModuleInfo in values)
                {
                    RoleModuleInfo.DeleteBy = userid;
                    RoleModuleInfo.DeleteTime = DateTime.Now;
                    RoleModuleInfo.IsDelete = true;
                    var res = await _yaeherRoleModuleService.DeleteYaeherRoleModule(RoleModuleInfo);
                }
            }
            #endregion
            #region 新增权限与菜单数据
            if (ModuleId.Length > 0)
            {
                for (int a=0;a< ModuleId.Length;a++)
                {
                    var CreateYaeherRoleModule = new YaeherRoleModule()
                    {
                        RoleId = RoleID,
                        ModuleId = int.Parse(ModuleId[a]),
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        IsDelete = false
                    };
                    var result = await _yaeherRoleModuleService.CreateYaeherRoleModule(CreateYaeherRoleModule);
                    RoleModuleId += result.Id;
                }
            }
            #endregion
            if (RoleModuleId > 0)
            {
                this.ObjectResultModule.Object = RoleModuleId;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateYaeherRoleModule",
                OperContent = JsonHelper.ToJson(YaeherRoleModuleJSon),
                OperType = "CreateYaeherRoleModule",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        /// <summary>
        /// 菜单与角色关联表 List  
        /// </summary>
        /// <param name="YaeherRoleModuleInfo"></param>
        /// <returns></returns>
        [Route("api/YaeherRoleModuleList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> YaeherRoleModuleList([FromBody]YaeherRoleModuleIn YaeherRoleModuleInfo)
        {
            if (!Commons.CheckSecret(YaeherRoleModuleInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherRoleModuleInfo.AndAlso(a => a.IsDelete == false);
            var values = await _yaeherRoleModuleService.YaeherRoleModuleList(YaeherRoleModuleInfo);
            YaeherModuleIn yaeherModuleIn = new YaeherModuleIn();
            yaeherModuleIn.AndAlso(a => a.IsDelete == false&& a.Enabled==true);
            var ModuleList= await _yaeherModuleService.YaeherModule(yaeherModuleIn);
            var RoleAndModuleList = from Modules in ModuleList
                                    join RoleAndModule in values on Modules.Id equals RoleAndModule.ModuleId
                                    select new { Modules.Id,Modules.ParentId,Modules.Names,Modules.LinkUrls,Modules.Areas,
                                        Modules.Controllers,Modules.Actionss,Modules.Icons,Modules.Codes,Modules.OrderSort,
                                        Modules.Description,Modules.IsMenu,
                                        RoleAndModule.RoleId
                                    };
            if (RoleAndModuleList.Count()== 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                YaeherRoleModuleJSon yaeherRoleModuleJSon = new YaeherRoleModuleJSon();
                foreach (var RoleModuls in RoleAndModuleList)
                {
                    yaeherRoleModuleJSon.ModuleId += RoleModuls.Id + ",";
                    yaeherRoleModuleJSon.RoleId = RoleModuls.RoleId;
                }
                this.ObjectResultModule.Object = yaeherRoleModuleJSon;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "YaeherRoleModuleList",
                OperContent = JsonHelper.ToJson(YaeherRoleModuleInfo),
                OperType = "YaeherRoleModuleList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
      
        #endregion
    }
}