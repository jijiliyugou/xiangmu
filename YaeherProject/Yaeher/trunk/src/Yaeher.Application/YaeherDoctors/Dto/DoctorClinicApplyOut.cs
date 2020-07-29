using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生申请门诊
    /// </summary>
    public class DoctorClinicApplyOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorClinicApplyDto"></param>
        /// <param name="DoctorClinicApplyInfo"></param>
        public DoctorClinicApplyOut(PagedResultDto<DoctorClinicApply> DoctorClinicApplyDto, DoctorClinicApplyIn DoctorClinicApplyInfo,List<SystemParameter> typelist1)
        {
            Items = DoctorClinicApplyDto.Items.Select(t=>new DoctorClinicApplyOutDetail(t, typelist1)).ToList();
            TotalCount = DoctorClinicApplyDto.TotalCount;
            TotalPage = DoctorClinicApplyDto.TotalCount / DoctorClinicApplyInfo.MaxResultCount;
            SkipCount = DoctorClinicApplyInfo.SkipCount;
            MaxResultCount = DoctorClinicApplyInfo.MaxResultCount;
        }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorClinicApplyDto"></param>
        /// <param name="DoctorClinicApplyInfo"></param>
        public DoctorClinicApplyOut(PagedResultDto<DoctorClinicApplyOutDetail> DoctorClinicApplyDto, DoctorClinicApplyIn DoctorClinicApplyInfo, List<SystemParameter> typelist1)
        {
            Items = DoctorClinicApplyDto.Items;
            TotalCount = DoctorClinicApplyDto.TotalCount;
            TotalPage = DoctorClinicApplyDto.TotalCount / DoctorClinicApplyInfo.MaxResultCount;
            SkipCount = DoctorClinicApplyInfo.SkipCount;
            MaxResultCount = DoctorClinicApplyInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorClinicApplyOutDetail> Items { get; set; }


    }
    /// <summary>
    /// pagemodel
    /// </summary>
    public class DoctorClinicApplyOutDetail : DoctorClinicApply
    {
        /// <summary>
        /// 
        /// </summary>
        public DoctorClinicApplyOutDetail() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        ///  <param name="typelist1"></param>
        public DoctorClinicApplyOutDetail(DoctorClinicApply item,List<SystemParameter> typelist1)
        {
            DoctorName = item.DoctorName;
            DoctorID = item.DoctorID;
            ApplyType = item.ApplyType;
            ClinicID = item.ClinicID;
            ClinicName = item.ClinicName;
            ApplyRemark = item.ApplyRemark;
            CheckTime = item.CheckTime;
            CheckRemark = item.CheckRemark;
            Id = item.Id;
            CreatedOn = item.CreatedOn;
            CheckResCode = item.CheckRes;
            CheckRes = typelist1.Find(t => t.Code == item.CheckRes).Name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="doctor"></param>
        /// <param name="user"></param>
        ///  <param name="typelist1"></param>
        ///  <param name="doctorFileApplies"></param>
        public DoctorClinicApplyOutDetail(DoctorClinicApply item,YaeherDoctor doctor,YaeherUser user, List<SystemParameter> typelist1,List<DoctorFileApply> doctorFileApplies)
        {
            UserImage = user.UserImage;
            DoctorName = item.DoctorName;
            DoctorID = item.DoctorID;
            ApplyType = item.ApplyType;
            ClinicID = item.ClinicID;
            ClinicName = item.ClinicName;
            ApplyRemark = item.ApplyRemark;
            CheckTime = item.CheckTime;
            CheckRemark = item.CheckRemark;
            Id = item.Id;
            CreatedOn = item.CreatedOn;
            CheckResCode = item.CheckRes;
            CheckRes = typelist1.Find(t => t.Code == item.CheckRes).Name;
            Certificateofpractice = doctorFileApplies.Count>0? doctorFileApplies.Find(t=>t.TypeDetail== "Certificateofpractice").Address:"";
            Qualificationcertificate = doctorFileApplies.Count > 0 ? doctorFileApplies.Find(t => t.TypeDetail == "Qualificationcertificate").Address : "";
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qualificationcertificate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Certificateofpractice { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string CheckResCode { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 成人(1)儿童(2)
        /// </summary>
        public int ClinicType { get; set; }
        
    }
}
