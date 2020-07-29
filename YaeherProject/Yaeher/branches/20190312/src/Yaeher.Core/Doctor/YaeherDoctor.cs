using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 医生基本信息
    /// </summary>
    public class YaeherDoctor : EntityBaseModule
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        [Description("医生姓名")]
        [MaxLength(500)]
        public string DoctorName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 医生住址
        /// </summary>
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 工作医院
        /// </summary>
        [MaxLength(200)]
        public string HospitalName { get; set; }
        /// <summary>
        /// 所在科室
        /// </summary>
        [MaxLength(20)]
        public string Department { get; set; }
        /// <summary>
        /// 工作年限
        /// </summary>
        public double WorkYear { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        [MaxLength(20)]
        public string Title { get; set; }
        /// <summary>
        /// 毕业学校
        /// </summary>
        [MaxLength(200)]
        public string GraduateSchool { get; set; }
        /// <summary>
        /// 是否相信中医
        /// </summary>
        public bool IsBelieveTCM { get; set; }
        /// <summary>
        /// 是否觉得有服务意识   
        /// </summary>
        public bool IsServiceConscious { get; set; }
        /// <summary>
        /// 关联微信号
        /// </summary>
        [MaxLength(100)]
        public string WechatNum { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        public int Recommender { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        [MaxLength(500)]
        public string RecommenderName { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        [MaxLength(100)]
        public string CheckRes { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public int Checker { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(200)]
        public string CheckRemark { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckTime { get; set; }
        /// <summary>
        /// 考试时间
        /// </summary>
        public DateTime TsetTime { get; set; }
        /// <summary>
        /// 试题编号
        /// </summary>
        [MaxLength(20)]
        public string TestID { get; set; }
        /// <summary>
        ///基础考试结果
        /// </summary>
        [MaxLength(100)]
        public string BaseTestRes { get; set; }
        /// <summary>
        ///模拟考试结果
        /// </summary>
        [MaxLength(100)]
        public string SimTestRes { get; set; }
        /// <summary>
        ///认证审核状态
        /// </summary>
        [MaxLength(100)]
        public string AuthCheckRes { get; set; }
        /// <summary>
        /// 认证审核人
        /// </summary>
        public int AuthChecker { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        [MaxLength(20)]
        public string AuthType { get; set; }
        /// <summary>
        ///认证审核备注
        /// </summary>
        [MaxLength(200)]
        public string AuthCheckRemark { get; set; }
        /// <summary>
        /// 认证审核时间
        /// </summary>
        public DateTime AuthCheckTime { get; set; }
        /// <summary>
        /// 背景图片
        /// </summary>
        [MaxLength(500)]
        public string UserImageFile { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [MaxLength(5000)]
        public string Resume { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        [MaxLength(100)]
        public string IDCard { get; set; }
        /// <summary>
        /// 是否开启分账
        /// </summary>
        public bool IsSharing{get;set;}
        /// <summary>
        /// 是否海外医生
        /// </summary>
        public bool IsAbroad { get; set; }
        /// <summary>
        /// 服务状态  是否可以咨询
        /// </summary>
        public bool ServiceState { get; set; }
        /// <summary>
        /// 接单状态 是否满单
        /// </summary>
        public bool ReceiptState{ get; set; }
    }
}
