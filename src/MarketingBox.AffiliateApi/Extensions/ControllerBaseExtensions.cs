using MarketingBox.AffiliateApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static string GetTenantId(this ControllerBase controllerBase)
        {
            return controllerBase.User.GetTenantId();
        }
        public static long GetUserId(this ControllerBase controllerBase)
        {
            return long.Parse(controllerBase.User.GetUserId());
        }
    }
}
