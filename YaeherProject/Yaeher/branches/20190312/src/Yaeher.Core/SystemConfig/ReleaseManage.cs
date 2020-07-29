using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 发布文章
    /// </summary>
    public class ReleaseManage : EntityBaseModule
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        [MaxLength(200)]
        public virtual string PaperTiltle { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public virtual string PaperContent { get; set; }
        /// <summary>
        /// 文章附件地址
        /// </summary>
        [MaxLength(200)]
        public virtual string PaperAddress { get; set; }
        /// <summary>
        /// 文章来源   公司 医生  个人 或者案例
        /// </summary>
        [MaxLength(20)]
        public virtual string PaperFrom { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(500)]
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 案例编号
        /// </summary>
        [MaxLength(30)]
        public virtual string ConsultNumber { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [MaxLength(20)]
        public virtual string CheckState { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(1000)]
        public virtual string CheckRemark { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime CheckTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual int Checker { get; set; }
        /// <summary>
        /// 阅读次数
        /// </summary>
        public virtual int ReadTotal { get; set; }
        /// <summary>
        /// 点赞次数
        /// </summary>
        public virtual int UpvoteTotal { get; set; }
        /// <summary>
        /// 转发次数
        /// </summary>
        public virtual int TransTotal { get; set; }
        /// <summary>
        /// 收藏次数
        /// </summary>
        public virtual int CollectTotal { get; set; }
        /// <summary>
        /// 文章图片
        /// </summary>
        [MaxLength(500)]
        public string ImageFie { get; set; }
        /// <summary>
        ///医生文章ID
        /// </summary>
        public int DoctorPaperID { get; set; }
    }
}
