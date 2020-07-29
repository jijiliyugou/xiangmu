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
    /// LeaguerInfoOutPage
    /// </summary>
    public class LeaguerInfoOutPage:PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="pagedto"></param>
        /// <param name="leaguer"></param>
        public LeaguerInfoOutPage(PagedResultDto<YaeherPatientLeaguerInfo> pagedto, LeaguerInfo leaguer)
        {
            Items = pagedto.Items;
            TotalCount = pagedto.TotalCount;
            TotalPage = pagedto.TotalCount / leaguer.MaxResultCount;
            SkipCount = leaguer.SkipCount;
            MaxResultCount = leaguer.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherPatientLeaguerInfo> Items { get; set; }

    }



}
