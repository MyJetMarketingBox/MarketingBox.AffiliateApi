using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Models.Redistribution;
using MarketingBox.Redistribution.Service.Domain.Models;
using MarketingBox.Redistribution.Service.Grpc;
using MarketingBox.Redistribution.Service.Grpc.Models;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/redistribution")]
    public class RedistributionController : Controller
    {
        private readonly IRedistributionService _redistributionService;

        public RedistributionController(IRedistributionService redistributionService)
        {
            _redistributionService = redistributionService;
        }

        [HttpGet]
        public async Task<ActionResult<Paginated<RedistributionEntity, long?>>> GetAsync(
            [FromQuery] GetRedistributionsRequestHttp request)
        {
            var tenantId = this.GetTenantId();
            var response = await _redistributionService.GetRedistributionsAsync(
                new GetRedistributionsRequest
                {
                    Asc = request.Order == PaginationOrder.Asc,
                    Cursor = request.Cursor,
                    Take = request.Limit,
                    AffiliateId = request.AffiliateId,
                    CampaignId = request.CampaignId,
                    CreatedBy = request.CreatedBy,
                    Name = request.Name,
                    TenantId = tenantId
                });
            return this.ProcessResult(
                response,
                (response.Data ?? Enumerable.Empty<RedistributionEntity>())
                .ToArray()
                .Paginate(request, Url, response.Total ?? default, x => x.Id));
        }

        [HttpPost]
        public async Task<ActionResult<RedistributionEntity>> CreateAsync(
            [FromBody] CreateRedistributionRequestHttp request)
        {
            request.ValidateEntity();

            var tenantId = this.GetTenantId();

            var response = await _redistributionService.CreateRedistributionAsync(new CreateRedistributionRequest()
            {
                CreatedBy = this.GetUserId(),
                Name = request.Name,
                AffiliateId = request.AffiliateId,
                CampaignId = request.CampaignId,
                Frequency = request.Frequency,
                Status = RedistributionState.Disable,
                PortionLimit = request.PortionLimit,
                DayLimit = request.DayLimit,
                UseAutologin = request.UseAutologin,
                RegistrationsIds = request.RegistrationsIds,
                FilesIds = request.FilesIds,
                TenantId = tenantId,
                RegistrationSearchRequest = request.RegistrationSearchRequest?.GetGrpcModel(tenantId)
            });
            return this.ProcessResult(response, response.Data);
        }

        [HttpPut]
        public async Task<ActionResult<RedistributionEntity>> UpdateState(
            [FromBody] UpdateRedistributionStateRequest request)
        {
            var response = await _redistributionService.UpdateRedistributionStateAsync(request);
            return this.ProcessResult(response, response.Data);
        }
    }
}