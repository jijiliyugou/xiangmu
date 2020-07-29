using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Yaeher
{
    /// <summary>
    /// 附件输入实体
    /// </summary>
    public class AttachmentIn : ListParameters<AttachmentService>, IPagedResultRequest
    {
        /// <summary>
        /// ServiceID
        /// </summary>
        public int ServiceID { get; set; }
        /// <summary>
        /// 咨询单号
        /// </summary>
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceNumber { get; set; }
        /// <summary>
        /// 咨询ID
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 文件来源
        /// </summary>
        public string FileFrom { get; set; }
        /// <summary>
        /// 文件类型 
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// 咨询 consultation，回答 asnwer 等
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// image 图片，video视频，voice 音频
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// 身份证正反，或者执业证，资格证
        /// </summary>
        public string TypeDetail { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual double FileSize { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int CreatedBy { get; set; }
       
    }
    /// <summary>
    /// 
    /// </summary>
    public class AttachmentListIn 
    {
        /// <summary>
        /// 
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AttachmentIn> Attach { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class FileAccess
    {
        /// <summary>
        /// 
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 服务类型 consultation:咨询，consultationReply:回答，avatar:头像，backgroundimage :背景图
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string MediaType { get; set; }
    }
   
}
