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
    public class BrandPayoutsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBrandPayoutService _brandPayoutService;

        public BrandPayoutsController(IMapper mapper, IBrandPayoutService brandPayoutService)
        {
            _mapper = mapper;
            _brandPayoutService = brandPayoutService;
        }

        [HttpGet]
        public async Task<ActionResult<Paginated<BrandPayoutModel, long?>>> SearchAsync(
            [FromQuery] BrandPayoutSearchRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandPayoutService.SearchAsync(new()
            {
                EntityId = request.BrandId,
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                Name = request.Name,
                GeoIds = request.GeoIds.Parse<long>(),
                PayoutTypes = request.PayoutTypes.Parse<Plan>(),
                TenantId = tenantId
            });
            return this.ProcessResult(
                response, (response.Data?.Select(_mapper.Map<BrandPayoutModel>)
                    .ToArray() ?? Array.Empty<BrandPayoutModel>())
                .Paginate(request, Url, response.Total ?? default, x => x.Id));
        }

        [HttpGet("{brandPayoutId}")]
        public async Task<ActionResult<BrandPayoutModel>> GetAsync(
            [FromRoute] long brandPayoutId)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandPayoutService.GetAsync(new PayoutByIdRequest()
                {PayoutId = brandPayoutId});
            return this.ProcessResult(
                response, _mapper.Map<BrandPayoutModel>(response.Data));
        }

        [HttpPost]
        public async Task<ActionResult<BrandPayoutModel>> CreateAsync([FromBody] PayoutUpsertRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("Request has invalid format");
            }

            request.ValidateEntity();
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<PayoutCreateRequest>(request);
            requestGrpc.TenantId = tenantId;
            var response = await _brandPayoutService.CreateAsync(requestGrpc);
            return this.ProcessResult(response, _mapper.Map<BrandPayoutModel>(response.Data));
        }

        [HttpPut("{brandPayoutId}")]
        public async Task<ActionResult<BrandPayoutModel>> UpdateAsync(
            [FromRoute] int brandPayoutId,
            [FromBody] PayoutUpsertRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("Request has invalid format");
            }

            request.ValidateEntity();
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<PayoutUpdateRequest>(request);
            requestGrpc.Id = brandPayoutId;
            requestGrpc.TenantId = tenantId;
            var response = await _brandPayoutService.UpdateAsync(requestGrpc);
            return this.ProcessResult(response, _mapper.Map<BrandPayoutModel>(response.Data));
        }

        [HttpDelete("{brandPayoutId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int brandPayoutId)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandPayoutService.DeleteAsync(new PayoutByIdRequest
                {PayoutId = brandPayoutId});
            return this.ProcessResult(response);
        }
    }
}