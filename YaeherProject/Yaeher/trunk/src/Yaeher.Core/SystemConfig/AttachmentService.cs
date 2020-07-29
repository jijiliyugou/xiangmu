using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 附件表
    /// </summary>
    public class AttachmentService: EntityBaseModule
    {
        /// <summary>
        /// 业务ID  回答ID 追问的ID 或者其他附件ID
        /// </summary>
        public virtual int ServiceID { get; set; }
        
        /// <summary>
        /// 咨询ID
        /// </summary>
        public virtual int ConsultID { get; set; }
        /// <summary>
        /// 业务number  回答number 追问的number 或者其他附件number
        /// </summary>
        [MaxLength(50)]
        public virtual string ServiceNumber { get; set; }
        /// <summary>
        /// 咨询单号
        /// </summary>
        [MaxLength(30)]
        public virtual string ConsultNumber { get; set; }

        /// <summary>
        /// 文件来源 咨询 回答 追问
        /// </summary>
        [MaxLength(20)]
        public virtual string FileFrom { get; set; }

        /// <summary>
        /// 咨询JSon
        /// </summary>
        public virtual string ConsultJSon { get; set; }
       
        /// <summary>
        /// 文件类型  视频 图片
        /// </summary>
        [MaxLength(10)]
        public virtual string FileType { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [MaxLength(100)]
        public virtual string Filename { get; set; }
        /// <summary>
        /// 正面，反面，执业证，资格证
        /// </summary>
        [MaxLength(100)]
        public string TypeDetail { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        [MaxLength(500)]
        public virtual string FileAddress { get; set; }
        /// <summary>
        /// 文件排序
        /// </summary>
        public virtual int FileOrder { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual double FileSize { get; set; }
        /// <summary>
        /// 文件缩略地址
        /// </summary>
        [MaxLength(500)]
        public virtual string FileContentAddress { get; set; }
        /// <summary>
        /// 音频文件时长
        /// </summary>
        public virtual double FileTotalTime { get; set; }
    }
}
