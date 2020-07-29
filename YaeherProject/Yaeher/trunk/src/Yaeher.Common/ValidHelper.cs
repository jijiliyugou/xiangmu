using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace YaeherCommon
{
    public class Consts
    {
        /// <summary>
        /// 加密字符串1
        /// </summary>
       // public const string SECRET1 = "kdjjjsjieiewe";
        public const string SECRET1 = "ds*^tgg)B5445";
        /// <summary>
        /// 加密字符串1
        /// </summary>
        //public const string SECRET2 = "dn!@xcc";
        public const string SECRET2 = "988Fddgb@^78956$&";
    }
    public static class ValidHelper
    {
        /// <summary>
        /// 检查sign签名字符串是否有效
        /// </summary>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="parameters">时间戳+硬代码字符串1+随机字符串+硬代码字符串2+apptype</param>
        /// <returns></returns>
        public static bool CheckSignature(string sign, string timestamp, IDictionary<string, string> parameters)
        {
            var result = Convert.ToDouble(ConfigurationManager.AppSettings["Timestamp"]);
            if (string.IsNullOrWhiteSpace(sign))
                return false;
            if (string.IsNullOrEmpty(timestamp))
                return false;
            System.DateTime startTime = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
            DateTime dtTime = startTime.AddSeconds(Convert.ToDouble(timestamp));
            double seconds = DateTime.Now.Subtract(dtTime).TotalSeconds;
            if (seconds > result)
                return false;

            var token = GetSignature(parameters);
            return token == sign;
        }

        /// <summary>
        /// 计算参数签名
        /// </summary>
        /// <param name="params">请求参数集，所有参数必须已转换为字符串类型</param>
        /// <returns>签名</returns>
        public static string GetSignature(IDictionary<string, string> parameters)
        {
            // 先将参数以其参数名的字典序升序进行排序
            IDictionary<string, string> sortedParams;

            if (parameters == null || parameters.Count == 0)
                sortedParams = new SortedDictionary<string, string>();
            else
                sortedParams = new SortedDictionary<string, string>(parameters);

            IEnumerator<KeyValuePair<string, string>> iterator = sortedParams.GetEnumerator();

            // 遍历排序后的字典，将所有参数按"key=value"格式拼接在一起
            List<string> arguList = new List<string>();
            while (iterator.MoveNext())
            {
                string key = (iterator.Current.Key ?? "").Trim();
                string value = (iterator.Current.Value ?? "").Trim();
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    arguList.Add(key + "=" + value);
                }
            }
            string baseString = string.Join("&", arguList.ToArray());

            //// 使用MD5对待签名串求签，基于UTF-8格式
            //MD5 md5 = MD5.Create();
            //byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(baseString));

            //// 将MD5输出的二进制结果转换为小写的十六进制
            //StringBuilder result = new StringBuilder();
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    string hex = bytes[i].ToString("x");
            //    if (hex.Length == 1)
            //    {
            //        result.Append("0");
            //    }
            //    result.Append(hex);
            //}

            //return result.ToString();
            return GetStrSignature(baseString).ToLower();
        }

        public static string GetStrSignature(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        /// <summary>
        /// 封装加密参数字典
        /// </summary>
        /// <param name="secretModel"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetSecretParams(SecretModels secretModel)
        {
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>();
            sortedParams.Add("timestamp", secretModel.timestamp.ToString());
            sortedParams.Add("secret1", Consts.SECRET1);
            sortedParams.Add("nonce", secretModel.nonce);
            sortedParams.Add("secret2", Consts.SECRET2);
            sortedParams.Add("apptype", secretModel.apptype);
            return sortedParams;

        }


        #region UrlBase64转码
        /// <summary>
        /// urlbase64转码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Base64Code(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="codestr"></param>
        /// <returns></returns>
        public static string Base64Decode(string codestr)
        {
            byte[] outputb = Convert.FromBase64String(codestr);
            string orgStr = Encoding.UTF8.GetString(outputb);
            return orgStr;
        }

        #endregion

        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取当前实例的日期和时间的计时周期数(Ticks表示一百纳秒的一千万分之一)
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - _unixEpoch;
            return delta.Ticks;
        }

        /// <summary>
        /// Ticks转换为Utc时间
        /// </summary>
        /// <param name="ticks">日期和时间的计时周期数</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long ticks)
        {
            return _unixEpoch.AddTicks(ticks);
        }

        private static int _mailExpires = 60;
        /// <summary>
        /// 时间戳过期默认60分钟
        /// </summary>
        public static int MailExpires
        {
            get
            {
                int.TryParse(AppSettings("mailexpres"), out _mailExpires);
                return _mailExpires;
            }
        }

        /// <summary>
        /// API Host
        /// </summary>
        public static string ApiHost => AppSettings("ApiHost");

        /// <summary>
        /// 读取AppSettings
        /// </summary>
        public static string AppSettings(string key)
        {
            var result = ConfigurationManager.AppSettings[key] ?? String.Empty;
            return result;
        }

    }
    /// <summary>
    /// 自签名继承
    /// </summary>
    public class SecretModel
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 平台
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Secret { get; set; }
    }
    public class SecretModels
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce { get; set; }

        /// <summary>
        /// 客户端来源
        /// </summary>
        public string apptype { get; set; }
    }


    public class ServerTimeStampModels
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public double TimeStamp { get; set; }
    }

    public class DefaultApiResult
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        public bool ActionResult { get; set; }
        public int Code { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        public string Status { get; set; }
    }
}
