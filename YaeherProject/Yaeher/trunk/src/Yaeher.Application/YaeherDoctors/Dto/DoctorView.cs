using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.Doctor;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生信息展示
    /// </summary>
    public class DoctorView : YaeherDoctor
    {
        /// <summary>
        /// 接单数
        /// </summary>
        public int ReceiptNumBer { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsCollect { get; set; }
        /// <summary>
        /// 医生已评价订单数
        /// </summary>
        public int EvaluationCount { get; set; }
        /// <summary>
        /// 平均时长
        /// </summary>
        public string AverageTime { get; set; }
        /// <summary>
        /// 医生头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 医生背景大图
        /// </summary>
        public string UserImageFile { get; set; }
        /// <summary>
        /// 医生简介
        /// </summary>
        public string DoctorIntroduce { get; set; }
        /// <summary>
        /// 医生星级
        /// </summary>
        public string DoctorLevel { get; set; }
        /// <summary>
        /// 医生星级
        /// </summary>
        public double AverageEvaluate { get; set; }
        /// <summary>
        /// 医生标签
        /// </summary>
        public IList<LableManage> lableManages { get; set; }
        /// <summary>
        /// 提供服务
        /// </summary>
        public List<ServiceMoneyStateList> serviceMoneyLists { get; set; }
        /// <summary>
        /// 医生发布文章
        /// </summary>
        public IList<DoctorPaperView> doctorPapers { get; set; }

        /// <summary>
        /// 执业经历
        /// </summary>
        public IList<DoctorEmployment> DoctorEmployment { get; set; }
        /// <summary>
        /// 门诊排班
        /// </summary>
        public IList<DoctorScheduling> DoctorScheduling { get; set; }
        
    }
    /// <summary>
    /// 文章
    /// </summary>
    public class DoctorPaperView: DoctorPaper
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageFie { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string CheckStatus { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string DoctorPaperFrom { get; set; }
    }
}
