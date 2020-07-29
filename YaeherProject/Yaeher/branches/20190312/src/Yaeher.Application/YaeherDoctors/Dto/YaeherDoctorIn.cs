using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生详情
    /// </summary>
    public class YaeherDoctorIn : ListParameters<YaeherDoctor>, IPagedResultRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string DoctorName { get; set;}
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 医生住址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 工作医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 所在科室
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 工作年限
        /// </summary>
        public double WorkYear { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 毕业学校
        /// </summary>
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
        public string WechatNum { get; set; }
        /// <summary>
        /// 微信名称(海外用)
        /// </summary>
        public string WecharName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        public int Recommender { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        public string RecommenderName { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        public string CheckRes { get; set; }  // （Success Fail）
        /// <summary>
        /// 审核人
        /// </summary>
        public int Checker { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
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
        public string TestID { get; set; }
        /// <summary>
        ///基础考试结果
        /// </summary>
        public string BaseTestRes { get; set; }  // （Success Fail）
        /// <summary>
        ///模拟考试结果
        /// </summary>
        public string SimTestRes { get; set; }  // （Success Fail）
        /// <summary>
        ///认证审核状态
        /// </summary>
        public string AuthCheckRes { get; set; } // （Success Fail）
        /// <summary>
        /// 认证审核人
        /// </summary>
        public int AuthChecker { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        public string AuthType { get; set; }
        /// <summary>
        ///认证审核备注
        /// </summary>
        public string AuthCheckRemark { get; set; }
        /// <summary>
        /// 认证审核时间
        /// </summary>
        public DateTime AuthCheckTime { get; set; }
        /// <summary>
        /// 背景图片
        /// </summary>
        public string UserImageFile { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Resume { get; set; }
        /// <summary>
        /// AuthCheck：Check(审核)    Test（考试）     Authen（认证）
        /// </summary>
        public string AuthCheck { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 是否海外
        /// </summary>
        public bool IsAbroad { get; set; }

    }
    /// <summary>
    /// 医生搜索
    /// </summary>
    public class YaeherDoctorSearch : ListParameters<YaeherDoctor>, IPagedResultRequest
    {
        /// <summary>
        /// 上下线状态
        /// </summary>
        public string OnlineState { get; set; }
        /// <summary>
        /// 科室Id
        /// </summary>
        public int ClinicID { get; set; }
        /// <summary>
        /// 医生idarra
        /// </summary>
        public List<int> DoctorIdArr { get; set; }
    }
    /// <summary>
    /// 患者端查询收藏医生信息
    /// </summary>
    public class YaeherPatientDoctorSearch
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 医生Id 合计
        /// </summary>

        public string IdArr { get; set; }
    }
    /// <summary>
    /// 带有科室Id的医生信息
    /// </summary>
    public class YaeherClinicDoctor : YaeherDoctor
    {
        /// <summary>
        /// 科室Id
        /// </summary>
        public int ClinicID { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImage { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class YaeherDoctorImage
    {
        /// <summary>
        /// 
        /// </summary>
        public string Secret { get; set;}
        /// <summary>
        /// 背景头像
        /// </summary>
        public string BackGroundImage { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DoctorInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int DoctorId { get; set; }
        /// <summary>
        /// 订单类型  订单 退单（order refund）
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 服务类型 图文 电话等 
        /// </summary>
        public string ServiceType { get; set; }

    }
}
