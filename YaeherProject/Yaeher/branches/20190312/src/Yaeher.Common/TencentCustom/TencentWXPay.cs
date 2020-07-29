using Senparc.CO2NET;
using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Yaeher.SystemConfig;

namespace Yaeher.Common.TencentCustom
{
    public class TencentWXPay
    {
        /// <summary>
        /// 患者微信支付下订单
        /// </summary>
        /// <returns></returns>
        public async Task<TencentJSPayRequestModel> UnifiedorderAsync(bool sharing, string spbillCreateIp, YaeherConsultation consul, YaeherUser user, ServiceMoneyList product, SystemConfigs tencentparam)
        {
            TencentJSPayRequestModel jspay = new TencentJSPayRequestModel();
            try
            {
                TenPayV3Info tenPayV3Info = new TenPayV3Info(tencentparam.AppID, tencentparam.AppSecret, tencentparam.TenPayMchId, tencentparam.TenPayKey, tencentparam.TenPayNotify, tencentparam.TenPayWxOpenNotify);

                //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                //  var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                // var sjs =;//随机数
                var sjs = consul.ConsultNumber.Substring(3, consul.ConsultNumber.Length - 3) + TenPayV3Util.BuildRandomStr(6);
                var sp_billno = sjs;
                //   var sp_billno = string.Format("{0}{1}", sjs, user.Id);
                var shortbillno = sp_billno;

                TenPayInfo tenPayInfo = new TenPayInfo();
                //   var body = product == null ? "怡芽问诊" : product.DoctorName+"医生" + product.ServiceType;
                var body = "怡禾健康咨询";
                var price = product == null ? 100 : (int)(product.ServiceExpense * 100);//单位：分
                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();

                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(tenPayV3Info.AppId, tenPayV3Info.MchId, body, sp_billno, price, spbillCreateIp, tenPayV3Info.TenPayV3Notify, TenPayV3Type.JSAPI, user.WecharOpenID, tenPayV3Info.Key, nonceStr);
                //CreateWrite("C:\\回调.txt", "订单参数:" + JsonHelper.ToJson(xmlDataInfo));
                var result = await UnifiedorderAsync(xmlDataInfo, sharing ? "Y" : "N");//调用统一订单接口
                                                                                       //JsSdkUiPackage jsPackage = new JsSdkUiPackage(TenPayV3Info.AppId, timeStamp, nonceStr,);

                if (result.IsResultCodeSuccess())
                {
                    var package = string.Format("prepay_id={0}", result.prepay_id);
                    var paysign = TenPayV3.GetJsPaySign(tenPayV3Info.AppId, timeStamp, nonceStr, package, tenPayV3Info.Key);

                    jspay.product = product;
                    jspay.timeStamp = timeStamp;
                    jspay.nonceStr = nonceStr;
                    jspay.package = package;
                    jspay.paySign = paysign;
                    jspay.appid = tenPayV3Info.AppId;
                    jspay.sp_billno = shortbillno;
                    jspay.code = result.result_code;
                    jspay.msg = result.return_msg;
                }
                else
                {
                    jspay.code = result.result_code;
                    jspay.msg = result.return_msg;
                }
            }
            catch (Exception ex)
            {
                jspay.code = "FAIL";
                jspay.msg = ex.Message.ToString();
            }
            return jspay;
        }
        /// <summary>
        /// 【异步方法】统一支付接口
        /// 统一支付接口，可接受JSAPI/NATIVE/APP 下预支付订单，返回预支付订单号。NATIVE 支付返回二维码code_url。
        /// </summary>
        /// <param name="dataInfo">微信支付需要post的xml数据</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<UnifiedorderResult> UnifiedorderAsync(TenPayV3UnifiedorderRequestData dataInfo, string profit_sharing, int timeOut = Config.TIME_OUT)
        {
            var urlFormat = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            #region 设置RequestHandler
            //创建支付应答对象
            var PackageRequestHandler = new RequestHandler(null);
            //初始化
            PackageRequestHandler.Init();

            //设置package订单参数
            //以下设置顺序按照官方文档排序，方便维护：https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_1
            PackageRequestHandler.SetParameter("appid", dataInfo.AppId);                       //公众账号ID
            PackageRequestHandler.SetParameter("mch_id", dataInfo.MchId);                      //商户号
            PackageRequestHandler.SetParameterWhenNotNull("sub_appid", dataInfo.SubAppId);     //子商户公众账号ID
            PackageRequestHandler.SetParameterWhenNotNull("sub_mch_id", dataInfo.SubMchId);    //子商户号
            PackageRequestHandler.SetParameterWhenNotNull("device_info", dataInfo.DeviceInfo); //自定义参数
            PackageRequestHandler.SetParameter("nonce_str", dataInfo.NonceStr);                //随机字符串
            PackageRequestHandler.SetParameterWhenNotNull("sign_type", dataInfo.SignType);     //签名类型，默认为MD5
            PackageRequestHandler.SetParameter("body", dataInfo.Body);                         //商品信息
            PackageRequestHandler.SetParameterWhenNotNull("detail", dataInfo.Detail);          //商品详细列表
            PackageRequestHandler.SetParameterWhenNotNull("attach", dataInfo.Attach);          //附加数据
            PackageRequestHandler.SetParameter("out_trade_no", dataInfo.OutTradeNo);           //商家订单号
            PackageRequestHandler.SetParameterWhenNotNull("fee_type", dataInfo.FeeType);       //符合ISO 4217标准的三位字母代码，默认人民币：CNY
            PackageRequestHandler.SetParameter("total_fee", dataInfo.TotalFee.ToString());     //商品金额,以分为单位(money * 100).ToString()
            PackageRequestHandler.SetParameter("spbill_create_ip", dataInfo.SpbillCreateIP);   //用户的公网ip，不是商户服务器IP
            PackageRequestHandler.SetParameterWhenNotNull("time_start", dataInfo.TimeStart);   //订单生成时间
            PackageRequestHandler.SetParameterWhenNotNull("time_expire", dataInfo.TimeExpire);  //订单失效时间
            PackageRequestHandler.SetParameterWhenNotNull("goods_tag", dataInfo.GoodsTag);     //商品标记
            PackageRequestHandler.SetParameter("notify_url", dataInfo.NotifyUrl);              //接收财付通通知的URL
            PackageRequestHandler.SetParameter("trade_type", dataInfo.TradeType.ToString());   //交易类型
            PackageRequestHandler.SetParameterWhenNotNull("product_id", dataInfo.ProductId);   //trade_type=NATIVE时（即扫码支付），此参数必传。
            PackageRequestHandler.SetParameterWhenNotNull("limit_pay", dataInfo.LimitPay);     //上传此参数no_credit--可限制用户不能使用信用卡支付
            PackageRequestHandler.SetParameterWhenNotNull("openid", dataInfo.OpenId);                     //用户的openId，trade_type=JSAPI时（即公众号支付），此参数必传
            PackageRequestHandler.SetParameterWhenNotNull("sub_openid", dataInfo.SubOpenid);              //用户子标识
            PackageRequestHandler.SetParameterWhenNotNull("profit_sharing", profit_sharing);              //是否可以分账
            if (dataInfo.SceneInfo != null)
            {
                PackageRequestHandler.SetParameter("scene_info", dataInfo.SceneInfo.ToString());   //场景信息
            }

            var Sign = PackageRequestHandler.CreateMd5Sign("key", dataInfo.Key);

            PackageRequestHandler.SetParameter("sign", Sign);                              //签名
            #endregion

            var data = PackageRequestHandler.ParseXML();//获取XML
            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            MemoryStream ms = new MemoryStream();
            await ms.WriteAsync(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            var resultXml = await RequestUtility.HttpPostAsync(urlFormat, null, ms, timeOut: timeOut);
            return new UnifiedorderResult(resultXml);
        }

        public async Task<OrderQueryResult> OrderQueryAsync(string WXPayBillno, SystemConfigs tencentparam)
        {


            TenPayV3Info TenPayV3Info = new TenPayV3Info(tencentparam.AppID, tencentparam.AppSecret, tencentparam.TenPayMchId, tencentparam.TenPayKey, tencentparam.TenPayNotify, tencentparam.TenPayWxOpenNotify);
            string nonceStr = TenPayV3Util.GetNoncestr();
            TenPayV3OrderQueryRequestData querydata = new TenPayV3OrderQueryRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, "", nonceStr, WXPayBillno, TenPayV3Info.Key);
            var tencentpayresult = await TenPayV3.OrderQueryAsync(querydata);



            return tencentpayresult;
        }

