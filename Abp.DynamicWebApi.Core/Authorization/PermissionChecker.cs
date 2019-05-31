using Abp.Authorization;
using Abp.DynamicWebApi.Authorization.Roles;
using Abp.DynamicWebApi.Authorization.Users;

namespace Abp.DynamicWebApi.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
