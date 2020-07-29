using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yaeher.Common;

namespace Yaeher.Scheduling.Dto
{
    /// <summary>
    /// 医生排班
    /// </summary>
    public class DoctorSchedulingOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorSchedulingDto"></param>
        /// <param name="DoctorSchedulingInfo"></param>
        public DoctorSchedulingOut(PagedResultDto<DoctorScheduling> DoctorSchedulingDto, DoctorSchedulingIn DoctorSchedulingInfo)
        {
            Items = DoctorSchedulingDto.Items.Select(t => new DoctorSchedulingOutList(t)).ToList();
            TotalCount = DoctorSchedulingDto.TotalCount;
            TotalPage = DoctorSchedulingDto.TotalCount / DoctorSchedulingInfo.MaxResultCount;
            SkipCount = DoctorSchedulingInfo.SkipCount;
            MaxResultCount = DoctorSchedulingInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorSchedulingOutList> Items { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DoctorSchedulingOutList: DoctorScheduling
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduling"></param>
        public DoctorSchedulingOutList(DoctorScheduling scheduling)
        {
            DoctorName = scheduling.DoctorName;
            DoctorID = scheduling.DoctorID;
            DoctorJSON = scheduling.DoctorJSON;
            SchedulingDate = scheduling.SchedulingDate;
            SchedulingTime = scheduling.SchedulingTime;
            Duplication = JsonHelper.FromJson<CodeList>(scheduling.Duplication).Value;
            ClinicType = JsonHelper.FromJson<CodeList>(scheduling.ClinicType).Value;
            ClinicIDAdd = scheduling.ClinicIDAdd;
            RegistrationFee = scheduling.RegistrationFee;
            ServiceState = scheduling.ServiceState;
            CreatedBy = scheduling.CreatedBy;
            CreatedOn = scheduling.CreatedOn;
            Id = scheduling.Id;
            SchedulingDateUtc = scheduling.SchedulingDate.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string SchedulingDateUtc { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DoctorSchedulingOutDetail : DoctorScheduling
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduling"></param>
        public DoctorSchedulingOutDetail(DoctorScheduling scheduling)
        {
            DoctorName = scheduling.DoctorName;
            DoctorID = scheduling.DoctorID;
            DoctorJSON = scheduling.DoctorJSON;
            SchedulingDate = scheduling.SchedulingDate;
            SchedulingTime = scheduling.SchedulingTime;
            Duplication = scheduling.Duplication;
            ClinicType = scheduling.ClinicType;
            ClinicIDAdd = scheduling.ClinicIDAdd;
            RegistrationFee = scheduling.RegistrationFee;
            ServiceState = scheduling.ServiceState;
            CreatedBy = scheduling.CreatedBy;
            Id = scheduling.Id;
            SchedulingDateUtc = scheduling.SchedulingDate.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string SchedulingDateUtc { get; set; }
    }
    /// <summary>
    /// 类型
    /// </summary>
    public class DoctorSchedulingType
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CodeList> SchedulingTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CodeList> Duplication { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CodeList> ClinicType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public DoctorSchedulingType(List<CodeList> a, List<CodeList> b, List<CodeList> c)
        {
            SchedulingTime = a;
            Duplication = b;
            ClinicType = c;
        }
    }
}
