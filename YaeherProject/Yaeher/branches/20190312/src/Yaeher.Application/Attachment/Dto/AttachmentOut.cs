using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 附件输出实体
    /// </summary>
    public class AttachmentOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="AttachmentInfoDto"></param>
        /// <param name="AttachmentInfo"></param>
        public AttachmentOut(PagedResultDto<AttachmentService> AttachmentInfoDto, AttachmentIn AttachmentInfo)
        {
            Items = AttachmentInfoDto.Items;
            TotalCount = AttachmentInfoDto.TotalCount;
            TotalPage = AttachmentInfoDto.TotalCount / AttachmentInfo.MaxResultCount;
            SkipCount = AttachmentInfo.SkipCount;
            MaxResultCount = AttachmentInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<AttachmentService> Items { get; set; }
    }
    public class TencentCosAccess
    {
        public string FileHeadName { get; set; }
        public string Bucket { get; set; }

        public string Region { get; set; }

        public string FileFolder { get; set; }
        public string SecretId { get; set; }
        public string SecretKey { get; set; }
    }
    public class FileAllType
    {
        public List<CodeList> Type { get; set; }
        public List<CodeList> MediaType { get; set; }
        public List<CodeList> TypeDetail { get; set; }
        public List<CodeList> DocumentDetail { get; set; }
        public List<CodeList> ImageThumNail { get; set; }
        public int MaxReplyLength { get; set; }
        public int MaxConsultationLength { get; set; }
        public int MaxReplyMaxLength { get; set; }
        public int ConsultationImageCount { get; set; }
        public double ConsultationImagesize { get; set; }
        public int VideoCount { get; set; }
        public double VideoSize { get; set; }
        public FileAllType(List<CodeList> a, List<CodeList> b, List<CodeList> c, List<CodeList> d, List<CodeList> e, int maxReplyLength, int maxConsultationLength, int maxReplyMaxLength, int consultationImageCount, double consultationImagesize, int videoCount, double videoSize)
        {
            Type = a;
            MediaType = b;
            TypeDetail = c;
            DocumentDetail = d;
            ImageThumNail = e;
            MaxReplyLength = maxReplyLength;
            MaxConsultationLength = maxConsultationLength;
            MaxReplyMaxLength = maxReplyMaxLength;
            ConsultationImageCount = consultationImageCount;
            ConsultationImagesize = consultationImagesize;
            VideoCount = videoCount;
            VideoSize = videoSize;
        }
    }
}
