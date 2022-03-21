using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Models.Campaigns.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/campaigns")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly IMapper _mapper;

        public CampaignController(ICampaignService campaignService, IMapper mapper)
        {
            _campaignService = campaignService;
            _mapper = mapper;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<CampaignModel, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<CampaignModel, long?>>> SearchAsync(
            [FromQuery] CampaignSearchRequest request)
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

            var response = await _campaignService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                CampaignId = request.Id,
                Cursor = request.Cursor,
                Name = request.Name,
                Take = request.Limit,
                TenantId = tenantId
            });

            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<CampaignModel>)
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{campaignId}")]
        [ProducesResponseType(typeof(CampaignModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignModel>> GetAsync(
            [Required, FromRoute] long campaignId)
        {
            var response = await _campaignService.GetAsync(new()
            {
                CampaignId = campaignId
            });

            return this.ProcessResult(response, _mapper.Map<CampaignModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(CampaignModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignModel>> CreateAsync(
            [FromBody] CampaignUpsertRequest request)
        {
            var tenantId = this.GetTenantId();

            var requestGrpc = _mapper.Map<Affiliate.Service.Grpc.Requests.Campaigns.CampaignCreateRequest>(request);
            requestGrpc.TenantId = tenantId;
            var response = await _campaignService.CreateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<CampaignModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{campaignId}")]
        [ProducesResponseType(typeof(CampaignModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignModel>> UpdateAsync(
            [Required, FromRoute] long campaignId,
            [FromBody] CampaignUpsertRequest request)
        {
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<Affiliate.Service.Grpc.Requests.Campaigns.CampaignUpdateRequest>(request);
            requestGrpc.TenantId = tenantId;
            requestGrpc.CampaignId = campaignId;
            var response = await _campaignService.UpdateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<CampaignModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{campaignId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [Required, FromRoute] long campaignId)
        {
            var tenantId = this.GetTenantId();

            var response = await _campaignService.DeleteAsync(new()
            {
                CampaignId = campaignId,
            });

            this.ProcessResult(response, true);
            return Ok();
        }
    }
}