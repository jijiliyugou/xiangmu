using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// PhoneReplyRecordOut
    /// </summary>
    public class PhoneReplyRecordOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="PhoneReplyRecordDto"></param>
        /// <param name="PhoneReplyRecordInfo"></param>
        public PhoneReplyRecordOut(PagedResultDto<PhoneReplyRecord> PhoneReplyRecordDto, PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            Items = PhoneReplyRecordDto.Items;
            TotalCount = PhoneReplyRecordDto.TotalCount;
            TotalPage = PhoneReplyRecordDto.TotalCount / PhoneReplyRecordInfo.MaxResultCount;
            SkipCount = PhoneReplyRecordInfo.SkipCount;
            MaxResultCount = PhoneReplyRecordInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<PhoneReplyRecord> Items { get; set; }
    }
}
