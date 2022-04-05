using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests;
using MarketingBox.AffiliateApi.Models.Country;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    //[Authorize]
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
        public async Task<ActionResult<Paginated<CountryModel, long?>>> GetAllAsync(
            [FromQuery] Models.SearchByNameRequest paginationRequest)
        {
            var request = _mapper.Map<SearchByNameRequest>(paginationRequest);
            var response = await _countryService.SearchAsync(request);
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<CountryModel>)
                    .ToArray()
                    .Paginate(paginationRequest, Url, response.Total ?? default, x => x.Id));
        }  
    }
}