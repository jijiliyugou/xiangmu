using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.Release
{
    /// <summary>
    /// 问答
    /// </summary>
    public class QuestionReleaseOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="QuestionReleaseDto"></param>
        /// <param name="QuestionReleaseInfo"></param>
        public QuestionReleaseOut(PagedResultDto<QuestionRelease> QuestionReleaseDto, QuestionReleaseIn QuestionReleaseInfo)
        {
            Items = QuestionReleaseDto.Items;
            TotalCount = QuestionReleaseDto.TotalCount;
            TotalPage = QuestionReleaseDto.TotalCount / QuestionReleaseInfo.MaxResultCount;
            SkipCount = QuestionReleaseInfo.SkipCount;
            MaxResultCount = QuestionReleaseInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<QuestionRelease> Items { get; set; }
    }
    /// <summary>
    /// 问答列表
    /// </summary>
    public class QuestionReleaseOutList : QuestionRelease
    {
        /// <summary>
        /// 医生名称
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生title
        /// </summary>
        public string DoctorTitle { get; set; }
        /// <summary>
        /// 医生医院
        /// </summary>
        public string Hospital { get; set; }
        /// <summary>
        /// 医生头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 科室信息
        /// </summary>
        public string ClinicName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class QuestionReleaseDeatil : QuestionRelease
    {
        /// <summary>
        /// question 
        /// </summary>
        /// <param name="release"></param>
        /// <param name="isparise"></param>
        /// <param name="doctor"></param>
        public QuestionReleaseDeatil(QuestionRelease release, bool isparise,YaeherDoctorUser doctor)
        {
            Id = release.Id;
            UserImage = doctor.UserImage;
            DoctorName = doctor.DoctorName;
            DoctorTitle = doctor.Title;
            HospitalName = doctor.HospitalName;
            Department = doctor.Department;
            HasParise = isparise;
            DescriptionTiltle = release.DescriptionTiltle;
            Answer = release.Answer;
            Title = release.Title;
            TitleDetail = release.TitleDetail;
            DoctorId = release.DoctorId;
            ReadTotal = release.ReadTotal;
            UpvoteTotal = release.UpvoteTotal;
            TransTotal = release.TransTotal;
            CollectTotal = release.CollectTotal;
            CheckState = release.CheckState;
            CheckRemark = release.CheckRemark;
            Checker = release.Checker;
            CheckTime = release.CheckTime;
            CreatedOn = release.CreatedOn;
            CreatedBy = release.CreatedBy;
            CreatedOn = release.CreatedOn;
        }
        /// <summary>
        /// 医生名字
        /// </summary>
        public string DoctorName { get; set;}
        /// <summary>
        /// 所在科室
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string DoctorTitle { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 是否点赞
        /// </summary>
        public bool HasParise { get; set; }
    }
}
