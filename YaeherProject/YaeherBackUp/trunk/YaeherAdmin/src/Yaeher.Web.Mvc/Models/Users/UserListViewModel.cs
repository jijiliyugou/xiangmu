using System.Collections.Generic;
using Yaeher.Roles.Dto;
using Yaeher.Users.Dto;

namespace Yaeher.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
