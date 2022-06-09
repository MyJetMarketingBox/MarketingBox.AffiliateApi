using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests.Payout;
using MarketingBox.AffiliateApi.Models.Payouts;
using MarketingBox.AffiliateApi.Models.Payouts.Requests;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Exceptions;
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
    public class AffiliatePayoutsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAffiliatePayoutService _affiliatePayoutService;

        public AffiliatePayoutsController(IMapper mapper, IAffiliatePayoutService affiliatePayoutService)
        {
            _mapper = mapper;
            _affiliatePayoutService = affiliatePayoutService;
        }

        [HttpGet]
        public async Task<ActionResult<Paginated<AffiliatePayoutModel, long?>>> SearchAsync(
            [FromQuery] AffiliatePayoutSearchRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliatePayoutService.SearchAsync(new()
            {
                EntityId = request.AffiliateId,
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                Name = request.Name,
                GeoIds = request.GeoIds.Parse<long>(),
                PayoutTypes = request.PayoutTypes.Parse<Plan>(),
                TenantId = tenantId
            });
            return this.ProcessResult(
                response, (response.Data?.Select(_mapper.Map<AffiliatePayoutModel>)
                    .ToArray() ?? Array.Empty<AffiliatePayoutModel>())
                .Paginate(request, Url, response.Total ?? default, x => x.Id));
        }

        [HttpGet("{affiliatePayoutId}")]
        public async Task<ActionResult<AffiliatePayoutModel>> GetAsync(
            [FromRoute] long affiliatePayoutId)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliatePayoutService.GetAsync(new PayoutByIdRequest()
            {
                PayoutId = affiliatePayoutId
            });
            return this.ProcessResult(
                response, _mapper.Map<AffiliatePayoutModel>(response.Data));
        }

        [HttpPost]
        public async Task<ActionResult<AffiliatePayoutModel>> CreateAsync([FromBody] PayoutUpsertRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("Request has invalid format");
            }

            request.ValidateEntity();

            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<PayoutCreateRequest>(request);
            requestGrpc.TenantId = tenantId;
            var response = await _affiliatePayoutService.CreateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<AffiliatePayoutModel>(response.Data));
        }

        [HttpPut("{affiliatePayoutId}")]
        public async Task<ActionResult<AffiliatePayoutModel>> UpdateAsync(
            [FromRoute] int affiliatePayoutId,
            [FromBody] PayoutUpsertRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("Request has invalid format");
            }

            request.ValidateEntity();

            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<PayoutUpdateRequest>(request);
            requestGrpc.Id = affiliatePayoutId;
            requestGrpc.TenantId = tenantId;
            var response = await _affiliatePayoutService.UpdateAsync(requestGrpc);
            return this.ProcessResult(response, _mapper.Map<AffiliatePayoutModel>(response.Data));
        }

        [HttpDelete("{affiliatePayoutId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int affiliatePayoutId)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliatePayoutService.DeleteAsync(new PayoutByIdRequest
                {PayoutId = affiliatePayoutId});
            return this.ProcessResult(response);
        }
    }
}