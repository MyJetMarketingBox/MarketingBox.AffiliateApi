using System;

namespace MarketingBox.AffiliateApi.Authorization
{
    public static class AuthorizationPolicies
    {
        public const String AffiliateAndHigher = "AffiliateAndHigher";

        public const String MasterAffiliateAndHigher = "MasterAffiliateAndHigher";

        public const String AffiliateManagerAndHigher = "AffiliateManagerAndHigher";

        public const String AdminOnly = "AdminOnly";
    }
}