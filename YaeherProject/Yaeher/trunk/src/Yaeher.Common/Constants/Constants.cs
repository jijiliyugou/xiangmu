using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Yaeher.Common.Constants
{
    public class Constants
    {
        /// <summary>
        /// 加密字符串1
        /// </summary>
        public const string SECRET1 = "ds*^tgg)B5445";

        /// <summary>
        /// 加密字符串2
        /// </summary>
        public const string SECRET2 = "988Fddgb@^78956$&";
    }
    public static class Commons
    {
        /// <summary>
        /// 读取AppSettings
        /// </summary>
        public static string AppSettings(string key)
        {
            var result = ConfigurationManager.AppSettings[key] ?? String.Empty;
            return result;
        }
        /// <summary>
        /// 患者端
        /// </summary>
        public static string PatientIp = AppSettings("PatientIp");
        /// <summary>
        /// 新增用户默认密码
        /// </summary>
        public static string DefaultPassword = AppSettings("DefaultPassword");
        /// <summary>
        /// 微信公众号token
        /// </summary>
        public static string WXToken = AppSettings("WXToken");
        /// <summary>
        /// 医生端
        /// </summary>
        public static string DoctorIp = AppSettings("DoctorIp");
        /// <summary>
        /// admin端口
        /// </summary>
        public static string AdminIp = AppSettings("AdminIp");
        /// <summary>
        /// 获取WXAccessTokenUrl 
        /// </summary>
        public static string WXAccessTokenUrl = AppSettings("WXAccessTokenUrl");
        /// <summary>
        /// AMRHelperFile
        /// </summary>
        public static string AMRHelperFile = AppSettings("AMRHelperFile");
        /// <summary>
        /// WecharWeb
        /// </summary>
        public static string WecharWeb = AppSettings("WecharWeb");

        private static int _timeStampExpires = 3;
        /// <summary>
        /// 时间戳过期默认3分钟
        /// </summary>
        public static int TimeStampExpires
        {
            get
            {
                int.TryParse(AppSettings("TimeStampExpires"), out _timeStampExpires);
                return _timeStampExpires;
            }
        }
        #region 签名算法

        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取当前实例的日期和时间的计时周期数(Ticks表示一百纳秒的一千万分之一)
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - _unixEpoch;
            return (int)delta.TotalSeconds;
        }

        /// <summary>
        /// 日期格式转换为时间戳(UTC)格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertToTimeStep(DateTime time)
        {
            if (time.Kind == DateTimeKind.Unspecified || time.Kind == DateTimeKind.Local)
            { time = time.ToUniversalTime(); }
            var delta = time - _unixEpoch;
            return (int)delta.TotalSeconds;
        }
        /// <summary>
        /// 日期格式转换为时间戳(UTC)格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertToTimeStepOutOfCheck(DateTime time)
        {
            var delta = time - _unixEpoch;
            return (int)delta.TotalSeconds;
        }
        /// <summary>
        /// Ticks转换为Utc时间
        /// </summary>
        /// <param name="ticks">日期和时间的计时周期数</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(double ticks)
        {
            return _unixEpoch.AddSeconds(ticks);
        }

        /// <summary>
        /// 检查sign签名字符串是否有效
        /// </summary>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="parameters">时间戳+硬代码字符串1+随机字符串+硬代码字符串2+apptype</param>
        /// <returns></returns>
        public static bool CheckSignature(string sign, long timestamp, IDictionary<string, string> parameters)
        {
            if (string.IsNullOrWhiteSpace(sign))
                return false;
            if (timestamp == 0)
                return false;
            System.DateTime dtTime = ConvertToDateTime(timestamp);
            double minutes = DateTime.UtcNow.Subtract(dtTime).Minutes;
            if (minutes > Commons.TimeStampExpires)
                return false;

            var token = GetSignature(parameters);
            return token == sign;
        }

        ///// <summary>
        ///// 验证时间戳
        ///// </summary>
        ///// <param name="timeStamp">时间戳</param>
        ///// <param name="expiresMinutes">分钟数</param>
        ///// <returns>True:过期</returns>
        //public static bool CheckTimeStamp(long timeStamp, int expiresMinutes)
        //{
        //    System.DateTime dtTime = ConvertToDateTime(timeStamp);
        //    double minutes = DateTime.UtcNow.Subtract(dtTime).Minutes;
        //    if (minutes > expiresMinutes)
        //        return true;
        //    return false;
        //}

        /// <summary>
        /// 计算参数签名
        /// </summary>
        /// <param name="parameters">请求参数集，所有参数必须已转换为字符串类型</param>
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
        public static bool CheckSecret(string secret)
        {
            // return true;
            try
            {
                var tupleSecret = GetSecretParams(secret);
                switch (tupleSecret.Item5)
                {
                    case "wx":
                        return CheckSignature(tupleSecret.Item2, tupleSecret.Item3, tupleSecret.Item1);
                    case "pc":
                        return CheckSignature(tupleSecret.Item2, tupleSecret.Item3, tupleSecret.Item1);
                    case "system":
                        return CheckSignature(tupleSecret.Item2, tupleSecret.Item3, tupleSecret.Item1);
                    default:
                        return false;
                }
                //    if (tupleSecret.Item5 != "wx" && tupleSecret.Item5 != "pc" && tupleSecret.Item5 != "system") { return false; }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 封装加密参数字典
        /// </summary>
        /// <param name="token">客户端签名</param>
        /// <returns>sortedParams,sign,timeStamp,accountSource</returns>
        public static Tuple<IDictionary<string, string>, string, long, string, string> GetSecretParams(string token)
        {
            var secret = Base64Decode(token);
            string sign = string.Empty;
            string source = string.Empty;
            string apptype = string.Empty;
            long timeStamp = 0;
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>();
            if (secret.IndexOf("timestamp=", StringComparison.Ordinal) >= 0 &&
                secret.IndexOf("nonce=", StringComparison.Ordinal) >= 0 &&
                secret.IndexOf("apptype=", StringComparison.Ordinal) >= 0)
            {
                var secretParams = secret.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in secretParams)
                {
                    var param = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    switch (param[0])
                    {
                        case "signature":
                            sign = param[1];
                            break;
                        case "timestamp":
                            sortedParams.Add(param[0], param[1]);
                            long.TryParse(param[1], out timeStamp);
                            break;
                        case "source":
                            source = param[1];
                            break;
                        case "apptype":
                            apptype = param[1];
                            sortedParams.Add(param[0], param[1]);
                            break;
                        default:
                            sortedParams.Add(param[0], param[1]);
                            break;
                    }
                }
                sortedParams.Add("secret1", Constants.SECRET1);
                sortedParams.Add("secret2", Constants.SECRET2);
            }
            return Tuple.Create(sortedParams, sign, timeStamp, source, apptype);
        }

        #endregion

        #region 计算文件的Hash值

        /// <summary>
        /// 计算文件MD5 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ComputeMd5(string fileName)
        {
            string hashMd5;
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                //计算文件的MD5值
                System.Security.Cryptography.MD5 calculator = System.Security.Cryptography.MD5.Create();
                Byte[] buffer = calculator.ComputeHash(fs);
                calculator.Clear();
                //将字节数组转换成十六进制的字符串形式
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var t in buffer)
                {
                    stringBuilder.Append(t.ToString("x2"));
                }
                hashMd5 = stringBuilder.ToString();
            }
            return hashMd5;
        }

        #endregion

    }
}
