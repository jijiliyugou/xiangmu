using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.LableManages.Dto
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class LableManageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="LableManageDto"></param>
        /// <param name="LableManageInfo"></param>
        public LableManageOut(PagedResultDto<LableManage> LableManageDto, LableManageIn LableManageInfo)
        {
            Items = LableManageDto.Items;
            TotalCount = LableManageDto.TotalCount;
            TotalPage = LableManageDto.TotalCount / LableManageInfo.MaxResultCount;
            SkipCount = LableManageInfo.SkipCount;
            MaxResultCount = LableManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<LableManage> Items { get; set; }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="LableManageDto"></param>
        /// <param name="LableManageInfo"></param>
        /// <param name="relation"></param>
        public LableManageOut(PagedResultDto<LableManage> LableManageDto, LableManageIn LableManageInfo,IList<DoctorRelation> relation)
        {
            Item = LableManageDto.Items.Select(t=>new LableManagePageDetail(t, relation)).ToList();
            TotalCount = LableManageDto.TotalCount;
            TotalPage = LableManageDto.TotalCount / LableManageInfo.MaxResultCount;
            SkipCount = LableManageInfo.SkipCount;
            MaxResultCount = LableManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<LableManagePageDetail> Item { get; set; }

    }
    /// <summary>
    ///医生个人中心标签
    /// </summary>
    public class LableManagePageDetail:LableManage
    {
        /// <summary>
        /// 标签
        /// </summary>
        public LableManagePageDetail(LableManage label, IList<DoctorRelation> relation)
        {
            LableName = label.LableName;
            LableRemark = label.LableRemark;
            Id = label.Id;
            HasOwner = (relation == null || relation.FirstOrDefault(t => t.LableID == label.Id) == null) ? false : true;
        }
        /// <summary>
        /// 是否添加
        /// </summary>
        public bool HasOwner { get; set;}
    }
}
