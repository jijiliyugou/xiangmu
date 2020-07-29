using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Myvas.AspNetCore.TencentCos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 附件
    /// </summary>
    public class AttachmentController : YaeherAppServiceBase
    {
        private readonly IAttachmentServices _attachmentService;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IYaeherDoctorService _yaeherDoctorService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDoctorFileApplyService _doctorFileApplyService;
        private readonly IAbpSession _IabpSession;
        private readonly IUserManagerService _userManagerService;
        private readonly IYaeherOperListService _yaeherOperListService;

        /// <summary>
        /// 科室 科室与标签 科室与医生关系
        /// </summary>
        /// <param name="attachmentService"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="session"></param>
        /// <param name="yaeherDoctorService"></param>
        /// <param name="doctorFileApplyService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="yaeherOperListService"></param>
        public AttachmentController(IAttachmentServices attachmentService,
                                    ISystemParameterService systemParameterService,
                                    IUnitOfWorkManager unitOfWorkManager,
                                    IAbpSession session,
                                    IYaeherDoctorService yaeherDoctorService,
                                    IDoctorFileApplyService doctorFileApplyService,
                                    IUserManagerService userManagerService,
                                    IYaeherOperListService yaeherOperListService)
        {
            _attachmentService = attachmentService;
            _systemParameterService = systemParameterService;
            _unitOfWorkManager = unitOfWorkManager;
            _yaeherDoctorService = yaeherDoctorService;
            _doctorFileApplyService = doctorFileApplyService;
            _IabpSession = session;
            _userManagerService = userManagerService;
            _yaeherOperListService = yaeherOperListService;
            //_cosHandler = cosHandler ?? throw new ArgumentNullException(nameof(cosHandler));
        }

        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="AttachmentInfo"></param>
        /// <returns></returns>
        [Route("api/CreateAttachment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateAttachment([FromBody]AttachmentIn AttachmentInfo)
        {
            if (!Commons.CheckSecret(AttachmentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
            var paramlist = await _systemParameterService.ParameterList(param);

            var att = new AttachmentService();
            if (!string.IsNullOrEmpty(AttachmentInfo.Filename))
            {
                att.Filename = AttachmentInfo.Filename;
                att.FileAddress = paramlist[0].ItemValue + "/" + AttachmentInfo.ServiceType + "/" + AttachmentInfo.MediaType + "/" + AttachmentInfo.Filename;
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.TypeDetail))
            {
                att.TypeDetail = AttachmentInfo.TypeDetail;
                att.FileAddress = paramlist[0].ItemValue + "/" + AttachmentInfo.ServiceType + "/" + AttachmentInfo.MediaType + "/" + AttachmentInfo.TypeDetail + "/" + AttachmentInfo.Filename;
            }

            if (AttachmentInfo.FileSize > 0) { att.FileSize = AttachmentInfo.FileSize; }
            if (AttachmentInfo.ConsultID > 0) { att.ConsultID = AttachmentInfo.ConsultID; }

            if (AttachmentInfo.ServiceID > 0) { att.ServiceID = AttachmentInfo.ServiceID; }
            att.FileFrom = AttachmentInfo.ServiceType;
            att.CreatedBy = userid;
            att.FileType = AttachmentInfo.MediaType;
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                if (AttachmentInfo.Id > 0 && AttachmentInfo.IsDelete)
                {
                    var file = await _attachmentService.AttachmentServiceInfoByID(AttachmentInfo.Id);
                    if (file != null)
                    {
                        file.IsDelete = true;
                        file.DeleteBy = userid;
                        file.DeleteTime = DateTime.Now;
                        await _attachmentService.DeleteAttachment(file);
                    }
                }
                if (AttachmentInfo.Id < 0)
                {
                    var result = await _attachmentService.CreateAttachment(att);
                    this.ObjectResultModule.Object = result;
                }

                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";

                unitOfWork.Complete();

            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateAttachment",
                OperContent = JsonHelper.ToJson(AttachmentInfo),
                OperType = "CreateAttachment",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion
            return ObjectResultModule;
        }
        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="AttachmentInfo"></param>
        /// <returns></returns>
        [Route("api/CreateMobileAttachment")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateMobileAttachment([FromBody]AttachmentIn AttachmentInfo)
        {
            if (!Commons.CheckSecret(AttachmentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
            var paramlist = await _systemParameterService.ParameterList(param);
            var att = new AttachmentService();
            if (!string.IsNullOrEmpty(AttachmentInfo.Filename))
            {
                att.Filename = AttachmentInfo.Filename;
                att.FileAddress = paramlist[0].ItemValue + "/" + AttachmentInfo.ServiceType + "/" + AttachmentInfo.MediaType + "/" + AttachmentInfo.Filename;
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.TypeDetail))
            {
                att.TypeDetail = AttachmentInfo.TypeDetail;
                att.FileAddress = paramlist[0].ItemValue + "/" + AttachmentInfo.ServiceType + "/" + AttachmentInfo.MediaType + "/" + AttachmentInfo.TypeDetail + "/" + AttachmentInfo.Filename;
            }

            if (AttachmentInfo.FileSize > 0) { att.FileSize = AttachmentInfo.FileSize; }
            if (AttachmentInfo.ConsultID > 0) { att.ConsultID = AttachmentInfo.ConsultID; }

            if (AttachmentInfo.ServiceID > 0) { att.ServiceID = AttachmentInfo.ServiceID; }
            att.FileFrom = AttachmentInfo.ServiceType;
            att.CreatedBy = AttachmentInfo.CreatedBy;
            att.FileType = AttachmentInfo.MediaType;
            att.ConsultNumber = AttachmentInfo.ConsultNumber;
            if (!string.IsNullOrEmpty(AttachmentInfo.ServiceNumber)) { att.ServiceNumber = AttachmentInfo.ServiceNumber; }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                if (AttachmentInfo.Id > 0 && AttachmentInfo.IsDelete)
                {
                    var file = await _attachmentService.AttachmentServiceInfoByID(AttachmentInfo.Id);
                    if (file != null)
                    {
                        file.IsDelete = true;
                        file.DeleteBy = AttachmentInfo.CreatedBy;
                        file.DeleteTime = DateTime.Now;
                        await _attachmentService.DeleteAttachment(file);
                    }
                }
                if (AttachmentInfo.Id <= 0)
                {
                    var result = await _attachmentService.CreateAttachment(att);
                    if (result.Id < 0)
                    {
                        this.ObjectResultModule.StatusCode = 204;
                        this.ObjectResultModule.Message = "NoContent";
                        this.ObjectResultModule.Object = "";
                    }
                    else
                    {
                        #region  图片压缩工作
                        //            if (att.FileType == "image")
                        //            {
                        //                try
                        //{
                        //    var result = await _attachmentService.AttachmentServiceInfoByID(4);
                        //    string key = @"https://yaeher-1257714652.cos.ap-guangzhou.myqcloud.com/consultation/image/wx-20181022-1540172749000.jpg";
                        //    var fullfiletype = key.Substring(0, key.LastIndexOf(".", StringComparison.Ordinal));
                        //    var filetype = key.Substring(key.LastIndexOf(".", StringComparison.Ordinal));
                        //    string contentkey = $"{fullfiletype}-00001" + filetype;
                        //    var downloaded = await _cosHandler.GetObjectAsync(key.ToString(), stream =>
                        //    {
                        //    });
                        //    Image originalImagePath = Image.FromStream(downloaded);
                        //    MemoryStream thumNailPath = new MemoryStream();
                        //    var thum = new ThumNail();
                        //    var imagestream = thum.MakeThumNail(originalImagePath, thumNailPath, 16, 16, "HW");
                        //    imagestream.Position = 0;
                        //    var res = await _cosHandler.PutObjectAsync(contentkey, imagestream);

                        //    result.TypeDetail = contentkey;
                        //    await _attachmentService.UpdateAttachmentService(result);

                        //}
                        //catch (Exception ex)
                        //{
                        //    Logger.Info("图片缩略图失败，" + ex.Message.ToString() + ex.StackTrace.ToString());
                        //}
                        //            }
                        #endregion

                        this.ObjectResultModule.Object = result;
                        this.ObjectResultModule.StatusCode = 200;
                        this.ObjectResultModule.Message = "success";
                    }
                }
                unitOfWork.Complete();
            }
            return ObjectResultModule;
        }
        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="AttachmentListInfo"></param>
        /// <returns></returns>
        [Route("api/CreateMobileAttachmentList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateMobileAttachmentList([FromBody]AttachmentListIn AttachmentListInfo)
        {
            if (!Commons.CheckSecret(AttachmentListInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var param = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
            var paramlist = await _systemParameterService.ParameterList(param);
            for (var i = 0; i < AttachmentListInfo.Attach.Count(); i++)
            {
                var AttachmentInfo = AttachmentListInfo.Attach[i];
                var att = new AttachmentService();
                if (!string.IsNullOrEmpty(AttachmentInfo.Filename))
                {
                    att.Filename = AttachmentInfo.Filename;
                    att.FileAddress = paramlist[0].ItemValue + "/" + AttachmentInfo.ServiceType + "/" + AttachmentInfo.MediaType + "/" + AttachmentInfo.Filename;
                }
                if (!string.IsNullOrEmpty(AttachmentInfo.TypeDetail))
                {
                    att.TypeDetail = AttachmentInfo.TypeDetail;
                    att.FileAddress = paramlist[0].ItemValue + "/" + AttachmentInfo.ServiceType + "/" + AttachmentInfo.MediaType + "/" + AttachmentInfo.TypeDetail + "/" + AttachmentInfo.Filename;
                }

                if (AttachmentInfo.FileSize > 0) { att.FileSize = AttachmentInfo.FileSize; }
                if (AttachmentInfo.ConsultID > 0) { att.ConsultID = AttachmentInfo.ConsultID; }

                if (AttachmentInfo.ServiceID > 0) { att.ServiceID = AttachmentInfo.ServiceID; }
                att.FileFrom = AttachmentInfo.ServiceType;
                att.CreatedBy = AttachmentInfo.CreatedBy;
                att.FileType = AttachmentInfo.MediaType;
                att.ConsultNumber = AttachmentInfo.ConsultNumber;
                if (!string.IsNullOrEmpty(AttachmentInfo.ServiceNumber)) { att.ServiceNumber = AttachmentInfo.ServiceNumber; }
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (AttachmentInfo.Id > 0 && AttachmentInfo.IsDelete)
                    {
                        var file = await _attachmentService.AttachmentServiceInfoByID(AttachmentInfo.Id);
                        if (file != null)
                        {
                            file.IsDelete = true;
                            file.DeleteBy = AttachmentInfo.CreatedBy;
                            file.DeleteTime = DateTime.Now;
                            await _attachmentService.DeleteAttachment(file);
                        }
                    }
                    if (AttachmentInfo.Id <= 0)
                    {
                        await _attachmentService.InsertAttachment(att);
                        this.ObjectResultModule.Object = "";
                        this.ObjectResultModule.StatusCode = 200;
                        this.ObjectResultModule.Message = "success";

                    }
                    unitOfWork.Complete();
                }
            }
            return ObjectResultModule;
        }

        /// <summary>
        /// 获取附件 List 
        /// </summary>
        /// <param name="AttachmentInfo"></param>
        /// <returns></returns>
        [Route("api/AttachmentList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> AttachmentList([FromBody]AttachmentIn AttachmentInfo)
        {
            if (!Commons.CheckSecret(AttachmentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            AttachmentInfo.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            if (AttachmentInfo.ServiceID > 0)
            {
                AttachmentInfo.AndAlso(t => t.ServiceID == AttachmentInfo.ServiceID);
            }
            if (AttachmentInfo.ConsultID > 0)
            {
                AttachmentInfo.AndAlso(t => t.ConsultID == AttachmentInfo.ConsultID);
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.TypeDetail))
            {
                AttachmentInfo.AndAlso(t => t.TypeDetail == AttachmentInfo.TypeDetail);
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.FileFrom))
            {
                AttachmentInfo.AndAlso(t => t.FileFrom == AttachmentInfo.FileFrom);
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.FileType))
            {
                AttachmentInfo.AndAlso(t => t.FileType == AttachmentInfo.FileType);
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(AttachmentInfo.StartTime))
            {
                StartTime = DateTime.Parse(AttachmentInfo.StartTime);
                if (string.IsNullOrEmpty(AttachmentInfo.EndTime))
                {
                    AttachmentInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.EndTime))
            {
                EndTime = DateTime.Parse(AttachmentInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.StartTime))
            {
                AttachmentInfo.AndAlso(t => t.CreatedOn >= StartTime && t.CreatedOn < EndTime.AddDays(+1));
            }
            if (!string.IsNullOrEmpty(AttachmentInfo.KeyWord))
            {
                AttachmentInfo.AndAlso(t => t.FileAddress.Contains(AttachmentInfo.KeyWord));
            }
            var values = await _attachmentService.AttachmentList(AttachmentInfo);
            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            return ObjectResultModule;
        }

        /// <summary>
        /// 获取附件 List 
        /// </summary>
        /// <param name="AttachmentInfo"></param>
        /// <returns></returns>
        [Route("api/AttachmentDetailList")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> AttachmentDetailList([FromBody]AttachmentIn AttachmentInfo)
        {
            if (!Commons.CheckSecret(AttachmentInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _attachmentService.ReplyDetailList(AttachmentInfo);
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
    }
}
