using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Models.Offers;
using MarketingBox.AffiliateApi.Models.Offers.Requests;
using MarketingBox.Sdk.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfferCreateRequestGRPC = MarketingBox.Affiliate.Service.Grpc.Requests.Offers.OfferCreateRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
   // [Authorize]
    [Route("/api/[controller]")]
    public class OffersController : ControllerBase
    {
        private IOfferService _offerService;
        private IMapper _mapper;

        public OffersController(IOfferService offerService, IMapper mapper)
        {
            _offerService = offerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OfferModel>> CreateAsync(
            [FromBody] OfferUpsertRequest upsertRequest)
        {
            var response =
                await _offerService.CreateAsync(_mapper.Map<OfferCreateRequestGRPC>(upsertRequest));
            return this.ProcessResult(response, _mapper.Map<OfferModel>(response.Data));
        }

        [HttpGet("{offerId}")]
        public async Task<ActionResult<OfferModel>> GetAsync(
            [FromRoute] int offerId)
        {
            var response = await _offerService.GetAsync(new() {Id = offerId});
            return this.ProcessResult(response, _mapper.Map<OfferModel>(response.Data));
        }

        [HttpDelete("{offerId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int offerId)
        {
            var response = await _offerService.DeleteAsync(new() {Id = offerId});
            return this.ProcessResult(response);
        }
    }
}