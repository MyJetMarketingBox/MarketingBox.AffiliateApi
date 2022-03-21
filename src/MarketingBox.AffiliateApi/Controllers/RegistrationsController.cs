using System.Collections.Generic;
using System.Linq;
using MarketingBox.AffiliateApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using RegistrationAdditionalInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationAdditionalInfo;
using RegistrationGeneralInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationGeneralInfo;
using RegistrationRouteInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationRouteInfo;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.AffiliateApi.Models.Registrations.Requests;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using RegistrationStatus = MarketingBox.AffiliateApi.Models.Registrations.RegistrationStatus;
using ValidationError = MarketingBox.Sdk.Common.Models.ValidationError;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/registrations")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<RegistrationModel, long?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Paginated<RegistrationModelForAffiliate, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<RegistrationModel, long?>>> SearchAsync(
            [FromQuery] RegistrationSearchRequest request)
        {
            if (request.Limit is < 1 or > 1000)
            {
                throw new ApiException(new Error
                {
                    ErrorMessage = BadRequestException.DefaultErrorMessage,
                    ValidationErrors = new List<ValidationError>
                    {
                        new ()
                        {
                            ErrorMessage = "Should be in the range 1..1000",
                            ParameterName = nameof(request.Limit)
                        }
                    }
                });
            }
            
            var tenantId = this.GetTenantId();
            var masterAffiliateId = this.GetAffiliateId();
            
            var response = await _registrationService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                TenantId = tenantId,
                AffiliateId = request.AffiliateId,
                MasterAffiliateId = masterAffiliateId,
                Type = request.Type ?? RegistrationsReportType.All
            });

            return this.ProcessResult(response,
                response.Data?
                    .Select(Map)
                    .ToArray()
                    .Paginate(request, Url, x => x.RegistrationId));
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
                Status = registrationDetails.Status.MapEnum<RegistrationStatus>(),
                GeneralInfo = new RegistrationGeneralInfo()
                {
                    Email = registrationDetails.Email,
                    CreatedAt = registrationDetails.CreatedAt,
                    ConversionDate = registrationDetails.ConversionDate,
                    Country = registrationDetails.Country,
                    FirstName = registrationDetails.FirstName,
                    Ip = registrationDetails.Ip,
                    LastName = registrationDetails.LastName,
                    Phone = registrationDetails.Phone
                },
                RegistrationId = registrationDetails.RegistrationId,
                RouteInfo = new RegistrationRouteInfo()
                {
                    AffiliateId = registrationDetails.AffiliateId,
                    CampaignId = registrationDetails.CampaignId,
                    IntegrationIdId = registrationDetails.IntegrationId,
                    BrandId = registrationDetails.BrandId
                }
            };
        }
    }
}