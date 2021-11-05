using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.CampaignBoxes;
using MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CampaignBoxCreateRequest = MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests.CampaignBoxCreateRequest;
using CampaignBoxUpdateRequest = MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests.CampaignBoxUpdateRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/campaign-rows")]
    public class CampaignRowController : ControllerBase
    {
        private readonly ICampaignRowService _campaignBoxService;

        public CampaignRowController(ICampaignRowService campaignBoxService)
        {
            _campaignBoxService = campaignBoxService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<CampaignRowModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<CampaignRowModel, long>>> SearchAsync(
            [FromQuery] CampaignBoxesSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();

            var response = await _campaignBoxService.SearchAsync(new ()
            {
                Asc = request.Order == PaginationOrder.Asc,
                BrandId= request.BrandId,
                Cursor = request.Cursor,
                CampaignRowId = request.Id,
                CampaignId = request.CampaignId,
                Take = request.Limit,
                TenantId = tenantId
            });

            return Ok(
                response.CampaignBoxes.Select(Map)
                    .ToArray()
                    .Paginate(request, Url, x => x.CampaignRowId));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{campaignRowId}")]
        [ProducesResponseType(typeof(CampaignRowModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<CampaignRowModel>> GetAsync(
            [FromRoute, Required] long campaignRowId)
        {
            var tenantId = this.GetTenantId();
            var response = await _campaignBoxService.GetAsync(new () { CampaignRowId = campaignRowId });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(CampaignRowModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignRowModel>> CreateAsync(
            [FromBody] CampaignBoxCreateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _campaignBoxService.CreateAsync(new ()
            {
                ActivityHours = request.ActivityHours.Select(x => new Affiliate.Service.Grpc.Models.CampaignRows.ActivityHours()
                {
                    Day = x.Day,
                    From = x.From,
                    IsActive = x.IsActive,
                    To = x.To
                }).ToArray(),
                BrandId = request.BrandId,
                CampaignId = request.CampaignId,
                CapType = request.CapType.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.CampaignRows.CapType>(),
                CountryCode = request.CountryCode,
                DailyCapValue = request.DailyCapValue,
                EnableTraffic = request.EnableTraffic,
                Information = request.Information,
                Priority = request.Priority,
                Weight = request.Weight
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{campaignRowId}")]
        [ProducesResponseType(typeof(CampaignRowModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignRowModel>> UpdateAsync(
            [Required, FromRoute] long campaignRowId,
            [FromBody] CampaignBoxUpdateRequest request)
        {
            var response = await _campaignBoxService.UpdateAsync(new ()
            {
                Sequence = request.Sequence,
                CampaignRowId = campaignRowId,
                ActivityHours = request.ActivityHours.Select(x => new Affiliate.Service.Grpc.Models.CampaignRows.ActivityHours()
                {
                    Day = x.Day,
                    From = x.From,
                    IsActive = x.IsActive,
                    To = x.To
                }).ToArray(),
                BrandId = request.BrandId,
                CampaignId = request.CampaignId,
                CapType = request.CapType.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.CampaignRows.CapType>(),
                CountryCode = request.CountryCode,
                DailyCapValue = request.DailyCapValue,
                EnableTraffic = request.EnableTraffic,
                Information = request.Information,
                Priority = request.Priority,
                Weight = request.Weight
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{campaignRowId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [Required, FromRoute] long campaignRowId)
        {
            var response = await _campaignBoxService.DeleteAsync(
                new ()
                {
                    CampaignRowId = campaignRowId,
                });

            return MapToResponseEmpty(response);
        }

        private ActionResult MapToResponse(Affiliate.Service.Grpc.Models.CampaignRows.CampaignRowResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.CampaignRow == null)
                return NotFound();

            return Ok(Map(response.CampaignRow));
        }

        private static CampaignRowModel Map(Affiliate.Service.Grpc.Models.CampaignRows.CampaignRow campaignRow)
        {
            return new CampaignRowModel()
            {
                BrandId = campaignRow.BrandId,
                CampaignId = campaignRow.CampaignId,
                ActivityHours = campaignRow.ActivityHours.Select(x => new ActivityHours()
                {
                    Day = x.Day,
                    From = x.From,
                    IsActive = x.IsActive,
                    To = x.To
                }).ToArray(),
                CampaignRowId = campaignRow.CampaignRowId,
                CapType = campaignRow.CapType.MapEnum<CapType>(),
                CountryCode =   campaignRow.CountryCode,
                DailyCapValue = campaignRow.DailyCapValue,
                EnableTraffic = campaignRow.EnableTraffic,
                Information =   campaignRow.Information,
                Priority =      campaignRow.Priority,
                Weight = campaignRow.Weight,
                Sequence = campaignRow.Sequence
            };
        }

        private ActionResult MapToResponseEmpty(Affiliate.Service.Grpc.Models.CampaignRows.CampaignRowResponse response)
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