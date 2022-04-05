using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests;
using MarketingBox.AffiliateApi.Models.Country;
using MarketingBox.AffiliateApi.Models.Language;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LanguagesController : ControllerBase
    {
        private ILanguageService _languageService;
        private IMapper _mapper;

        public LanguagesController(ILanguageService languageService, IMapper mapper)
        {
            _languageService = languageService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<Paginated<LanguageModel, long?>>> SearchAsync(
            [FromQuery] Models.SearchByNameRequest paginationRequest)
        {
            var request = _mapper.Map<SearchByNameRequest>(paginationRequest);
            var response = await _languageService.SearchAsync(request);
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<LanguageModel>)
                    .ToArray()
                    .Paginate(paginationRequest, Url, x => x.Id));
        }  
    }
}