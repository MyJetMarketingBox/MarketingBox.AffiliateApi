using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Boxes;
using MarketingBox.AffiliateApi.Models.Boxes.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
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

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
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
                        new ()
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
                    .Select(Map)
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

            return this.ProcessResult(response, Map(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(CampaignModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignModel>> CreateAsync(
            [FromBody] CampaignCreateRequest request)
        {
            var tenantId = this.GetTenantId();

            var response = await _campaignService.CreateAsync(new()
            {
                Name = request.Name,
                TenantId = tenantId
            });

            return this.ProcessResult(response, Map(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{campaignId}")]
        [ProducesResponseType(typeof(CampaignModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignModel>> UpdateAsync(
            [Required, FromRoute] long campaignId,
            [FromBody] CampaignUpdateRequest request)
        {
            var tenantId = this.GetTenantId();

            var response = await _campaignService.UpdateAsync(new()
            {
                Name = request.Name,
                TenantId = tenantId,
                CampaignId = campaignId,
                Sequence = request.Sequence
            });

            return this.ProcessResult(response, Map(response.Data));
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

        private static CampaignModel Map(MarketingBox.Affiliate.Service.Grpc.Models.Campaigns.Campaign campaign)
        {
            return new CampaignModel()
            {
                Sequence = campaign.Sequence,
                Name = campaign.Name,
                Id = campaign.Id
            };
        }
    }
}