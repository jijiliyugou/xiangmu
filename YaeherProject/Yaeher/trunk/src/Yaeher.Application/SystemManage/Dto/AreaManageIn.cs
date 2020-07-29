using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
   
    /// <summary>
    /// 地区管理
    /// </summary>
    public class AreaManageIn : ListParameters<AreaManage>, IPagedResultRequest
    {

    }
}
