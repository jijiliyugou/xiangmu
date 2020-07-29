using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemConfig
{
    /// <summary>
    /// 获取通话录音地址
    /// </summary>
    public class ALICCListCallDetail
    {
        /// <summary>
        /// 话务ID，全局唯一，对应于 ACC的ACID
        /// </summary>
        public string ContactId { get; set; }
        /// <summary>
        /// 电话开始时间，内呼从进入IVR开始，外呼从开始接通计算
        /// </summary>
        public long StartTime { get; set; }
        /// <summary>
        /// 通话持续时间，单位为秒
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 通话类型, 参考枚举类型 ContactType， Inbound(0) 内呼, Outbound(1) 外呼, Internal(2) 内部通话;
        /// </summary>
        public String ContactType { get; set; }
        /// <summary>
        /// 电话结束原因
        /// </summary>
        public String ContactDisposition { get; set; }
        /// <summary>
        /// 主叫号码
        /// </summary>
        public String CallingNumber { get; set; }
        /// <summary>
        /// 被叫号码
        /// </summary>
        public String CalledNumber { get; set; }
        /// <summary>
        /// 呼叫中心实例ID，背靠背代理所用电话号码需要归属于该呼叫中心实例。
        /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
        /// 录音文件名字
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 录音文件下载url，此url被两个引号包裹，例如： “”urlDetail “” ，需要您自行处理一下，此url有效期为1小时
        /// </summary>
        public string SignatureUrl { get; set; }
    }

    /// <summary>
    /// 通话记录主表
    /// </summary>
    public class CallDetailRecordInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public Page<CallDetailRecords> CallDetailRecords { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int HttpStatusCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
    }
    /// <summary>
    /// 通话记录Page
    /// </summary>
    public class CallDetailRecords
    {
        /// <summary>
        /// 对象总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页序号
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 对象列表
        /// </summary>
        public CallDetail List { get; set; }
    }

    /// <summary>
    /// 通话记录List
    /// </summary>
    public class CallDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CallDetailRecord> CallDetailRecord { get; set; }
    }
    /// <summary>
    /// 通话记录明细
    /// </summary>
    public class CallDetailRecord
    {
        /// <summary>
        /// 	电话开始时间
        /// </summary>
        public long StartTime { get; set; }
        /// <summary>
        /// 通话类型
        /// </summary>
        public String ContactType { get; set; }
        /// <summary>
        /// 电话结束原因
        /// </summary>
        public String ContactDisposition { get; set; }

        /// <summary>
        /// 被叫号码
        /// </summary>
        public String CalledNumber { get; set; }
        /// <summary>
        /// 坐席信息
        /// </summary>
        public RecordingInfo Recordings { get; set; }
        /// <summary>
        /// 呼叫中心实例ID
        /// </summary>
        public String InstanceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String ExtraAttr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Agents Agents { get; set; }

        /// <summary>
        /// 参与通话的座席，多个座席之间以逗号分隔。
        /// </summary>
        public String AgentNames { get; set; }
        /// <summary>
        /// 参与通话的座席所述的技能组，多个技能组以逗号分隔
        /// </summary>
        public String SkillGroupNames { get; set; }
        /// <summary>
        /// 主叫号码
        /// </summary>
        public String CallingNumber { get; set; }
        /// <summary>
        /// 通话持续时间
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 话务ID，全局唯一，对应于 ACC的ACID
        /// </summary>
        public String ContactId { get; set; }
    }

    /// <summary>
    /// 通话记录
    /// </summary>
    public class RecordingInfo
    {
        /// <summary>
        /// 座席ID
        /// </summary>
        public List<Recordings> Recording { get; set; }
    }
    /// <summary>
    /// 通话记录详细
    /// </summary>
    public class Recordings
    {
        /// <summary>
        /// 
        /// </summary>
        public String FileDescription { get; set; }
        /// <summary>
        /// 座席名称
        /// </summary>
        public String AgentName { get; set; }
        /// <summary>
        /// 录音文件名
        /// </summary>
        public String FileName { get; set; }
        /// <summary>
        /// 通话时长
        /// </summary>
        public String Duration { get; set; }
        /// <summary>
        /// 话务ID，全局唯一，对应于 ACC的ACID
        /// </summary>
        public String ContactId { get; set; }
        /// <summary>
        /// 通话开始时间
        /// </summary>
        public long StartTime { get; set; }
        /// <summary>
        /// 座席ID
        /// </summary>
        public String AgentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Agents Agents { get; set; }
    }

    /// <summary>
    /// 坐席记录
    /// </summary>
    public class Agents {
        /// <summary>
        /// 坐席信息
        /// </summary>
        public string [] CallDetailAgent { get; set; }
    }
    /// <summary>
    /// 坐席详细记录
    /// </summary>
    public class CallDetailAgent {

        //public string AgentName { get; set; }
        //public int RingTime { get; set; }
        //public string SkillGroupName { get; set; }
        //public string ContactId { get; set; }
        //public string AgentId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T>
    {
        /// <summary>
        /// 对象总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页序号
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CallDetail List { get; set; }
    }

    /// <summary>
    /// 获取下载文件路径
    /// </summary>
    public class MediaDownload{
        /// <summary>
        /// 文件路径
        /// </summary>
        public MediaDownloadParam MediaDownloadParam { get; set; }
        public string HttpStatusCode { get; set; }
        public string Success { get; set; }
        public string Code { get; set; }
    }
    /// <summary>
    /// 文件路径
    /// </summary>
    public class MediaDownloadParam
    {
        /// <summary>
        /// 录音文件下载url
        /// </summary>
        public string SignatureUrl { get; set; }
        /// <summary>
        /// 录音文件名
        /// </summary>
        public string FileName { get; set; }
    }

}
