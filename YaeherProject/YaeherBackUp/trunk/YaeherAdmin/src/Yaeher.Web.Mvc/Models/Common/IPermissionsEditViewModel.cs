using System.Collections.Generic;
using Yaeher.Roles.Dto;

namespace Yaeher.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}