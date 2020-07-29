
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Yaeher.Common.TencentCustom;

namespace Yaeher.Common.TencentCustom
{
    public class TencentPayModel
    {
        [Required]
        public string Secret { get; set; }
        /// <summary>
        /// ServiceMoneylistid
        /// </summary>

        public int productId { get; set; }
        /// <summary>
        /// 订单num
        /// </summary>
        [Required]
        public string ConsultNumber { get; set; }
    }
    public class PayNotifyModel
    {
        public string return_code { get; set; }

        public string return_msg { get; set; }
    }
    public class TencentJSPayRequestModel
    {
        public ServiceMoneyList product { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string package { get; set; }
        public string paySign { get; set; }
        public string appid { get; set; }
        public string sp_billno { get; set; }//商户订单号
        public string code { get; set; }//成功失败
        public string msg { get; set; }//成功失败信息
    }
    public class OrderQueryModel : BaseResultModel
    {
        public string totalmoney { get; set; }
        public string transaction_id { get; set; }
    }

    public class BaseResultModel
    {
        public string code { get; set; }//成功失败
        public string msg { get; set; }//成功失败信息
    }
    public class RefundModel
    {
        public string Secret { get; set; }
        public string outTradeNo { get; set; }
        public string outRefundNo { get; set; }
        public int totalFee { get; set; }
        public int refundFee { get; set; }
        public string msg { get; set; }
    }

    /// <summary>
    /// 医生首款账号新增结果
    /// </summary>
    public class AddReceiverResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string err_code_des { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string receiver { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }
    }
    /// <summary>
    /// 单次分账结果
    /// </summary>
    public class SharingResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string return_code { get; set; }

        public string return_msg { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string out_order_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string order_id { get; set; }
    }
    /// <summary>
    /// 分账过后查询结果
    /// </summary>
    public class SharingResultQuery : SharingResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string receivers { get; set; }
    }

    ///// <summary>
    ///// 退款申请接口
    ///// </summary>
    ///// <returns></returns>
    //public async Task<string> Refund(RefundModel model)
    //{
    //    string nonceStr = TenPayV3Util.GetNoncestr();

    //    string outTradeNo = model.BillNo;
    //    string outRefundNo = "OutRefunNo-" + DateTime.Now.Ticks;
    //    int totalFee = model.BillFee;
    //    int refundFee = totalFee;
    //    string opUserId = model.tenPayV3Info.MchId;
    //    var notifyUrl = "https://sdk.weixin.senparc.com/TenPayV3/RefundNotifyUrl";
    //    var dataInfo = new TenPayV3RefundRequestData(model.tenPayV3Info.AppId, model.tenPayV3Info.MchId, model.tenPayV3Info.Key,
    //        null, nonceStr, null, outTradeNo, outRefundNo, totalFee, refundFee, opUserId, null, notifyUrl: notifyUrl);
    //    var cert = @"D:\cert\apiclient_cert_SenparcRobot.p12";//根据自己的证书位置修改
    //    var password = model.tenPayV3Info.MchId;//默认为商户号，建议修改
    //    var result = TenPayV3.Refund(dataInfo, cert, password);
    //    return "ok";
    //    //return Content(string.Format("退款结果：{0} {1}。您可以刷新当前页面查看最新结果。", result.result_code, result.err_code_des));
    //    //return Json(result, JsonRequestBehavior.AllowGet);

    //    #region 原始方法

    //    //RequestHandler packageReqHandler = new RequestHandler(null);

    //    //设置package订单参数
    //    //packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		 //公众账号ID
    //    //packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);	     //商户号
    //    //packageReqHandler.SetParameter("out_trade_no", "124138540220170502163706139412"); //填入商家订单号
    //    ////packageReqHandler.SetParameter("out_refund_no", "");                //填入退款订单号
    //    //packageReqHandler.SetParameter("total_fee", "");                    //填入总金额
    //    //packageReqHandler.SetParameter("refund_fee", "100");                //填入退款金额
    //    //packageReqHandler.SetParameter("op_user_id", TenPayV3Info.MchId);   //操作员Id，默认就是商户号
    //    //packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
    //    //string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
    //    //packageReqHandler.SetParameter("sign", sign);	                    //签名
    //    ////退款需要post的数据
    //    //string data = packageReqHandler.ParseXML();

    //    ////退款接口地址
    //    //string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
    //    ////本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
    //    //string cert = @"D:\cert\apiclient_cert_SenparcRobot.p12";
    //    ////私钥（在安装证书时设置）
    //    //string password = TenPayV3Info.MchId;
    //    //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
    //    ////调用证书
    //    //X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

    //    //#region 发起post请求
    //    //HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
    //    //webrequest.ClientCertificates.Add(cer);
    //    //webrequest.Method = "post";

    //    //byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
    //    //webrequest.ContentLength = postdatabyte.Length;
    //    //Stream stream;
    //    //stream = webrequest.GetRequestStream();
    //    //stream.Write(postdatabyte, 0, postdatabyte.Length);
    //    //stream.Close();

    //    //HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
    //    //StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
    //    //string responseContent = streamReader.ReadToEnd();
    //    //#endregion

    //    //// var res = XDocument.Parse(responseContent);
    //    ////string openid = res.Element("xml").Element("out_refund_no").Value;
    //    //return Content("申请成功：<br>" + HttpUtility.RequestUtility.HtmlEncode(responseContent));

    //    #endregion

    //}
    /// <summary>
    /// 商品实体类
    /// </summary>
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ProductModel()
        {
        }

        public ProductModel(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }


        private static List<ProductModel> ProductList { get; set; }

        public static List<ProductModel> GetFakeProductList()
        {
            var list = ProductList ?? new List<ProductModel>()
            {
                new ProductModel(1,"产品1",(decimal)0.01),
                new ProductModel(2,"产品2",(decimal)2.00),
                new ProductModel(3,"产品3",(decimal)3.00),
                new ProductModel(4,"产品4",(decimal)4.00),
                new ProductModel(5,"捐赠1",(decimal)10.00),
                new ProductModel(6,"捐赠2",(decimal)50.00),
                new ProductModel(7,"捐赠3",(decimal)100.00),
                new ProductModel(8,"捐赠4",(decimal)500.00),
            };
            ProductList = ProductList ?? list;

            return list;
        }
    }
    public class TenPayInfo
    {
        public TenPayV3Info TenPayV3Info { get; set; }

        public TenPayV3UnifiedorderRequestData RequestData { get; set; }
        public string wxopenid { get; set; }

    }

    #region 微信分账
    public class CreateReceiver
    {
        public string mch_id { get; set; }
        public string appid { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string sign_type { get; set; }

        public receiver receiver { get; set; }
    }
    public class receiver
    {
        public string type { get; set; }
        public string account { get; set; }
        public string name { get; set; }
    }
    public class receivershare
    {
        public string type { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
    }
    public class PostResultModel
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
    }

    public class TencentTicletModel
    {
        public string Secret { get; set; }
        public string url { get; set; }
    }
    public class TencentTicletResponseModel
    {
        public string appId { get; set; }
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }
    #endregion
}
