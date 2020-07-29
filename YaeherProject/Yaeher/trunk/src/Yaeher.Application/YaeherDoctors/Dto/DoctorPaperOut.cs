using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherDoctors.Dto
{
    /// <summary>
    /// 医生发布文章
    /// </summary>
    public class DoctorPaperOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorPaperOutOutDto"></param>
        /// <param name="DoctorPaperInfo"></param>
        public DoctorPaperOut(PagedResultDto<DoctorPaper> DoctorPaperOutOutDto, DoctorPaperIn DoctorPaperInfo)
        {
            Items = DoctorPaperOutOutDto.Items;
            TotalCount = DoctorPaperOutOutDto.TotalCount;
            TotalPage = DoctorPaperOutOutDto.TotalCount / DoctorPaperInfo.MaxResultCount;
            SkipCount = DoctorPaperInfo.SkipCount;
            MaxResultCount = DoctorPaperInfo.MaxResultCount;
        }
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="DoctorPaperOutOutDto"></param>
        /// <param name="DoctorPaperInfo"></param>
        public DoctorPaperOut(PagedResultDto<DoctorPaperView> DoctorPaperOutOutDto, DoctorPaperIn DoctorPaperInfo)
        {
            Items = DoctorPaperOutOutDto.Items;
            TotalCount = DoctorPaperOutOutDto.TotalCount;
            TotalPage = DoctorPaperOutOutDto.TotalCount / DoctorPaperInfo.MaxResultCount;
            SkipCount = DoctorPaperInfo.SkipCount;
            MaxResultCount = DoctorPaperInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<DoctorPaper> Items { get; set; }
    }
    /// <summary>
    /// DoctorPagerDetail
    /// </summary>
    public class DoctorPagerDetail: DoctorPaper
    {
        /// <summary>
        /// DoctorPagerDetail
        /// </summary>
        /// <param name="a"></param>
        public DoctorPagerDetail(DoctorPaper a)
        {
            ImageFie = a.ImageFie;
            DoctorName = a.DoctorName;
            DoctorID = a.DoctorID;
            PaperTiltle = a.PaperTiltle;
            PaperContent = a.PaperContent;
            PaperFrom = a.PaperFrom;
            ConsultNumber = a.ConsultNumber;
            PaperAddress = a.PaperAddress;
            Checker = a.Checker;
            CheckState = a.CheckState;
            CheckRemark = a.CheckRemark;
            CheckTime = a.CheckTime;
            Id = a.Id;
            ConsultID = a.ConsultID;
            CreatedOn = a.CreatedOn;
        }
        /// <summary>
        /// 咨询Id
        /// </summary>
        public int ConsultID { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageFie { get; set; }

    }
    /// <summary>
    /// DoctorPaper
    /// </summary>
    public class DoctorPaperType
    {
        /// <summary>
        /// 来源
        /// </summary>
        public List<CodeList> DoctorPaperFrom { get;set;}
        /// <summary>
        /// 状态
        /// </summary>
        public List<CodeList> DoctorPaperState { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        /// <param name="from"></param>
        /// <param name="state"></param>
        public DoctorPaperType(List<CodeList> from, List<CodeList> state)
        {
            DoctorPaperFrom= from;
            DoctorPaperState= state;
        }

    }
}
