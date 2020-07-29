using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.SystemManage;

namespace YaeherPatientAPI.Web.Host.Controllers
{
    /// <summary>
    /// 患者成员管理API
    /// </summary>
    public class YaeherPatientLeaguerInfoController : YaeherAppServiceBase
    {
        private readonly ILeaguerInfoService _LeaguerService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherOperListService _yaeherOperListService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="leaguerservice"></param>
        /// <param name="session"></param>
        /// <param name="yaeherOperListService"></param>
        public YaeherPatientLeaguerInfoController(ILeaguerInfoService leaguerservice, 
                                                  IAbpSession session,
                                                  IYaeherOperListService yaeherOperListService)
        {
            _LeaguerService = leaguerservice;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
        }
        /// <summary>
        /// 新增患者成员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateLeaguerInfo")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateLeaguerInfo([FromBody] YaeherPatientLeaguerInfo input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (input.PhoneNumber != ""||input.PhoneNumber!=null)
            {
                input.PhoneNumber = input.PhoneNumber.Replace(" ", "");
                bool IsPhone=Regex.IsMatch(input.PhoneNumber, "^(0\\d{2,3}-?\\d{7,8}(-\\d{3,5}){0,1})|((1)\\d{10})$");
                if (!IsPhone ||(input.PhoneNumber.Length>0 && input.PhoneNumber.Length != 11))
                {
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "请输入11位手机号码。";
                    this.ObjectResultModule.Object = "";
                    return this.ObjectResultModule;
                }
            }
            var create = new YaeherPatientLeaguerInfo()
            {
                LeaguerName = input.LeaguerName,
                UserID = userid,
                Relationship = input.Relationship,
                PhoneNumber = input.PhoneNumber,
                Birthday = input.Birthday,
                Sex = input.Sex,
                HasAllergic = input.HasAllergic,
                AllergicHistory = input.AllergicHistory,
                Address = input.Address,
                Email = input.Email,
                Wechat = input.Wechat,
                IDCard = input.IDCard,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _LeaguerService.CreateLeaguerInfo(create);
            if (res.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = res;
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
                OperExplain = "CreateLeaguerInfo",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateLeaguerInfo",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        
        /// <summary>
        /// 获取患者成员列表
        /// </summary>
        /// <param name="PageLeaguer"> PageLeaguer 数据</param>
        /// <returns></returns>
        [Route("api/LeaguerInfoPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> LeaguerInfoPage([FromBody]LeaguerInfo PageLeaguer)
        {
            if (!Commons.CheckSecret(PageLeaguer.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PageLeaguer.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            if (!string.IsNullOrEmpty(PageLeaguer.Name))
            {
                PageLeaguer.AndAlso(t => t.LeaguerName.Contains(PageLeaguer.Name));
            }

            var values = await _LeaguerService.LeaguerInfoPage(PageLeaguer);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new LeaguerInfoOutPage(values, PageLeaguer);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "LeaguerInfoPage",
                OperContent = JsonHelper.ToJson(PageLeaguer),
                OperType = "LeaguerInfoPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 患者成员详情
        /// </summary>
        /// <param name="PageLeaguer"> PageLeaguer 数据</param>
        /// <returns></returns>
        [Route("api/LeaguerInfoById")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> LeaguerInfoById([FromBody]LeaguerInfoById PageLeaguer)
        {
            if (!Commons.CheckSecret(PageLeaguer.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = await _LeaguerService.LeaguerInfoById(PageLeaguer.Id);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "LeaguerInfoById",
                OperContent = JsonHelper.ToJson(PageLeaguer),
                OperType = "LeaguerInfoById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取患者成员List 
        /// </summary>
        /// <param name="PageLeaguer"> PageLeaguer 数据</param>
        /// <returns></returns>
        [Route("api/LeaguerInfoList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> LeaguerInfoList([FromBody]LeaguerInfo PageLeaguer)
        {
            if (!Commons.CheckSecret(PageLeaguer.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PageLeaguer.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            if (!string.IsNullOrEmpty(PageLeaguer.Name))
            {
                PageLeaguer.AndAlso(t => t.LeaguerName.Contains(PageLeaguer.Name));
            }
            var value = await _LeaguerService.LeaguerInfoList(PageLeaguer);

            this.ObjectResultModule.Object = value;
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "LeaguerInfoList",
                OperContent = JsonHelper.ToJson(PageLeaguer),
                OperType = "LeaguerInfoList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取患者成员List 
        /// </summary>
        /// <param name="PageLeaguer"> PageLeaguer 数据</param>
        /// <returns></returns>
        [Route("api/QueryLeaguerInfoList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QueryLeaguerInfoList([FromBody]LeaguerInfoIn PageLeaguer)
        {
            if (!Commons.CheckSecret(PageLeaguer.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (PageLeaguer.leaguer==null||PageLeaguer.leaguer.Count < 1)
            {
                return new ObjectResultModule("",200,"");
            }
             var value = await _LeaguerService.QueryLeaguerInfoList(PageLeaguer);
            
            this.ObjectResultModule.Object = value;
            this.ObjectResultModule.Message = "sucess";
            this.ObjectResultModule.StatusCode = 200;

            return this.ObjectResultModule;
        }
        
        /// <summary>
        /// 更新患者成员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateLeaguerInfo")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateLeaguerInfo([FromBody] YaeherPatientLeaguerInfo input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _LeaguerService.LeaguerInfoById(input.Id);
            if (input.PhoneNumber != "" || input.PhoneNumber != null)
            {
                input.PhoneNumber = input.PhoneNumber.Replace(" ", "");
                bool IsPhone = Regex.IsMatch(input.PhoneNumber, "^(0\\d{2,3}-?\\d{7,8}(-\\d{3,5}){0,1})|((1)\\d{10})$");
                if (!IsPhone || (input.PhoneNumber.Length > 0 && input.PhoneNumber.Length != 11))
                {
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "请输入11位手机号码。";
                    this.ObjectResultModule.Object = "";
                    return this.ObjectResultModule;
                }
            }
            if (query != null)
            {
                query.LeaguerName = input.LeaguerName;
                query.UserID = userid;
                query.Relationship = input.Relationship;
                query.PhoneNumber = input.PhoneNumber;
                query.Birthday = input.Birthday;
                query.Sex = input.Sex;
                query.AllergicHistory = input.AllergicHistory;
                query.Address = input.Address;
                query.HasAllergic = input.HasAllergic;
                query.Email = input.Email;
                query.Wechat = input.Wechat;
                query.IDCard = input.IDCard;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _LeaguerService.UpdateLeaguerInfo(query);

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
                OperExplain = "UpdateLeaguerInfo",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateLeaguerInfo",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除患者成员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteLeaguerInfo")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteLeaguerInfo([FromBody] YaeherPatientLeaguerInfo input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _LeaguerService.LeaguerInfoById(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _LeaguerService.DeleteLeaguerInfo(query);

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
                OperExplain = "DeleteLeaguerInfo",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteLeaguerInfo",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
    }
}
