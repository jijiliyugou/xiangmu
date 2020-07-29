using System;
using System.Security.Claims;

namespace Yaeher.Configuration
{
    public class DefaultClaimsIdentityFactory 
    {
        public virtual ClaimsIdentity Create(YaeherUser user, string authenticationType)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.LoginName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.LoginName),
            };
            var identity = new ClaimsIdentity(claims, authenticationType, ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return identity;
        }
    }
}