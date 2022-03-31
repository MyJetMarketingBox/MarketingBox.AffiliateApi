using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests;
using MarketingBox.Affiliate.Service.Grpc.Requests.OfferAffiliate;
using MarketingBox.AffiliateApi.Models.OfferAffiliates;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class OfferAffiliatesController : ControllerBase
    {
        private IOfferAffiliateService _offerAffiliateService;
        private IMapper _mapper;

        public OfferAffiliatesController(IOfferAffiliateService offerAffiliateService, IMapper mapper)
        {
            _offerAffiliateService = offerAffiliateService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<Paginated<OfferAffiliateModel, long?>>> GetAllAsync(
            [FromQuery] PaginationRequest<long?> paginationRequest)
        {
            var request = new GetAllRequest
            {
                Asc = paginationRequest.Order == PaginationOrder.Asc,
                Cursor = paginationRequest.Cursor,
                Take = paginationRequest.Limit
            };
            var response = await _offerAffiliateService.GetAllAsync(request);
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<OfferAffiliateModel>)
                    .ToArray()
                    .Paginate(paginationRequest, Url, x => x.Id));
        }

        [HttpPost]
        public async Task<ActionResult<OfferAffiliateModel>> CreateAsync([FromBody] OfferAffiliateUpsertRequest upsertRequest)
        {
            var response = await _offerAffiliateService.CreateAsync(_mapper.Map<OfferAffiliateCreateRequest>(upsertRequest));
            return this.ProcessResult(response, _mapper.Map<OfferAffiliateModel>(response.Data));
        }

        [HttpGet("{offerAffiliateId}")]
        public async Task<ActionResult<OfferAffiliateModel>> GetAsync(
            [FromRoute] int offerAffiliateId)
        {
            var response = await _offerAffiliateService.GetAsync(new(){OfferAffiliateId = offerAffiliateId});
            return this.ProcessResult(response, _mapper.Map<OfferAffiliateModel>(response.Data));
        }

        [HttpDelete("{offerAffiliateId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int offerAffiliateId)
        {
            var response = await _offerAffiliateService.DeleteAsync(new () {OfferAffiliateId = offerAffiliateId});
            return this.ProcessResult(response);
        }
    }
}