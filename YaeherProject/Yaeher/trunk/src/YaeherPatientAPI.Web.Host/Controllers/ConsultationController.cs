using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Yaeher;
using Yaeher.ClinicManage;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.HangfireJob;
using Yaeher.Common.TencentCustom;
using Yaeher.Consultation;
using Yaeher.Consultation.Dto;
using Yaeher.EventBus;
using Yaeher.EventBus.Dto;
using Yaeher.EventEntitys;
using Yaeher.SystemConfig;
using Yaeher.SystemManage;
using Yaeher.SystemManage.Dto;
using Yaeher.YaeherDoctors;
using Yaeher.YaeherDoctors.Dto;

namespace YaeherPatientAPI.Web.Host.Controllers
{
    /// <summary>
    /// 咨询 以及订单管理
    /// </summary>
    public class ConsultationController : YaeherAppServiceBase
    {
        #region
        private readonly IConsultationEvaluationService _consultationEvaluationService;
        private readonly IConsultationReplyService _consultationReplyService;
        private readonly IConsultationService _consultationService;
        private readonly IPhoneReplyRecordService _phoneReplyRecordService;
        private readonly IUserManagerService _userManagerService;
        private readonly IYaeherOperListService _yaeherOperListService;
        private readonly IOrderManageService _orderManageService;
        private readonly IOrderTradeRecordService _orderTradeRecordService;
        private readonly IRefundManageService _refundManageService;
        private readonly IIncomeDevideService _incomeDevideService;
        private readonly ILeaguerInfoService _leaguerservice;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _IabpSession;
        private readonly ISystemParameterService _systemParameterService;
        private readonly IAttachmentServices _attachmentService;
        private readonly IHangFireJobService _hangFireJobService;
        private IPublishsService _publishsService;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultationEvaluationService"></param>
        /// <param name="consultationReplyService"></param>
        /// <param name="consultationService"></param>
        /// <param name="phoneReplyRecordService"></param>
        /// <param name="orderManageService"></param>
        /// <param name="orderTradeRecordService"></param>
        /// <param name="refundManageService"></param>
        /// <param name="incomeDevideService"></param>
        /// <param name="leaguerservice"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="systemParameterService"></param>
        /// <param name="attachmentServices"></param>
        /// <param name="userManagerService"></param>
        /// <param name="publishsService"></param>
        /// <param name="hangFireJobService"></param>
        /// <param name="session"></param>
        /// <param name="yaeherOperListService"></param>
        public ConsultationController(IConsultationEvaluationService consultationEvaluationService,
                                      IConsultationReplyService consultationReplyService,
                                      IConsultationService consultationService,
                                      IPhoneReplyRecordService phoneReplyRecordService,
                                      IOrderManageService orderManageService,
                                      IOrderTradeRecordService orderTradeRecordService,
                                      IRefundManageService refundManageService,
                                      IIncomeDevideService incomeDevideService,
                                      ILeaguerInfoService leaguerservice,
                                      IUnitOfWorkManager unitOfWorkManager,
                                      ISystemParameterService systemParameterService,
                                      IAttachmentServices attachmentServices,
                                      IUserManagerService userManagerService,
                                      IPublishsService publishsService,
                                      IHangFireJobService hangFireJobService,
                                      IAbpSession session,
                                      IYaeherOperListService yaeherOperListService)
        {
            _consultationEvaluationService = consultationEvaluationService;
            _consultationReplyService = consultationReplyService;
            _consultationService = consultationService;
            _phoneReplyRecordService = phoneReplyRecordService;

            _orderManageService = orderManageService;
            _orderTradeRecordService = orderTradeRecordService;
            _refundManageService = refundManageService;
            _incomeDevideService = incomeDevideService;
            _leaguerservice = leaguerservice;
            _unitOfWorkManager = unitOfWorkManager;
            _systemParameterService = systemParameterService;
            _IabpSession = session;
            _attachmentService = attachmentServices;
            _userManagerService = userManagerService;
            _publishsService = publishsService;
            _hangFireJobService = hangFireJobService;
            _yaeherOperListService = yaeherOperListService;
        }
        #region 咨询管理
        /// <summary>
        /// 咨询回答 Byid
        /// </summary>
        /// <param name="ConsultationAdd"></param>
        /// <returns></returns>
        [Route("api/ConsultationById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationById([FromBody]YaeherConsultationAdd ConsultationAdd)
        {
            if (!Commons.CheckSecret(ConsultationAdd.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherConsultation values = null;
            if (ConsultationAdd.Id > 0)
            {
                values = await _consultationService.YaeherConsultationByID(ConsultationAdd.Id);
            }
            else
            {
                values = await _consultationService.YaeherConsultationByNumber(ConsultationAdd.ConsultNumber);
            }
            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationById",
                OperContent = JsonHelper.ToJson(ConsultationAdd),
                OperType = "ConsultationById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }

        /// <summary>
        /// 咨询管理 新增
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/CreateConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateConsultation([FromBody] YaeherConsultationAdd ConsultationInfo)
        {
            //Logger.Info("CreateConsultation:" + JsonHelper.ToJson(ConsultationInfo));
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            //医生信息
            var Content = "{\"Id\":" + ConsultationInfo.DoctorID + ",\"secret\":\"" + secret + "\"}";
            var doctoruser = await this.PostResponseAsync(Commons.AdminIp + "api/DoctorInformation", Content);
            var doctorUserResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherDoctorInfo>>>(doctoruser);
            if (doctorUserResult == null || doctorUserResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (doctorUserResult.result.item.DoctorOnlineRecord.OnlineState != "online")
            { return new ObjectResultModule("", 400, "医生已下线不允许提交咨询！"); }
            //医生提供服务信息
            var ServiceResult = doctorUserResult.result.item.ServiceMoneyLists.Find(t => t.Id == ConsultationInfo.ServiceMoneyListId);
            if (ServiceResult == null || ServiceResult.Id < 1 || ServiceResult.ServiceState == false) { return new ObjectResultModule("", 400, "医生该服务已下线请更换后重新提交！"); }
            DateTime startdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime enddate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            //检查当天是否提交过该商品未支付订单
            ConsultationIn se = new ConsultationIn(); se.AndAlso(t => t.IsDelete == false && t.ConsultState == "unpaid" && t.ServiceMoneyListId == ConsultationInfo.ServiceMoneyListId && t.CreatedBy == userid && t.CreatedOn >= startdate && t.CreatedOn < enddate);
            var patientconsultation = await _consultationService.YaeherConsultationList(se);
            if (patientconsultation.Count > 0) { return new ObjectResultModule("", 400, "您今天已经过提交该医生同类型的服务,请不要重复提交！"); }

            #region 判断当日接单是否超额
            OrderTradeRecordIn refundManageIn = new OrderTradeRecordIn();
            var StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            refundManageIn.DoctorId = ConsultationInfo.DoctorID;
            refundManageIn.ServiceID = ConsultationInfo.ServiceMoneyListId;
            refundManageIn.AndAlso(t => t.IsDelete == false);
            refundManageIn.AndAlso(t => t.PayMoney > 0);
            refundManageIn.AndAlso(t => t.CreatedOn >= StartTime);
            refundManageIn.AndAlso(t => t.CreatedOn < StartTime.AddDays(1));
            var ordertradelist = await _orderTradeRecordService.PatientOrderTradeRecordList(refundManageIn);
           
            //var returntrade = ordertradelist.Where(t => t.PayMoney < 0);
            //RefundManageIn refundManageIn1 = new RefundManageIn();
            //refundManageIn1.AndAlso(t => t.IsDelete == false);
            //refundManageIn1.AndAlso(t => t.CreatedOn >= StartTime);
            //refundManageIn1.AndAlso(t => t.CreatedOn < StartTime.AddDays(1));
            //refundManageIn1.AndAlso(t => t.DoctorID == ConsultationInfo.DoctorID);
            //refundManageIn1.AndAlso(t => t.ServiceID == ConsultationInfo.ServiceMoneyListId);
            //refundManageIn1.AndAlso(t => t.CheckState == "success");
            //var refundmanage = await _refundManageService.PayCheckRefundManageList(refundManageIn1);
            var allconsultation = ordertradelist.Count();
            if (ServiceResult.ServiceFrequency <= allconsultation)
            {
                return new ObjectResultModule("", 400, "医生该服务已满单,请明天再来！");

            }
            #endregion
            //获取用户信息
            Content = "{\"Id\":" + userid.ToString() + ",\"secret\":\"" + secret + "\"}";
            var user = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherUserById/", Content);
            var UserResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherUser>>>(user);
            if (UserResult == null || UserResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            // 电话号码判断
            if (ConsultationInfo.PhoneNumber != "" || ConsultationInfo.PhoneNumber != null)
            {
                ConsultationInfo.PhoneNumber = ConsultationInfo.PhoneNumber.Replace(" ", "");
                bool IsPhone = Regex.IsMatch(ConsultationInfo.PhoneNumber, "^(0\\d{2,3}-?\\d{7,8}(-\\d{3,5}){0,1})|((1)\\d{10})$");
                if (!IsPhone || (ConsultationInfo.PhoneNumber.Length > 0 && ConsultationInfo.PhoneNumber.Length != 11))
                {
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "请输入11位手机号码。";
                    this.ObjectResultModule.Object = "";
                    return this.ObjectResultModule;
                }
            }
            //患者信息
            var Patient = await _leaguerservice.LeaguerInfoById(ConsultationInfo.PatientID);
            var Age = Patient.Birthday;
            var labelname = "";
            var labeljson = "";
            var labelId = 0;
            //标签信息
            var labelResult = doctorUserResult.result.item.LableManageList.Find(t => t.Id == ConsultationInfo.IIInessId);
            labelname = labelResult.LableName;
            labelId = labelResult.Id;
            labeljson = JsonHelper.ToJson(labelResult);
            //系统自动退款时间
            Content = "{\"SystemCode\":\"SystemOverTime\",\"secret\":\"" + secret + "\"}";
            var replymparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", Content);
            var replylistparam = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(replymparam);
            if (replylistparam == null || replylistparam.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var CreateConsultation = new YaeherConsultation()
                {
                    ConsultNumber = "CN-" + DateTime.Now.ToString("yyyyMMddhhmm") + new RandomCode().RamdomRecode(4),
                    ConsultantID = userid,
                    ConsultantName = UserResult.result.item.FullName,
                    ConsultantJSON = JsonHelper.ToJson(UserResult.result.item),
                    DoctorName = doctorUserResult.result.item.DoctorInfo.DoctorName,
                    DoctorID = ConsultationInfo.DoctorID,
                    DoctorJSON = JsonHelper.ToJson(doctorUserResult.result.item.DoctorInfo),
                    PatientID = ConsultationInfo.PatientID,
                    PatientName = Patient.LeaguerName,
                    PatientJSON = JsonHelper.ToJson(Patient),
                    ConsultType = ServiceResult.ServiceType,
                    ServiceMoneyListId = ServiceResult.Id,
                    IIInessType = labelname,
                    IIInessJSON = labeljson,
                    PhoneNumber = ConsultationInfo.PhoneNumber,
                    PatientCity = ConsultationInfo.PatientCity,
                    IIInessDescription = ConsultationInfo.IIInessDescription,
                    //  Overtime = DateTime.Now.AddHours(double.Parse(replylistparam.result.item[0].ItemValue)),//咨询退单超时时间
                    Overtime = DateTime.Now.AddDays(double.Parse(replylistparam.result.item[0].ItemValue)),
                    InquiryTimes = int.Parse(doctorUserResult.result.item.InquiryMaxCount.ItemValue),//最大追问次数4
                    HasInquiryTimes = int.Parse(doctorUserResult.result.item.InquiryMaxCount.ItemValue),//剩余追问次数3
                    Completetime = DateTime.Now.AddDays(double.Parse(doctorUserResult.result.item.ConsultationSucessTime.ItemValue)),
                    HasReply = false,//默认未回复
                    Age = CalculateAgeString(Patient.Birthday, DateTime.Now),
                    ConsultState = "unpaid", //咨询问诊状态 未支付
                    OvertimeRemindTimes = 0,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    RefundNumber = ""
                };
                var result = await _consultationService.CreateYaeherConsultation(CreateConsultation);
                if (ConsultationInfo.Attach.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var AttachmentInfo in ConsultationInfo.Attach)
                    {
                        if (AttachmentInfo.Id < 1 || AttachmentInfo.IsDelete == true)
                        {
                            var content = "{\"Id\":\"" + AttachmentInfo.Id + "\",\"Filename\":\"" + AttachmentInfo.Filename + "\",\"IsDelete\":\"" + AttachmentInfo.IsDelete + "\",\"ConsultID\":\"" + result.Id + "\",\"ConsultNumber\":\"" + result.ConsultNumber + "\",\"ServiceType\":\"consultation\",\"CreatedBy\":\"" + userid + "\",\"MediaType\":\"" + AttachmentInfo.MediaType + "\"},";
                            sb.Append(content);
                            //var param2 = await this.PostResponseAsync(Commons.AdminIp + "api/CreateMobileAttachment/", content);
                        }
                    }
                    var sbstr = sb.ToString().TrimEnd(',');
                    var filelist = "{\"secret\":\"" + secret + "\",\"attach\":[" + sbstr + "]}";
                    var param2 = await this.PostResponseAsync(Commons.AdminIp + "api/CreateMobileAttachmentList", filelist);

                }
                this.ObjectResultModule.Object = result;
                unitOfWork.Complete();
            }

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateConsultation",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "CreateConsultation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion


            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            return ObjectResultModule;
        }


        /// <summary>
        /// 计算年龄字符串
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">第1个日期参数</param>
        /// <param name="p_SecondDateTime">第2个日期参数</param>
        private static string CalculateAgeString(System.DateTime p_FirstDateTime, System.DateTime p_SecondDateTime)
        {
            //判断时间段是否为正。若为负，调换两个时间点的位置。
            if (System.DateTime.Compare(p_FirstDateTime, p_SecondDateTime) > 0)
            {
                System.DateTime stmpDateTime = p_FirstDateTime;
                p_FirstDateTime = p_SecondDateTime;
                p_SecondDateTime = stmpDateTime;
            }

            //判断返回字符串的格式。若为空，则给默认值：{0}岁{1}月{2}天
            //  if (string.IsNullOrEmpty(p_ReturnFormat)) p_ReturnFormat = "{0}岁{1}月{2}天";

            //定义：年、月、日
            int year, month, day;

            //计算：天
            day = p_SecondDateTime.Day - p_FirstDateTime.Day;
            if (day < 0)
            {
                day += System.DateTime.DaysInMonth(p_FirstDateTime.Year, p_FirstDateTime.Month);
                p_FirstDateTime = p_FirstDateTime.AddMonths(1);
            }
            //计算：月
            month = p_SecondDateTime.Month - p_FirstDateTime.Month;
            if (month < 0)
            {
                month += 12;
                p_FirstDateTime = p_FirstDateTime.AddYears(1);
            }
            //计算：年
            year = p_SecondDateTime.Year - p_FirstDateTime.Year;

            //返回格式化后的结果
            var nl = "";
            if (year > 0)
            {
                nl += year + "岁";
            }
            if (month > 0)
            {
                nl += month + "月";
            }
            if (day > 0)
            {
                nl += day + "天";
            }
            return nl;
        }

        /// <summary>
        /// 咨询管理 修改
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateConsultation([FromBody] YaeherConsultationAdd ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateConsultation = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
            var leaguer = await _leaguerservice.LeaguerInfoById(ConsultationInfo.PatientID);
            if (leaguer == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (UpdateConsultation.ConsultState != "created" && UpdateConsultation.ConsultState != "unpaid") { return new ObjectResultModule("", 400, "医生未回复前咨询信息才能进行修改！"); }

            //var Content = "{\"SystemCode\":\"ConsultationCanEditTime\"}";
            //var wxparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", Content);
            //var wxparamlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(wxparam);
            //if (wxparamlist == null || wxparamlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            // 电话号码判断
            if (ConsultationInfo.PhoneNumber != "" || ConsultationInfo.PhoneNumber != null)
            {
                ConsultationInfo.PhoneNumber = ConsultationInfo.PhoneNumber.Replace(" ", "");
                bool IsPhone = Regex.IsMatch(ConsultationInfo.PhoneNumber, "^(0\\d{2,3}-?\\d{7,8}(-\\d{3,5}){0,1})|((1)\\d{10})$");
                if (!IsPhone || (ConsultationInfo.PhoneNumber.Length > 0 && ConsultationInfo.PhoneNumber.Length != 11))
                {
                    this.ObjectResultModule.StatusCode = 400;
                    this.ObjectResultModule.Message = "请输入11位手机号码。";
                    this.ObjectResultModule.Object = "";
                    return this.ObjectResultModule;
                }
            }

            var secret = await CreateSecret();
            if (UpdateConsultation != null)
            {
                var order = await _orderManageService.OrderManageByconsultNumber(UpdateConsultation.ConsultNumber);
                OrderTradeRecord ordertrade = null;
                if (order != null)
                {
                    ordertrade = await _orderTradeRecordService.OrderTradeRecordExpress(t => t.IsDelete == false && t.PayMoney > 0 && t.OrderNumber == order.OrderNumber);
                }
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (leaguer.Id != ConsultationInfo.PatientID && order != null)
                    {
                        order.PatientID = leaguer.Id;
                        order.PatientName = leaguer.LeaguerName;
                        await _orderManageService.UpdateOrderManage(order);
                    }
                    UpdateConsultation.PatientID = leaguer.Id;
                    UpdateConsultation.PatientName = leaguer.LeaguerName;
                    UpdateConsultation.PatientJSON = JsonHelper.ToJson(leaguer);

                    var labelname = "";
                    var labeljson = "";
                    var labelId = 0;

                    var LabelContent = "{\"Id\":" + ConsultationInfo.IIInessId.ToString() + ",\"secret\":\"" + secret + "\"}";
                    var label = await this.PostResponseAsync(Commons.AdminIp + "api/LableById/", LabelContent);
                    var labelResult = JsonHelper.FromJson<APIResult<ResultModule<LableManage>>>(label);
                    if (labelResult == null || labelResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
                    labelname = labelResult.result.item.LableName;
                    labelId = labelResult.result.item.Id;
                    labeljson = JsonHelper.ToJson(labelResult.result.item);

                    UpdateConsultation.IIInessType = labelname;
                    UpdateConsultation.IIInessJSON = labeljson;

                    UpdateConsultation.PhoneNumber = ConsultationInfo.PhoneNumber;
                    UpdateConsultation.PatientCity = ConsultationInfo.PatientCity;
                    UpdateConsultation.IIInessDescription = ConsultationInfo.IIInessDescription;

                    UpdateConsultation.ModifyOn = DateTime.Now;
                    UpdateConsultation.ModifyBy = userid;

                    var result = await _consultationService.UpdateYaeherConsultation(UpdateConsultation);

                    if (ConsultationInfo.Attach.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var AttachmentInfo in ConsultationInfo.Attach)
                        {
                            if(AttachmentInfo.Id < 1 && AttachmentInfo.IsDelete == true)
                            {

                            }
                            else if(AttachmentInfo.Id < 1 || AttachmentInfo.IsDelete == true)
                            {
                                var content = "{\"Id\":\"" + AttachmentInfo.Id + "\",\"Filename\":\"" + AttachmentInfo.Filename + "\",\"IsDelete\":\"" + AttachmentInfo.IsDelete + "\",\"ConsultID\":\"" + result.Id + "\",\"ConsultNumber\":\"" + result.ConsultNumber + "\",\"ServiceType\":\"consultation\",\"CreatedBy\":\"" + userid + "\",\"MediaType\":\"" + AttachmentInfo.MediaType + "\"},";
                                sb.Append(content);
                                //var param2 = await this.PostResponseAsync(Commons.AdminIp + "api/CreateMobileAttachment/", content);
                            }
                        }
                        var sbstr = sb.ToString().TrimEnd(',');
                        var filelist = "{\"secret\":\"" + secret + "\",\"attach\":[" + sbstr + "]}";
                        var param2 = await this.PostResponseAsync(Commons.AdminIp + "api/CreateMobileAttachmentList", filelist);
                    }

                    #region  发布修改咨询  暂时不增加接受消息人不做处理
                    if (UpdateConsultation.ConsultState == "created")
                    {
                        Consultation consultation = new Consultation();
                        consultation.yaeherConsultation = result;       // 咨询主表
                        consultation.orderManage = order;             // 订单管理表    
                        consultation.orderTradeRecords = ordertrade;       // 交易记录表
                        Publishs Consultationpublishs = new Publishs()
                        {
                            TemplateCode = "DoctorNotice",
                            OperationType = "EditConsultant",  // 修改咨询
                            MessageRemark = UpdateConsultation.IIInessDescription,  // 修改咨询
                            Publisher = "Patient",
                            PublishUrl = "Patient",
                            EventName = "发布 修改咨询",
                            EventCode = "Consultation",
                            BusinessID = result.Id.ToString(),
                            BusinessCode = result.ConsultNumber,
                            BusinessJSON = JsonHelper.ToJson(consultation),
                            PublishedTime = result.CreatedOn,
                            PublishStatus = true,
                            CreatedBy = userid,
                            CreatedOn = DateTime.Now,
                            Secret = secret,
                        };
                        var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                        var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                        if (ConsultationJson != null)
                        {
                            Consultationpublishs.PublishStatus = true;
                        }
                        else
                        {
                            Consultationpublishs.PublishStatus = false;
                        }
                        Consultationpublishs.ServerClient = "Client";
                        var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                    }
                    #endregion

                    unitOfWork.Complete();
                    this.ObjectResultModule.Object = result;
                    this.ObjectResultModule.StatusCode = 200;
                    this.ObjectResultModule.Message = "success";
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
                OperExplain = "UpdateConsultation",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "UpdateConsultation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }

        /// <summary>
        /// 咨询单状态 修改
        /// </summary>
        /// <param name="consultationInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateConsultationStatus")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateConsultationStatus([FromBody]YaeherConsultation consultationInfo)
        {
            if (!Commons.CheckSecret(consultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            await _consultationService.UpdateYaeherConsultation(consultationInfo);
            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询管理 查询
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/Consultation")]
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<ObjectResultModule> Consultation([FromBody] YaeherConsultation ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            if (ConsultationInfo.Id > 0)
            {
                var Consultation = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
                ObjectResultModule.Object = Consultation;
            }
            else if (!string.IsNullOrEmpty(ConsultationInfo.ConsultNumber))
            {
                YaeherConsultation Consultation = new YaeherConsultation();
                if (ConsultationInfo.RefundType == "return")
                {
                    Consultation = await _consultationService.ConsultationByNumber(ConsultationInfo.ConsultNumber);
                }
                else
                {
                    Consultation = await _consultationService.YaeherConsultationByNumber(ConsultationInfo.ConsultNumber);
                }
                ObjectResultModule.Object = Consultation;
            }
            ObjectResultModule.StatusCode = 200;
            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询回访
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/CreateReturnVisit")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateReturnVisit([FromBody] YaeherConsultationAdd ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var UpdateConsultation = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
            if (UpdateConsultation != null)
            {
                UpdateConsultation.IsReturnVisit = true;
                UpdateConsultation.ReturnVisitTime = DateTime.Now;
                UpdateConsultation.ReturnVisit = ConsultationInfo.ReturnVisit;
                var result = await _consultationService.UpdateYaeherConsultation(UpdateConsultation);

                #region  回访咨询 暂时不做发送消息处理
                Consultation consultation = new Consultation();
                consultation.yaeherConsultation = result;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "DoctorNotice",
                    OperationType = "ReturnVisit",  //  咨询回访
                    MessageRemark = ConsultationInfo.ReturnVisit,  // 咨询回访
                    Publisher = "Patient",
                    PublishUrl = "Patient",
                    EventName = "发布 咨询回访",
                    EventCode = "Consultation",
                    BusinessID = result.Id.ToString(),
                    BusinessCode = result.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(consultation),
                    PublishedTime = result.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion

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
                OperExplain = "CreateReturnVisit",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "CreateReturnVisit",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询管理 删除
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteConsultation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteConsultation([FromBody] YaeherConsultation ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _consultationService.DeleteYaeherConsultation(query);
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
                OperExplain = "DeleteConsultation",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "DeleteConsultation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询管理 Page
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationPage([FromBody] ConsultationIn ConsultationInfo)
        {

            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            ConsultationInfo.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            var values = await _consultationService.YaeherConsultationPage(ConsultationInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var docarray = "";
                for (var i = 0; i < values.Items.Count; i++)
                {
                    docarray += "" + values.Items[i].DoctorID + ",";
                }
                docarray = docarray.TrimEnd(',');
                var Content = "{\"IDArray\":\"" + docarray.ToString() + "\",\"secret\":\"" + secret + "\"}";
                var user = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherUserByIDArray/", Content);
                if (user == null) { return new ObjectResultModule("", 204, "NoContent"); }
                var UserResult = JsonHelper.FromJson<APIResult<List<YaeherUserDocMsg>>>(user);
                if (UserResult == null || UserResult.result.Count < 1) { return new ObjectResultModule("", 204, "NoContent"); }
                var dict = new Dictionary<int, Tuple<string>>();
                foreach (var item in values.Items)
                {
                    var doc = UserResult.result.FirstOrDefault(t => t.YaeherDoctorId == item.DoctorID);
                    dict.Add(item.Id, Tuple.Create(doc == null ? "" : doc.UserImage));
                }
                var content = "{\"Type\":\"ConfigPar\",\"SystemCode\":\"ConsultState\",\"secret\":\"" + secret + "\"}";
                var param = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList", content);
                var paramlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(param);
                if (paramlist == null || paramlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

                //var param = new SystemParameterIn() { Type = "ConfigPar" };
                //param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultState");
                //var paramlist = await _systemParameterService.ParameterList(param);

                this.ObjectResultModule.Object = new ConsultationOut(values, ConsultationInfo, dict, paramlist.result.item);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationPage",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "ConsultationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询管理 List 
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationList([FromBody]ConsultationIn ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationInfo.AndAlso(t => !t.IsDelete);

            if (ConsultationInfo.DoctorID > 0)//支付参数接口检查患者端该医生已经产生的订单数量
            {
                ConsultationInfo.AndAlso(t => t.DoctorID == ConsultationInfo.DoctorID);
                ConsultationInfo.AndAlso(t => t.ConsultState != "return");
                DateTime StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime EndTime = StartTime.AddDays(+1);
                ConsultationInfo.AndAlso(t => (t.CreatedOn >= StartTime));
                ConsultationInfo.AndAlso(t => (t.CreatedOn < EndTime));
                ConsultationInfo.AndAlso(t => (t.ServiceMoneyListId == ConsultationInfo.ServiceMoneyListId));

            }
            else
            {
                ConsultationInfo.AndAlso(t => t.CreatedBy == userid);
            }
            var values = await _consultationService.YaeherConsultationList(ConsultationInfo);
            if (values.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = values;
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
                OperExplain = "ConsultationList",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "ConsultationList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }

        /// <summary>
        /// 咨询详情
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationDetail")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationDetail([FromBody]ConsultationIn ConsultationInfo)
        {
            //Logger.Info("开始：" + DateTime.Now + "：" + JsonHelper.ToJson(ConsultationInfo.ConsultNumber));
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            YaeherConsultation values = new YaeherConsultation();
            var secret = await CreateSecret();
            // 当传入ID时 已ID查询  当已咨询单号查询时查询
            if (ConsultationInfo.Id > 0)
            {
                values = await _consultationService.YaeherConsultationByID(ConsultationInfo.Id);
            }
            else
            {
                values = await _consultationService.YaeherConsultationByNumber(ConsultationInfo.ConsultNumber);
            }
            if (values == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var Order = await _orderManageService.OrderManageByconsultNumber(values.ConsultNumber);
            //  if (Order == null) { return new ObjectResultModule("", 204, "NoContent"); }
            OrderTradeRecord orderTradeRecord = null;
            if (Order != null)
            {
                orderTradeRecord = await _orderTradeRecordService.OrderTradeRecordExpress(t => !t.IsDelete && t.OrderID == Order.Id && t.PaymentSourceCode == "order" && t.PayMoney > 0);

            }//  if (orderTradeRecord == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var Content = "{\"Id\":" + userid + ",\"secret\":\"" + secret + "\"}";
            var user = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherUserById/", Content);
            var PatientUser = JsonHelper.FromJson<APIResult<ResultModule<YaeherUser>>>(user);
            if (PatientUser == null || PatientUser.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (PatientUser.result.item.RoleName == "patient" && values.CreatedBy != userid)
            {
                return new ObjectResultModule("", 400, "不允许查看其他患者的订单！");
            }
            //var canevalu = false;
            var EvaluationIn = new ConsultationEvaluationIn();
            EvaluationIn.AndAlso(t => !t.IsDelete && t.ConsultNumber == values.ConsultNumber);
            var eva = await _consultationEvaluationService.ConsultationEvaluationList(EvaluationIn);
            //if (eva.Count > 0) { canevalu = true; }

            ConsultationReplyIn consultationReplyIn = new ConsultationReplyIn();
            consultationReplyIn.ConsultNumber = values.ConsultNumber;
            var replys = await _consultationReplyService.ReplyDetailList(consultationReplyIn);

            PhoneReplyRecordIn phoneReplyRecordIn = new PhoneReplyRecordIn();
            phoneReplyRecordIn.ConsultNumber = values.ConsultNumber;
            var phonereplys = await _phoneReplyRecordService.ReplyDetailList(phoneReplyRecordIn);

            Content = "{\"ConsultNumber\":\"" + values.ConsultNumber + "\",\"secret\":\"" + secret + "\"}";
            var Attach = await this.PostResponseAsync(Commons.AdminIp + "api/AttachmentDetailList/", Content);
            var Attachmentreply = JsonHelper.FromJson<APIResult<ResultModule<IList<ReplyDetail>>>>(Attach);
            //  if (Attachmentreply == null || Attachmentreply.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var doctor = JsonHelper.FromJson<YaeherDoctor>(values.DoctorJSON);

            // var order = await _orderManageService.OrderManageByconsultNumber(values.ConsultNumber);
            var serverid = values.ServiceMoneyListId;

            Content = "{\"Id\":" + doctor.UserID + ",\"secret\":\"" + secret + "\"}";
            var doctoruser = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherUserById/", Content);

            var UserResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherUser>>>(doctoruser);
            if (UserResult == null || UserResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var questionatt = new List<ReplyDetail>();
            if (Attachmentreply != null && Attachmentreply.result.item != null && Attachmentreply.result.item.Count > 0)
            {
                foreach (var item in replys)
                {
                    var query = from a in Attachmentreply.result.item
                                where a.ReplyNumber == item.ReplyNumber
                                select new ConsultationFile
                                {
                                    FileUrl = a.Message,
                                    FileName = a.FileName,
                                    FileSize = a.FileSize,
                                    MediaType = a.Mediatype,
                                    FileContentUrl = a.FileContentAddress,
                                    FileTotalTime = a.FileTotalTime,
                                    Id = a.Id,
                                };
                    item.ConsultationFile = query.ToList();
                    //var rep = Attachmentreply.result.item.(t => t.ReplyId == item.ReplyId).ToList();
                    //if (rep != null) { item.Message = rep; }
                }
                //replys = replys.Union(Attachmentreply.result.item).ToList();
                // replys = replys.Select(t => new ReplyDetail(t, Attachmentreply.result.item)).ToList();
                questionatt = Attachmentreply.result.item.Where(t => t.ConsultNumber == values.ConsultNumber && t.ServiceType == "consultation").ToList();
            }
            if (phonereplys.Count > 0)
            {
                replys = replys.Union(phonereplys).ToList();
            }
            //var param = new SystemParameterIn() { Type = "ConfigPar" };
            //param.AndAlso(t => !t.IsDelete && t.SystemCode == "ConsultState");
            //var paramlist = await _systemParameterService.ParameterList(param);

            var content = "{\"Type\":\"ConfigPar\",\"SystemCode\":\"ConsultState\",\"secret\":\"" + secret + "\"}";
            var param = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", content);
            var paramlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(param);
            if (paramlist == null || paramlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var InquiryContent = "{\"SystemCode\":\"InquiryMaxCount\",\"secret\":\"" + secret + "\"}";
            var answerparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", InquiryContent);
            var paramResult = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(answerparam);
            if (paramResult == null || paramResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var ReplyMinutesContent = "{\"SystemCode\":\"ReplyMinutesTime\",\"secret\":\"" + secret + "\"}";
            var ReplyMinutesparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", ReplyMinutesContent);
            var ReplyMinutesparamResult = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(ReplyMinutesparam);
            if (ReplyMinutesparamResult == null || ReplyMinutesparamResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var Recommenddoctoruser = new YaeherDoctorUser();
            if (values.RecommendDoctorID > 0)
            {
                var DocContent = "{\"Id\":" + values.RecommendDoctorID.ToString() + ",\"secret\":\"" + secret + "\"}";
                var Doctor = await this.PostResponseAsync(Commons.DoctorIp + "api/YaeherDoctorById/", DocContent);
                var DocResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherDoctorUser>>>(Doctor);
                if (DocResult == null || DocResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
                Recommenddoctoruser = DocResult.result.item;
            }

            var IIInesslabel = JsonHelper.FromJson<LableManage>(values.IIInessJSON);
            //Logger.Info("结束："+DateTime.Now + "：" + JsonHelper.ToJson(ConsultationInfo.ConsultNumber));
            this.ObjectResultModule.Object = new ConsultationDetailOut(values, orderTradeRecord, Recommenddoctoruser, questionatt, replys, UserResult.result.item, eva, serverid, IIInesslabel.Id, paramlist.result.item, paramResult.result.item, ReplyMinutesparamResult.result.item[0]);
            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationDetail",
                OperContent = JsonHelper.ToJson(ConsultationInfo),
                OperType = "ConsultationDetail",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 咨询回答追问
        /// <summary>
        /// 咨询回答 新增
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/CreateConsultationReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateConsultationReply([FromBody] ConsultationReplyAdd ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var consultation = await _consultationService.YaeherConsultationByID(ConsultationReplyInfo.ConsultID);
            if (consultation == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (consultation.HasInquiryTimes == 0) { return new ObjectResultModule("", 400, "剩余追问次数为0不允许继续追问！"); }
            if (consultation.HasInquiryTimes != consultation.InquiryTimes && consultation.HasReply == false) { return new ObjectResultModule("", 400, "医生未回复不允许继续追问！"); }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var CreateConsultationReply = new ConsultationReply()
                {
                    SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                    ConsultNumber = consultation.ConsultNumber,
                    ConsultID = consultation.Id,
                    ConsultantID = consultation.ConsultantID,
                    ConsultantName = consultation.ConsultantName,
                    PatientID = consultation.PatientID,
                    PatientName = consultation.PatientName,
                    DoctorName = consultation.DoctorName,
                    DoctorID = consultation.DoctorID,
                    ConsultType = consultation.ConsultType,
                    PatientTelephone = consultation.PhoneNumber,
                    PatientCity = consultation.PatientCity,
                    IllnessDescription = consultation.IIInessDescription,
                    ReplyType = ConsultationReplyInfo.ReplyTypeCode,
                    RepayIllnessDescription = ConsultationReplyInfo.RepayIllnessDescription,
                    ReplyState = ConsultationReplyInfo.ReplyStateCode,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                };
                consultation.HasInquiryTimes--;
                consultation.HasReply = false;
                await _consultationService.UpdateYaeherConsultation(consultation);
                var result = await _consultationReplyService.CreateConsultationReply(CreateConsultationReply);
                if (ConsultationReplyInfo.Attach.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var AttachmentInfo in ConsultationReplyInfo.Attach)
                    {
                      //  var content = "{\"Id\":\"" + AttachmentInfo.Id + "\",\"Filename\":\"" + AttachmentInfo.Filename + "\",\"IsDelete\":\"" + AttachmentInfo.IsDelete + "\",\"ConsultID\":\"" + result.Id + "\",\"ConsultNumber\":\"" + result.ConsultNumber + "\",\"ServiceType\":\"consultation\",\"CreatedBy\":\"" + userid + "\",\"MediaType\":\"" + AttachmentInfo.MediaType + "\"},";
                       
                        var content = "{\"Filename\":\"" + AttachmentInfo.Filename + "\",\"secret\":\"" + secret + "\",\"ServiceID\":\"" + result.Id + "\",\"ServiceNumber\":\"" + result.SequenceNo + "\",\"FileSize\":\"" + AttachmentInfo.FileSize + "\",\"ConsultID\":\"" + consultation.Id + "\",\"ConsultNumber\":\"" + result.ConsultNumber + "\",\"ServiceType\":\"inquiries\",\"CreatedBy\":\"" + userid + "\",\"MediaType\":\"" + AttachmentInfo.MediaType + "\"},";
                        sb.Append(content);
                    }
                    var sbstr = sb.ToString().TrimEnd(',');
                    var filelist = "{\"secret\":\"" + secret + "\",\"attach\":[" + sbstr + "]}";
                    var param2 = await this.PostResponseAsync(Commons.AdminIp + "api/CreateMobileAttachmentList", filelist);

                 //   var param2 = await this.PostResponseAsync(Commons.AdminIp + "api/CreateMobileAttachment/", content);
                }
                #region  咨询回答追问 接受消息 医生  DoctorNotice Inquiry
                Consultation replyConsultation = new Consultation();
                replyConsultation.yaeherConsultation = consultation;
                replyConsultation.consultationReply = result;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "DoctorNotice",
                    OperationType = "Inquiry",  //   咨询追问
                    Publisher = "Patient",
                    PublishUrl = "Patient",
                    EventName = "咨询 追问",
                    EventCode = "Consultation",
                    BusinessID = result.Id.ToString(),
                    BusinessCode = result.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(replyConsultation),
                    PublishedTime = result.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion

                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";

                unitOfWork.Complete();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateConsultationReply",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "CreateConsultationReply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询回答 修改
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateConsultationReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateConsultationReply([FromBody] ConsultationReply ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var UpdateConsultationReply = await _consultationReplyService.ConsultationReplyByID(ConsultationReplyInfo.Id);
            var consultation = await _consultationService.YaeherConsultationByID(ConsultationReplyInfo.ConsultID);
            if (UpdateConsultationReply != null)
            {
                UpdateConsultationReply.ConsultNumber = UpdateConsultationReply.ConsultNumber;
                UpdateConsultationReply.ConsultID = UpdateConsultationReply.ConsultID;
                UpdateConsultationReply.ConsultantID = UpdateConsultationReply.ConsultantID;
                UpdateConsultationReply.ConsultantName = UpdateConsultationReply.ConsultantName;
                UpdateConsultationReply.PatientID = UpdateConsultationReply.PatientID;
                UpdateConsultationReply.PatientName = UpdateConsultationReply.PatientName;
                UpdateConsultationReply.DoctorName = UpdateConsultationReply.DoctorName;
                UpdateConsultationReply.DoctorID = UpdateConsultationReply.DoctorID;
                UpdateConsultationReply.ConsultType = ConsultationReplyInfo.ConsultType;
                UpdateConsultationReply.PatientTelephone = ConsultationReplyInfo.PatientTelephone;
                UpdateConsultationReply.PatientCity = ConsultationReplyInfo.PatientCity;
                UpdateConsultationReply.IllnessDescription = ConsultationReplyInfo.IllnessDescription;
                UpdateConsultationReply.ReplyType = ConsultationReplyInfo.ReplyType;
                UpdateConsultationReply.RepayIllnessDescription = ConsultationReplyInfo.RepayIllnessDescription;
                UpdateConsultationReply.ReplyState = ConsultationReplyInfo.ReplyState;
                UpdateConsultationReply.ModifyOn = DateTime.Now;
                UpdateConsultationReply.ModifyBy = userid;

                var result = await _consultationReplyService.UpdateConsultationReply(UpdateConsultationReply);
                #region  咨询追问 修改追问不发送消息
                Consultation replyConsultation = new Consultation();
                replyConsultation.yaeherConsultation = consultation;
                replyConsultation.consultationReply = result;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "DoctorNotice",
                    OperationType = "Inquiry",  //   咨询追问
                    Publisher = "Patient",
                    PublishUrl = "Patient",
                    EventName = "发布 咨询回答修改",
                    EventCode = "Consultation",
                    BusinessID = result.Id.ToString(),
                    BusinessCode = result.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(replyConsultation),
                    PublishedTime = result.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion
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
                OperExplain = "UpdateConsultationReply",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "UpdateConsultationReply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询回答 删除
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteConsultationReply")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteConsultationReply([FromBody] ConsultationReply ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var query = await _consultationReplyService.ConsultationReplyByID(ConsultationReplyInfo.Id);
            var consultation = await _consultationService.YaeherConsultationByID(ConsultationReplyInfo.ConsultID);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _consultationReplyService.DeleteConsultationReply(query);
                #region  咨询回答
                Consultation replyConsultation = new Consultation();
                replyConsultation.consultationReply = res;
                replyConsultation.yaeherConsultation = consultation;
                Publishs Consultationpublishs = new Publishs()
                {
                    Publisher = "Patient",
                    PublishUrl = "Patient",
                    EventName = "发布 删除咨询回答",
                    EventCode = "Consultation",
                    BusinessID = res.Id.ToString(),
                    BusinessCode = res.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(replyConsultation),
                    PublishedTime = res.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion
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
                OperExplain = "DeleteConsultationReply",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "DeleteConsultationReply",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询回答 Page
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationReplyPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationReplyPage([FromBody]ConsultationReplyIn ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationReplyInfo.AndAlso(t => !t.IsDelete);
            var values = await _consultationReplyService.ConsultationReplyPage(ConsultationReplyInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ConsultationReplyOut(values, ConsultationReplyInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationReplyPage",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "ConsultationReplyPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询回答 List 
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationReplyList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationReplyList([FromBody]ConsultationReplyIn ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationReplyInfo.AndAlso(t => !t.IsDelete);
            var values = await _consultationReplyService.ConsultationReplyList(ConsultationReplyInfo);
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
                OperExplain = "ConsultationReplyList",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "ConsultationReplyList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询回答 Byid
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationReplyById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationReplyById([FromBody]ConsultationReplyIn ConsultationReplyInfo)
        {
            if (!Commons.CheckSecret(ConsultationReplyInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _consultationReplyService.ConsultationReplyByID(ConsultationReplyInfo.Id);
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
                OperExplain = "ConsultationReplyById",
                OperContent = JsonHelper.ToJson(ConsultationReplyInfo),
                OperType = "ConsultationReplyById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 咨询评分 评价

        /// <summary>
        /// 咨询评分 新增
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/CreateConsultationEvaluation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateConsultationEvaluation([FromBody] ConsultationEvaluationAdd ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var Consultation = await _consultationService.YaeherConsultationByID(ConsultationEvaluationInfo.ConsultID);
            if (Consultation == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (Consultation.IsEvaluate)
            {
                return new ObjectResultModule("", 400, "请不要重复提交评价数据！");
            }
            //var content = "{\"Type\":\"ConfigPar\",\"SystemCode\":\"ConsultationEvaluationReason\"}";
            //var param = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", content);
            //var typelist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(param);
            //if (typelist == null || typelist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            //var EvaluationReasonlist = new List<ConsultationEvaluationReason>();
            //foreach (var item in typelist.result.item)
            //{
            //    var newcode = new ConsultationEvaluationReason() { Level = int.Parse(item.Code.Substring(0, 1)), Code = item.Code, Value = item.Name, Type = item.SystemType, TypeCode = item.SystemCode };
            //    EvaluationReasonlist.Add(newcode);
            //}

            var content = "{\"secret\":\"" + secret + "\",\"KeyWord\":\"StarClass\"}";
            var param1 = await this.PostResponseAsync(Commons.AdminIp + "api/LabelConfigList/", content);
            var typelist1 = JsonHelper.FromJson<APIResult<ResultModule<List<YaeherLabelConfig>>>>(param1);
            if (typelist1 == null || typelist1.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }


            var reasson = new List<YaeherLabelConfig>();
            if (ConsultationEvaluationInfo.TitleList != null && ConsultationEvaluationInfo.TitleList.Count > 0)
            {
                foreach (var item in ConsultationEvaluationInfo.TitleList)
                {
                    var detail = typelist1.result.item.Find(t => t.Id == item.Id);
                    // var newcode = new CodeList() { Code = detail.Code, Value = detail.Name, Type = detail.SystemType, TypeCode = detail.SystemCode };
                    if (detail != null)
                    {
                        reasson.Add(detail);
                    }
                }
            }
            if (ConsultationEvaluationInfo.LabelId < 1)
            {
                return new ObjectResultModule("", 400, "请选择星级之后提交！");
            }
            var startt = typelist1.result.item.Find(t => t.Id == ConsultationEvaluationInfo.LabelId);
            if (startt == null)
            {
                return new ObjectResultModule("", 400, "请选择星级之后提交！");
            }
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var CreateConsultationEvaluation = new ConsultationEvaluation()
                {
                    SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                    ConsultNumber = Consultation.ConsultNumber,
                    ConsultID = Consultation.Id,
                    ConsultantID = Consultation.ConsultantID,
                    ConsultantName = Consultation.ConsultantName,
                    PatientID = Consultation.PatientID,
                    PatientName = Consultation.PatientName,
                    DoctorName = Consultation.DoctorName,
                    DoctorID = Consultation.DoctorID,
                    EvaluateContent = ConsultationEvaluationInfo.EvaluateContent,
                    EvaluateReason = JsonHelper.ToJson(reasson),
                    StarTitle = JsonHelper.ToJson(startt),
                    EvaluateLevel = int.Parse(startt.LabelCode),
                    QualityLevel = int.Parse(startt.LabelCode),  // 默认将评价分数存入
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                };
                var result = await _consultationEvaluationService.CreateConsultationEvaluation(CreateConsultationEvaluation);

                Consultation.IsEvaluate = true;
                Consultation.ModifyBy = userid;
                Consultation.ModifyOn = DateTime.Now;
                await _consultationService.UpdateYaeherConsultation(Consultation);

                #region  咨询评分  接受消息为医生 DoctorEvaluate Evaluation
                Consultation evaluation = new Consultation();
                evaluation.yaeherConsultation = Consultation;
                evaluation.consultationEvaluation = result;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "DoctorEvaluate",
                    OperationType = "Evaluation",  //    咨询评分
                    MessageRemark = CreateConsultationEvaluation.EvaluateContent,  //  评价内容
                    Publisher = "Patient",
                    PublishUrl = "Patient",
                    EventName = "咨询评分",
                    EventCode = "Consultation",
                    BusinessID = result.Id.ToString(),
                    BusinessCode = result.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(evaluation),
                    PublishedTime = result.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion

                this.ObjectResultModule.Object = result;
                unitOfWork.Complete();
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateConsultationEvaluation",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "CreateConsultationEvaluation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询评分 修改
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateConsultationEvaluation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateConsultationEvaluation([FromBody] ConsultationEvaluation ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var UpdateConsultationEvaluation = await _consultationEvaluationService.ConsultationEvaluationByID(ConsultationEvaluationInfo.Id);
            if (UpdateConsultationEvaluation != null)
            {
                UpdateConsultationEvaluation.ConsultNumber = ConsultationEvaluationInfo.ConsultNumber;
                UpdateConsultationEvaluation.ConsultID = ConsultationEvaluationInfo.ConsultID;
                UpdateConsultationEvaluation.ConsultantID = ConsultationEvaluationInfo.ConsultantID;
                UpdateConsultationEvaluation.ConsultantName = ConsultationEvaluationInfo.ConsultantName;
                UpdateConsultationEvaluation.PatientID = ConsultationEvaluationInfo.PatientID;
                UpdateConsultationEvaluation.PatientName = ConsultationEvaluationInfo.PatientName;
                UpdateConsultationEvaluation.DoctorName = ConsultationEvaluationInfo.DoctorName;
                UpdateConsultationEvaluation.DoctorID = ConsultationEvaluationInfo.DoctorID;
                UpdateConsultationEvaluation.EvaluateContent = ConsultationEvaluationInfo.EvaluateContent;
                UpdateConsultationEvaluation.EvaluateReason = ConsultationEvaluationInfo.EvaluateReason;
                UpdateConsultationEvaluation.EvaluateLevel = ConsultationEvaluationInfo.EvaluateLevel;
                UpdateConsultationEvaluation.QualityLevel = ConsultationEvaluationInfo.QualityLevel;
                UpdateConsultationEvaluation.IsQuality = ConsultationEvaluationInfo.IsQuality;
                UpdateConsultationEvaluation.QualityReason = ConsultationEvaluationInfo.QualityReason;
                UpdateConsultationEvaluation.CreatedBy = ConsultationEvaluationInfo.CreatedBy;
                UpdateConsultationEvaluation.CreatedOn = ConsultationEvaluationInfo.CreatedOn;
                UpdateConsultationEvaluation.ModifyOn = DateTime.Now;
                UpdateConsultationEvaluation.ModifyBy = userid;

                var result = await _consultationEvaluationService.UpdateConsultationEvaluation(UpdateConsultationEvaluation);

                #region  咨询评分  评分暂时不使用 不做发布消息
                Consultation evaluation = new Consultation();
                evaluation.consultationEvaluation = result;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "DoctorEvaluate",
                    OperationType = "Evaluation",  //    咨询评分
                    MessageRemark = UpdateConsultationEvaluation.EvaluateContent,  // 评价
                    Publisher = "Patient",
                    PublishUrl = "Patient",
                    EventName = "咨询评分",
                    EventCode = "Consultation",
                    BusinessID = result.Id.ToString(),
                    BusinessCode = result.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(evaluation),
                    PublishedTime = result.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion

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
                OperExplain = "UpdateConsultationEvaluation",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "UpdateConsultationEvaluation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询评分 删除
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteConsultationEvaluation")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteConsultationEvaluation([FromBody] ConsultationEvaluation ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var query = await _consultationEvaluationService.ConsultationEvaluationByID(ConsultationEvaluationInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _consultationEvaluationService.DeleteConsultationEvaluation(query);
                #region  咨询评分 评价不消息发送处理
                Consultation evaluation = new Consultation();
                evaluation.consultationEvaluation = res;
                Publishs Consultationpublishs = new Publishs()
                {
                    TemplateCode = "DoctorEvaluate",
                    OperationType = "Evaluation",  //    咨询评分
                    Publisher = "Patient",
                    PublishUrl = "Patient",
                    EventName = "咨询评分",
                    EventCode = "Consultation",
                    BusinessID = res.Id.ToString(),
                    BusinessCode = res.ConsultNumber,
                    BusinessJSON = JsonHelper.ToJson(evaluation),
                    PublishedTime = res.CreatedOn,
                    PublishStatus = true,
                    CreatedBy = userid,
                    CreatedOn = DateTime.Now,
                    Secret = secret,
                };
                var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                if (ConsultationJson != null)
                {
                    Consultationpublishs.PublishStatus = true;
                }
                else
                {
                    Consultationpublishs.PublishStatus = false;
                }
                Consultationpublishs.ServerClient = "Client";
                var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                #endregion
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
                OperExplain = "DeleteConsultationEvaluation",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "DeleteConsultationEvaluation",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询评分 Page
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationPage([FromBody]ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationEvaluationInfo.AndAlso(t => !t.IsDelete && t.CreatedBy == userid);
            var values = await _consultationEvaluationService.ConsultationEvaluationPage(ConsultationEvaluationInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new ConsultationEvaluationOut(values, ConsultationEvaluationInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationEvaluationPage",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "ConsultationEvaluationPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 咨询评分 List 
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationList([FromBody]ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            ConsultationEvaluationInfo.AndAlso(t => !t.IsDelete);
            var values = await _consultationEvaluationService.ConsultationEvaluationList(ConsultationEvaluationInfo);
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
                OperExplain = "ConsultationEvaluationList",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "ConsultationEvaluationList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 咨询评分详情
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationEvaluationById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationEvaluationById([FromBody]ConsultationEvaluationIn ConsultationEvaluationInfo)
        {
            if (!Commons.CheckSecret(ConsultationEvaluationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var values = await _consultationEvaluationService.ConsultationEvaluationByID(ConsultationEvaluationInfo.Id);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var Content = "{\"IDArray\":\"" + values.DoctorID.ToString() + "\",\"secret\":\"" + secret + "\"}";
            var user = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherUserByIDArray/", Content);
            if (user == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var UserResult = JsonHelper.FromJson<APIResult<List<YaeherUserDocMsg>>>(user);
            if (UserResult == null || UserResult.result.Count < 1) { return new ObjectResultModule("", 204, "NoContent"); }

            if (values == null)
            {
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                var result = new ConsultationEvaluationDetail(values, UserResult.result[0]);
                this.ObjectResultModule.Object = result;
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "ConsultationEvaluationById",
                OperContent = JsonHelper.ToJson(ConsultationEvaluationInfo),
                OperType = "ConsultationEvaluationById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 电话回复记录
        /// <summary>
        /// 电话回复记录 新增
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/CreatePhoneReplyRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreatePhoneReplyRecord([FromBody] PhoneReplyRecord PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreatePhoneReplyRecord = new PhoneReplyRecord()
            {
                SequenceNo = PhoneReplyRecordInfo.SequenceNo == null ? DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6) : PhoneReplyRecordInfo.SequenceNo,
                Id = PhoneReplyRecordInfo.Id,
                ConsultNumber = PhoneReplyRecordInfo.ConsultNumber,
                ConsultID = PhoneReplyRecordInfo.ConsultID,
                ConsultantID = PhoneReplyRecordInfo.ConsultantID,
                ConsultantName = PhoneReplyRecordInfo.ConsultantName,
                PatientID = PhoneReplyRecordInfo.PatientID,
                PatientName = PhoneReplyRecordInfo.PatientName,
                DoctorID = PhoneReplyRecordInfo.DoctorID,
                DoctorName = PhoneReplyRecordInfo.DoctorName,
                CallTime = PhoneReplyRecordInfo.CallTime,
                CallDuration = PhoneReplyRecordInfo.CallDuration,
                CallIntro = PhoneReplyRecordInfo.CallIntro,
                RecordAddress = PhoneReplyRecordInfo.RecordAddress,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var result = await _phoneReplyRecordService.CreatePhoneReplyRecord(CreatePhoneReplyRecord);
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
                OperExplain = "CreatePhoneReplyRecord",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "CreatePhoneReplyRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 电话回复记录 修改
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/UpdatePhoneReplyRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdatePhoneReplyRecord([FromBody] PhoneReplyRecord PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdatePhoneReplyRecord = await _phoneReplyRecordService.PhoneReplyRecordByID(PhoneReplyRecordInfo.Id);
            if (UpdatePhoneReplyRecord != null)
            {
                UpdatePhoneReplyRecord.ConsultNumber = PhoneReplyRecordInfo.ConsultNumber;
                UpdatePhoneReplyRecord.ConsultID = PhoneReplyRecordInfo.ConsultID;
                UpdatePhoneReplyRecord.ConsultantID = PhoneReplyRecordInfo.ConsultantID;
                UpdatePhoneReplyRecord.ConsultantName = PhoneReplyRecordInfo.ConsultantName;
                UpdatePhoneReplyRecord.PatientID = PhoneReplyRecordInfo.PatientID;
                UpdatePhoneReplyRecord.PatientName = PhoneReplyRecordInfo.PatientName;
                UpdatePhoneReplyRecord.DoctorID = PhoneReplyRecordInfo.DoctorID;
                UpdatePhoneReplyRecord.DoctorName = PhoneReplyRecordInfo.DoctorName;
                UpdatePhoneReplyRecord.CallTime = PhoneReplyRecordInfo.CallTime;
                UpdatePhoneReplyRecord.CallDuration = PhoneReplyRecordInfo.CallDuration;
                UpdatePhoneReplyRecord.CallIntro = PhoneReplyRecordInfo.CallIntro;
                UpdatePhoneReplyRecord.RecordAddress = PhoneReplyRecordInfo.RecordAddress;
                UpdatePhoneReplyRecord.CreatedBy = PhoneReplyRecordInfo.CreatedBy;
                UpdatePhoneReplyRecord.CreatedOn = PhoneReplyRecordInfo.CreatedOn;
                UpdatePhoneReplyRecord.ModifyOn = DateTime.Now;
                UpdatePhoneReplyRecord.ModifyBy = userid;

                var result = await _phoneReplyRecordService.UpdatePhoneReplyRecord(UpdatePhoneReplyRecord);

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
                OperExplain = "UpdatePhoneReplyRecord",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "UpdatePhoneReplyRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 电话回复记录 删除
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DeletePhoneReplyRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeletePhoneReplyRecord([FromBody] PhoneReplyRecord PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _phoneReplyRecordService.PhoneReplyRecordByID(PhoneReplyRecordInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _phoneReplyRecordService.DeletePhoneReplyRecord(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeletePhoneReplyRecord",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "DeletePhoneReplyRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 电话回复记录 Page
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/PhoneReplyRecordPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> PhoneReplyRecordPage([FromBody]PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PhoneReplyRecordInfo.AndAlso(t => !t.IsDelete);
            var values = await _phoneReplyRecordService.PhoneReplyRecordPage(PhoneReplyRecordInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new PhoneReplyRecordOut(values, PhoneReplyRecordInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "PhoneReplyRecordPage",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "PhoneReplyRecordPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 电话回复记录 List 
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/PhoneReplyRecordList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> PhoneReplyRecordList([FromBody]PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            PhoneReplyRecordInfo.AndAlso(t => !t.IsDelete);
            var values = await _phoneReplyRecordService.PhoneReplyRecordList(PhoneReplyRecordInfo);
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
                OperExplain = "PhoneReplyRecordList",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "PhoneReplyRecordList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 电话回复记录 Byid
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [Route("api/PhoneReplyRecordById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> PhoneReplyRecordById([FromBody]PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            if (!Commons.CheckSecret(PhoneReplyRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _phoneReplyRecordService.PhoneReplyRecordByID(PhoneReplyRecordInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.Object = "";
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
                OperExplain = "PhoneReplyRecordById",
                OperContent = JsonHelper.ToJson(PhoneReplyRecordInfo),
                OperType = "PhoneReplyRecordById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单管理
        /// <summary>
        /// 订单管理 新增
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateOrderManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateOrderManage([FromBody] OrderManage OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateOrderManage = new OrderManage()
            {
                SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                OrderNumber = OrderManageInfo.OrderNumber,
                ConsultNumber = OrderManageInfo.ConsultNumber,
                ConsultID = OrderManageInfo.ConsultID,
                ConsultType = OrderManageInfo.ConsultType,
                ConsultantID = OrderManageInfo.ConsultantID,
                ConsultantName = OrderManageInfo.ConsultantName,
                PatientID = OrderManageInfo.PatientID,
                PatientName = OrderManageInfo.PatientName,
                DoctorName = OrderManageInfo.DoctorName,
                DoctorID = OrderManageInfo.DoctorID,
                OrderCurrency = OrderManageInfo.OrderCurrency,
                OrderMoney = OrderManageInfo.OrderMoney,
                ReceivablesType = OrderManageInfo.ReceivablesType,
                ReceivablesNumber = OrderManageInfo.ReceivablesNumber,
                ServiceID = OrderManageInfo.ServiceID,
                ServiceName = OrderManageInfo.ServiceName,
                SellerMoneyID = OrderManageInfo.SellerMoneyID,
                //TradeType = OrderManageInfo.TradeType,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _orderManageService.CreateOrderManage(CreateOrderManage);
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
                OperExplain = "CreateOrderManage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "CreateOrderManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单管理 修改
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateOrderManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateOrderManage([FromBody] OrderManage OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateOrderManage = await _orderManageService.OrderManageByID(OrderManageInfo.Id);
            if (UpdateOrderManage != null)
            {
                UpdateOrderManage.OrderNumber = OrderManageInfo.OrderNumber;
                UpdateOrderManage.ConsultNumber = OrderManageInfo.ConsultNumber;
                UpdateOrderManage.ConsultID = OrderManageInfo.ConsultID;
                UpdateOrderManage.ConsultType = OrderManageInfo.ConsultType;
                UpdateOrderManage.ConsultantID = OrderManageInfo.ConsultantID;
                UpdateOrderManage.ConsultantName = OrderManageInfo.ConsultantName;
                UpdateOrderManage.PatientID = OrderManageInfo.PatientID;
                UpdateOrderManage.PatientName = OrderManageInfo.PatientName;
                UpdateOrderManage.DoctorName = OrderManageInfo.DoctorName;
                UpdateOrderManage.DoctorID = OrderManageInfo.DoctorID;
                UpdateOrderManage.OrderCurrency = OrderManageInfo.OrderCurrency;
                UpdateOrderManage.OrderMoney = OrderManageInfo.OrderMoney;
                UpdateOrderManage.ReceivablesType = OrderManageInfo.ReceivablesType;
                UpdateOrderManage.ReceivablesNumber = OrderManageInfo.ReceivablesNumber;
                UpdateOrderManage.ServiceID = OrderManageInfo.ServiceID;
                UpdateOrderManage.ServiceName = OrderManageInfo.ServiceName;
                UpdateOrderManage.SellerMoneyID = OrderManageInfo.SellerMoneyID;
                //UpdateOrderManage.TradeType = OrderManageInfo.TradeType;
                UpdateOrderManage.ModifyOn = DateTime.Now;
                UpdateOrderManage.ModifyBy = userid;

                var result = await _orderManageService.UpdateOrderManage(UpdateOrderManage);

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
                OperExplain = "UpdateOrderManage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "UpdateOrderManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单管理 删除
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteOrderManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteOrderManage([FromBody] OrderManage OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _orderManageService.OrderManageByID(OrderManageInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _orderManageService.DeleteOrderManage(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteOrderManage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "DeleteOrderManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单管理 Page
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/OrderManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderManagePage([FromBody]OrderManageIn OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            OrderManageInfo.AndAlso(t => !t.IsDelete);
            var values = await _orderManageService.OrderManagePage(OrderManageInfo);

            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new OrderManageOut(values, OrderManageInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderManagePage",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "OrderManagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单管理 List 
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/OrderManageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderManageList([FromBody]OrderManageIn OrderManageInfo)
        {
            //Logger.Info("OrderManageInfo:" + JsonHelper.ToJson(OrderManageInfo));
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            OrderManageInfo.AndAlso(t => !t.IsDelete);
            if (OrderManageInfo.ServiceID > 0)
            {
                OrderManageInfo.AndAlso(t => t.ServiceID == OrderManageInfo.ServiceID);
            }
            if (OrderManageInfo.DoctorID > 0)
            {
                OrderManageInfo.AndAlso(t => t.DoctorID == OrderManageInfo.DoctorID);
            }
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderManageInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderManageInfo.StartTime);
                if (string.IsNullOrEmpty(OrderManageInfo.EndTime))
                {
                    OrderManageInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderManageInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderManageInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderManageInfo.StartTime))
            {
                OrderManageInfo.AndAlso(t => t.CreatedOn >= StartTime);
                OrderManageInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));

            }
            var values = await _orderManageService.OrderManageList(OrderManageInfo);
            //Logger.Info("OrderManageListResult:" + JsonHelper.ToJson(values));

            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderManageList",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "OrderManageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单管理 Byid
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        [Route("api/OrderManageById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderManageById([FromBody]OrderManageIn OrderManageInfo)
        {
            if (!Commons.CheckSecret(OrderManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _orderManageService.OrderManageByID(OrderManageInfo.Id);
            if (values == null)
            {
                this.ObjectResultModule.Object = "";
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
                OperExplain = "OrderManageById",
                OperContent = JsonHelper.ToJson(OrderManageInfo),
                OperType = "OrderManageById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单交易记录
        /// <summary>
        /// 订单交易记录 新增
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/CreateOrderTradeRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateOrderTradeRecord([FromBody] OrderTradeRecord OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateOrderTradeRecord = new OrderTradeRecord()
            {
                SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                OrderID = OrderTradeRecordInfo.OrderID,
                OrderNumber = OrderTradeRecordInfo.OrderNumber,
                PayType = OrderTradeRecordInfo.PayType,
                OrderCurrency = OrderTradeRecordInfo.OrderCurrency,
                TenpayNumber = OrderTradeRecordInfo.TenpayNumber,
                VoucherNumber = OrderTradeRecordInfo.VoucherNumber,
                VoucherJSON = OrderTradeRecordInfo.VoucherJSON,
                PayMoney = OrderTradeRecordInfo.PayMoney,
                PayAchiveTime = OrderTradeRecordInfo.PayAchiveTime,
                PaySerialNumber = OrderTradeRecordInfo.PaySerialNumber,
                PaymentState = OrderTradeRecordInfo.PaymentState,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _orderTradeRecordService.CreateOrderTradeRecord(CreateOrderTradeRecord);
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
                OperExplain = "CreateOrderTradeRecord",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "CreateOrderTradeRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单交易记录 修改
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateOrderTradeRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateOrderTradeRecord([FromBody] OrderTradeRecord OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;

            var UpdateOrderTradeRecord = await _orderTradeRecordService.OrderTradeRecordByID(OrderTradeRecordInfo.Id);
            if (UpdateOrderTradeRecord != null)
            {
                UpdateOrderTradeRecord.OrderID = OrderTradeRecordInfo.OrderID;
                UpdateOrderTradeRecord.OrderNumber = OrderTradeRecordInfo.OrderNumber;
                UpdateOrderTradeRecord.PayType = OrderTradeRecordInfo.PayType;
                UpdateOrderTradeRecord.OrderCurrency = OrderTradeRecordInfo.OrderCurrency;
                UpdateOrderTradeRecord.TenpayNumber = OrderTradeRecordInfo.TenpayNumber;
                UpdateOrderTradeRecord.VoucherNumber = OrderTradeRecordInfo.VoucherNumber;
                UpdateOrderTradeRecord.VoucherJSON = OrderTradeRecordInfo.VoucherJSON;
                UpdateOrderTradeRecord.PayMoney = OrderTradeRecordInfo.PayMoney;
                UpdateOrderTradeRecord.PayAchiveTime = OrderTradeRecordInfo.PayAchiveTime;
                UpdateOrderTradeRecord.PaySerialNumber = OrderTradeRecordInfo.PaySerialNumber;
                UpdateOrderTradeRecord.PaymentState = OrderTradeRecordInfo.PaymentState;
                UpdateOrderTradeRecord.ModifyOn = DateTime.Now;
                UpdateOrderTradeRecord.ModifyBy = userid;

                var result = await _orderTradeRecordService.UpdateOrderTradeRecord(UpdateOrderTradeRecord);

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
                OperExplain = "UpdateOrderTradeRecord",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "UpdateOrderTradeRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单交易记录 删除
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteOrderTradeRecord")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteOrderTradeRecord([FromBody] OrderTradeRecord OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _orderTradeRecordService.OrderTradeRecordByID(OrderTradeRecordInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _orderTradeRecordService.DeleteOrderTradeRecord(query);

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
                OperExplain = "DeleteOrderTradeRecord",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "DeleteOrderTradeRecord",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单交易记录 Page
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTradeRecordPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTradeRecordPage([FromBody]OrderTradeRecordIn OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            OrderTradeRecordInfo.AndAlso(t => !t.IsDelete);
            var values = await _orderTradeRecordService.OrderTradeRecordPage(OrderTradeRecordInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new OrderTradeRecordOut(values, OrderTradeRecordInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTradeRecordPage",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderTradeRecordPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单交易记录 List 
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTradeRecordList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTradeRecordList([FromBody]OrderTradeRecordIn OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            OrderTradeRecordInfo.AndAlso(t => !t.IsDelete);
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderTradeRecordInfo.StartTime);
                if (string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
                {
                    OrderTradeRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderTradeRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                OrderTradeRecordInfo.AndAlso(t => t.CreatedOn >= StartTime);
                OrderTradeRecordInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));

            }
            var values = await _orderTradeRecordService.OrderTradeRecordList(OrderTradeRecordInfo);
            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTradeRecordList",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderTradeRecordList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单交易记录 List 
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/DoctorOrderTradeRecordList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DoctorOrderTradeRecordList([FromBody]OrderTradeRecordIn OrderTradeRecordInfo)
        {
            //Logger.Info("OrderTradeRecordInfo:" + JsonHelper.ToJson(OrderTradeRecordInfo));
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            OrderTradeRecordInfo.AndAlso(t => !t.IsDelete);
            DateTime StartTime = new DateTime();
            DateTime EndTime = new DateTime();
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                StartTime = DateTime.Parse(OrderTradeRecordInfo.StartTime);
                if (string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
                {
                    OrderTradeRecordInfo.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.EndTime))
            {
                EndTime = DateTime.Parse(OrderTradeRecordInfo.EndTime);
            }
            if (!string.IsNullOrEmpty(OrderTradeRecordInfo.StartTime))
            {
                OrderTradeRecordInfo.AndAlso(t => t.CreatedOn >= StartTime);
                OrderTradeRecordInfo.AndAlso(t => t.CreatedOn < EndTime.AddDays(+1));

            }
            //OrderTradeRecordIn refundManageIn = new OrderTradeRecordIn();
            //var StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            //refundManageIn.DoctorId = consul.DoctorID;
            //refundManageIn.ServiceID = consul.ServiceMoneyListId;
            //refundManageIn.AndAlso(t => t.IsDelete == false && t.CreatedOn >= StartTime && t.CreatedOn < StartTime.AddDays(1));
            //var ordertradelist = await _orderTradeRecordService.DoctorOrderTradeRecordList(refundManageIn);


            var values = await _orderTradeRecordService.DoctorOrderTradeRecordList(OrderTradeRecordInfo);
            //Logger.Info("OrderTradeRecordInforesut:" + JsonHelper.ToJson(values));
            this.ObjectResultModule.Object = values;
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";

            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTradeRecordList",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderTradeRecordList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }

        /// <summary>
        /// 订单交易记录 Byid
        /// </summary>
        /// <param name="OrderTradeRecordInfo"></param>
        /// <returns></returns>
        [Route("api/OrderTradeRecordById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTradeRecordById([FromBody]OrderTradeRecordIn OrderTradeRecordInfo)
        {
            if (!Commons.CheckSecret(OrderTradeRecordInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _orderTradeRecordService.OrderTradeRecordByID(OrderTradeRecordInfo.Id);
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
                OperExplain = "OrderTradeRecordById",
                OperContent = JsonHelper.ToJson(OrderTradeRecordInfo),
                OperType = "OrderTradeRecordById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单退单管理
        /// <summary>
        /// 订单退单管理 新增
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/CreateRefundManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateRefundManage([FromBody] RefundManagePatientInfo RefundManageInfo)
        {
            //Logger.Info("退单：" + JsonHelper.ToJson(RefundManageInfo));
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var secret = await CreateSecret();
            var content = "{\"Type\":\"ConfigPar\",\"SystemCode\":\"PatientRefundManageType\",\"secret\":\"" + secret + "\"}";
            var param = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", content);
            var paramlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(param);
            if (paramlist == null || paramlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }


            var ReplyMinutesContent = "{\"SystemCode\":\"ReplyMinutesTime\",\"secret\":\"" + secret + "\"}";
            var ReplyMinutesparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", ReplyMinutesContent);
            var paramResult = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(ReplyMinutesparam);
            if (paramResult == null || paramResult.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var Content = "{\"SystemCode\":\"WxPayBusinessId\",\"secret\":\"" + secret + "\"}";
            var wxparam = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherMobileParameterList/", Content);
            var wxparamlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemParameter>>>>(wxparam);
            if (wxparamlist == null || wxparamlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var yaeherConsultation = await _consultationService.YaeherConsultationByID(RefundManageInfo.ConsultID);
            if (yaeherConsultation == null || yaeherConsultation.CreatedBy != userid) { return new ObjectResultModule("", 204, "NoContent"); }
            if (yaeherConsultation.RefundState == "return") { return new ObjectResultModule("", 400, "已经退单"); }
            if (yaeherConsultation.ConsultState != "created") { return new ObjectResultModule("", 400, "只有创建的咨询单才能退单！"); }

            //Logger.Info("CanhargebackTime:"+ CanhargebackTime.ToString("yyyyMMddHHmmss"));
            var Order = await _orderManageService.OrderManageByconsultNumber(yaeherConsultation.ConsultNumber);
            if (Order == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var ordertrade = await _orderTradeRecordService.OrderTradeRecordExpress(t => !t.IsDelete && t.OrderID == Order.Id && t.PaymentSourceCode == "order" && t.PayMoney > 0);
            if (ordertrade == null) { return new ObjectResultModule("", 204, "NoContent"); }

            TimeSpan ts = DateTime.UtcNow.Subtract(ordertrade.CreatedOn);
            DateTime CanhargebackTime = ordertrade.CreatedOn.AddDays(double.Parse(paramResult.result.item[0].ItemValue.ToString() == "" ? "0" : paramResult.result.item[0].ItemValue.ToString()));

            if ((DateTime.Now < CanhargebackTime))//能退单时间，从数据库拿 按照支付时间计算
            {
                return new ObjectResultModule("", 400, "暂不允许退单！");
            }

            Content = "{\"Id\":" + yaeherConsultation.CreatedBy.ToString() + ",\"secret\":\"" + secret + "\"}";
            var user = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherUserById/", Content);
            var Consultant = JsonHelper.FromJson<APIResult<ResultModule<YaeherUser>>>(user);
            if (Consultant == null) { return new ObjectResultModule("", 204, "NoContent"); }

            Content = "{\"Id\":" + RefundManageInfo.LabelId + ",\"secret\":\"" + secret + "\"}";
            var labelpost = await this.PostResponseAsync(Commons.AdminIp + "api/YaeherLabelConfigById/", Content);
            var labelresult = JsonHelper.FromJson<APIResult<ResultModule<YaeherLabelList>>>(labelpost);
            if (labelresult == null || labelresult.result.item == null || labelresult.result.item.Id < 1) { return new ObjectResultModule("", 204, "NoContent"); }

            var Patient = JsonHelper.FromJson<YaeherPatientLeaguerInfo>(yaeherConsultation.PatientJSON);
            if (Patient == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var DocContent = "{\"Id\":" + yaeherConsultation.DoctorID.ToString() + ",\"secret\":\"" + secret + "\"}";
            var Doctor = await this.PostResponseAsync(Commons.DoctorIp + "api/YaeherDoctorById/", DocContent);
            var DocResult = JsonHelper.FromJson<APIResult<ResultModule<YaeherDoctorUser>>>(Doctor);
            if (DocResult == null) { return new ObjectResultModule("", 204, "NoContent"); }

            var ServiceContent = "{\"Id\":" + Order.ServiceID.ToString() + ",\"secret\":\"" + secret + "\"}";
            var Service = await this.PostResponseAsync(Commons.DoctorIp + "api/ServiceMoneyListByID/", ServiceContent);
            var ServiceResult = JsonHelper.FromJson<APIResult<ResultModule<ServiceMoneyList>>>(Service);
            if (ServiceResult == null) { return new ObjectResultModule("", 204, "NoContent"); }

            content = "{\"ConsultNumber\":\"" + yaeherConsultation.ConsultNumber.ToString() + "\",\"secret\":\"" + secret + "\"}";
            var consulrequet = await this.PostResponseAsync(Commons.DoctorIp + "api/Consultation/", content);
            var DoctorConsul = JsonHelper.FromJson<APIResult<ResultModule<YaeherConsultation>>>(consulrequet);
            if (DoctorConsul == null || DoctorConsul.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            if (DoctorConsul.result.item.ConsultState == "return") { return new ObjectResultModule("", 400, "医生已退单,请不要退单"); }

            var Content1 = "{\"SystemType\":\"TencentWechar\",\"secret\":\"" + secret + "\"}";
            var tencentmparam = await this.PostResponseAsync(Commons.AdminIp + "api/SystemConfigsList/", Content1);
            var tencentmparamlist = JsonHelper.FromJson<APIResult<ResultModule<List<SystemConfigs>>>>(tencentmparam);
            if (tencentmparamlist == null || tencentmparamlist.result.item == null) { return new ObjectResultModule("", 204, "NoContent"); }
            var tencentparam = tencentmparamlist.result.item.FirstOrDefault();
            OrderTradeRecordIn orderTradeRecordIn = new OrderTradeRecordIn();
            orderTradeRecordIn.AndAlso(t => !t.IsDelete && t.PayType == "wxpay" && t.OrderID == Order.Id && t.PaymentState == "paid" && t.PaymentSourceCode == "order");
            var refundordertradelist = await _orderTradeRecordService.OrderTradeRecordList(orderTradeRecordIn);
            var refundordertrade = refundordertradelist.FirstOrDefault();
            TencentWXPay tencentWXPay = new TencentWXPay();
            var RefundNumber = "RN-" + DateTime.Now.ToString("yyyyMMddhhmm") + new RandomCode().RamdomRecode(4);
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var CreateRefundManage = new RefundManage()
                    {
                        SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                        RefundNumber =RefundNumber,
                        ConsultID = RefundManageInfo.ConsultID,
                        ConsultNumber = yaeherConsultation.ConsultNumber,
                        OrderID = Order.Id,
                        OrderNumber = Order.OrderNumber,
                        ConsultantID = yaeherConsultation.CreatedBy,
                        ConsultantName = Consultant.result.item.FullName,
                        PatientID = yaeherConsultation.PatientID,
                        PatientName = Patient.LeaguerName,
                        DoctorID = yaeherConsultation.DoctorID,
                        DoctorName = DocResult.result.item.DoctorName,
                        OrderCurrency = "rmb",
                        OrderMoney = Order.OrderMoney,//退单表退单金额为订单金额
                        ServiceID = Order.ServiceID,
                        ServiceName = ServiceResult.result.item.DoctorName + ServiceResult.result.item.ServiceType,
                        SellerMoneyID = wxparamlist.result.item[0].ItemValue,
                        CheckState = "success",//患者退单自动通过
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        RefundReason = JsonHelper.ToJson(labelresult.result.item),
                        RefundRemarks = RefundManageInfo.RefundRemarks,
                    };
                    var result = await _refundManageService.CreateRefundManage(CreateRefundManage);

                    yaeherConsultation.RefundState = "return";
                    yaeherConsultation.ConsultState = "return";
                    yaeherConsultation.RefundBy = userid;
                    yaeherConsultation.RefundTime = DateTime.Now;
                    yaeherConsultation.RefundType = "patientreturn";
                    yaeherConsultation.RefundNumber = result.RefundNumber;
                    yaeherConsultation.RefundReason = JsonHelper.ToJson(labelresult.result.item);
                    yaeherConsultation.RefundRemarks = RefundManageInfo.RefundRemarks;

                    var res = await _consultationService.UpdateYaeherConsultation(yaeherConsultation);

                   

                    var record = new OrderTradeRecord()
                    {
                        SequenceNo = DateTime.Now.ToString("yyyyMMddHHmmss") + new RandomCode().GenerateCheckCodeNum(6),
                        OrderID = Order.Id,
                        OrderNumber = Order.OrderNumber,
                        PayType = "wxpay",
                        OrderCurrency = "rmb",
                        TenpayNumber = "",//支付账号
                        VoucherNumber = "",//代金券编号
                        VoucherJSON = "",//代金券Json
                        PayMoney = -Convert.ToDecimal(ordertrade.PayMoney),//退款金额为支付金额
                        PaymentState = "paid",
                        CreatedBy = userid,
                        PaymentSourceCode = "patientreturn",
                        PaymentSource = "患者退单",
                        WXPayBillno = refundordertrade.WXPayBillno,
                        WXOrderQuery = refundordertrade.WXOrderQuery,
                        WXTransactionId = refundordertrade.WXTransactionId,
                        CreatedOn = DateTime.Now
                    };
                    var result2 = await _orderTradeRecordService.CreateOrderTradeRecord(record);

                    //Logger.Info("Order" + JsonHelper.ToJson(Order));
                  

                    #region  咨询退单 咨询退单接受人为医生 DoctorNotice PatientReturn
                    Consultation evaluation = new Consultation();
                    evaluation.yaeherConsultation = res;
                    evaluation.refundManage = result;
                    evaluation.orderTradeRecords = result2;
                    Publishs Consultationpublishs = new Publishs()
                    {
                        TemplateCode = "DoctorReturn",
                        OperationType = "PatientReturn",  //    咨询评分
                        MessageRemark = yaeherConsultation.RefundRemarks,  // 退单
                        Publisher = "Patient",
                        PublishUrl = "Patient",
                        EventName = "患者退单",
                        EventCode = "Consultation",
                        BusinessID = res.Id.ToString(),
                        BusinessCode = res.ConsultNumber,
                        BusinessJSON = JsonHelper.ToJson(evaluation),
                        PublishedTime = res.CreatedOn,
                        PublishStatus = true,
                        CreatedBy = userid,
                        CreatedOn = DateTime.Now,
                        Secret = secret,
                    };
                    var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
                    var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
                    if (ConsultationJson != null)
                    {
                        Consultationpublishs.PublishStatus = true;
                    }
                    else
                    {
                        Consultationpublishs.PublishStatus = false;
                    }
                    Consultationpublishs.ServerClient = "Client";
                    var ConsultationResult = await _publishsService.CreatePublishs(Consultationpublishs);
                    #endregion


                    unitOfWork.Complete();
                }
                #region 发起微信支付退款流程
                if (!string.IsNullOrEmpty(refundordertrade.WXPayBillno))//过滤前期的未支付数据
                {
                    string outTradeNo = refundordertrade.WXPayBillno;
                    string outRefundNo = RefundNumber;
                    var totalFee = (int)(ordertrade.PayMoney * 100);//单位：分
                    int refundFee = totalFee;

                    //Logger.Info("outTradeNo:" + outTradeNo + "outRefundNo" + outRefundNo + "totalFee:" + totalFee + "refundFee:" + refundFee + "tencentparam" + JsonHelper.ToJson(tencentparam));

                    var refundpayresult = await tencentWXPay.RefundAsync(outTradeNo, outRefundNo, totalFee, refundFee, tencentparam);
                    //Logger.Info("refundpayresult:" + JsonHelper.ToJson(refundpayresult));
                    if (refundpayresult.code != "SUCCESS")
                    {
                        HangfireScheduleJob job = new HangfireScheduleJob();
                        RefundModel refundModel = new RefundModel();
                        refundModel.Secret = await CreateSecret();
                        refundModel.outTradeNo = outTradeNo;
                        refundModel.outRefundNo = outRefundNo;
                        refundModel.totalFee = totalFee;
                        refundModel.refundFee = refundFee;
                        refundModel.msg = "";
                        JobModel model = new JobModel();
                        model.CallbackUrl = Commons.AdminIp + "api/RefundAsync";
                        model.CallbackContent = JsonHelper.ToJson(refundModel);
                        job.Enqueue(model);
                       // return new ObjectResultModule("", 400, "退款支付失败,请联系管理员");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateRefundManage",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "CreateRefundManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            this.ObjectResultModule.Object = "";
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            return ObjectResultModule;
        }
        /// <summary>
        /// 订单退单管理 删除
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteRefundManage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteRefundManage([FromBody] RefundManage RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _refundManageService.RefundManageByID(RefundManageInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _refundManageService.DeleteRefundManage(query);

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
                OperExplain = "DeleteRefundManage",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "DeleteRefundManage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单退单管理 Page
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/RefundManagePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundManagePage([FromBody]RefundManageIn RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            //var User = _userManagerService.UserManager(userid);
            //// 判断当角色为医生角色时
            //if (!User.IsAdmin && User.IsDoctor)
            //{
            //    RefundManageInfo.AndAlso(t => t.DoctorID == User.YaeherDoctorInfo.Id);
            //}
            if (!string.IsNullOrEmpty(RefundManageInfo.KeyWord))
            {
                RefundManageInfo.AndAlso(t => t.DoctorName.Contains(RefundManageInfo.KeyWord) ||
                                              t.ConsultNumber.Contains(RefundManageInfo.KeyWord) ||
                                              t.OrderNumber.Contains(RefundManageInfo.KeyWord) ||
                                              t.ConsultantName.Contains(RefundManageInfo.KeyWord) ||
                                              t.PatientName.Contains(RefundManageInfo.KeyWord));
            }
            RefundManageInfo.AndAlso(t => !t.IsDelete);
            RefundManageInfo.RefundType = "patientreturn";
            var values = await _refundManageService.RefundManagePage(RefundManageInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new RefundManageOut(values, RefundManageInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "RefundManagePage",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "RefundManagePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单退单管理 List 
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/RefundManageList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundManageList([FromBody]RefundManageIn RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            RefundManageInfo.AndAlso(t => !t.IsDelete);
            var values = await _refundManageService.RefundManageList(RefundManageInfo);
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
                OperExplain = "RefundManageList",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "RefundManageList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单退单管理 Byid
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        [Route("api/RefundManageById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> RefundManageById([FromBody]RefundManageIn RefundManageInfo)
        {
            if (!Commons.CheckSecret(RefundManageInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _refundManageService.RefundManageByID(RefundManageInfo.Id);
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
                OperExplain = "RefundManageById",
                OperContent = JsonHelper.ToJson(RefundManageInfo),
                OperType = "RefundManageById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        #region 订单分配
        /// <summary>
        /// 订单分配 新增
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/CreateIncomeDevide")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateIncomeDevide([FromBody]  IncomeDevide IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var CreateIncomeDevide = new IncomeDevide()
            {
                ConsultNumber = IncomeDevideInfo.ConsultNumber,
                ConsultID = IncomeDevideInfo.ConsultID,
                DoctorID = IncomeDevideInfo.DoctorID,
                DoctorName = IncomeDevideInfo.DoctorName,
                OrderID = IncomeDevideInfo.OrderID,
                OrderNumber = IncomeDevideInfo.OrderNumber,
                OrderMoney = IncomeDevideInfo.OrderMoney,
                OrderCurrency = IncomeDevideInfo.OrderCurrency,
                DevideMoney = IncomeDevideInfo.DevideMoney,
                DevideRatio = IncomeDevideInfo.DevideRatio,
                CreatedBy = userid,
                CreatedOn = DateTime.Now,

            };
            var result = await _incomeDevideService.CreateIncomeDevide(CreateIncomeDevide);
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
                OperExplain = "CreateIncomeDevide",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "CreateIncomeDevide",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单分配 修改
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/UpdateIncomeDevide")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateIncomeDevide([FromBody]  IncomeDevide IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var UpdateIncomeDevide = await _incomeDevideService.IncomeDevideByID(IncomeDevideInfo.Id);
            if (UpdateIncomeDevide != null)
            {
                UpdateIncomeDevide.ConsultNumber = IncomeDevideInfo.ConsultNumber;
                UpdateIncomeDevide.ConsultID = IncomeDevideInfo.ConsultID;
                UpdateIncomeDevide.DoctorID = IncomeDevideInfo.DoctorID;
                UpdateIncomeDevide.DoctorName = IncomeDevideInfo.DoctorName;
                UpdateIncomeDevide.OrderID = IncomeDevideInfo.OrderID;
                UpdateIncomeDevide.OrderNumber = IncomeDevideInfo.OrderNumber;
                UpdateIncomeDevide.OrderMoney = IncomeDevideInfo.OrderMoney;
                UpdateIncomeDevide.OrderCurrency = IncomeDevideInfo.OrderCurrency;
                UpdateIncomeDevide.DevideMoney = IncomeDevideInfo.DevideMoney;
                UpdateIncomeDevide.DevideRatio = IncomeDevideInfo.DevideRatio;

                UpdateIncomeDevide.ModifyOn = DateTime.Now;
                UpdateIncomeDevide.ModifyBy = userid;

                var result = await _incomeDevideService.UpdateIncomeDevide(UpdateIncomeDevide);

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
                OperExplain = "UpdateIncomeDevide",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "UpdateIncomeDevide",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单分配 删除
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/DeleteIncomeDevide")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteIncomeDevide([FromBody]  IncomeDevide IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _incomeDevideService.IncomeDevideByID(IncomeDevideInfo.Id);
            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _incomeDevideService.DeleteIncomeDevide(query);

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
                OperExplain = "DeleteIncomeDevide",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "DeleteIncomeDevide",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单分配 Page
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeDevidePage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDevidePage([FromBody] IncomeDevideIn IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            IncomeDevideInfo.AndAlso(t => !t.IsDelete);
            var values = await _incomeDevideService.IncomeDevidePage(IncomeDevideInfo);
            if (values.Items.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new IncomeDevideOut(values, IncomeDevideInfo);
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "success";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "IncomeDevidePage",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "IncomeDevidePage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 订单分配 List 
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeDevideList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDevideList([FromBody]IncomeDevideIn IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            IncomeDevideInfo.AndAlso(t => !t.IsDelete);
            var values = await _incomeDevideService.IncomeDevideList(IncomeDevideInfo);
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
                OperExplain = "IncomeDevideList",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "IncomeDevideList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        /// <summary>
        /// 订单分配 Byid
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        [Route("api/IncomeDevideById")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> IncomeDevideById([FromBody]IncomeDevideIn IncomeDevideInfo)
        {
            if (!Commons.CheckSecret(IncomeDevideInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _incomeDevideService.IncomeDevideByID(IncomeDevideInfo.Id);
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
                OperExplain = "IncomeDevideById",
                OperContent = JsonHelper.ToJson(IncomeDevideInfo),
                OperType = "IncomeDevideById",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return ObjectResultModule;
        }
        #endregion

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ConsultationInfo"></param>
        /// <returns></returns>
        [Route("api/ConsultationToDoctor")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> ConsultationToDoctor([FromBody]ConsultationIn ConsultationInfo)
        {
            if (!Commons.CheckSecret(ConsultationInfo.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var secret = await CreateSecret();
            // 发布咨询 
            Consultation consultation = new Consultation();
            consultation.yaeherConsultation = await _consultationService.ConsultationByNumber(ConsultationInfo.ConsultNumber);                          // 咨询主表
            consultation.orderManage = await _orderManageService.OrderManageByconsultNumber(ConsultationInfo.ConsultNumber);                            // 订单管理表    
            consultation.orderTradeRecords = await _orderTradeRecordService.OrderTradeRecordByOrderNumber(consultation.orderManage.OrderNumber);        // 交易记录表

            TencentWXPay tencentWXPay = new TencentWXPay();
            var tencentparam = new SystemConfigs();
            tencentparam.AppID = "wx7f1de9c94cdc8f5e";
            tencentparam.AppSecret = "3e2bcfcd9239a356b6660e37c1d7a236";
            tencentparam.TenPayMchId = "1350249901";
            tencentparam.TenPayKey = "8FrTmTM1S9i1t0WhIl17AXyEa8L7NUrm";
            tencentparam.TenPayNotify = "http://patient.yaeherhealth.com/api/PayNotify";
            tencentparam.TenPayWxOpenNotify = "";
            var query = await tencentWXPay.OrderQueryAsync(consultation.orderTradeRecords.WXPayBillno, tencentparam);
            if (query.result_code != "SUCCESS")
            {
                return new ObjectResultModule(query,400,"订单付款状态异常！");
            }
            Publishs Consultationpublishs = new Publishs()
            {
                TemplateCode = "DoctorNotice",
                OperationType = "AddConsultation",  // 新增咨询
                Publisher = "Patient",
                PublishUrl = "Patient",
                EventName = "发布 新增咨询",
                EventCode = "Consultation",
                BusinessID = consultation.yaeherConsultation.Id.ToString(),
                BusinessCode = consultation.yaeherConsultation.ConsultNumber,
                BusinessJSON = JsonHelper.ToJson(consultation),
                PublishedTime = consultation.yaeherConsultation.CreatedOn,
                PublishStatus = true,
                CreatedBy = consultation.yaeherConsultation.CreatedBy,
                CreatedOn = DateTime.Now,
                Secret = secret,
            };
            var ConsultationParma = await this.PostResponseAsync(Commons.DoctorIp + "api/ConsultationManage/", JsonHelper.ToJson(Consultationpublishs));
            var ConsultationJson = JsonHelper.FromJson<APIResult<ResultModule<Publishs>>>(ConsultationParma);
            this.ObjectResultModule.StatusCode = 200;
            this.ObjectResultModule.Message = "success";
            return ObjectResultModule;
        }
    }
}