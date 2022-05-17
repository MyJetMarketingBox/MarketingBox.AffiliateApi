using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests.BrandBox;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.BrandBox;
using MarketingBox.AffiliateApi.Models.BrandBox.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BrandBoxSearchRequestApi = MarketingBox.AffiliateApi.Models.BrandBox.Requests.BrandBoxSearchRequest;
using BrandBoxSearchRequestGrpc = MarketingBox.Affiliate.Service.Grpc.Requests.BrandBox.BrandBoxSearchRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BrandBoxController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBrandBoxService _brandBoxService;

        public BrandBoxController(IMapper mapper, IBrandBoxService brandBoxService)
        {
            _mapper = mapper;
            _brandBoxService = brandBoxService;
        }

        [HttpGet]
        public async Task<ActionResult<Paginated<BrandBoxModel, long?>>> SearchAsync(
            [FromQuery] BrandBoxSearchRequestApi paginationRequest)
        {
            var tenantId = this.GetTenantId();
            var request = new BrandBoxSearchRequestGrpc
            {
                Asc = paginationRequest.Order == PaginationOrder.Asc,
                Cursor = paginationRequest.Cursor,
                Take = paginationRequest.Limit,
                Name = paginationRequest.Name,
                TenantId = tenantId
            };
            var response = await _brandBoxService.SearchAsync(request);
            return this.ProcessResult(
                response,
                (response.Data?
                    .Select(_mapper.Map<BrandBoxModel>)
                    .ToArray() ?? Array.Empty<BrandBoxModel>())
                .Paginate(paginationRequest, Url, response.Total ?? default, x => x.Id));
        }

        [HttpPost]
        public async Task<ActionResult<BrandBoxModel>> CreateAsync([FromBody] BrandBoxUpsertRequest upsertRequest)
        {
            var tenantId = this.GetTenantId();
            var request = _mapper.Map<BrandBoxCreateRequest>(upsertRequest);
            request.CreatedBy = this.GetUserId();
            request.TenantId = tenantId;
            var response = await _brandBoxService.CreateAsync(request);
            return this.ProcessResult(response, _mapper.Map<BrandBoxModel>(response.Data));
        }

        [HttpPut("{brandBoxId}")]
        public async Task<ActionResult<BrandBoxModel>> UpdateAsync(
            [FromRoute] long brandBoxId,
            [FromBody] BrandBoxUpsertRequest updateUpsertRequest)
        {
            var tenantId = this.GetTenantId();
            var request = _mapper.Map<BrandBoxUpdateRequest>(updateUpsertRequest);
            request.Id = brandBoxId;
            request.TenantId = tenantId;
            var response = await _brandBoxService.UpdateAsync(request);
            return this.ProcessResult(response, _mapper.Map<BrandBoxModel>(response.Data));
        }

        [HttpDelete("{brandBoxId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long brandBoxId)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandBoxService.DeleteAsync(new BrandBoxByIdRequest
                {BrandBoxId = brandBoxId, TenantId = tenantId});
            return this.ProcessResult(response);
        }

        [HttpGet("{brandBoxId}")]
        public async Task<ActionResult<BrandBoxModel>> GetAsync([FromRoute] long brandBoxId)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandBoxService.GetAsync(new BrandBoxByIdRequest
                {BrandBoxId = brandBoxId, TenantId = tenantId});
            return this.ProcessResult(response, _mapper.Map<BrandBoxModel>(response.Data));
        }
    }
}