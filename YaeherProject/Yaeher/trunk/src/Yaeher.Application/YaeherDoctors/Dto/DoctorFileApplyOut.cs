using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生上传文件
    /// </summary>
    public class DoctorFileApplyOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorFileApplyDto"></param>
        /// <param name="DoctorFileApplyInfo"></param>
        public DoctorFileApplyOut(PagedResultDto<DoctorFileApply> DoctorFileApplyDto, DoctorFileApplyIn DoctorFileApplyInfo)
        {
            Items = DoctorFileApplyDto.Items;
            TotalCount = DoctorFileApplyDto.TotalCount;
            TotalPage = DoctorFileApplyDto.TotalCount / DoctorFileApplyInfo.MaxResultCount;
            SkipCount = DoctorFileApplyInfo.SkipCount;
            MaxResultCount = DoctorFileApplyInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorFileApply> Items { get; set; }


    }
}
