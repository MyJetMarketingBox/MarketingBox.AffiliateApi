using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using System.Linq;
using AutoMapper;
using AutoWrapper.Wrappers;
using MarketingBox.Affiliate.Service.Messages.Affiliates;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.ServiceBus;
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
        private readonly IServiceBusPublisher<AffiliateDeleteMessage> _serviceBusPublisher;
        private readonly IMapper _mapper;

        public AffiliateController(IAffiliateService affiliateService,
            IServiceBusPublisher<AffiliateDeleteMessage> serviceBusPublisher,
            ILogger<AffiliateController> logger,
            IMapper mapper)
        {
            _affiliateService = affiliateService;
            _serviceBusPublisher = serviceBusPublisher;
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

            var response = await _affiliateService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                AffiliateId = request.Id,
                CreatedAt = request.CreatedAt,
                Email = request.Email,
                Username = request.Username,
                Take = request.Limit,
                TenantId = tenantId
            });
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<AffiliateModel>)
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
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

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{affiliateId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [Required, FromRoute] long affiliateId)
        {
            try
            {
                await _serviceBusPublisher.PublishAsync(new AffiliateDeleteMessage()
                {
                    AffiliateId = affiliateId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }

            return Ok();
        }
    }
}