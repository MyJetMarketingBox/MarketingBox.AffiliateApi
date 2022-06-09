using System;
using MarketingBox.Affiliate.Service.Grpc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.AffiliateApi.Models.Integrations;
using MarketingBox.AffiliateApi.Models.Integrations.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/integrations")]
    public class IntegrationController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;
        private readonly IMapper _mapper;

        public IntegrationController(IIntegrationService integrationService, IMapper mapper)
        {
            _integrationService = integrationService;
            _mapper = mapper;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<IntegrationModel, long?>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<IntegrationModel, long?>>> SearchAsync(
            [FromQuery] IntegrationsSearchRequest request)
        {
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
            return this.ProcessResult(
                response,
                (response.Data?
                    .Select(_mapper.Map<IntegrationModel>)
                    .ToArray() ?? Array.Empty<IntegrationModel>())
                    .Paginate(request, Url, response.Total ?? default, x => x.Id));
        }

        [HttpGet("{integrationId}")]
        [ProducesResponseType(typeof(IntegrationModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<IntegrationModel>> GetAsync(
            [FromRoute] long integrationId)
        {
            var tenantId = this.GetTenantId();
            var response = await _integrationService.GetAsync(new ()
            {
                 IntegrationId = integrationId
            });

            return this.ProcessResult(response, _mapper.Map<IntegrationModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(IntegrationModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<IntegrationModel>> CreateAsync(
            
            [FromBody] IntegrationUpsertRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _integrationService.CreateAsync(new ()
            {
                Name = request.Name,
                TenantId = tenantId
            });

            return this.ProcessResult(response, _mapper.Map<IntegrationModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{integrationId}")]
        [ProducesResponseType(typeof(IntegrationModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<IntegrationModel>> UpdateAsync(
            
            [Required, FromRoute] long integrationId,
            [FromBody] IntegrationUpsertRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _integrationService.UpdateAsync(new ()
            {
                Name = request.Name,
                TenantId = tenantId,
                IntegrationId = integrationId
            });

            return this.ProcessResult(response, _mapper.Map<IntegrationModel>(response.Data));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{integrationId}")]
        [ProducesResponseType(typeof(IntegrationModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<IntegrationModel>> DeleteAsync(
            
            [Required, FromRoute] long integrationId)
        {
            var tenantId = this.GetTenantId();
            var response = await _integrationService.DeleteAsync(new ()
            {
                IntegrationId = integrationId
            });

            this.ProcessResult(response, true);
            return Ok();
        }
    }
}