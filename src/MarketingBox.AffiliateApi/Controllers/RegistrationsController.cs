using System.Linq;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Authorization;
using RegistrationAdditionalInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationAdditionalInfo;
using RegistrationGeneralInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationGeneralInfo;
using RegistrationRouteInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationRouteInfo;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.AffiliateApi.Models.Registrations.Requests;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Grpc;
using RegistrationStatus = MarketingBox.AffiliateApi.Models.Registrations.RegistrationStatus;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
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
        [Authorize(Policy = AuthorizationPolicies.AffiliateAndHigher)]
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<RegistrationModel, long>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Paginated<RegistrationModelForAffiliate, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<RegistrationModel, long>>> SearchAsync(
            [FromQuery] RegistrationSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should be in the range 1..1000");

                return BadRequest();
            }

            var role = this.GetRole();
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

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Registrations == null || !response.Registrations.Any())
                return NotFound();

            //if (role == UserRole.Affiliate)
            //    return Ok(response.Registrations.Select(x => new RegistrationModelForAffiliate()
            //        {
            //            Status = x.Status.MapEnum<RegistrationStatus>(),
            //            GeneralInfo = new RegistrationGeneralInfoForAffiliate()
            //            {
            //                CreatedAt = x.CreatedAt,
            //                ConversionDate = x.ConversionDate,
            //                Country = x.Country,
            //            },
            //            RegistrationId = x.RegistrationId
            //        })
            //        .ToArray()
            //        .Paginate(request, Url, x => x.RegistrationId));

            return Ok(response.Registrations.Select(x => new RegistrationModel()
            {
                AdditionalInfo = new RegistrationAdditionalInfo()
                {
                    Funnel = x.Funnel,
                    AffCode = x.AffCode,
                    Sub1 = x.Sub1,
                    Sub2 = x.Sub2,
                    Sub3 = x.Sub3,
                    Sub4 = x.Sub4,
                    Sub5 = x.Sub5,
                    Sub6 = x.Sub6,
                    Sub7 = x.Sub7,
                    Sub8 = x.Sub8,
                    Sub9 = x.Sub9,
                    Sub10 = x.Sub10
                },
                Status = x.Status.MapEnum<RegistrationStatus>(),
                GeneralInfo = new RegistrationGeneralInfo()
                {
                    Email = x.Email,
                    CreatedAt = x.CreatedAt,
                    ConversionDate = x.ConversionDate,
                    Country = x.Country,
                    FirstName = x.FirstName,
                    Ip = x.Ip,
                    LastName = x.LastName,
                    Phone = x.Phone
                },
                RegistrationId = x.RegistrationId,
                RouteInfo = new RegistrationRouteInfo()
                {
                    AffiliateId = x.AffiliateId,
                    CampaignId = x.CampaignId,
                    IntegrationIdId = x.IntegrationId,
                    BrandId = x.BrandId
                }
            })
                .ToArray()
                .Paginate(request, Url, x => x.RegistrationId));
        }
    }
}