using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.CampaignBoxes;
using MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using CampaignBoxCreateRequest = MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests.CampaignBoxCreateRequest;
using CampaignBoxUpdateRequest = MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests.CampaignBoxUpdateRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
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
        public async Task<ActionResult<Paginated<CampaignRowModel, long?>>> SearchAsync(
            [FromQuery] CampaignBoxesSearchRequest request)
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

            var response = await _campaignBoxService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                BrandId = request.BrandId,
                Cursor = request.Cursor,
                CampaignRowId = request.Id,
                CampaignId = request.CampaignId,
                Take = request.Limit,
                TenantId = tenantId
            });
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(Map)
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
            var response = await _campaignBoxService.GetAsync(new() {CampaignRowId = campaignRowId});

            return this.ProcessResult(response, Map(response.Data));
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
            var response = await _campaignBoxService.CreateAsync(new()
            {
                ActivityHours = request.ActivityHours.Select(x =>
                    new Affiliate.Service.Grpc.Models.CampaignRows.ActivityHours()
                    {
                        Day = x.Day,
                        From = x.From,
                        IsActive = x.IsActive,
                        To = x.To
                    }).ToList(),
                BrandId = request.BrandId,
                CampaignId = request.CampaignId,
                CapType = request.CapType.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.CampaignRows.CapType>(),
                GeoId = request.GeoId,
                DailyCapValue = request.DailyCapValue,
                EnableTraffic = request.EnableTraffic,
                Information = request.Information,
                Priority = request.Priority,
                Weight = request.Weight
            });

            return this.ProcessResult(response, Map(response.Data));
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
            var response = await _campaignBoxService.UpdateAsync(new()
            {
                CampaignRowId = campaignRowId,
                ActivityHours = request.ActivityHours.Select(x =>
                    new Affiliate.Service.Grpc.Models.CampaignRows.ActivityHours()
                    {
                        Day = x.Day,
                        From = x.From,
                        IsActive = x.IsActive,
                        To = x.To
                    }).ToList(),
                BrandId = request.BrandId,
                CampaignId = request.CampaignId,
                CapType = request.CapType.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.CampaignRows.CapType>(),
                GeoId = request.GeoId,
                DailyCapValue = request.DailyCapValue,
                EnableTraffic = request.EnableTraffic,
                Information = request.Information,
                Priority = request.Priority,
                Weight = request.Weight
            });

            return this.ProcessResult(response, Map(response.Data));
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
                new()
                {
                    CampaignRowId = campaignRowId,
                });

            this.ProcessResult(response, true);
            return Ok();
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
                GeoId = campaignRow.GeoId,
                GeoName = campaignRow.GeoName,
                DailyCapValue = campaignRow.DailyCapValue,
                EnableTraffic = campaignRow.EnableTraffic,
                Information = campaignRow.Information,
                Priority = campaignRow.Priority,
                Weight = campaignRow.Weight,
            };
        }

    }
}