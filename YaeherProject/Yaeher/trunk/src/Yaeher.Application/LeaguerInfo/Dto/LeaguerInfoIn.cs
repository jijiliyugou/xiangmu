using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Yaeher
{
    /// <summary>
    /// 根据name搜索
    /// </summary>
    public class LeaguerInfo : ListParameters<YaeherPatientLeaguerInfo>, IPagedResultRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LeaguerInfoIn
    {
        /// <summary>
        /// 
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> leaguer { get; set; }
    }
    /// <summary>
    /// 根据id查询详情
    /// </summary>
    public class LeaguerInfoById
    {
        /// <summary>
        /// 自签名
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
    }
}
