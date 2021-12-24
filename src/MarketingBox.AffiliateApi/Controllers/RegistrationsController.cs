using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Authorization;
using RegistrationAdditionalInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationAdditionalInfo;
using RegistrationGeneralInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationGeneralInfo;
using RegistrationRouteInfo = MarketingBox.AffiliateApi.Models.Registrations.RegistrationRouteInfo;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.AffiliateApi.Models.Registrations.Requests;

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
                MasterAffiliateId = masterAffiliateId
            });

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Registrations == null || !response.Registrations.Any())
                return NotFound();

            if (role == UserRole.Affiliate)
                return Ok(response.Registrations.Select(x => new RegistrationModelForAffiliate()
                    {
                        Status = x.Status.MapEnum<RegistrationStatus>(),
                        GeneralInfo = new RegistrationGeneralInfoForAffiliate()
                        {
                            CreatedAt = x.GeneralInfo.CreatedAt,
                            DepositedAt = x.GeneralInfo.DepositedAt,
                            ConversionDate = x.GeneralInfo.ConversionDate,
                            Country = x.GeneralInfo.Country,
                        },
                        RegistrationId = x.RegistrationId,
                        Sequence = x.Sequence,
                        UniqueId = x.UniqueId
                    })
                    .ToArray()
                    .Paginate(request, Url, x => x.RegistrationId));


            return Ok(response.Registrations.Select(x => new RegistrationModel()
            {
                AdditionalInfo = new RegistrationAdditionalInfo()
                {
                    So = x.AdditionalInfo.So,
                    Sub = x.AdditionalInfo.Sub,
                    Sub1 = x.AdditionalInfo.Sub1,
                    Sub10 = x.AdditionalInfo.Sub10,
                    Sub2 = x.AdditionalInfo.Sub2,
                    Sub3 = x.AdditionalInfo.Sub3,
                    Sub4 = x.AdditionalInfo.Sub4,
                    Sub5 = x.AdditionalInfo.Sub5,
                    Sub6 = x.AdditionalInfo.Sub6,
                    Sub7 = x.AdditionalInfo.Sub7,
                    Sub8 = x.AdditionalInfo.Sub8,
                    Sub9 = x.AdditionalInfo.Sub9
                },
                Status = x.Status.MapEnum<RegistrationStatus>(),
                GeneralInfo = new RegistrationGeneralInfo()
                {
                    Email = x.GeneralInfo.Email,
                    CreatedAt = x.GeneralInfo.CreatedAt,
                    DepositedAt = x.GeneralInfo.DepositedAt,
                    ConversionDate = x.GeneralInfo.ConversionDate,
                    Country = x.GeneralInfo.Country,
                    FirstName = x.GeneralInfo.FirstName,
                    Ip = x.GeneralInfo.Ip,
                    LastName = x.GeneralInfo.LastName,
                    Phone = x.GeneralInfo.Phone
                },
                RegistrationId = x.RegistrationId,
                RouteInfo = new RegistrationRouteInfo()
                {
                    AffiliateId = x.RouteInfo.AffiliateId,
                    CampaignId = x.RouteInfo.CampaignId,
                    IntegrationIdId = x.RouteInfo.IntegrationId,
                    BrandId = x.RouteInfo.BrandId
                },
                Sequence = x.Sequence,
                    //Type = x.,
                    UniqueId = x.UniqueId
            })
                .ToArray()
                .Paginate(request, Url, x => x.RegistrationId));
        }
    }
}