        public async Task<SharingResultQuery> ProfitSharingQueryAsync(string WXPayBillno,string TransactionId, SystemConfigs tencentparam)
        {
            var nonce_str = TenPayV3Util.GetNoncestr();
            var requestHandler2 = new RequestHandler();
            requestHandler2.SetParameter("mch_id", tencentparam.TenPayMchId);
            requestHandler2.SetParameter("transaction_id", TransactionId);
            requestHandler2.SetParameter("out_order_no", WXPayBillno);
            requestHandler2.SetParameter("nonce_str", nonce_str);
            requestHandler2.SetParameter("sign_type", "HMAC-SHA256");
            ////HMAC-SHA256加密签名
            var sha256Sign = requestHandler2.CreateSha256Sign("key", tencentparam.TenPayKey);
            
            requestHandler2.SetParameter("sign", sha256Sign);
            

            string urlFormat = "https://api.mch.weixin.qq.com/pay/profitsharingquery";
            var data = requestHandler2.ParseXML();//获取XML
            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            MemoryStream ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            var resultXml = await RequestUtility.HttpPostAsync(urlFormat, null, ms);

            SharingResultQuery result = new SharingResultQuery();
            var _resultXml = XDocument.Parse(resultXml);
            result.return_code = GetXmlValue(_resultXml, "return_code");
            result.result_code = GetXmlValue(_resultXml, "result_code");
            result.err_code = GetXmlValue(_resultXml, "err_code");
            result.err_code_des = GetXmlValue(_resultXml, "err_code_des");
            result.mch_id = GetXmlValue(_resultXml, "mch_id");
            result.appid = GetXmlValue(_resultXml, "appid");
            result.nonce_str = GetXmlValue(_resultXml, "nonce_str");
            result.sign = GetXmlValue(_resultXml, "sign");
            result.status = GetXmlValue(_resultXml, "status");
            result.receivers = GetXmlValue(_resultXml, "receivers");
            return result;
        }
        public async Task<BaseResultModel> RefundAsync(string outTradeNo, string outRefundNo, int totalFee, int refundFee, SystemConfigs tencentparam, string msg = null)
        {
            BaseResultModel model = new BaseResultModel();
            try
            {
                TenPayV3Info TenPayV3Info = new TenPayV3Info(tencentparam.AppID, tencentparam.AppSecret, tencentparam.TenPayMchId, tencentparam.TenPayKey, tencentparam.TenPayNotify, tencentparam.TenPayWxOpenNotify);
                string nonceStr = TenPayV3Util.GetNoncestr();
                //string outTradeNo = Order.WXPayBillno;
                //string outRefundNo = result.RefundNumber;
                //var totalFee = (int)(ServiceResult.result.item.ServiceExpense * 100);//单位：分
                //var totalFee = totalFee;//单位：分
                //int refundFee = totalFee;
                string opUserId = TenPayV3Info.MchId;
                var notifyUrl = "http://admin/integraltel.com/api/RefundPayNotify";
                var dataInfo = new TenPayV3RefundRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, TenPayV3Info.Key,
                    null, nonceStr, null, outTradeNo, outRefundNo, totalFee, refundFee, opUserId, null, refundDescription: msg, notifyUrl: notifyUrl);
                var cert = @"C:\cert\apiclient_cert.p12";//根据自己的证书位置修改
                var password = TenPayV3Info.MchId;//默认为商户号，建议修改
                Thread.Sleep(50);
                var wxpayresult = await TenPayV3.RefundAsync(dataInfo, cert, password);
                Thread.Sleep(50);
                model.code = wxpayresult.result_code;
                model.msg = wxpayresult.return_msg;
            }
            catch (Exception ex)
            {
                model.code = "FAIL";
                model.msg = ex.Message.ToString();
            }
            return model;
        }

