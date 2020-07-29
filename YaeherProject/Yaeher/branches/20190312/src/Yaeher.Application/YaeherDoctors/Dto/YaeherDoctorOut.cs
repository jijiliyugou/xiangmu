using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生基本信息
    /// </summary>
    public class YaeherDoctorOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherDoctorOutDto"></param>
        /// <param name="YaeherDoctorOutInfo"></param>
        public YaeherDoctorOut(PagedResultDto<YaeherDoctor> YaeherDoctorOutDto, YaeherDoctorIn YaeherDoctorOutInfo)
        {
            Items = YaeherDoctorOutDto.Items;
            TotalCount = YaeherDoctorOutDto.TotalCount;
            TotalPage = YaeherDoctorOutDto.TotalCount / YaeherDoctorOutInfo.MaxResultCount;
            SkipCount = YaeherDoctorOutInfo.SkipCount;
            MaxResultCount = YaeherDoctorOutInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherDoctor> Items { get; set; }

    }
    /// <summary>
    /// 医生基本信息
    /// </summary>
    public class YaeherDoctorUserOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherDoctorOutDto"></param>
        /// <param name="YaeherDoctorOutInfo"></param>
        public YaeherDoctorUserOut(PagedResultDto<YaeherDoctorUser> YaeherDoctorOutDto, YaeherDoctorIn YaeherDoctorOutInfo)
        {
            Items = YaeherDoctorOutDto.Items;
            TotalCount = YaeherDoctorOutDto.TotalCount;
            TotalPage = YaeherDoctorOutDto.TotalCount / YaeherDoctorOutInfo.MaxResultCount;
            SkipCount = YaeherDoctorOutInfo.SkipCount;
            MaxResultCount = YaeherDoctorOutInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherDoctorUser> Items { get; set; }

    }
    /// <summary>
    /// 返回带有user信息的YaeherDoctor信息
    /// </summary>
    public class YaeherDoctorUser : YaeherDoctor
    {
        public YaeherDoctorUser()
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        public YaeherDoctorUser(YaeherDoctor doctor, YaeherUser user, List<SystemParameter> paramlist)
        {
            DoctorName = doctor.DoctorName;
            UserID = doctor.UserID;
            Address = doctor.Address;
            HospitalName = doctor.HospitalName;
            Department = doctor.Department;
            WorkYear = doctor.WorkYear;
            Title = doctor.Title;
            GraduateSchool = doctor.GraduateSchool;
            IsBelieveTCM = doctor.IsBelieveTCM;
            IsServiceConscious = doctor.IsServiceConscious;
            WechatNum = doctor.WechatNum;
            PhoneNumber = doctor.PhoneNumber;
            Recommender = doctor.Recommender;
            CheckRes = doctor.CheckRes;
            Checker = doctor.Checker;
            CheckRemark = doctor.CheckRemark;
            CheckTime = doctor.CheckTime;
            TsetTime = doctor.TsetTime;
            TestID = doctor.TestID;
            BaseTestRes = doctor.BaseTestRes;
            SimTestRes = doctor.SimTestRes;
            AuthCheckRes = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : paramlist.Find(t => t.Code == doctor.AuthCheckRes).Name;
            AuthCheckResCode = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : doctor.AuthCheckRes;
            AuthChecker = doctor.AuthChecker;
            AuthCheckRemark = doctor.AuthCheckRemark;
            AuthCheckTime = doctor.AuthCheckTime;
            CreatedOn = doctor.CreatedOn;
            UserImage = user.UserImage;
            CreatedOn = doctor.CreatedOn;
            Id = doctor.Id;
            Resume = doctor.Resume;
            Sex = user.Sex;
            AuthType = doctor.AuthType;
            UserImageFile = doctor.UserImageFile;
        }
        //public YaeherDoctorUser(YaeherDoctor doctor, YaeherUser user, List<SystemParameter> paramlist)
        //{
        //    DoctorName = doctor.DoctorName;
        //    UserID = doctor.UserID;
        //    Address = doctor.Address;
        //    HospitalName = doctor.HospitalName;
        //    Department = doctor.Department;
        //    WorkYear = doctor.WorkYear;
        //    Title = doctor.Title;
        //    GraduateSchool = doctor.GraduateSchool;
        //    IsBelieveTCM = doctor.IsBelieveTCM;
        //    IsServiceConscious = doctor.IsServiceConscious;
        //    WechatNum = doctor.WechatNum;
        //    PhoneNumber = doctor.PhoneNumber;
        //    Recommender = doctor.Recommender;
        //    CheckRes = doctor.CheckRes;
        //    Checker = doctor.Checker;
        //    AuthType = string.IsNullOrEmpty(doctor.AuthType) ? "" : paramlist.Find(t => t.Code == doctor.AuthType).Name;
        //    CheckRemark = doctor.CheckRemark;
        //    CheckTime = doctor.CheckTime;
        //    TsetTime = doctor.TsetTime;
        //    TestID = doctor.TestID;
        //    BaseTestRes = doctor.BaseTestRes;
        //    SimTestRes = doctor.SimTestRes;
        //    AuthCheckRes = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : paramlist.Find(t => t.Code == doctor.AuthCheckRes).Name;
        //    AuthCheckResCode = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : doctor.AuthCheckRes;
        //    AuthChecker = doctor.AuthChecker;
        //    AuthCheckRemark = doctor.AuthCheckRemark;
        //    AuthCheckTime = doctor.AuthCheckTime;
        //    CreatedOn = doctor.CreatedOn;
        //    UserImage = user.UserImage;
        //    CreatedOnUtc = doctor.CreatedOn.ToString("yyyy-MM-ddTHH:mm:ss");
        //    Id = doctor.Id;
        //    Resume = doctor.Resume;
        //    Sex = user.Sex;
        //    AuthType = doctor.AuthType;
        //    UserImageFile = doctor.UserImageFile;
        //}
        public YaeherDoctorUser(YaeherDoctor doctor, YaeherUser user, List<SystemParameter> paramlist, IList<DoctorFileApply> file, List<SystemParameter> paramlist1)
        {
            AuthType = string.IsNullOrEmpty(doctor.AuthType) ? "" : paramlist1.Find(t => t.Code == doctor.AuthType).Name;
            File =file;
            DoctorName = doctor.DoctorName;
            UserID = doctor.UserID;
            Address = doctor.Address;
            HospitalName = doctor.HospitalName;
            Department = doctor.Department;
            WorkYear = doctor.WorkYear;
            Title = doctor.Title;
            GraduateSchool = doctor.GraduateSchool;
            IsBelieveTCM = doctor.IsBelieveTCM;
            IsServiceConscious = doctor.IsServiceConscious;
            WechatNum = doctor.WechatNum;
            PhoneNumber = doctor.PhoneNumber;
            Recommender = doctor.Recommender;
            CheckRes = doctor.CheckRes;
            Checker = doctor.Checker;
            CheckRemark = doctor.CheckRemark;
            CheckTime = doctor.CheckTime;
            TsetTime = doctor.TsetTime;
            TestID = doctor.TestID;
            BaseTestRes = doctor.BaseTestRes;
            SimTestRes = doctor.SimTestRes;
            AuthCheckRes = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : paramlist.Find(t => t.Code == doctor.AuthCheckRes).Name;
            AuthCheckResCode = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : doctor.AuthCheckRes;
            AuthChecker = doctor.AuthChecker;
            AuthCheckRemark = doctor.AuthCheckRemark;
            AuthCheckTime = doctor.AuthCheckTime;
            CreatedOn = doctor.CreatedOn;
            UserImage = user.UserImage;
            IDCard = doctor.IDCard;
            CreatedOn= doctor.CreatedOn;
            Id = doctor.Id;
            Resume = doctor.Resume;
            Sex = user.Sex;
            UserImageFile = doctor.UserImageFile;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public YaeherDoctorUser(YaeherDoctor doctor, YaeherUser user, List<SystemParameter> paramlist, QualityCommittee quality)
        {
            DoctorName = doctor.DoctorName;
            UserID = doctor.UserID;
            Address = doctor.Address;
            HospitalName = doctor.HospitalName;
            Department = doctor.Department;
            WorkYear = doctor.WorkYear;
            Title = doctor.Title;
            GraduateSchool = doctor.GraduateSchool;
            IsBelieveTCM = doctor.IsBelieveTCM;
            IsServiceConscious = doctor.IsServiceConscious;
            WechatNum = doctor.WechatNum;
            PhoneNumber = doctor.PhoneNumber;
            Recommender = doctor.Recommender;
            CheckRes = doctor.CheckRes;
            Checker = doctor.Checker;
            CheckRemark = doctor.CheckRemark;
            CheckTime = doctor.CheckTime;
            TsetTime = doctor.TsetTime;
            TestID = doctor.TestID;
            BaseTestRes = doctor.BaseTestRes;
            SimTestRes = doctor.SimTestRes;
            AuthCheckRes = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : paramlist.Find(t => t.Code == doctor.AuthCheckRes).Name;
            AuthCheckResCode = string.IsNullOrEmpty(doctor.AuthCheckRes) ? "unupload" : doctor.AuthCheckRes;
            AuthChecker = doctor.AuthChecker;
            AuthCheckRemark = doctor.AuthCheckRemark;
            AuthCheckTime = doctor.AuthCheckTime;
            CreatedOn = doctor.CreatedOn;
            UserImage = user.UserImage;
            CreatedOn = doctor.CreatedOn;
            Id = doctor.Id;
            Resume = doctor.Resume;
            Sex = user.Sex;
            AuthType = doctor.AuthType;
            UserImageFile = doctor.UserImageFile;
            IsQuality = (quality == null || quality.QualityState == "close") ? false : true;
        }
        /// <summary>
        /// 是否质控
        /// </summary>
        public bool IsQuality { get; set; }

        /// <summary>
        /// 1 男，2 女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 认证结果
        /// </summary>
        public string AuthCheckResCode { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 头像原图
        /// </summary>
        public string UserImageContent { get; set; }
        /// <summary>
        /// 背景图片原图
        /// </summary>
        public string UserImageFileContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 身份证，执业证资格证
        /// </summary>
        public IList<DoctorFileApply> File { get; set; }
    }

}
