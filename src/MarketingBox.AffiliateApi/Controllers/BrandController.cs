using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Brands;
using MarketingBox.AffiliateApi.Models.Brands.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ApiController]
    [Route("/api/brands")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<BrandModel, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<BrandModel, long?>>> SearchAsync(
            [FromQuery] BrandsSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                throw new ApiException(new Error
                {
                    ErrorMessage = "validation error",
                    ValidationErrors = new()
                    {
                        new()
                        {
                            ParameterName = nameof(request.Limit),
                            ErrorMessage = "Should be in the range 1..1000"
                        }
                    }
                });
            }

            var tenantId = this.GetTenantId();
            var status = request.Status?.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Brands.BrandStatus>();

            var response = await _brandService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                BrandId = request.Id,
                IntegrationId = request.IntegrationId,
                Name = request.Name,
                Status = status,
                Take = request.Limit,
                TenantId = tenantId
            });

            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<BrandModel>)
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{brandId}")]
        [ProducesResponseType(typeof(BrandModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<BrandModel>> GetAsync(
            [FromRoute, Required] long brandId)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandService.GetAsync(new() {BrandId = brandId});

            return this.ProcessResult(response, _mapper.Map<BrandModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(BrandModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<BrandModel>> CreateAsync(
            [FromBody] BrandUpsertRequest request)
        {
            var tenantId = this.GetTenantId();
            
            var requestGrpc = _mapper.Map<Affiliate.Service.Grpc.Requests.Brands.BrandCreateRequest>(request);
            requestGrpc.TenantId = tenantId;
            var response = await _brandService.CreateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<BrandModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{brandId}")]
        [ProducesResponseType(typeof(BrandModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<BrandModel>> UpdateAsync(
            [Required, FromRoute] long brandId,
            [FromBody] BrandUpsertRequest request)
        {
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<Affiliate.Service.Grpc.Requests.Brands.BrandUpdateRequest>(request);
            requestGrpc.TenantId = tenantId;
            requestGrpc.BrandId = brandId;
            var response = await _brandService.UpdateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<BrandModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{brandId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [Required, FromRoute] long brandId)
        {
            var response = await _brandService.DeleteAsync(new()
            {
                BrandId = brandId
            });
            this.ProcessResult(response, true);
            return Ok();
        }
    }
}