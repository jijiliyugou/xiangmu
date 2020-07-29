using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class DoctorParaSetIn : ListParameters<DoctorParaSet>, IPagedResultRequest
    {

        /// <summary>
        /// 参数code
        /// </summary>
        public virtual string DoctorParaSetCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public virtual string DoctorParaSetName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public virtual string ItemValue { get; set; }
    }
}
