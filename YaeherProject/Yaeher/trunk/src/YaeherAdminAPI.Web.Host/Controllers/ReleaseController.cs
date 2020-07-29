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
using Yaeher.Release;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    ///  文章 问答 发布管理
    /// </summary>
    public class ReleaseController : YaeherAppServiceBase
    {
        private readonly IArticleOperListService _articleOperListService;
        private readonly IQuestionReleaseService _questionReleaseService;
        private readonly IReleaseManageService _releaseManageService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IYaeherUserService _yaeherUserService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IUserManagerService _userManagerService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IDoctorPaperService _doctorPaperService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleOperListService"></param>
        /// <param name="questionReleaseService"></param>
        /// <param name="releaseManageService"></param>
        /// <param name="session"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="yaeherUserService"></param>
        /// <param name="yaeherOperListService"></param>
        /// <param name="doctorPaperService"></param>
        public ReleaseController(IArticleOperListService articleOperListService,
                                IQuestionReleaseService questionReleaseService,
                                IReleaseManageService releaseManageService,
                                IAbpSession session,
                                ISystemParameterService systemParameterService,
                                IYaeherDoctorService yaeherDoctorService,
                                IUserManagerService userManagerService,
                                IYaeherUserService yaeherUserService,
                                IYaeherOperListService yaeherOperListService,
                                IDoctorPaperService doctorPaperService)
        {
            _articleOperListService = articleOperListService;
            _questionReleaseService = questionReleaseService;
            _releaseManageService = releaseManageService;
            _IabpSession = session;
            _yaeherDoctorService = yaeherDoctorService;
            _yaeherUserService = yaeherUserService;
            _systemParameterService = systemParameterService;
            _userManagerService = userManagerService;
            _yaeherOperListService = yaeherOperListService;
            _doctorPaperService = doctorPaperService;
        }

        #region 文章发布
        /// <summary>
        /// 文章发布 新增
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateReleaseManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateReleaseManage([FromBody]ReleaseManage ReleaseManageInfo)
        {
            if (!Commons.CheckSecret(ReleaseManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateRelease = new ReleaseManage()
            {
                PaperTiltle = ReleaseManageInfo.PaperTiltle,
                PaperContent = ReleaseManageInfo.PaperContent,
                PaperFrom = ReleaseManageInfo.PaperFrom,
                DoctorID = ReleaseManageInfo.DoctorID,
                DoctorName = ReleaseManageInfo.DoctorName,
                ConsultNumber = ReleaseManageInfo.ConsultNumber,
                CheckState = ReleaseManageInfo.CheckState,
                CheckRemark = ReleaseManageInfo.CheckRemark,
                CheckTime = ReleaseManageInfo.CheckTime,
                Checker = ReleaseManageInfo.Checker,
                ReadTotal = ReleaseManageInfo.ReadTotal,
                UpvoteTotal = ReleaseManageInfo.UpvoteTotal,
                TransTotal = ReleaseManageInfo.TransTotal,
                CollectTotal = ReleaseManageInfo.CollectTotal,
                PaperAddress=ReleaseManageInfo.PaperAddress,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _releaseManageService.CreateReleaseManage(CreateRelease);
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
                OperExplain = "CreateReleaseManage",
                OperContent = JsonHelper.ToJson(ReleaseManageInfo),
                OperType = "CreateReleaseManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 文章发布 修改
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateReleaseManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateReleaseManage([FromBody] ReleaseManage ReleaseManageInfo)
        {
            if (!Commons.CheckSecret(ReleaseManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateRelease = await _releaseManageService.ReleaseManageByID(ReleaseManageInfo.Id);
            if (UpdateRelease != null)
            {
                UpdateRelease.PaperTiltle = ReleaseManageInfo.PaperTiltle;
                UpdateRelease.PaperContent = ReleaseManageInfo.PaperContent;
                UpdateRelease.PaperFrom = ReleaseManageInfo.PaperFrom;
                UpdateRelease.DoctorID = ReleaseManageInfo.DoctorID;

                UpdateRelease.DoctorName = ReleaseManageInfo.DoctorName;
                UpdateRelease.ConsultNumber = ReleaseManageInfo.ConsultNumber;
                UpdateRelease.CheckState = ReleaseManageInfo.CheckState;
                UpdateRelease.CheckRemark = ReleaseManageInfo.CheckRemark;
                UpdateRelease.CheckTime = ReleaseManageInfo.CheckTime;
                UpdateRelease.Checker = ReleaseManageInfo.Checker;
                UpdateRelease.ReadTotal = ReleaseManageInfo.ReadTotal;
                UpdateRelease.UpvoteTotal = ReleaseManageInfo.UpvoteTotal;
                UpdateRelease.TransTotal = ReleaseManageInfo.TransTotal;
                UpdateRelease.CollectTotal = ReleaseManageInfo.CollectTotal;
                UpdateRelease.PaperAddress = ReleaseManageInfo.PaperAddress;
                UpdateRelease.ModifyOn = DateTime.Now;
                UpdateRelease.ModifyBy = userid;

                var result = await _releaseManageService.UpdateReleaseManage(UpdateRelease);

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
                OperExplain = "UpdateReleaseManage",
                OperContent = JsonHelper.ToJson(ReleaseManageInfo),
                OperType = "UpdateReleaseManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 文章发布 删除
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteReleaseManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteReleaseManage([FromBody] ReleaseManage ReleaseManageInfo)
        {
            if (!Commons.CheckSecret(ReleaseManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _releaseManageService.ReleaseManageByID(ReleaseManageInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _releaseManageService.DeleteReleaseManage(query);

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
                OperExplain = "DeleteReleaseManage",
                OperContent = JsonHelper.ToJson(ReleaseManageInfo),
                OperType = "DeleteReleaseManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 文章发布 Page
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [Route("api/ReleaseManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ReleaseManagePage([FromBody]ReleaseManageIn ReleaseManageInfo)
        {
            if (!Commons.CheckSecret(ReleaseManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ReleaseManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(ReleaseManageInfo.StartTime);
                if (string.IsNullOrEmpty(ReleaseManageInfo.EndTime))
                {
                    ReleaseManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ReleaseManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(ReleaseManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ReleaseManageInfo.StartTime))
            {
                ReleaseManageInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ReleaseManageInfo.KeyWord))
            {
                ReleaseManageInfo.AndAlso(t => t.PaperTiltle.Contains(ReleaseManageInfo.KeyWord));
            }
            if (!string.IsNullOrEmpty(ReleaseManageInfo.CheckState))
            {
                  ReleaseManageInfo.AndAlso(t => t.CheckState==ReleaseManageInfo.CheckState);
            }
            ReleaseManageInfo.AndAlso(t => t.IsDelete == false);
            var values = await _releaseManageService.ReleaseManagePage(ReleaseManageInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ReleaseManageOut(values, ReleaseManageInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ReleaseManagePage",
                OperContent = JsonHelper.ToJson(ReleaseManageInfo),
                OperType = "ReleaseManagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 文章发布 List 
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [Route("api/ReleaseManageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ReleaseManageList([FromBody]ReleaseManageIn ReleaseManageInfo)
        {
            if (!Commons.CheckSecret(ReleaseManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ReleaseManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(ReleaseManageInfo.StartTime);
                if (string.IsNullOrEmpty(ReleaseManageInfo.EndTime))
                {
                    ReleaseManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ReleaseManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(ReleaseManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(ReleaseManageInfo.StartTime))
            {
                ReleaseManageInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(ReleaseManageInfo.KeyWord))
            {
                ReleaseManageInfo.AndAlso(t => t.PaperTiltle.Contains(ReleaseManageInfo.KeyWord));
            }
            ReleaseManageInfo.AndAlso(t => t.IsDelete == false);
            var values = await _releaseManageService.ReleaseManageList(ReleaseManageInfo);
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
                OperExplain = "ReleaseManageList",
                OperContent = JsonHelper.ToJson(ReleaseManageInfo),
                OperType = "ReleaseManageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 文章发布 Byid
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [Route("api/ReleaseManageById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ReleaseManageById([FromBody]ReleaseManageIn ReleaseManageInfo)
        {
            if (!Commons.CheckSecret(ReleaseManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _releaseManageService.ReleaseManageByID(ReleaseManageInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
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
                OperExplain = "ReleaseManageById",
                OperContent = JsonHelper.ToJson(ReleaseManageInfo),
                OperType = "ReleaseManageById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }



        /// <summary>
        /// 发布文章  展示到咨询菜单中的文章  以及医生主页上显示的文章
        /// </summary>
        /// <param name="ReleaseManageInfo"></param>
        /// <returns></returns>
        [Route("api/ReleaseArticle")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ReleaseArticle([FromBody] ReleaseManage ReleaseManageInfo)
        {
            if (!Commons.CheckSecret(ReleaseManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var ReleaseInfo = await _releaseManageService.ReleaseManageByID(ReleaseManageInfo.Id);
            if (ReleaseInfo != null)
            {
                ReleaseInfo.Checker = userid;
                ReleaseInfo.CheckRemark = "文章发布";
                ReleaseInfo.CheckTime = DateTime.Now; ;
                ReleaseInfo.CheckState = "success";
                ReleaseInfo.ModifyOn = DateTime.Now;
                ReleaseInfo.ModifyBy = userid;

                if (ReleaseInfo.PaperFrom == "doctor")
                {
                    DoctorPaper doctorPaper = await _doctorPaperService.DoctorPaperByID(ReleaseInfo.DoctorPaperID);

                    doctorPaper.PaperTiltle = ReleaseInfo.PaperTiltle;
                    doctorPaper.PaperContent = ReleaseInfo.PaperContent;
                    doctorPaper.PaperFrom = ReleaseInfo.PaperFrom;
                    doctorPaper.DoctorID = ReleaseInfo.DoctorID;
                    doctorPaper.DoctorName = ReleaseInfo.DoctorName;
                    doctorPaper.ConsultNumber = ReleaseInfo.ConsultNumber;
                    doctorPaper.CheckState = ReleaseInfo.CheckState;
                    doctorPaper.CheckRemark = ReleaseInfo.CheckRemark;
                    doctorPaper.CheckTime = ReleaseInfo.CheckTime;
                    doctorPaper.Checker = ReleaseInfo.Checker;
                    doctorPaper.PaperAddress = ReleaseInfo.PaperAddress;
                    doctorPaper.ModifyOn = DateTime.Now;
                    doctorPaper.ModifyBy = userid;
                    var UpdateDoctorPaper = await _doctorPaperService.UpdateDoctorPaper(doctorPaper);
                }
                var result = await _releaseManageService.UpdateReleaseManage(ReleaseInfo);
                if (result != null)
                {
                    this.ObjectResultModule.Object = result;
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 500;
                    this.ObjectResultModule.Message = "发布失败！";
                }
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
                OperExplain = "UpdateReleaseManage",
                OperContent = JsonHelper.ToJson(ReleaseManageInfo),
                OperType = "UpdateReleaseManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }

        #endregion

        #region 问答发布
        /// <summary>
        /// 问答发布 新增
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [Route("api/CreateQuestionRelease")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateQuestionRelease([FromBody] QuestionRelease QuestionReleaseInfo)
        {
            if (!Commons.CheckSecret(QuestionReleaseInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateQuestionRelease = new QuestionRelease()
            {
                DescriptionTiltle = QuestionReleaseInfo.DescriptionTiltle,
                Answer = QuestionReleaseInfo.Answer,
                ReadTotal = QuestionReleaseInfo.ReadTotal,
                UpvoteTotal = QuestionReleaseInfo.UpvoteTotal,
                TransTotal = QuestionReleaseInfo.TransTotal,
                CollectTotal = QuestionReleaseInfo.CollectTotal,
                CheckState = QuestionReleaseInfo.CheckState,
                CheckRemark = QuestionReleaseInfo.CheckRemark,
                Checker = QuestionReleaseInfo.Checker,
                CheckTime = QuestionReleaseInfo.CheckTime,
                DoctorId= userid,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _questionReleaseService.CreateQuestionRelease(CreateQuestionRelease);
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
                OperExplain = "CreateQuestionRelease",
                OperContent = JsonHelper.ToJson(QuestionReleaseInfo),
                OperType = "CreateQuestionRelease",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 问答发布修改
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateQuestionRelease")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateQuestionRelease([FromBody] QuestionRelease QuestionReleaseInfo)
        {
            if (!Commons.CheckSecret(QuestionReleaseInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateQuestionRelease = await _questionReleaseService.QuestionReleaseByID(QuestionReleaseInfo.Id);
            if (UpdateQuestionRelease != null)
            {
                UpdateQuestionRelease.DescriptionTiltle = QuestionReleaseInfo.DescriptionTiltle;
                UpdateQuestionRelease.Answer = QuestionReleaseInfo.Answer;
                UpdateQuestionRelease.ReadTotal = QuestionReleaseInfo.ReadTotal;
                UpdateQuestionRelease.UpvoteTotal = QuestionReleaseInfo.UpvoteTotal;
                UpdateQuestionRelease.TransTotal = QuestionReleaseInfo.TransTotal;
                UpdateQuestionRelease.CollectTotal = QuestionReleaseInfo.CollectTotal;
                UpdateQuestionRelease.CheckState = QuestionReleaseInfo.CheckState;
                UpdateQuestionRelease.CheckRemark = QuestionReleaseInfo.CheckRemark;
                UpdateQuestionRelease.Checker = QuestionReleaseInfo.Checker;
                UpdateQuestionRelease.CheckTime = QuestionReleaseInfo.CheckTime;
                UpdateQuestionRelease.DoctorId = userid;
                UpdateQuestionRelease.ModifyOn = DateTime.Now;
                UpdateQuestionRelease.ModifyBy = userid;

                var result = await _questionReleaseService.UpdateQuestionRelease(UpdateQuestionRelease);

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
                OperExplain = "UpdateQuestionRelease",
                OperContent = JsonHelper.ToJson(QuestionReleaseInfo),
                OperType = "UpdateQuestionRelease",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 问答发布删除
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteQuestionRelease")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteQuestionRelease([FromBody] QuestionRelease QuestionReleaseInfo)
        {
            if (!Commons.CheckSecret(QuestionReleaseInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _questionReleaseService.QuestionReleaseByID(QuestionReleaseInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _questionReleaseService.DeleteQuestionRelease(query);

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
                OperExplain = "DeleteQuestionRelease",
                OperContent = JsonHelper.ToJson(QuestionReleaseInfo),
                OperType = "DeleteQuestionRelease",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 问答发布 Page
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [Route("api/QuestionReleasePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QuestionReleasePage([FromBody]QuestionReleaseIn QuestionReleaseInfo)
        {
            if (!Commons.CheckSecret(QuestionReleaseInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            {
                StartTime = DateTime.Parse(QuestionReleaseInfo.StartTime);
                if (string.IsNullOrEmpty(QuestionReleaseInfo.EndTime))
                {
                    QuestionReleaseInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.EndTime))
            {
                EndTime = DateTime.Parse(QuestionReleaseInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            {
                QuestionReleaseInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.KeyWord))
            {
                QuestionReleaseInfo.AndAlso(t => t.Answer.Contains(QuestionReleaseInfo.KeyWord)|| t.DescriptionTiltle.Contains(QuestionReleaseInfo.KeyWord));
            }
            QuestionReleaseInfo.AndAlso(t => t.IsDelete == false);
            var values = await _questionReleaseService.QuestionReleasePage(QuestionReleaseInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new QuestionReleaseOut(values, QuestionReleaseInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QuestionReleasePage",
                OperContent = JsonHelper.ToJson(QuestionReleaseInfo),
                OperType = "QuestionReleasePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 问答发布 List 
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [Route("api/QuestionReleaseList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QuestionReleaseList([FromBody]QuestionReleaseIn QuestionReleaseInfo)
        {
            if (!Commons.CheckSecret(QuestionReleaseInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            {
                StartTime = DateTime.Parse(QuestionReleaseInfo.StartTime);
                if (string.IsNullOrEmpty(QuestionReleaseInfo.EndTime))
                {
                    QuestionReleaseInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.EndTime))
            {
                EndTime = DateTime.Parse(QuestionReleaseInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.StartTime))
            {
                QuestionReleaseInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(QuestionReleaseInfo.KeyWord))
            {
                QuestionReleaseInfo.AndAlso(t => t.Answer.Contains(QuestionReleaseInfo.KeyWord) || t.DescriptionTiltle.Contains(QuestionReleaseInfo.KeyWord));
            }
            QuestionReleaseInfo.AndAlso(t => t.IsDelete == false);
            var values = await _questionReleaseService.QuestionReleaseList(QuestionReleaseInfo);
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
                OperExplain = "QuestionReleaseList",
                OperContent = JsonHelper.ToJson(QuestionReleaseInfo),
                OperType = "QuestionReleaseList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 问答详情
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [Route("api/QuestionReleaseById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QuestionReleaseById([FromBody]QuestionReleaseIn QuestionReleaseInfo)
        {
            if (!Commons.CheckSecret(QuestionReleaseInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _questionReleaseService.PatientQuestionReleaseByID(QuestionReleaseInfo.Id);
            
            var operater = await _articleOperListService.ArticleOperListByExpression(t =>t.IsDelete==false && t.ArticleID == values.Id );
            var haspraise = operater.FirstOrDefault(t=> t.CreatedBy == userid&&t.Type== "praise" && t.OperType== "question");
            YaeherDoctorUser yaeherDoctorUser = new YaeherDoctorUser() ;
            //yaeherDoctorUser.UserImage = User.YaeherUserInfo.UserImage;
            //yaeherDoctorUser.DoctorName = User.YaeherDoctorInfo.DoctorName;
            //yaeherDoctorUser.Title = User.YaeherDoctorInfo.Title;
            //yaeherDoctorUser.HospitalName = User.YaeherDoctorInfo.HospitalName;
            //yaeherDoctorUser.Department = User.YaeherDoctorInfo.Department;
            var releasedetail = new QuestionReleaseDeatil(values, haspraise == null ? false : true, yaeherDoctorUser);
          

            var paramread = new SystemParameterIn() { SystemType = "ReadQuestionReleaseMinutesTime" };
            var readpara = await _systemParameterService.ParameterList(paramread);

            var nowday = DateTime.Now.AddHours(-double.Parse(readpara[0].ItemValue));
            var nextday = nowday.AddHours(DateTime.Now.Hour+double.Parse(readpara[0].ItemValue));
            var questionrelease = await _questionReleaseService.QuestionReleaseByID(values.Id);
            var read = operater.Where(t=>t.CreatedOn>nowday&&t.CreatedOn<nextday&&t.OperType== "question" && t.Type== "read").ToList();
            if (read.Count==0)
            {
                questionrelease.ReadTotal++;
                var res = await _questionReleaseService.UpdateQuestionRelease(questionrelease);
                var CreateArticleOper = new ArticleOperList()
                {
                    ArticleID = QuestionReleaseInfo.Id,
                    OperType = "question",
                    Type = "read",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    ModifyOn = DateTime.Now,
                    ModifyBy = userid
                };
                var ressult = await _articleOperListService.CreateArticleOperList(CreateArticleOper);
            }
            this.ObjectResultModule.Object = releasedetail;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QuestionReleaseById",
                OperContent = JsonHelper.ToJson(QuestionReleaseInfo),
                OperType = "QuestionReleaseById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }

        /// <summary>
        /// 问答详情点赞
        /// </summary>
        /// <param name="QuestionReleaseInfo"></param>
        /// <returns></returns>
        [Route("api/QuestionReleasepraise")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> QuestionReleasepraise([FromBody]QuestionReleaseIn QuestionReleaseInfo)
        {
            if (!Commons.CheckSecret(QuestionReleaseInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var questionrelease = await _questionReleaseService.QuestionReleaseByID(QuestionReleaseInfo.Id);

            var Articlelist = await this._articleOperListService.ArticleOperListByExpression(t =>t.CreatedBy==userid&& t.OperType=="praise" &&t.ArticleID == questionrelease.Id);
            var Article = Articlelist.FirstOrDefault();
            if (Article != null)
            {
                if (!Article.IsDelete)
                {
                    questionrelease.UpvoteTotal--;
                    await _questionReleaseService.UpdateQuestionRelease(questionrelease);
                    Article.IsDelete = true;
                    Article.DeleteBy = userid;
                    Article.DeleteTime = DateTime.Now;
                    var res = await _articleOperListService.UpdateArticleOperList(Article);
                    this.ObjectResultModule.Message = "unpraise success";
                }
                else
                {
                    questionrelease.UpvoteTotal++;
                    await _questionReleaseService.UpdateQuestionRelease(questionrelease);

                    Article.IsDelete = false;
                    Article.DeleteBy = 0;
                    var res = await _articleOperListService.UpdateArticleOperList(Article);

                    this.ObjectResultModule.Message = "praise success";
                }
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var CreateArticleOper = new ArticleOperList()
                {
                    ArticleID = QuestionReleaseInfo.Id,
                    OperType = "question",
                    Type="praise",
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    ModifyOn = DateTime.Now,
                    ModifyBy = userid
                };
                var res = await _articleOperListService.CreateArticleOperList(CreateArticleOper);
                if (res.Id > 0)
                {
                    questionrelease.UpvoteTotal++;
                    await _questionReleaseService.UpdateQuestionRelease(questionrelease);
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "collect sucess";
                    this.ObjectResultModule.Object = res;
                }
                else
                {
                    this.ObjectResultModule.Object = "";
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "error!";
                }
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "QuestionReleasepraise",
                OperContent = JsonHelper.ToJson(QuestionReleaseInfo),
                OperType = "QuestionReleasepraise",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        #endregion

        #region 文章 问答操作日志
        /// <summary>
        ///文章 问答操作类型
        /// </summary>
        /// <param name="ConsultationReplyParameter"></param>
        /// <returns></returns>
        [Route("api/ArticleOperParameter")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ArticleOperParameter([FromBody] ArticleOperList ConsultationReplyParameter)
        {

            if (!Commons.CheckSecret(ConsultationReplyParameter.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "Articleoper");
            var paramlist = await _systemParameterService.ParameterList(param);
            var ArticleOperType = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode= new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode =item.SystemCode };
                ArticleOperType.Add(newcode);
            }
            //{
            //    new CodeList(){Code="read",Value="阅读",Type="文章操作",TypeCode="articleoper"},
            //    new CodeList(){Code="praise",Value="点赞",Type="文章操作",TypeCode="articleoper"},
            //    new CodeList(){Code="relay",Value="转发",Type="文章操作",TypeCode="articleoper"},
            //    new CodeList(){Code="collect",Value="收藏",Type="文章操作",TypeCode="articleoper"},
            //};
            var ArticleOper = new ArticleOperListOParam(ArticleOperType);
            this.ObjectResultModule.Object = ArticleOper;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 文章 问答操作日志 修改
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateArticleOper")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateArticleOper([FromBody]  ArticleOperList ArticleOperListInfo)
        {
            if (!Commons.CheckSecret(ArticleOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateArticleOper = await _articleOperListService.ArticleOperListByID(ArticleOperListInfo.Id);
            if (UpdateArticleOper != null)
            {
                UpdateArticleOper.ArticleID = ArticleOperListInfo.ArticleID;
                UpdateArticleOper.OperType = ArticleOperListInfo.OperType;
                UpdateArticleOper.ModifyOn = DateTime.Now;
                UpdateArticleOper.ModifyBy = userid;
                var result = await _articleOperListService.UpdateArticleOperList(UpdateArticleOper);

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
            return ObjectResultModule;
        }
        /// <summary>
        /// 文章 问答操作日志 删除
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteArticleOper")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteArticleOper([FromBody]  ArticleOperList ArticleOperListInfo)
        {
            if (!Commons.CheckSecret(ArticleOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _articleOperListService.ArticleOperListByID(ArticleOperListInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _articleOperListService.DeleteArticleOperList(query);

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
            return this.ObjectResultModule;

        }

        /// <summary>
        /// 文章 问答操作日志 Page
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [Route("api/ArticleOperPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ArticleOperPage([FromBody] ArticleOperListIn ArticleOperListInfo)
        {
            if (!Commons.CheckSecret(ArticleOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ArticleOperListInfo.StartTime))
            {
                if (string.IsNullOrEmpty(ArticleOperListInfo.EndTime))
                {
                    ArticleOperListInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ArticleOperListInfo.StartTime))
            {
                StartTime = DateTime.Parse(ArticleOperListInfo.StartTime);
            }
            if (!string.IsNullOrEmpty(ArticleOperListInfo.EndTime))
            {
                EndTime = DateTime.Parse(ArticleOperListInfo.EndTime).AddDays(+1);
            }
            if (!string.IsNullOrEmpty(ArticleOperListInfo.OperType))
            {
                ArticleOperListInfo.AndAlso(a => a.OperType == ArticleOperListInfo.OperType);
            }
            ArticleOperListInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(ArticleOperListInfo.StartTime))
            {
                ArticleOperListInfo.AndAlso(a => a.CreatedOn >= StartTime);
                ArticleOperListInfo.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            var values = await _articleOperListService.ArticleOperListPage(ArticleOperListInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ArticleOperListOut(values, ArticleOperListInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return this.ObjectResultModule;
        }

        /// <summary>
        /// 文章 问答操作日志 List 
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [Route("api/ArticleOperList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ArticleOperList([FromBody]ArticleOperListIn ArticleOperListInfo)
        {
            if (!Commons.CheckSecret(ArticleOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(ArticleOperListInfo.StartTime))
            {
                if (string.IsNullOrEmpty(ArticleOperListInfo.EndTime))
                {
                    ArticleOperListInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(ArticleOperListInfo.StartTime))
            {
                StartTime = DateTime.Parse(ArticleOperListInfo.StartTime);
            }
            if (!string.IsNullOrEmpty(ArticleOperListInfo.EndTime))
            {
                EndTime = DateTime.Parse(ArticleOperListInfo.EndTime).AddDays(+1);
            }
            if (!string.IsNullOrEmpty(ArticleOperListInfo.OperType))
            {
                ArticleOperListInfo.AndAlso(a => a.OperType == ArticleOperListInfo.OperType);
            }
            ArticleOperListInfo.AndAlso(a => a.IsDelete == false);
            if (!string.IsNullOrEmpty(ArticleOperListInfo.StartTime))
            {
                ArticleOperListInfo.AndAlso(a => a.CreatedOn >= StartTime);
                ArticleOperListInfo.AndAlso(a => a.CreatedOn < EndTime.AddDays(+1));
            }
            var values = await _articleOperListService.ArticleOperListList(ArticleOperListInfo);
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
            return ObjectResultModule;
        }
        /// <summary>
        /// 文章 问答操作日志 Byid
        /// </summary>
        /// <param name="ArticleOperListInfo"></param>
        /// <returns></returns>
        [Route("api/ArticleOperById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ArticleOperById([FromBody]ArticleOperListIn ArticleOperListInfo)
        {
            if (!Commons.CheckSecret(ArticleOperListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _articleOperListService.ArticleOperListByID(ArticleOperListInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = values;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            return ObjectResultModule;
        }
        #endregion
    }
}