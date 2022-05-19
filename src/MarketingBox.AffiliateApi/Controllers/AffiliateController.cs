using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using System.Linq;
using AutoMapper;
using MarketingBox.AffiliateApi.Models.Affiliates;
using MarketingBox.AffiliateApi.Models.Affiliates.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AffiliateSearchRequest = MarketingBox.AffiliateApi.Models.Affiliates.Requests.AffiliateSearchRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/affiliates")]
    public class AffiliateController : ControllerBase
    {
        private readonly ILogger<AffiliateController> _logger;
        private readonly IAffiliateService _affiliateService;
        private readonly IMapper _mapper;

        public AffiliateController(IAffiliateService affiliateService,
            ILogger<AffiliateController> logger,
            IMapper mapper)
        {
            _affiliateService = affiliateService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<AffiliateModel, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<AffiliateModel, long?>>> SearchAsync(
            [FromQuery] AffiliateSearchRequest request)
        {
            var tenantId = this.GetTenantId();

            var response = await _affiliateService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                AffiliateId = request.Id,
                CreatedAt = request.CreatedAt,
                Email = request.Email,
                Username = request.Username,
                Take = request.Limit,
                TenantId = tenantId,
                Phone = request.Phone,
                State = request.State
            });
            return this.ProcessResult(
                response,
                (response.Data?.Select(_mapper.Map<AffiliateModel>)
                    .ToArray() ?? Array.Empty<AffiliateModel>())
                    .Paginate(request, Url, response.Total ?? default, x => x.Id));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{affiliateId}")]
        [ProducesResponseType(typeof(AffiliateModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateModel>> GetAsync(
            [FromRoute, Required] long affiliateId)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateService.GetAsync(new()
            {
                AffiliateId = affiliateId
            });

            return this.ProcessResult(response, _mapper.Map<AffiliateModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(AffiliateModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateModel>> CreateAsync(
            [FromBody] AffiliateUpsertRequest request)
        {
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<Affiliate.Service.Grpc.Requests.Affiliates.AffiliateCreateRequest>(request);
            requestGrpc.TenantId = tenantId;
            var response = await _affiliateService.CreateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<AffiliateModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{affiliateId}")]
        [ProducesResponseType(typeof(AffiliateModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateModel>> UpdateAsync(
            [Required, FromRoute] long affiliateId,
            [FromBody] AffiliateUpsertRequest request)
        {
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<Affiliate.Service.Grpc.Requests.Affiliates.AffiliateUpdateRequest>(request);
            requestGrpc.TenantId = tenantId;
            requestGrpc.AffiliateId = affiliateId;
            var response = await _affiliateService.UpdateAsync(requestGrpc);
            
            return this.ProcessResult(response, _mapper.Map<AffiliateModel>(response.Data));
        }
    }
}