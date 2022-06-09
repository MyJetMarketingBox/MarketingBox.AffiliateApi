using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using RegistrationAdditionalInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationAdditionalInfo;
using RegistrationGeneralInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationGeneralInfo;
using RegistrationRouteInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationRouteInfo;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.AffiliateApi.Models.Registrations.Requests;
using MarketingBox.Registration.Service.Domain.Models.Registrations.Deposit;
using MarketingBox.Registration.Service.Grpc;
using MarketingBox.Registration.Service.Grpc.Requests.Deposits;
using MarketingBox.Reporting.Service.Domain.Models.Registrations;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.Extensions.Logging;
using IRegistrationService = MarketingBox.Reporting.Service.Grpc.IRegistrationService;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly ILogger<RegistrationsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRegistrationService _registrationService;
        private readonly IDepositService _depositService;

        public RegistrationsController(
            IRegistrationService registrationService,
            ILogger<RegistrationsController> logger,
            IMapper mapper,
            IDepositService depositService)
        {
            _registrationService = registrationService;
            _logger = logger;
            _mapper = mapper;
            _depositService = depositService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<RegistrationModel, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<RegistrationModel, long?>>> SearchAsync(
            [FromQuery] RegistrationSearchRequest request)
        {
            request.ValidateEntity();
            
            var tenantId = this.GetTenantId();

            var response = await _registrationService.SearchAsync(request.GetGrpcModel(tenantId));

            return this.ProcessResult(response,
                (response.Data?
                    .Select(Map)
                    .ToArray() ?? Array.Empty<RegistrationModel>())
                .Paginate(request, Url, response.Total ?? default, x => x.RegistrationId));
        }

        [HttpPut("{registrationId}/update-status")]
        public async Task<ActionResult<RegistrationModel>> UpdateStatusAsync(
            [FromRoute] long registrationId,
            [FromBody] RegistrationUpdateStatusRequest request)
        {
            var response = await _depositService.UpdateDepositStatusAsync(new UpdateDepositStatusRequest()
            {
                Mode = DepositUpdateMode.Manually,
                Comment = request.Comment,
                NewStatus = request.Status,
                RegistrationId = registrationId,
                TenantId = this.GetTenantId(),
                UserId = this.GetUserId()
            });

            return this.ProcessResult(response, Map(response.Data));
        }

        [HttpGet("status-log")]
        public async Task<ActionResult<List<StatusChangeLog>>> GetStatusLogAsync(
            [FromQuery] long? userId,
            [FromQuery] long? registrationId,
            [FromQuery] DepositUpdateMode? mode)
        {
            var tenantId = this.GetTenantId();
            var response = await _depositService.GetStatusChangeLogAsync(new GetStatusChangeLogRequest()
            {
                Mode = mode,
                RegistrationId = registrationId,
                UserId = userId,
                TenantId = tenantId
            });

            return this.ProcessResult(response, response.Data ?? new List<StatusChangeLog>());
        }

        private static RegistrationModel Map(RegistrationDetails registrationDetails)
        {
            return new RegistrationModel()
            {
                AdditionalInfo = new RegistrationAdditionalInfo()
                {
                    Funnel = registrationDetails.Funnel,
                    AffCode = registrationDetails.AffCode,
                    Sub1 = registrationDetails.Sub1,
                    Sub2 = registrationDetails.Sub2,
                    Sub3 = registrationDetails.Sub3,
                    Sub4 = registrationDetails.Sub4,
                    Sub5 = registrationDetails.Sub5,
                    Sub6 = registrationDetails.Sub6,
                    Sub7 = registrationDetails.Sub7,
                    Sub8 = registrationDetails.Sub8,
                    Sub9 = registrationDetails.Sub9,
                    Sub10 = registrationDetails.Sub10
                },
                Status = registrationDetails.Status,
                GeneralInfo = new RegistrationGeneralInfo()
                {
                    Email = registrationDetails.Email,
                    CreatedAt = registrationDetails.CreatedAt,
                    DepositDate = registrationDetails.DepositDate,
                    ConversionDate = registrationDetails.ConversionDate,
                    CountryId = registrationDetails.CountryId,
                    FirstName = registrationDetails.FirstName,
                    Ip = registrationDetails.Ip,
                    LastName = registrationDetails.LastName,
                    Phone = registrationDetails.Phone
                },
                RegistrationId = registrationDetails.RegistrationId,
                RouteInfo = new RegistrationRouteInfo()
                {
                    AffiliateId = registrationDetails.AffiliateId,
                    AffiliateName = registrationDetails.AffiliateName,
                    CampaignId = registrationDetails.CampaignId,
                    CampaignName = registrationDetails.CampaignName,
                    IntegrationId = registrationDetails.IntegrationId,
                    IntegrationName = registrationDetails.Integration,
                    BrandId = registrationDetails.BrandId,
                    BrandName = registrationDetails.CustomerBrand,
                    OfferId = registrationDetails.OfferId,
                    OfferName = registrationDetails.OfferName
                }
            };
        }

        private static RegistrationModel Map(Registration.Service.Domain.Models.Registrations.Registration registration)
        {
            return new RegistrationModel()
            {
                AdditionalInfo = new RegistrationAdditionalInfo()
                {
                    Funnel = registration.Funnel,
                    AffCode = registration.AffCode,
                    Sub1 = registration.Sub1,
                    Sub2 = registration.Sub2,
                    Sub3 = registration.Sub3,
                    Sub4 = registration.Sub4,
                    Sub5 = registration.Sub5,
                    Sub6 = registration.Sub6,
                    Sub7 = registration.Sub7,
                    Sub8 = registration.Sub8,
                    Sub9 = registration.Sub9,
                    Sub10 = registration.Sub10
                },
                Status = registration.Status,
                GeneralInfo = new RegistrationGeneralInfo()
                {
                    Email = registration.Email,
                    CreatedAt = registration.CreatedAt,
                    DepositDate = registration.DepositDate,
                    ConversionDate = registration.ConversionDate,
                    CountryId = registration.CountryId,
                    FirstName = registration.FirstName,
                    Ip = registration.Ip,
                    LastName = registration.LastName,
                    Phone = registration.Phone
                },
                RegistrationId = registration.Id,
                RouteInfo = new RegistrationRouteInfo()
                {
                    AffiliateId = registration.AffiliateId,
                    AffiliateName = registration.AffiliateName,
                    CampaignId = registration.CampaignId,
                    CampaignName = registration.CampaignName,
                    IntegrationId = registration.IntegrationId,
                    IntegrationName = registration.Integration,
                    BrandId = registration.BrandId,
                    BrandName = registration.CustomerBrand,
                    OfferId = registration.OfferId,
                    OfferName = registration.OfferName
                }
            };
        }
    }
}