        public async Task<AddReceiverResult> ProfitSharingAddReceiver(receiver receiver, SystemConfigs tencentparam)
        {
            var nonce_str = TenPayV3Util.GetNoncestr();
            var requestHandler2 = new RequestHandler();
            requestHandler2.SetParameter("mch_id", tencentparam.TenPayMchId);
            requestHandler2.SetParameter("appid", tencentparam.AppID);
            requestHandler2.SetParameter("nonce_str", nonce_str);
            requestHandler2.SetParameter("sign_type", "HMAC-SHA256");
            requestHandler2.SetParameter("receiver", "{\"type\":\"" + receiver.type + "\",\"account\":\"" + receiver.account + "\",\"name\":\"" + receiver.name + "\"}");
            ////HMAC-SHA256加密签名
            var sha256Sign = requestHandler2.CreateSha256Sign("key", tencentparam.TenPayKey);
            requestHandler2.SetParameter("sign", sha256Sign);
            string urlFormat = "https://api.mch.weixin.qq.com/pay/profitsharingaddreceiver";
            var data = requestHandler2.ParseXML();//获取XML
            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            MemoryStream ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            var resultXml = await RequestUtility.HttpPostAsync(urlFormat, null, ms);

            AddReceiverResult result = new AddReceiverResult();
            var _resultXml = XDocument.Parse(resultXml);
            result.return_code = GetXmlValue(_resultXml, "return_code");
            result.result_code = GetXmlValue(_resultXml, "result_code");
            result.err_code = GetXmlValue(_resultXml, "err_code");
            result.err_code_des = GetXmlValue(_resultXml, "err_code_des");
            result.mch_id = GetXmlValue(_resultXml, "mch_id");
            result.appid = GetXmlValue(_resultXml, "appid");
            result.receiver = GetXmlValue(_resultXml, "receiver");
            result.nonce_str = GetXmlValue(_resultXml, "nonce_str");
            result.sign = GetXmlValue(_resultXml, "sign");
            return result;

        }
        public async Task<AddReceiverResult> ProfitSharingRemoveReceiver(receiver receiver, SystemConfigs tencentparam)
        {
            var nonce_str1 = TenPayV3Util.GetNoncestr();
            var requestHandler3 = new RequestHandler();
            requestHandler3.SetParameter("mch_id", tencentparam.TenPayMchId);
            requestHandler3.SetParameter("appid", tencentparam.AppID);
            requestHandler3.SetParameter("nonce_str", nonce_str1);

            requestHandler3.SetParameter("sign_type", "HMAC-SHA256");
            requestHandler3.SetParameter("receiver", "{\"type\":\"" + receiver.type + "\",\"account\":\"" + receiver.account + "\",\"name\":\"" + receiver.name + "\"}");
            ////HMAC-SHA256加密签名
            var sha256Sign1 = requestHandler3.CreateSha256Sign("key", tencentparam.TenPayKey);

            requestHandler3.SetParameter("sign", sha256Sign1);
            string urldelete = "https://api.mch.weixin.qq.com/pay/profitsharingremovereceiver";
            var data1 = requestHandler3.ParseXML();//获取XML
            var formDataBytes1 = data1 == null ? new byte[0] : Encoding.UTF8.GetBytes(data1);
            MemoryStream ms1 = new MemoryStream();
            ms1.Write(formDataBytes1, 0, formDataBytes1.Length);
            ms1.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            var resultXml1 = await RequestUtility.HttpPostAsync(urldelete, null, ms1);

            AddReceiverResult result = new AddReceiverResult();
            var _resultXml = XDocument.Parse(resultXml1);
            result.return_code = GetXmlValue(_resultXml, "return_code");
            result.result_code = GetXmlValue(_resultXml, "result_code");
            result.err_code = GetXmlValue(_resultXml, "err_code");
            result.err_code_des = GetXmlValue(_resultXml, "err_code_des");
            result.mch_id = GetXmlValue(_resultXml, "mch_id");
            result.appid = GetXmlValue(_resultXml, "appid");
            result.receiver = GetXmlValue(_resultXml, "receiver");
            result.nonce_str = GetXmlValue(_resultXml, "nonce_str");
            result.sign = GetXmlValue(_resultXml, "sign");
            return result;
        }

