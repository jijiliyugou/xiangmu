using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的分享
    /// </summary>
    public class ShareDoctorOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ShareDoctorDto"></param>
        /// <param name="ShareDoctorInfo"></param>
        public ShareDoctorOut(PagedResultDto<ShareDoctor> ShareDoctorDto, ShareDoctorIn ShareDoctorInfo)
        {
            Items = ShareDoctorDto.Items;
            TotalCount = ShareDoctorDto.TotalCount;
            TotalPage = ShareDoctorDto.TotalCount / ShareDoctorInfo.MaxResultCount;
            SkipCount = ShareDoctorInfo.SkipCount;
            MaxResultCount = ShareDoctorInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ShareDoctor> Items { get; set; }

    }
}
