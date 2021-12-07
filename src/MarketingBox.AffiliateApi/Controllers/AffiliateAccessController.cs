using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.AffiliateAccesses;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.AffiliateAccess.Requests;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
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
        public async Task<ActionResult<Paginated<AffiliateAccessModel, long>>> SearchAsync(
            [FromQuery] AffiliateAccessSearchRequest request)
        {
            if (request.Limit is < 1 or > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be in the range 1..1000");

                return BadRequest();
            }
            var tenantId = this.GetTenantId();
            var response = await _affiliateAccessService.SearchAsync(new ()
            {
                MasterAffiliateId = request.MasterAffiliateId,
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                TenantId = tenantId
            });
            
            if (response.AffiliateAccesses != null && response.AffiliateAccesses.Any())
                return Ok(
                    response.AffiliateAccesses.Select(Map)
                        .ToArray()
                        .Paginate(request, Url, x => x.MasterAffiliateId));
            return NotFound();
        }
        
        [HttpGet("{masterAffiliateId}/{affiliateId}")]
        [ProducesResponseType(typeof(AffiliateAccessModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<AffiliateAccessModel>> GetAsync(
            [FromRoute, Required] long masterAffiliateId,
            [FromRoute, Required] long affiliateId)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateAccessService.GetAsync(new ()
            {
                AffiliateId = affiliateId,
                MasterAffiliateId = masterAffiliateId,
                TenantId = tenantId
            });
            return MapToResponse(response);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(AffiliateAccessModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateAccessModel>> CreateAsync(
            [FromBody] AffiliateAccessCreateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateAccessService.CreateAsync(new ()
            {
                AffiliateId = request.AffiliateId,
                MasterAffiliateId = request.MasterAffiliateId,
                TenantId = tenantId
            });
            return MapToResponse(response);
        }

        [HttpDelete("{masterAffiliateId}/{affiliateId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [FromRoute, Required] long masterAffiliateId,
            [FromRoute, Required] long affiliateId)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateAccessService.DeleteAsync(new ()
            {
                TenantId = tenantId,
                MasterAffiliateId = masterAffiliateId,
                AffiliateId = affiliateId
            });

            return MapToResponseEmpty(response);
        }

        private ActionResult MapToResponse(AffiliateAccessResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.AffiliateAccess == null)
                return NotFound();

            return Ok(Map(response.AffiliateAccess));
        }

        private static AffiliateAccessModel Map(AffiliateAccess access)
        {
            return new AffiliateAccessModel()
            {
                MasterAffiliateId = access.MasterAffiliateId,
                AffiliateId = access.AffiliateId
            };
        }

        private ActionResult MapToResponseEmpty(AffiliateAccessResponse response)
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