        public async Task<SharingResult> profitsharingfinish(SystemConfigs tencentparam,string amount, string transaction_id, string out_order_no)
        {
            var requestHandler2 = new RequestHandler();
            StringBuilder sb = new StringBuilder();
            requestHandler2.SetParameter("mch_id", tencentparam.TenPayMchId);
            requestHandler2.SetParameter("appid", tencentparam.AppID);
            var profitsharingnonce_str = TenPayV3Util.GetNoncestr();
            requestHandler2.SetParameter("nonce_str", profitsharingnonce_str);
            requestHandler2.SetParameter("sign_type", "HMAC-SHA256");
            requestHandler2.SetParameter("transaction_id", transaction_id);
            requestHandler2.SetParameter("out_order_no", out_order_no);
            requestHandler2.SetParameter("amount", amount);
            requestHandler2.SetParameter("description", "分账完成");
            ////HMAC-SHA256加密签名
            var sha256Sign = requestHandler2.CreateSha256Sign("key", tencentparam.TenPayKey);
            requestHandler2.SetParameter("sign", sha256Sign);
            var urlFormat = "https://api.mch.weixin.qq.com/secapi/pay/profitsharingfinish";

            var data = requestHandler2.ParseXML();//获取XML
            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            var ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            var cert = @"C:\cert\apiclient_cert.p12";//根据自己的证书位置修改
            var password = tencentparam.TenPayMchId;//默认为商户号，建议修改
            var resultXml = await ShareAsync(requestHandler2, urlFormat, cert, password);
            var result = new SharingResult();
            var _resultXml = XDocument.Parse(resultXml);
            result.return_code = GetXmlValue(_resultXml, "return_code");
            result.result_code = GetXmlValue(_resultXml, "result_code");
            result.err_code = GetXmlValue(_resultXml, "err_code");
            result.err_code_des = GetXmlValue(_resultXml, "err_code_des");
            result.return_msg = GetXmlValue(_resultXml, "return_msg");
            result.mch_id = GetXmlValue(_resultXml, "mch_id");
            result.appid = GetXmlValue(_resultXml, "appid");
            result.nonce_str = GetXmlValue(_resultXml, "nonce_str");
            result.sign = GetXmlValue(_resultXml, "sign");
            result.transaction_id = GetXmlValue(_resultXml, "transaction_id");
            result.out_order_no = GetXmlValue(_resultXml, "out_order_no");
            result.order_id = GetXmlValue(_resultXml, "order_id");
            return result;


        }

