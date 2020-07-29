using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// RefundManageOut
    /// </summary>
    public class RefundManageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="RefundManageDto"></param>
        /// <param name="RefundManageInfo"></param>
        public RefundManageOut(PagedResultDto<RefundManage> RefundManageDto, RefundManageIn RefundManageInfo)
        {
            Items = RefundManageDto.Items;
            TotalCount = RefundManageDto.TotalCount;
            TotalPage = RefundManageDto.TotalCount / RefundManageInfo.MaxResultCount;
            SkipCount = RefundManageInfo.SkipCount;
            MaxResultCount = RefundManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<RefundManage> Items { get; set; }
    }
    /// <summary>
    /// 退单类型
    /// </summary>
    public class RefundManageType
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
      /// 创建者
      /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public int ModifyBy { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyOn { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public int DeleteBy { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>

        public DateTime DeleteTime { get; set; }
        /// <summary>
        /// 自签名
        /// </summary>
        [NotMapped]
        public string Secret { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type {get;set;}

    }
}
