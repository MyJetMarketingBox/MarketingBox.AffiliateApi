using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.RegistrationFile
{
    public class GetRegistrationsFromFileRequestHttp : PaginationRequest<long?>
    {
        public long FileId { get; set; }
    }
}