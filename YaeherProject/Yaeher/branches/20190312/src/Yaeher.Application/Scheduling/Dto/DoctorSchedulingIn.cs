using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Yaeher.Scheduling.Dto
{
    /// <summary>
    /// 医生排班
    /// </summary>
    public class DoctorSchedulingIn : ListParameters<DoctorScheduling>, IPagedResultRequest
    { 

        public int Id { get; set; }
    }
    /// <summary>
    /// 新增model
    /// </summary>
    public class DoctorSchedulingInAdd : DoctorScheduling
    {
        /// <summary>
        /// 排版时段多选
        /// </summary>
        public List<CodeList> SchedulingTimeList { get; set; }
    }


}
