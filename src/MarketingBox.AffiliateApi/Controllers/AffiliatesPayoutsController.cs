using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests.Payout;
using MarketingBox.AffiliateApi.Models.Payouts;
using MarketingBox.Sdk.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AffiliatePayoutsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAffiliatePayoutService _brandPayoutService;
        public AffiliatePayoutsController(IMapper mapper, IAffiliatePayoutService brandPayoutService)
        {
            _mapper = mapper;
            _brandPayoutService = brandPayoutService;
        }

        [HttpGet]
        public async Task<ActionResult<AffiliatePayoutModel>> GetAsync(
            [FromQuery] long affiliatePayoutId)
        {
            var response = await _brandPayoutService.GetAsync(new PayoutByIdRequest(){PayoutId = affiliatePayoutId});
            return this.ProcessResult(
                response,_mapper.Map<AffiliatePayoutModel>(response.Data));
        }

        [HttpPost]
        public async Task<ActionResult<AffiliatePayoutModel>> CreateAsync([FromBody] PayoutUpsertRequest request)
        {
            var response = await _brandPayoutService.CreateAsync(_mapper.Map<PayoutCreateRequest>(request));
            return this.ProcessResult(response, _mapper.Map<AffiliatePayoutModel>(response.Data));
        }

        [HttpPut("{affiliatePayoutId}")]
        public async Task<ActionResult<AffiliatePayoutModel>> UpdateAsync(
            [FromRoute] int affiliatePayoutId,
            [FromBody] PayoutUpsertRequest request)
        {
            var requestGrpc = _mapper.Map<PayoutUpdateRequest>(request);
            requestGrpc.Id = affiliatePayoutId;
            var response = await _brandPayoutService.UpdateAsync(requestGrpc);
            return this.ProcessResult(response, _mapper.Map<AffiliatePayoutModel>(response.Data));
        }

        [HttpDelete("{affiliatePayoutId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int affiliatePayoutId)
        {
            var response = await _brandPayoutService.DeleteAsync(new PayoutByIdRequest {PayoutId = affiliatePayoutId});
            return this.ProcessResult(response);
        }
    }
}