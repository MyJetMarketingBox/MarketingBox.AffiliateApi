using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.RegistrationFile
{
    public class GetFilesRequestHttp : PaginationRequest<long?>
    {
        public string FileName { get; set; }
    }
}