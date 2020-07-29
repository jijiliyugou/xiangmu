using System;
using System.ComponentModel.DataAnnotations;

namespace Yaeher
{
    /// <summary>
    /// 问答发布
    /// </summary>
    public class QuestionRelease:EntityBaseModule
    {
        /// <summary>
        /// 问题描述
        /// </summary>
        [MaxLength(3000)]
        public virtual string DescriptionTiltle { get; set; }
        /// <summary>
        /// 问题回答
        /// </summary>
        [MaxLength(3000)]
        public virtual string Answer { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(100)]
        public virtual string Title { get; set; }
        /// <summary>
        /// 标题详情
        /// </summary>
        [MaxLength(100)]
        public virtual string TitleDetail { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorId { get; set; }
        /// <summary>
        /// 阅读量
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
        /// 审核人
        /// </summary>
        public virtual int Checker { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime CheckTime { get; set; }
    }
}
