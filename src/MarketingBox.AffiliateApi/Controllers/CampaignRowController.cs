using System;
using MarketingBox.Affiliate.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc.Requests.CampaignRows;
using MarketingBox.AffiliateApi.Models.CampaignRows;
using MarketingBox.AffiliateApi.Models.CampaignRows.Requests;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using CampaignRowSearchRequest = MarketingBox.AffiliateApi.Models.CampaignRows.Requests.CampaignRowSearchRequest;


namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/campaign-rows")]
    public class CampaignRowController : ControllerBase
    {
        private readonly ICampaignRowService _campaignBoxService;
        private readonly IMapper _mapper;

        public CampaignRowController(ICampaignRowService campaignBoxService,
            IMapper mapper)
        {
            _campaignBoxService = campaignBoxService;
            _mapper = mapper;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<CampaignRowModel, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<CampaignRowModel, long?>>> SearchAsync(
            [FromQuery] CampaignRowSearchRequest request)
        {
            var tenantId = this.GetTenantId();

            var response = await _campaignBoxService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                BrandId = request.BrandId,
                Cursor = request.Cursor,
                CampaignRowId = request.Id,
                CampaignIds = request.CampaignIds.Parse<long>(),
                Priority = request.Priority,
                Weight = request.Weight,
                CapType = request.CapType,
                EnableTraffic = request.EnableTraffic,
                GeoIds = request.GeoIds.Parse<long>(),
                DailyCapValue = request.DailyCapValue,
                Take = request.Limit,
                TenantId = tenantId
            });
            return this.ProcessResult(
                response,
                (response.Data?
                    .Select(_mapper.Map<CampaignRowModel>)
                    .ToArray() ?? Array.Empty<CampaignRowModel>())
                .Paginate(request, Url, response.Total ?? default, x => x.CampaignRowId));
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
            var response =
                await _campaignBoxService.GetAsync(new() {CampaignRowId = campaignRowId});

            return this.ProcessResult(response, _mapper.Map<CampaignRowModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(CampaignRowModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignRowModel>> CreateAsync(
            [FromBody, Required] CampaignRowUpsertRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("Request has invalid format");
            }

            request.ValidateEntity();

            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<CampaignRowCreateRequest>(request);
            requestGrpc.TenantId = tenantId;
            var response = await _campaignBoxService.CreateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<CampaignRowModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{campaignRowId}")]
        [ProducesResponseType(typeof(CampaignRowModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CampaignRowModel>> UpdateAsync(
            [Required, FromRoute] long campaignRowId,
            [FromBody] CampaignRowUpsertRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("Request has invalid format");
            }

            request.ValidateEntity();
            
            var tenantId = this.GetTenantId();
            var requestGrpc = _mapper.Map<CampaignRowUpdateRequest>(request);
            requestGrpc.CampaignRowId = campaignRowId;
            requestGrpc.TenantId = tenantId;
            var response = await _campaignBoxService.UpdateAsync(requestGrpc);

            return this.ProcessResult(response, _mapper.Map<CampaignRowModel>(response.Data));
        }
        
        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{campaignRowId}/enableTraffic")]
        public async Task<ActionResult<CampaignRowModel>> EnableTrafficAsync(
            [Required, FromRoute] long campaignRowId,
            [FromBody, Required] UpdateTrafficRequestHttp request)
        {
            var tenantId = this.GetTenantId();
            var requestGrpc = new UpdateTrafficRequest
            {
                EnableTraffic = request.EnableTraffic.Value,
                CampaignRowId = campaignRowId
            };
            var response = await _campaignBoxService.UpdateTrafficAsync(requestGrpc);
            return this.ProcessResult(response, _mapper.Map<CampaignRowModel>(response.Data));
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
            var tenantId = this.GetTenantId();
            var response = await _campaignBoxService.DeleteAsync(
                new()
                {
                    CampaignRowId = campaignRowId
                });

            this.ProcessResult(response, true);
            return Ok();
        }
    }
}