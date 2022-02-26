using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.AffiliateAccesses;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.AffiliateAccess.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AffiliateManagerAndHigher)]
    [ApiController]
    [Route("/api/affiliateaccess")]
    public class AffiliateAccessController : ControllerBase
    {
        private readonly IAffiliateAccessService _affiliateAccessService;

        public AffiliateAccessController(IAffiliateAccessService affiliateAccessService)
        {
            _affiliateAccessService = affiliateAccessService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Paginated<AffiliateAccessModel, long>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<AffiliateAccessModel, long?>>> SearchAsync(
            [FromQuery] AffiliateAccessSearchRequest request)
        {
            if (request.Limit is < 1 or > 1000)
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
            var response = await _affiliateAccessService.SearchAsync(new()
            {
                MasterAffiliateId = request.MasterAffiliateId,
                AffiliateId = request.AffiliateId,
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                TenantId = tenantId
            });

            return this.ProcessResult(
                response,
                response.Data?
                    .Select(Map)
                    .ToArray()
                    .Paginate(request, Url, x => x.MasterAffiliateId));
        }

        [HttpGet("{masterAffiliateId}/{affiliateId}")]
        [ProducesResponseType(typeof(AffiliateAccessModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateAccessModel>> GetAsync(
            [FromRoute, Required] long masterAffiliateId,
            [FromRoute, Required] long affiliateId)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateAccessService.GetAsync(new()
            {
                AffiliateId = affiliateId,
                MasterAffiliateId = masterAffiliateId,
                TenantId = tenantId
            });
            return this.ProcessResult(response, Map(response.Data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AffiliateAccessModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateAccessModel>> CreateAsync(
            [FromBody] AffiliateAccessCreateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateAccessService.CreateAsync(new()
            {
                AffiliateId = request.AffiliateId,
                MasterAffiliateId = request.MasterAffiliateId,
                TenantId = tenantId
            });
            return this.ProcessResult(response, Map(response.Data));
        }

        [HttpDelete("{masterAffiliateId}/{affiliateId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [FromRoute, Required] long masterAffiliateId,
            [FromRoute, Required] long affiliateId)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateAccessService.DeleteAsync(new()
            {
                TenantId = tenantId,
                MasterAffiliateId = masterAffiliateId,
                AffiliateId = affiliateId
            });

            this.ProcessResult(response, true);
            return Ok();
        }
        private static AffiliateAccessModel Map(AffiliateAccess access)
        {
            return new AffiliateAccessModel()
            {
                MasterAffiliateId = access.MasterAffiliateId,
                AffiliateId = access.AffiliateId
            };
        }
    }
}