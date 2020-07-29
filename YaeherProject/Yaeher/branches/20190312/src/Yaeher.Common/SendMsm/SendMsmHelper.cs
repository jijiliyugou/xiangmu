using System;
using System.Collections.Generic;
using System.Text;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Aliyun.Net.SDK.Core;
using Aliyun.Net.SDK.Core.Exceptions;
using Aliyun.Net.SDK.Core.Profile;
using Yaeher.SystemConfig;

namespace Yaeher.Common.SendMsm
{
    public  class SendMsmHelper
    {
        public String product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
        public String domain = "dysmsapi.aliyuncs.com";//阿里短信API产品域名（接口地址固定，无需修改）
        public String accessKeyId = "LTAIGlqbG8qsUiSl";//阿里短信accessKeyId，
        public String accessKeySecret = "hNHBRsNbOCbyyK0XVyVnj64Xv9OwHN";//阿里短信accessKeySecret，

        /// <summary>
        /// 发送短信
        /// </summary>
        public String SendMsm(YaeherSendMsm sendMsmInfo)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest ();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
                request.PhoneNumbers = sendMsmInfo.PhoneNumbers;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "深圳怡禾健康管理有限公司";
                //必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
                request.TemplateCode = MsnTemplateCode(sendMsmInfo.MessageType);
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = sendMsmInfo.TemplateParam;
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = sendMsmInfo.OutId;
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                return sendSmsResponse.Message;
            }
            catch (ServerException e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// 查询发送记录
        /// </summary>
        /// <param name="bizId"></param>
        /// <returns></returns>
        public QuerySendDetailsResponse querySendDetails(YaeherSendMsm sendMsmInfo)
        {
            //初始化acsClient,暂不支持region化
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            //组装请求对象
            QuerySendDetailsRequest request = new QuerySendDetailsRequest();
            //必填-号码
            request.PhoneNumber = sendMsmInfo.PhoneNumbers;
            //必填-发送日期 支持30天内记录查询，格式yyyyMMdd       
            request.SendDate = DateTime.Now.ToString("yyyyMMdd");
            //必填-页大小
            request.PageSize = 10;
            //必填-当前页码从1开始计数
            request.CurrentPage = 1;
            QuerySendDetailsResponse querySendDetailsResponse = null;
            try
            {
                querySendDetailsResponse = acsClient.GetAcsResponse(request);
            }
            catch (ServerException e)
            {
                throw e;
            }
            catch (ClientException e)
            {
                throw e;
            }
            return querySendDetailsResponse;
        }


        /// <summary>
        /// 短信类型
        /// </summary>
        /// <returns></returns>
        public string MsnTemplateCode(string MessageType) {
            string TemplateCode = "";
            switch (MessageType)
            {
                case "Verification":   // 验证
                    TemplateCode= "SMS_150737840";
                    break;
                //case "Notice":         // 咨询提交通知 老模板
                //    TemplateCode = "SMS_152206389";
                //    break;
                case "Notice":         // 咨询提醒
                    TemplateCode = "SMS_154594190";
                    break;
                case "Question":         // 咨询追问
                    TemplateCode = "SMS_154588931";
                    break;
                case "Authentication": // 认证 
                    TemplateCode = "SMS_152210126";
                    break;
                case "ChangePassword":  // 修改密码
                    TemplateCode = "SMS_146440024";
                    break;
                case "Return":  // 咨询退单
                    TemplateCode = "SMS_152286144";
                    break;
                case "Complete":  // 咨询完成
                    TemplateCode = "SMS_152281917";
                    break;
                case "QualityReturn":  // 质控退单
                    TemplateCode = "SMS_152281965";
                    break;
            }
            return TemplateCode;
        }
    }
}
