using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Brands;
using MarketingBox.AffiliateApi.Models.Brands.Requests;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc.Models.Integrations;
using MarketingBox.AffiliateApi.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ApiController]
    [Route("/api/integrations")]
    public class IntegrationController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;

        public IntegrationController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<IntegrationModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<IntegrationModel, long>>> SearchAsync(
            [FromQuery] IntegrationsSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();

            var response = await _integrationService.SearchAsync(new ()
            {
                Asc = request.Order == PaginationOrder.Asc,
                IntegrationId = request.Id,
                Cursor = request.Cursor,
                Name = request.Name,
                Take = request.Limit,
                TenantId = tenantId
            });

            return Ok(
                response.Integrations.Select(Map)
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
        }

        [HttpGet("{integrationId}")]
        [ProducesResponseType(typeof(IntegrationModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<IntegrationModel, long>>> GetAsync(
            [FromRoute] long integrationId)
        {
            var response = await _integrationService.GetAsync(new ()
            {
                 IntegrationId = integrationId
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(IntegrationModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<IntegrationModel>> CreateAsync(
            
            [FromBody] IntegrationCreateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _integrationService.CreateAsync(new ()
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
        [HttpPut("{integrationId}")]
        [ProducesResponseType(typeof(IntegrationModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<IntegrationModel>> UpdateAsync(
            
            [Required, FromRoute] long integrationId,
            [FromBody] IntegrationUpdateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _integrationService.UpdateAsync(new ()
            {
                Name = request.Name,
                TenantId = tenantId,
                Sequence = request.Sequence,
                IntegrationId = integrationId
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{integrationId}")]
        [ProducesResponseType(typeof(Integration), StatusCodes.Status200OK)]
        public async Task<ActionResult<Integration>> DeleteAsync(
            
            [Required, FromRoute] long integrationId)
        {
            var tenantId = this.GetTenantId();
            var response = await _integrationService.DeleteAsync(new ()
            {
                IntegrationId = integrationId
            });

            return MapToResponse(response);
        }

        private ActionResult MapToResponse(Affiliate.Service.Grpc.Models.Integrations.IntegrationResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Integration != null)
                return Ok(Map(response.Integration));

            return Ok();
        }

        private static IntegrationModel Map(Affiliate.Service.Grpc.Models.Integrations.Integration integration)
        {
            return new()
            {
                Sequence = integration.Sequence,
                Name = integration.Name,
                Id = integration.Id
            };
        }

        private ActionResult MapToResponseEmpty(Affiliate.Service.Grpc.Models.Brands.BrandResponse response)
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