        public async Task<SharingResult> ProfitSharing(List<receivershare> receivers, SystemConfigs tencentparam, string transaction_id, string out_order_no)
        {
            var requestHandler2 = new RequestHandler();
            StringBuilder sb = new StringBuilder();
            requestHandler2.SetParameter("mch_id", tencentparam.TenPayMchId);
            requestHandler2.SetParameter("appid", tencentparam.AppID);
            var profitsharingnonce_str = TenPayV3Util.GetNoncestr();
            requestHandler2.SetParameter("nonce_str", profitsharingnonce_str);
            requestHandler2.SetParameter("sign_type", "HMAC-SHA256");
            requestHandler2.SetParameter("transaction_id", transaction_id);
            requestHandler2.SetParameter("out_order_no", out_order_no);
            sb.Append("[");
            foreach (var item in receivers)
            {
                sb.Append("{\"type\":\"" + item.type + "\",\"account\":\"" + item.account + "\",\"amount\": " + item.amount + " ,\"description\":\"订单结算\"},");

            }
            var receive = sb.ToString().TrimEnd(',');
            receive += "]";
            requestHandler2.SetParameter("receivers", receive);
            ////HMAC-SHA256加密签名
            var sha256Sign = requestHandler2.CreateSha256Sign("key", tencentparam.TenPayKey);
            requestHandler2.SetParameter("sign", sha256Sign);
            var urlFormat = "https://api.mch.weixin.qq.com/secapi/pay/profitsharing";

            var data = requestHandler2.ParseXML();//获取XML
            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            var ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
            var cert = @"C:\cert\apiclient_cert.p12";//根据自己的证书位置修改
            var password = tencentparam.TenPayMchId;//默认为商户号，建议修改
            var resultXml = await ShareAsync(requestHandler2, urlFormat, cert, password);

            var result = new SharingResult();
            var _resultXml = XDocument.Parse(resultXml);
            result.return_code = GetXmlValue(_resultXml, "return_code");
            result.result_code = GetXmlValue(_resultXml, "result_code");
            result.err_code = GetXmlValue(_resultXml, "err_code");
            result.err_code_des = GetXmlValue(_resultXml, "err_code_des");
            result.return_msg = GetXmlValue(_resultXml, "return_msg");
            result.mch_id = GetXmlValue(_resultXml, "mch_id");
            result.appid = GetXmlValue(_resultXml, "appid");
            result.nonce_str = GetXmlValue(_resultXml, "nonce_str");
            result.sign = GetXmlValue(_resultXml, "sign");
            result.transaction_id = GetXmlValue(_resultXml, "transaction_id");
            result.out_order_no = GetXmlValue(_resultXml, "out_order_no");
            result.order_id = GetXmlValue(_resultXml, "order_id");
            return result;
        }
        /// <summary>
        /// 【异步方法】发送接口
        /// </summary>
        /// <param name="PackageRequestHandler"></param>
        /// <param name="url"></param>
        /// <param name="cert">证书绝对路径，如@"F:\apiclient_cert.p12"</param>
        /// <param name="certPassword">证书密码</param>
        /// <returns></returns>
        public static async Task<string> ShareAsync(RequestHandler PackageRequestHandler, string url, string cert, string certPassword)
        {
            try
            {
                var data = PackageRequestHandler.ParseXML();

                //var urlFormat = "https://api.mch.weixin.qq.com/secapi/pay/refund";

                //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
                //string cert = cert;// @"F:\apiclient_cert.p12";
                //私钥（在安装证书时设置）
                string responseContent = await CertPostAsync(cert, certPassword, data, url);

                return responseContent;
                // var resxml = XDocument.Parse(responseContent);


                //  return new RefundResult(responseContent);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 【异步方法】带证书提交
        /// </summary>
        /// <param name="cert">证书绝对路径</param>
        /// <param name="certPassword">证书密码</param>
        /// <param name="data">数据</param>
        /// <param name="url">Url</param>
        /// <returns></returns>
        private static async Task<string> CertPostAsync(string cert, string certPassword, string data, string url, int timeOut = Config.TIME_OUT)
        {
            try
            {
                string password = certPassword;
                var dataBytes = Encoding.UTF8.GetBytes(data);
                using (MemoryStream ms = new MemoryStream(dataBytes))
                {
                    //调用证书
                    X509Certificate2 cer = new X509Certificate2(cert, certPassword, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

                    string responseContent = await RequestUtility.HttpPostAsync(
                        url,
                        postStream: ms,
                        cer: cer,
                        timeOut: timeOut);

                    return responseContent;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 获取Xml结果中对应节点的值
        /// </summary>
        /// <param name="_resultXml"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string GetXmlValue(XDocument _resultXml, string nodeName)
        {
            if (_resultXml == null || _resultXml.Element("xml") == null
                || _resultXml.Element("xml").Element(nodeName) == null)
            {
                return "";
            }
            return _resultXml.Element("xml").Element(nodeName).Value;
        }


    }
}
