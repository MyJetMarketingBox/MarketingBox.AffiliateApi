using System;
using System.Linq;
using System.Security.Claims;

namespace MarketingBox.AffiliateApi.Authorization
{
    public static class UserExtensions
    {
        public static bool HasTenantId(this ClaimsPrincipal user)
        {
            return user.HasClaim("tenant-id");
        }

        public static bool HasUserId(this ClaimsPrincipal user)
        {
            return user.HasClaim("user-id");
        }

        public static bool HasUserRole(this ClaimsPrincipal user)
        {
            return user.HasClaim("user-role");
        }

        public static string GetTenantId(this ClaimsPrincipal user)
        {
            return user.GetTenantIdOrDefault() ?? throw new InvalidOperationException("There is no tenant-id claim");
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.GetUserIdOrDefault() ?? throw new InvalidOperationException("There is no user-id claim");
        }

        public static UserRole GetUserRole(this ClaimsPrincipal user)
        {
            return user.GetUserRoleOrDefault() ?? throw new InvalidOperationException("There is no api-key-id claim");
        }

        public static string GetTenantIdOrDefault(this ClaimsPrincipal user)
        {
            return user.GetClaimOrDefault("tenant-id");
        }

        public static string GetUserIdOrDefault(this ClaimsPrincipal user)
        {
            return user.GetClaimOrDefault("user-id");
        }

        public static UserRole? GetUserRoleOrDefault(this ClaimsPrincipal user)
        {
            UserRole role;
            var claim = user.GetClaimOrDefault("user-role");

            if (string.IsNullOrEmpty(claim))
                return null;

            if (Enum.TryParse(claim, out role))
            {
                return role;
            }

            return null;
        }

        public static bool HasClaim(this ClaimsPrincipal user, string claim)
        {
            return user.Identities
                .SelectMany(x => x.Claims)
                .Any(c => c.Type == claim);
        }

        public static string GetClaimOrDefault(this ClaimsPrincipal user, string claim)
        {
            return user.Identities
                .SelectMany(x => x.Claims)
                .Where(c => c.Type == claim)
                .Select(x => x.Value)
                .SingleOrDefault();
        }
    }
}