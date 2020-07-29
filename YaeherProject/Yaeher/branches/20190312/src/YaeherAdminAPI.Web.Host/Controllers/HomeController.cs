using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using Yaeher.Controllers;
using Abp.Authorization;
using Yaeher;
using System;
using Yaeher.SystemManage;
using Yaeher.Common.Constants;
using System.Collections.Generic;
using Yaeher.SystemManage.Dto;
using Yaeher.Consultation.Dto;
using Abp.Runtime.Session;
using Yaeher.Common.TencentCustom;
using Yaeher.SystemConfig;

namespace YaeherAdminAPI.Web.Host.Controllers
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public class HomeController : YaeherControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IDoctorParaSetService _doctorParaSetService;
        //private readonly IAbpSession _IabpSession;
        /// <summary>
        /// Home 构造函数
        /// </summary>
        /// <param name="notificationPublisher"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="doctorParaSetService"></param>
        public HomeController(INotificationPublisher notificationPublisher, ISystemParameterService systemParameterService
            , IDoctorParaSetService doctorParaSetService
            //, IAbpSession abpSession
            )
        {
            _notificationPublisher = notificationPublisher;
            _systemParameterService = systemParameterService;
            _doctorParaSetService= doctorParaSetService;
            //_IabpSession = abpSession;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Redirect("/swagger/index.html");
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );
            return Content("Sent notification: " + message);
        }
        
        /// <summary>
        /// 获取评价理由
        /// </summary>
        /// <param name="ConsultationReplyParameter"></param>
        /// <returns></returns>
        [Route("api/ConsultationReplyParameter")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationReplyParameter([FromBody] ConsultationReplyAdd ConsultationReplyParameter)
        {
            if (!Commons.CheckSecret(ConsultationReplyParameter.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }


            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultationReplyType");
            var typelist = await _systemParameterService.ParameterList(param);

            var ReplyType = new List<CodeList>();
            foreach (var item in typelist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                ReplyType.Add(newcode);
            }

            var param1 = new SystemParameterIn() { Type = "ConfigPar" };
            param1.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultationReplyState");
            var typelist1 = await _systemParameterService.ParameterList(param1);

            var ReplyState = new List<CodeList>();
            foreach (var item in typelist1)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                ReplyState.Add(newcode);
            }
            var param2 = new SystemParameterIn() { Type = "ConfigPar" };
            param2.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultationReplyState");
            var typelist2 = await _systemParameterService.ParameterList(param2);


            var ConsultState = new List<CodeList>();
            foreach (var item in typelist2)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                ConsultState.Add(newcode);
            }
            var paramlen = new SystemParameterIn() { SystemType = "ConsultationReplyLength" };
            var paramlenlist = await _systemParameterService.ParameterList(paramlen);


            var maxReplyLength = int.Parse(paramlenlist[0].ItemValue);//最大回复数

            var paramlen1 = new SystemParameterIn() { SystemType = "ConsultationContentLength" };
            var paramlenlist1 = await _systemParameterService.ParameterList(paramlen1);


            var maxConsultationLength = int.Parse(paramlenlist1[0].ItemValue);//最大咨询数
            var ConsultationReplyCodeList = new ConsultationReplyCodeList(ReplyState, ReplyType, ConsultState, maxReplyLength, maxConsultationLength);
            this.ObjectResultModule.Object = ConsultationReplyCodeList;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取退单理由
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/RefundManageType")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundManageType([FromBody] RefundManageType RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var param2 = new SystemParameterIn() { Type = "ConfigPar" };
            param2.AndAlso(t => !t.IsDelete && t.SystemCode == "PatientRefundManageType");
            var paramlist = await _systemParameterService.ParameterList(param2);

            var reundType = new List<CodeList>();
            foreach (var item in paramlist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                reundType.Add(newcode);
            }

            this.ObjectResultModule.Object = reundType;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取评价理由
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationReson")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationReson([FromBody] ConsultationEvaluationReason RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var list = new ConsultationEvaluationReasonList();

            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultationEvaluationReason");
            var typelist = await _systemParameterService.ParameterList(param);


            var EvaluationReasonlist = new List<ConsultationEvaluationReason>();
            foreach (var item in typelist)
            {
                var newcode = new ConsultationEvaluationReason() { Level = int.Parse(item.Code.Substring(0, 1)), Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                EvaluationReasonlist.Add(newcode);
            }
            var param1 = new SystemParameterIn() { Type = "ConfigPar" };
            param1.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultationEvaluationDetail");
            var typelist1 = await _systemParameterService.ParameterList(param1);

            var detail = new List<CodeList>();
            foreach (var item in typelist1)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                detail.Add(newcode);
            }
            list.EvaluationDetail = detail;
            list.ConsultationEvaluationReason = EvaluationReasonlist;
            this.ObjectResultModule.Object = list;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取附件类型业务类型
        /// </summary>
        /// <param name="file"> FileAccess 数据</param>
        /// <returns></returns>
        [Route("api/TencentCosAccessTokenType")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> TencentCosAccessTokenType([FromBody]SecretModel file)
        {
            if (!Commons.CheckSecret(file.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete);
            var Totaltypelist = await _systemParameterService.ParameterList(param);

            //var param = new SystemParameterIn() { Type = "ConfigPar" };
         //  param.AndAlso(t => !t.IsDelete && t.SystemCode == "UploadType");
            var typelist = Totaltypelist.FindAll(t=>t.SystemCode== "UploadType");

            //var mediaparam = new SystemParameterIn() { Type = "ConfigPar" };
            //mediaparam.AndAlso(t => !t.IsDelete && t.SystemCode == "MediaType");
            var medialist = Totaltypelist.FindAll(t => t.SystemCode == "MediaType");

            //var detailparam = new SystemParameterIn() { Type = "ConfigPar" };
            //detailparam.AndAlso(t => !t.IsDelete && t.SystemCode == "FileTypeDetail");
            var detaillist = Totaltypelist.FindAll(t => t.SystemCode == "FileTypeDetail");

            //var documentparam = new SystemParameterIn() { Type = "ConfigPar" };
            //documentparam.AndAlso(t => !t.IsDelete && t.SystemCode == "DocumentsUse");
            var documentlist = Totaltypelist.FindAll(t => t.SystemCode == "DocumentsUse");

            var imagethumnaillist=Totaltypelist.FindAll(t => t.SystemCode == "ImageThumNail");

            var coderesult = new List<CodeList>();
            foreach (var item in typelist)
            {
                var newcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                coderesult.Add(newcode);
            }
            var mediatype = new List<CodeList>();
            foreach (var item in medialist)
            {
                var mediacode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                mediatype.Add(mediacode);
            }
            var typedetail = new List<CodeList>();
            foreach (var item in detaillist)
            {
                var detailcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                typedetail.Add(detailcode);
            }
            var documentdetail = new List<CodeList>();
            foreach (var item in documentlist)
            {
                var detailcode = new CodeList() { Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
                documentdetail.Add(detailcode);
            }
            var imagethumnail=new List<CodeList>();
            foreach (var item in imagethumnaillist)
            {
                var detailcode = new CodeList() { Code = item.Code, Value = item.ItemValue, Type = item.SystemType, TypeCode = item.SystemCode };
                imagethumnail.Add(detailcode);
            }

            var param1 = new DoctorParaSetIn();
            param1.AndAlso(t => !t.IsDelete);
            var Totalpara = await _doctorParaSetService.DoctorParaSetList(param1);

            //var paramlen = new SystemParameterIn() { SystemType = "ConsultationReplyLength" };
            //var paramlenlist = await _systemParameterService.ParameterList(paramlen);
            var maxReplyLength = int.Parse(Totalpara.Find(t => t.DoctorParaSetCode == "ConsultationReplyLength").ItemValue);//最大追问数

            //var paramlen1 = new SystemParameterIn() { SystemType = "ConsultationContentLength" };
            //var paramlenlist1 = await _systemParameterService.ParameterList(paramlen1);
            var maxConsultationLength = int.Parse(Totalpara.Find(t => t.DoctorParaSetCode == "ConsultationContentLength").ItemValue);//最大咨询数

            var maxReplyMaxLength = int.Parse(Totalpara.Find(t => t.DoctorParaSetCode == "ReplyMaxLength").ItemValue);//最大回复字数
            //paramlen1 = new SystemParameterIn() { SystemType = "ConsultationImageCount" };
            //paramlenlist1 = await _systemParameterService.ParameterList(paramlen1);
            var ConsultationImageCount = int.Parse(Totalpara.Find(t => t.DoctorParaSetCode == "ConsultationImageCount").ItemValue);//咨询上传图片数

            //paramlen1 = new SystemParameterIn() { SystemType = "ConsultationImagesize" };
            //paramlenlist1 = await _systemParameterService.ParameterList(paramlen1);
            var ConsultationImagesize = double.Parse(Totalpara.Find(t => t.DoctorParaSetCode == "ConsultationImagesize").ItemValue);//图片大小

            //paramlen1 = new SystemParameterIn() { SystemType = "VideoCount" };
            //paramlenlist1 = await _systemParameterService.ParameterList(paramlen1);
            var VideoCount = int.Parse(Totalpara.Find(t => t.DoctorParaSetCode == "VideoCount").ItemValue);//视频数

            //paramlen1 = new SystemParameterIn() { SystemType = "VideoSize" };
            //paramlenlist1 = await _systemParameterService.ParameterList(paramlen1);
            var VideoSize = double.Parse(Totalpara.Find(t => t.DoctorParaSetCode == "VideoSize").ItemValue);//视频大小


            this.ObjectResultModule.Object = new FileAllType(coderesult, mediatype, typedetail, documentdetail,imagethumnail, maxReplyLength,  maxConsultationLength,maxReplyMaxLength, ConsultationImageCount, ConsultationImagesize, VideoCount, VideoSize);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "";
            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取注册用户信息 List 
        /// </summary>
        /// <param name="file"> YaeherUserIn 数据</param>
        /// <returns></returns>
        [Route("api/TencentCosAccessToken")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> TencentCosAccessToken([FromBody]FileAccess file)
        {
            if (!Commons.CheckSecret(file.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var param = new SystemParameterIn() { Type = "ConfigPar" };
            param.AndAlso(t => !t.IsDelete && t.SystemCode == "UploadType");
            var typelist = await _systemParameterService.ParameterList(param);
            var mediaparam = new SystemParameterIn() { Type = "ConfigPar" };
            mediaparam.AndAlso(t => !t.IsDelete && t.SystemCode == "MediaType");
            var medialist = await _systemParameterService.ParameterList(mediaparam);
            var paramurl = new SystemParameterIn() { SystemType = "TencentCosBaseUrl" };
            var paramlisturl = await _systemParameterService.ParameterList(paramurl);
            var urlbase = paramlisturl[0];
            var result = new TencentCosAccess();
            var type = typelist.Find(t => t.Code == file.ServiceType);
            result.FileFolder = type.ItemValue;
            //switch (file.ServiceType)
            //{
            //    case "consultation":
            //        result.FileFolder = "consultation";
            //        break;
            //    case "inquiries":
            //        result.FileFolder = "inquiries";
            //        break;
            //    case "answer":
            //        result.FileFolder = "answer";
            //        break;
            //    case "avatar":
            //        result.FileFolder = "avatar";
            //        break;
            //    case "backgroundimage":
            //        result.FileFolder = "backgroundimage";
            //        break;
            //    case "doctorpaper":
            //        result.FileFolder = "doctorpaper";
            //        break;
            //    case "releasedoctorpaper":
            //        result.FileFolder = "releasedoctorpaper";
            //        break;
            //    case "idcard":
            //        result.FileFolder = "idcard";
            //        break;
            //    case "certificate":
            //        result.FileFolder = "certificate";
            //        break;
            //}
            //switch (file.MediaType)
            //{
            //    case "image":
            //        result.FileFolder += "/image";
            //        break;
            //    case "video":
            //        result.FileFolder += "/video";
            //        break;
            //    case "voice":
            //        result.FileFolder += "/voice";
            //        break;
            //}
            var media = medialist.Find(t => t.Code == file.MediaType);
            result.FileFolder += "/" + media.ItemValue;
            result.FileHeadName=urlbase.ItemValue+"/"+file.ServiceType+"/"+file.MediaType+"/";

            var cosparam = new SystemParameterIn() { Type = "ConfigPar" };
            cosparam.AndAlso(t => !t.IsDelete && t.SystemCode == "CosType");
            var coslist = await _systemParameterService.ParameterList(cosparam);

            result.Bucket = coslist.Find(t => t.Code == "CosBucket").ItemValue;
            result.Region = coslist.Find(t => t.Code == "CosRegion").ItemValue;
            result.SecretId = coslist.Find(t => t.Code == "CosSecretId").ItemValue;
            result.SecretKey = coslist.Find(t => t.Code == "CosSecretKey").ItemValue;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "";
            this.ObjectResultModule.Object = result;
            return this.ObjectResultModule;
        }
        
    }
}
