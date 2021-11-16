using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Authorization;

namespace MarketingBox.AffiliateApi.Extensions
{
    public static class UserRoleExtension
    {
        public static HashSet<UserRole> _restrictedRoles = new HashSet<UserRole>()
        {
            UserRole.Affiliate,
            UserRole.MasterAffiliate,
            UserRole.MasterAffiliateReferral,
        };
        public static bool IsRestricted(this UserRole role)
        {
            return _restrictedRoles.Contains(role);
        }
    }
}
