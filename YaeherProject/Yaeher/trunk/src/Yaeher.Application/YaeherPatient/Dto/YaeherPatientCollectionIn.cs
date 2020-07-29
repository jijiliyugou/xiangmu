using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 我的收藏
    /// </summary>
    public class YaeherPatientCollectionIn : ListParameters<YaeherPatientCollection>, IPagedResultRequest
    {
        /// <summary>
        ///操作类型
        ///收藏 点赞
        /// </summary>
        public string Opentype { get; set; }
    }
   
}
