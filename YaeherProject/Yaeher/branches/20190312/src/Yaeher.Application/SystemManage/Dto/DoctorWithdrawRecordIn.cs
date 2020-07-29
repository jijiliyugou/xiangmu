using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 医生提现记录表
    /// </summary>
    public class DoctorWithdrawRecordIn : ListParameters<DoctorWithdrawRecord>, IPagedResultRequest
    {
        /// <summary>
        /// 提现流水号
        /// </summary>
        public virtual string SequenceNo { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public virtual string DoctorName { get; set; }
    }
}
