using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests.Offers;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.OfferAffiliates;
using MarketingBox.AffiliateApi.Models.Offers;
using MarketingBox.AffiliateApi.Models.Offers.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfferCreateRequestGRPC = MarketingBox.Affiliate.Service.Grpc.Requests.Offers.OfferCreateRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
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
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<OfferCreateRequestGRPC>(upsertRequest);
            requestGrpc.TenantId = tenantId;
            var response = await _offerService.CreateAsync(requestGrpc);
            return this.ProcessResult(response, _mapper.Map<OfferModel>(response.Data));
        }
        
        [HttpPut("{offerId}")]
        public async Task<ActionResult<OfferModel>> UpdateAsync(
            [FromRoute] long offerId,
            [FromBody] OfferUpsertRequest upsertRequest)
        {
            var affiliateId = this.GetUserId();
            var tenantId = this.GetTenantId();
            var request = _mapper.Map<OfferUpdateRequest>(upsertRequest);
            request.OfferId = offerId;
            request.AffiliateId = affiliateId;
            request.TenantId = tenantId;
            var response =
                await _offerService.UpdateAsync(request);
            return this.ProcessResult(response, _mapper.Map<OfferModel>(response.Data));
        }

        [HttpGet("{offerId}")]
        public async Task<ActionResult<OfferModel>> GetAsync(
            [FromRoute] int offerId)
        {
            var tenantId = this.GetTenantId();
            var affiliateId = this.GetUserId();
            var response = await _offerService.GetByIdWithAccessAsync(new()
                {Id = offerId, AffiliateId = affiliateId});
            return this.ProcessResult(response, _mapper.Map<OfferModel>(response.Data));
        }

        [HttpDelete("{offerId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int offerId)
        {
            var tenantId = this.GetTenantId();
            var affiliateId = this.GetUserId();
            var response = await _offerService.DeleteAsync(new()
                {Id = offerId, AffiliateId = affiliateId});
            return this.ProcessResult(response);
        }

        [HttpGet]
        public async Task<ActionResult<Paginated<OfferModel, long?>>> SearchAsync(
            [FromQuery] Models.Offers.Requests.OfferSearchRequest paginationRequest)
        {
            var affiliateId = this.GetUserId();
            var tenantId = this.GetTenantId();
            var request = new Affiliate.Service.Grpc.Requests.Offers.OfferSearchRequest
            {
                Asc = paginationRequest.Order == PaginationOrder.Asc,
                Cursor = paginationRequest.Cursor,
                Take = paginationRequest.Limit,
                AffiliateId = affiliateId,
                Privacies = paginationRequest.Privacies.Parse<OfferPrivacy>(),
                States = paginationRequest.States.Parse<OfferState>(),
                BrandIds = paginationRequest.BrandIds.Parse<long>(),
                LanguageIds = paginationRequest.LanguageIds.Parse<int>(),
                OfferName = paginationRequest.OfferName,
                GeoIds = paginationRequest.GeoIds.Parse<int>(),
                OfferId = paginationRequest.OfferId,
                TenantId = tenantId
            };
            var response = await _offerService.SearchAsync(request);
            return this.ProcessResult(
                response,
                (response.Data?
                    .Select(_mapper.Map<OfferModel>)
                    .ToArray() ?? Array.Empty<OfferModel>())
                    .Paginate(paginationRequest, Url, response.Total ?? default, x => x.Id));
        }
                
        [HttpGet("{offerId}/url")]
        public async Task<ActionResult<ProxyLinkModel>> GetUrl([FromRoute] long offerId)
        {
            var affiliateId = this.GetUserId();
            var tenantId = this.GetTenantId();
            var url = await _offerService.GetUrlAsync(new()
            {
                OfferId = offerId,
                AffiliateId = affiliateId
            });
            return this.ProcessResult(url, new ProxyLinkModel
            {
                Url = url.Data
            });
        }
    }
}