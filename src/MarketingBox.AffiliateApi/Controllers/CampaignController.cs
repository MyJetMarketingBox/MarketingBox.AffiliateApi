using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Boxes;
using MarketingBox.AffiliateApi.Models.Boxes.Requests;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Authorization;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
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
        [ProducesResponseType(typeof(Paginated<CampaignModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<CampaignModel, long>>> SearchAsync(
            [FromQuery] CampaignSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();

            var response = await _campaignService.SearchAsync(new ()
            {
                Asc = request.Order == PaginationOrder.Asc,
                CampaignId = request.Id,
                Cursor = request.Cursor,
                Name = request.Name,
                Take = request.Limit,
                TenantId = tenantId
            });

            return Ok(
                response.Boxes.Select(Map)
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{campaignId}")]
        [ProducesResponseType(typeof(CampaignModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<CampaignModel, long>>> SearchAsync(
            [Required, FromRoute] long campaignId)
        {
            var response = await _campaignService.GetAsync(new ()
            {
                CampaignId = campaignId
            });

            return MapToResponse(response);
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

            var response = await _campaignService.CreateAsync(new ()
            {
                Name = request.Name,
                TenantId = tenantId
            });

            return MapToResponse(response);
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

            var response = await _campaignService.UpdateAsync(new ()
            {
                Name = request.Name,
                TenantId = tenantId,
                CampaignId = campaignId,
                Sequence = request.Sequence
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{campaignId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateAsync(
            [Required, FromRoute] long campaignId)
        {
            var tenantId = this.GetTenantId();

            var response = await _campaignService.DeleteAsync(new ()
            {
                CampaignId = campaignId,
            });

            return MapToResponseEmpty(response);
        }

        private ActionResult MapToResponse(Affiliate.Service.Grpc.Models.Campaigns.CampaignResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Campaign == null)
                return NotFound();

            return Ok(Map(response.Campaign));
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

        private ActionResult MapToResponseEmpty(Affiliate.Service.Grpc.Models.Campaigns.CampaignResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}