using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.Constants;
using Yaeher.SystemConfig;

namespace Yaeher.Common.TencentCustom
{
    public class SendWechaMessage
    {
        /// <summary>
        /// 将发送消息转为模板
        /// </summary>
        /// <param name="SendTemplateInfo"></param>
        /// <returns></returns>
        public string WecharContent(SendTemplate SendTemplateInfo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"touser\": \"" + SendTemplateInfo.ToUser + "\",");
            stringBuilder.Append("\"template_id\": \"" + SendTemplateInfo.TemplateId + "\",");
            stringBuilder.Append("\"url\": \"" + SendTemplateInfo.BackUrl + "\",");
            if (SendTemplateInfo.Appid != null)
            {
                stringBuilder.Append("\"miniprogram\":{");
                stringBuilder.Append("\"appid\": \"" + SendTemplateInfo.Appid + "\",");
                stringBuilder.Append("\"pagepath\": \"" + SendTemplateInfo.Pagepath + "\"");
                stringBuilder.Append("},");
            }
            stringBuilder.Append("\"data\":{ ");
            stringBuilder.Append("\"first\":{");
            stringBuilder.Append("\"value\": \"" + SendTemplateInfo.FirstMessage + "\",");
            stringBuilder.Append("\"color\": \"#173177\"");
            stringBuilder.Append("},");

            stringBuilder.Append("\"keyword1\":{");
            stringBuilder.Append("\"value\": \"" + SendTemplateInfo.Keyword1 + "\",");
            stringBuilder.Append("\"color\": \"#173177\"");
            stringBuilder.Append("},");

            stringBuilder.Append("\"keyword2\":{ ");
            stringBuilder.Append("\"value\": \"" + SendTemplateInfo.Keyword2 + "\",");
            stringBuilder.Append("\"color\": \"#173177\"");
            stringBuilder.Append("},");

            if (SendTemplateInfo.Keyword3 != null)
            {
                stringBuilder.Append("\"keyword3\":{");
                stringBuilder.Append("\"value\": \"" + SendTemplateInfo.Keyword3 + "\",");
                stringBuilder.Append("\"color\": \"#173177\"");
                stringBuilder.Append("},");
            }
            stringBuilder.Append("\"remark\":{ ");
            stringBuilder.Append("\"value\": \"" + SendTemplateInfo.MessageRemark + "\",");
            stringBuilder.Append("\"color\": \"#173177\"");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        /// <summary>
        ///  咨询者接受消息模板
        /// </summary>
        /// <param name="sendTemplate"></param>
        /// <returns></returns>
        public SendTemplate ConsultantWecharTemplate(WecharSendMessage wecharSendMessage,string Inquiry)
        {
            SendTemplate sendTemplate = new SendTemplate();
            sendTemplate.ToUser = wecharSendMessage.ToUser;
            sendTemplate.TemplateId = wecharSendMessage.TemplateId;
            sendTemplate.BackUrl = Commons.WecharWeb + "?link=record-detail&consultNumber=" + wecharSendMessage.ConsultNumber;
            sendTemplate.FirstMessage = wecharSendMessage.FirstMessage;
            YaeherConsultation yaeherConsultation = JsonHelper.FromJson<YaeherConsultation>(wecharSendMessage.ConsultJson);
            #region 2019-1-8 修改
            switch (wecharSendMessage.OperationType)
            {
                case "ReplayConsultant":  // 回复订单
                    sendTemplate.Keyword1 = "您提交的关于"+yaeherConsultation.IIInessType+"的咨询";
                    sendTemplate.Keyword2 = "请点击查看回复详情";
                    sendTemplate.Keyword3 = wecharSendMessage.DoctorName;
                    break;
                case "RemindInquiry":  //  追问提醒
                    sendTemplate.FirstMessage = "您的追问期限还剩"+Inquiry+"，请尽快追问";
                    sendTemplate.Keyword1 = "您提交的关于" + yaeherConsultation.IIInessType + "的咨询";
                    sendTemplate.Keyword2 = "追问过期时间为：" + yaeherConsultation.Completetime.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.Keyword3 = wecharSendMessage.DoctorName;
                    sendTemplate.MessageRemark = "请点击详情对医生进行追问";
                    break;
                case "RemindEvaluation":  //  评价提醒
                    sendTemplate.Keyword1 = yaeherConsultation.PatientName;
                    sendTemplate.Keyword2 = yaeherConsultation.DoctorName;
                    sendTemplate.MessageRemark = "请点击详情对医生进行评价";
                    break;
                case "Systemreturn":  //  系统退单
                    sendTemplate.Keyword1 = wecharSendMessage.DoctorName;
                    sendTemplate.Keyword2 = "医生未能及时回复";
                    sendTemplate.Keyword3 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.MessageRemark = "您可以继续选择其他医生咨询";
                    break;
                case "DoctorReturn":  // 医生退单
                    sendTemplate.Keyword1 = wecharSendMessage.DoctorName;
                    sendTemplate.Keyword2 = "医生主动退单";
                    sendTemplate.Keyword3 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.MessageRemark = "退单原因请查看详情";
                    break;

            }
            #endregion
            return sendTemplate;
        }

