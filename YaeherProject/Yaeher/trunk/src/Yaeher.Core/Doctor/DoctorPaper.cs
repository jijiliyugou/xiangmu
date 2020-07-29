using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 发布文章
    /// </summary>
   public class DoctorPaper:EntityBaseModule
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [MaxLength(100)]
        public string PaperTiltle { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string PaperContent { get; set; }
        /// <summary>
        /// 文章来源 
        /// 个人 或者案例
        /// </summary>
        [MaxLength(20)]
        public string PaperFrom { get; set; }

        /// <summary>
        /// 案例编号
        /// YaeherPatientConsultation表ConsultNumber
        /// </summary>
        [MaxLength(30)]
        public string ConsultNumber { get; set; }
        /// <summary>
        /// 案例Id
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 文章附件地址
        /// </summary>
        [MaxLength(200)]
        public string PaperAddress { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public int Checker { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [MaxLength(10)]
        public string CheckState { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(2000)]
        public string CheckRemark { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 文章图片
        /// </summary>
        [MaxLength(100)]
        public string ImageFie { get; set; }

    }
}
