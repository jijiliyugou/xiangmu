using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class YaeherRoleIn : ListParameters<YaeherRole>, IPagedResultRequest
    {

    }
}
