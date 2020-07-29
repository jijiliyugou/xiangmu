using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生推荐排序
    /// </summary>
    public class RecommendedOrderIn : ListParameters<RecommendedOrdering>, IPagedResultRequest
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int ItemSort { get; set; }
    }
}
