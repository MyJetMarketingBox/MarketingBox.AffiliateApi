using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.Country;
using MarketingBox.AffiliateApi.Models.Country;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Route("/api/countries")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountryController(
            ICountryService countryService,
            IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<Paginated<Country, int>>> GetAllAsync(
            [FromQuery] PaginationRequest<int> paginationRequest)
        {
            var asc = paginationRequest.Order == PaginationOrder.Asc;
            if (!asc && paginationRequest.Cursor == 0)
            {
                paginationRequest.Cursor = paginationRequest.Limit;
            }
            var request = new GetAllRequest
            {
                Asc = asc,
                Cursor = paginationRequest.Cursor,
                Take = paginationRequest.Limit
            };
            var response = await _countryService.GetAllAsync(request);
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<Country>)
                    .ToArray()
                    .Paginate(paginationRequest, Url, x => x.Id));
        }  
    }
}