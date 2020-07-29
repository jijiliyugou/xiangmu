using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Common.HangfireJob;
using Yaeher.Common.SendMsm;
using Yaeher.Common.TencentCustom;
using Yaeher.HangFire;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 微信消息发送记录
    /// </summary>
    public class WecharSendMessageService : IWecharSendMessageService
    {
        TencentToken tencentToken = new TencentToken();
        private readonly IRepository<WecharSendMessage> _repository;
        private readonly IRepository<YaeherUser> _YaeherUserrepository;
        private readonly IRepository<YaeherDoctor> _YaeherDoctorrepository;
        private readonly IRepository<YaeherConsultation> _YaeherConsultationrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<YaeherMessageTemplate> _YaeherMessageTemplaterepository;
        private readonly IRepository<SendMessageTemplate> _SendMessagerepository;
        //发送短信
        private readonly IRepository<YaeherMessageRemind> _MessageRemindrepository;
        private readonly IRepository<SystemToken> _SystemTokenrepository;
        private readonly ISystemTokenService _systemTokenService;
        private readonly IRepository<ConsultationEvaluation> _Evaluationrepository;
        private readonly IRepository<DoctorParaSet> _DoctorParaSetrepository;
        private readonly IRepository<YaeherOperList> _YaeherOperListrepository;
        private readonly IRepository<SystemParameter> _SystemParameterrepository;
        private readonly IRepository<HangFireJob> _HangFireJobrepository;
       /// <summary>
       /// 微信消息发送记录 构造函数
       /// </summary>
       /// <param name="repository"></param>
       /// <param name="YaeherUserrepository"></param>
       /// <param name="YaeherConsultationrepository"></param>
       /// <param name="YaeherMessageTemplaterepository"></param>
       /// <param name="YaeherDoctorrepository"></param>
       /// <param name="SendMessagerepository"></param>
       /// <param name="MessageRemindrepository"></param>
       /// <param name="unitOfWorkManager"></param>
       /// <param name="SystemTokenrepository"></param>
       /// <param name="systemTokenService"></param>
       /// <param name="Evaluationrepository"></param>
       /// <param name="DoctorParaSetrepository"></param>
       /// <param name="YaeherOperListrepository"></param>
       /// <param name="HangFireJobrepository"></param>
       /// <param name="SystemParameterrepository"></param>
        public WecharSendMessageService(IRepository<WecharSendMessage> repository, 
                                        IRepository<YaeherUser> YaeherUserrepository,
                                        IRepository<YaeherConsultation> YaeherConsultationrepository,
                                        IRepository<YaeherMessageTemplate> YaeherMessageTemplaterepository,
                                        IRepository<YaeherDoctor> YaeherDoctorrepository,
                                        IRepository<SendMessageTemplate> SendMessagerepository,
                                        IRepository<YaeherMessageRemind> MessageRemindrepository,
                                        IUnitOfWorkManager unitOfWorkManager,
                                        IRepository<SystemToken> SystemTokenrepository,
                                        ISystemTokenService systemTokenService,
                                        IRepository<ConsultationEvaluation> Evaluationrepository,
                                        IRepository<DoctorParaSet> DoctorParaSetrepository,
                                        IRepository<YaeherOperList> YaeherOperListrepository,
                                        IRepository<HangFireJob> HangFireJobrepository,
                                        IRepository<SystemParameter> SystemParameterrepository)
        {
            _repository = repository;
            _YaeherUserrepository = YaeherUserrepository;
            _YaeherConsultationrepository = YaeherConsultationrepository;
            _YaeherMessageTemplaterepository = YaeherMessageTemplaterepository;
            _YaeherDoctorrepository = YaeherDoctorrepository;
            _SendMessagerepository = SendMessagerepository;
            _MessageRemindrepository = MessageRemindrepository;
            _unitOfWorkManager = unitOfWorkManager;
            _SystemTokenrepository = SystemTokenrepository;
            _systemTokenService = systemTokenService;
            _Evaluationrepository = Evaluationrepository;
            _DoctorParaSetrepository = DoctorParaSetrepository;
            _YaeherOperListrepository = YaeherOperListrepository;
            _HangFireJobrepository = HangFireJobrepository;
            _SystemParameterrepository = SystemParameterrepository;
        }
        /// <summary>
        /// 微信消息发送记录 List
        /// </summary>
        /// <param name="WecharSendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<WecharSendMessage>> WecharSendMessageList(WecharSendMessageIn WecharSendMessageInfo)
        {
            var WecharSendMessages = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(WecharSendMessageInfo.Expression);
            return await WecharSendMessages.ToListAsync();
        }

        /// <summary>
        /// 微信消息发送记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<WecharSendMessage> WecharSendMessageByID(int Id)
        {
            var WecharSendMessages = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return WecharSendMessages;
        }
        /// <summary>
        /// 微信消息发送记录 page
        /// </summary>
        /// <param name="WecharSendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<WecharSendMessage>> WecharSendMessagePage(WecharSendMessageIn WecharSendMessageInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(WecharSendMessageInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / WecharSendMessageInfo.MaxResultCount;
            var WecharSendMessageList = await query.PageBy(WecharSendMessageInfo.SkipTotal, WecharSendMessageInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<WecharSendMessage>(tasksCount, WecharSendMessageList.MapTo<List<WecharSendMessage>>());
        }
        /// <summary>
        /// 新建 微信消息发送记录
        /// </summary>
        /// <param name="sendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<WecharSendMessage> CreateWecharSendMessage(SendMessageInfo sendMessageInfo)
        {
            var YaeherConsultationInfo =await _YaeherConsultationrepository.FirstOrDefaultAsync(a => a.ConsultNumber == sendMessageInfo.ConsultNumber);
            // 咨询人
            var ConsultantUserInfo =await _YaeherUserrepository.FirstOrDefaultAsync(a => a.Id == YaeherConsultationInfo.ConsultantID);
            var DoctorInfo =await _YaeherDoctorrepository.FirstOrDefaultAsync(a => a.Id == YaeherConsultationInfo.DoctorID);
            // 医生
            var DoctorUserInfo =await _YaeherUserrepository.FirstOrDefaultAsync(a => a.Id == DoctorInfo.UserID);
            // 查询微信模板ID
            WecharSendMessage wecharSendMessage = new WecharSendMessage();
            // 查询当前模板ID
            var YaeherMessageInfo =await _YaeherMessageTemplaterepository.FirstOrDefaultAsync(a => a.TemplateCode == sendMessageInfo.TemplateCode);
            wecharSendMessage.TemplateId = YaeherMessageInfo.TemplateId;
            // 是否自动发送
            bool SendState =bool.Parse(_SystemParameterrepository.GetAll().Where(a => a.SystemCode == "WecharSendState").FirstOrDefault().Code);
            // 查询消息模板内容
            var SendMessageList= _SendMessagerepository.GetAll().Where(a => a.TemplateCode == sendMessageInfo.TemplateCode && a.OperationType == sendMessageInfo.OperationType);
            try
            {
                if (SendMessageList.Count() > 0)
                {
                    SendWechaMessage sendWechaMessage = new SendWechaMessage();
                    SendMsmHelper sendMsmHelper = new SendMsmHelper();
                    List<WecharSendMessage> WecharSendMessageList = new List<WecharSendMessage>();
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var IsDoctor = false;
                        string MessageType = string.Empty;
                        foreach (var SendMessageInfo in SendMessageList)
                        {
                            WecharSendMessage wecharMessage = new WecharSendMessage();
                            wecharMessage.ConsultNumber = sendMessageInfo.ConsultNumber;
                            wecharMessage.TemplateCode = sendMessageInfo.TemplateCode;
                            wecharMessage.OperationType = sendMessageInfo.OperationType;
                            wecharMessage.ConsultantName = YaeherConsultationInfo.PatientName;      // 修改为患者用户名
                            wecharMessage.DoctorName = YaeherConsultationInfo.DoctorName;
                            wecharMessage.ConsultJson = JsonHelper.ToJson(YaeherConsultationInfo);
                            wecharMessage.BackUrl = SendMessageInfo.BackUrl;
                            wecharMessage.FirstMessage = SendMessageInfo.FirstMessage;   // 查询标语
                            wecharMessage.Keyword1 = SendMessageInfo.Keyword1;           // 称呼人
                            wecharMessage.Keyword2 = SendMessageInfo.Keyword2;           // 时间
                            wecharMessage.Keyword3 = SendMessageInfo.Keyword3;           // 内容
                            wecharMessage.MessageRemark =SendMessageInfo.MessageRemark;  // 备注
                            wecharMessage.TemplateId = YaeherMessageInfo.TemplateId;
                            // 将对应信息转为发送模板
                            SendTemplate WecharTemplate = new SendTemplate();
                            switch (SendMessageInfo.Recipient)
                            {
                                case "Patient":  // 接受人为咨询者
                                    wecharMessage.ToUser = ConsultantUserInfo.WecharOpenID;
                                    WecharTemplate = sendWechaMessage.ConsultantWecharTemplate(wecharMessage,sendMessageInfo.Inquiry);     // 赋值咨询者信息
                                    break;
                                case "Doctor":  //  接受人为医生
                                    wecharMessage.ToUser = DoctorUserInfo.WecharOpenID;
                                    IsDoctor = true;
                                    WecharTemplate = sendWechaMessage.DoctorWecharTemplate(wecharMessage,sendMessageInfo.EvaluateLevel,sendMessageInfo.WarningTime);  // 赋值医生信息
                                    MessageType = WecharTemplate.MessageType;
                                    break;
                            }
                            // 将对应信息转为发送实际内容
                            wecharMessage.WecharData = sendWechaMessage.WecharContent(WecharTemplate);
                            #region
                            // 执行微信消息发送
                            TemplateModel templateModel = new TemplateModel();
                            if (SendState)  // 是否实时发送
                            {
                                var TokenInfo = _systemTokenService.SystemTokenList("Wechar").Result;
                                templateModel = sendWechaMessage.SendWecharMessage(wecharMessage.WecharData, TokenInfo.access_token).Result;
                            }
                            if(templateModel.errcode =="error"||!SendState)
                            {
                                #region 增加重试机制
                                HangFireJob WecharhangFireJob = new HangFireJob();
                                WecharhangFireJob.JobName = "微信定时服务";
                                WecharhangFireJob.JobCode = "WechaMessageSend";
                                WecharhangFireJob.BusinessID = Commons.GetCurrentTimeStepNumber();  //int.Parse(new RandomCode().GenerateCheckCodeNum(5));
                                WecharhangFireJob.BusinessCode = sendMessageInfo.ConsultNumber;
                                Random rd = new Random();
                                int Seconds = rd.Next(1, 5);
                                WecharhangFireJob.JobRunTime = DateTime.Now.AddSeconds(Seconds);  // 随机时间
                                WecharhangFireJob.JobSates = "Open";
                                WecharhangFireJob.CallbackUrl = Commons.AdminIp + "api/SendWechar/";
                                WecharhangFireJob.JobParameter = wecharMessage.WecharData;  // 将需要发送的内容整理好
                                
                                HangfireScheduleJob job = new HangfireScheduleJob();
                                JobModel model = new JobModel();
                                model.CallbackUrl = WecharhangFireJob.CallbackUrl;//回调URL
                                model.queues = "adminqueue";
                                model.CallbackContent = JsonHelper.ToJson(WecharhangFireJob);//回调参数
                                model.Timespan = WecharhangFireJob.JobRunTime;   //运行时间
                                var returnjobid = job.Schedule(model);
                                if (returnjobid.IndexOf("error") < 0)
                                {
                                    WecharhangFireJob.JobRunID = returnjobid;
                                }
                                WecharhangFireJob =await _HangFireJobrepository.InsertAsync(WecharhangFireJob);
                                templateModel.msgid = WecharhangFireJob.BusinessID.ToString();
                                templateModel.errmsg = "Undo";
                                #endregion

                                if (templateModel.errcode == "error")
                                {
                                    #region 将错误日志存起来
                                    YaeherOperList yaeherOperList = new YaeherOperList();
                                    yaeherOperList.CreatedOn = DateTime.Now;
                                    yaeherOperList.OperExplain = JsonHelper.ToJson(sendMessageInfo);
                                    yaeherOperList.OperContent = "error:" + templateModel.errmsg;
                                    yaeherOperList.OperType = "发送消息异常测试备用：" + sendMessageInfo.OperationType;
                                    await _YaeherOperListrepository.InsertAsync(yaeherOperList);
                                    #endregion
                                }
                            }
                            wecharMessage.MsgID = templateModel.msgid;
                            wecharMessage.Status = templateModel.errmsg;
                            wecharMessage.MsgType = wecharMessage.MsgType;
                            WecharSendMessageList.Add(wecharMessage);
                            #endregion
                        }
                        #region 发送短信
                        if (IsDoctor)
                        {
                            YaeherSendMsm yaeherSendMsm = new YaeherSendMsm();
                            yaeherSendMsm.PhoneNumbers = DoctorUserInfo.PhoneNumber==null? DoctorInfo .PhoneNumber : DoctorUserInfo.PhoneNumber;  // 医生电话
                            yaeherSendMsm.MessageType = MessageType;  // 短信类型
                            yaeherSendMsm.TemplateParam = "{\"remark\":\"" + sendMessageInfo.ConsultNumber + "\"}";  // 提示短信内容
                            // 发送短信 并存储记录
                            if (yaeherSendMsm.PhoneNumbers != null&& yaeherSendMsm.MessageType !=null)
                            {
                                var SendMessage = sendMsmHelper.SendMsm(yaeherSendMsm);  // 发送短信
                                YaeherMessageRemind MessageRemind = new YaeherMessageRemind();
                                MessageRemind.UserID = 0;
                                MessageRemind.UserName = "System";
                                MessageRemind.PhoneNumber = yaeherSendMsm.PhoneNumbers;
                                MessageRemind.MessageType = yaeherSendMsm.MessageType;
                                MessageRemind.Message = SendMessage.ToString();  //将发送短信消息存起来
                                MessageRemind.VerificationCode = "";
                                MessageRemind.Message = yaeherSendMsm.TemplateParam;
                                MessageRemind.CreatedBy = 0;
                                MessageRemind.CreatedOn = DateTime.Now;
                                MessageRemind.EffectiveLength = 0;
                                MessageRemind.EffectiveTime = DateTime.Now;
                                 await _MessageRemindrepository.InsertAsync(MessageRemind);
                            }
                        }
                        #endregion
                        #region 将发微信的消息记录
                        if(WecharSendMessageList.Count>0)
                        {
                            foreach (var wecharMessage in WecharSendMessageList)
                            {
                                await _repository.InsertAsync(wecharMessage);
                            }
                        }
                        #endregion
                        #region 增加有效用户访问记录 暂时关掉获取有效访问量
                        //YaeherOperList yaeherOperList = new YaeherOperList();
                        //yaeherOperList.CreatedOn = DateTime.Now;
                        //yaeherOperList.OperExplain = "ConsultNumber："+ sendMessageInfo.ConsultNumber;
                        //yaeherOperList.OperContent = JsonHelper.ToJson(ConsultantUserInfo);
                        //yaeherOperList.OperType = "发送消息成功";
                        //await _YaeherOperListrepository.InsertAsync(yaeherOperList);
                        #endregion
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                #region 增加有效用户访问记录
                YaeherOperList yaeherOperList = new YaeherOperList();
                yaeherOperList.CreatedOn = DateTime.Now;
                yaeherOperList.OperExplain =JsonHelper.ToJson(sendMessageInfo);
                yaeherOperList.OperContent ="error:" + ex.ToString();
                yaeherOperList.OperType = "发送消息异常："+sendMessageInfo.OperationType;
                await _YaeherOperListrepository.InsertAsync(yaeherOperList);
                #endregion
            }
            return wecharSendMessage;
        }


        /// <summary>
        /// 微信消息发送记录ByNumber
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <param name="MsgID"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<WecharSendMessage> WecharSendMessageByNumber(string ConsultNumber,string MsgID )
        {
            var WecharSendMessages = await _repository.FirstOrDefaultAsync(t => t.ConsultNumber == ConsultNumber && t.MsgID==MsgID && t.Status=="Undo" && !t.IsDelete);
            return WecharSendMessages;
        }

        /// <summary>
        /// 更新 微信消息发送记录
        /// </summary>
        /// <param name="WecharSendMessageInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<WecharSendMessage> UpdateWecharSendMessage(WecharSendMessage WecharSendMessageInfo)
        {
            return await _repository.UpdateAsync(WecharSendMessageInfo);
        }
    }
}
