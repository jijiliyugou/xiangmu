using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// Ali呼叫中心
    /// </summary>
    public class AliCCC: EntityBaseModule
    {
        /// <summary>
        /// 系统规定参数，StartBack2BackCall
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 呼叫中心实例ID，背靠背代理所用电话号码需要归属于该呼叫中心实例。
        /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
        /// 联系流ID，此联系流用来辅助建立双方通话；该字段是扩展用，请留空。
        /// </summary>
        public string WorkflowId { get; set; }
        /// <summary>
        /// 背靠背代理所用电话号码，该电话号码需要归属于instanceId指定的呼叫中心实例;若不指定，则会随机选一个指定实例下可用于外呼的号码
        /// </summary>
        public string CallCenterNumber { get; set; }
        /// <summary>
        /// 主叫号码。
        /// </summary>
        public string Caller { get; set; }
        /// <summary>
        /// 被叫号码。
        /// </summary>
        public string Callee { get; set; }
        /// <summary>
        /// 返回值的类型，支持 JSON 与 XML。默认为 XML。(对于软电话SDK中所需要的返回值，需要为JSON格式)
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// API 版本号，为日期形式：YYYY-MM-DD，本版本对应为 2017-07-05。
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///请求的时间戳。日期格式按照 ISO8601 标准表示，并需要使用 UTC 时间。格式为： YYYY-MM-DDThh:mm:ssZ例如，2014-05-26T12:00:00Z（为北京时间 2014 年 5 月 26 日 20 点 0 分 0 秒）。
        /// </summary>
        public string Timestamp { get; set; }
        /// <summary>
        /// 	认证方式，当前为 BEARERTOKEN
        /// </summary>
        public string SignatureType { get; set; }
        /// <summary>
        /// 请求地域，当前为 cn-shanghai
        /// </summary>
        public string RegionId { get; set; }
        /// <summary>
        /// 唯一随机数，用于防止网络重放攻击。用户在不同请求间要使用不同的随机数值，长度为45位
        /// </summary>
        public string SignatureNonce { get; set; }
        /// <summary>
        /// 访问令牌，也就是 access_token
        /// </summary>
        public string BearerToken { get; set; }

        #region 获取通话记录
        /// <summary>
        /// 分页序号
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 获取的历史数据的起始时间。缺省为0，代表从当天的0时开始。
        /// </summary>
        public long StartTime { get; set; }
        /// <summary>
        /// 获取的历史数据的终止时间。缺省为0，代表截止到目前的时间。
        /// </summary>
        public long StopTime { get; set; }
        #endregion

    }
}
