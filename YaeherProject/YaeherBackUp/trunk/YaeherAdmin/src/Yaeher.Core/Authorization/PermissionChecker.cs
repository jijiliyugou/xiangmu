using Abp.Authorization;
using Yaeher.Authorization.Roles;
using Yaeher.Authorization.Users;

namespace Yaeher.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