        /// <summary>
        /// 医生端接受消息模板
        /// </summary>
        /// <param name="wecharSendMessage"></param>
        /// <returns></returns>
        public SendTemplate DoctorWecharTemplate(WecharSendMessage wecharSendMessage,string EvaluateLevel,string WarningTime)
        {
            SendTemplate sendTemplate = new SendTemplate();
            sendTemplate.ToUser = wecharSendMessage.ToUser;
            sendTemplate.TemplateId = wecharSendMessage.TemplateId;
            sendTemplate.BackUrl = Commons.WecharWeb + "?link=order-detail&consultNumber=" + wecharSendMessage.ConsultNumber;
            sendTemplate.FirstMessage = wecharSendMessage.FirstMessage;
            YaeherConsultation yaeherConsultation = JsonHelper.FromJson<YaeherConsultation>(wecharSendMessage.ConsultJson);
            switch (wecharSendMessage.OperationType)
            {
                case "AddConsultation":  // 咨询者发起咨询 
                    sendTemplate.Keyword1 = wecharSendMessage.ConsultantName;
                    sendTemplate.Keyword2 = yaeherConsultation.CreatedOn.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.Keyword3 = "用户提交了一个关于"+yaeherConsultation.IIInessType+"问题的咨询";
                    sendTemplate.MessageRemark = "请您及时回复";
                    sendTemplate.MessageType = "Notice";  //发送短信
                    break;
                case "Inquiry":  // 咨询者追问
                    sendTemplate.Keyword1 = wecharSendMessage.ConsultantName;
                    sendTemplate.Keyword2 = yaeherConsultation.CreatedOn.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.Keyword3 =  "用户提交了一个关于"+yaeherConsultation.IIInessType+"问题的追问";
                    sendTemplate.MessageRemark = "请您及时回复";
                    sendTemplate.MessageType = "Question"; //发送短信
                    break;
                case "Evaluation":  //咨询评分
                    sendTemplate.BackUrl = Commons.WecharWeb + "evaluate-detail?typeEvaluate=3&consultNumber=" + wecharSendMessage.ConsultNumber+"&rShow=no";
                    sendTemplate.Keyword1 = wecharSendMessage.ConsultantName;
                    sendTemplate.Keyword2 = "评价：" + EvaluateLevel;
                    sendTemplate.MessageRemark =  "点击消息可查看评价详情";
                    break;
                case "WarningRemind":  //  咨询预警
                    sendTemplate.FirstMessage = "您有一个咨询超过"+WarningTime+"未处理";
                    sendTemplate.Keyword1 =  wecharSendMessage.ConsultantName +"用户提交了一个关于"+yaeherConsultation.IIInessType+"问题的咨询";
                    sendTemplate.Keyword2 = yaeherConsultation.CreatedOn.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.MessageRemark =  "请您及时回复";
                    break;
                case "PatientReturn":  //咨询退单
                    sendTemplate.Keyword1 = wecharSendMessage.ConsultantName;
                    sendTemplate.Keyword2 = "用户主动退单";
                    sendTemplate.Keyword3 = yaeherConsultation.RefundTime.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.MessageRemark = "温馨提示：及时回复可避免用户主动退单";
                    break;
                case "QualityReturn":  //质控退单
                    sendTemplate.Keyword1 = wecharSendMessage.ConsultantName;
                    sendTemplate.Keyword2 = "质控审核退单";
                    sendTemplate.Keyword3 = yaeherConsultation.RefundTime.ToString("yyyy-MM-dd HH:mm");
                    sendTemplate.MessageRemark = "温馨提示：严格遵守怡禾操作规范有助于减少质控退单";
                    break;
            }
            return sendTemplate;
        }
        /// <summary>
        /// 执行发送消息
        /// </summary>
        /// <param name="WecharContent"></param>
        /// <param name="systemToken"></param>
        /// <returns></returns>
        public async Task<TemplateModel> SendWecharMessage(string WecharContent,string systemToken)
        {
            TencentUserManage tencentUserManage = new TencentUserManage();
            var templateModel = await tencentUserManage.SendWeCharTemplate(WecharContent,systemToken);
            return templateModel;
        }
    }
}
