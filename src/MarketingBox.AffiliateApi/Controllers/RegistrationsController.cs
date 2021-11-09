using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Models.Leads;
using MarketingBox.AffiliateApi.Models.Leads.Requests;
using RegistrationAdditionalInfo = MarketingBox.AffiliateApi.Models.Leads.RegistrationAdditionalInfo;
using RegistrationGeneralInfo = MarketingBox.AffiliateApi.Models.Leads.RegistrationGeneralInfo;
using RegistrationRouteInfo = MarketingBox.AffiliateApi.Models.Leads.RegistrationRouteInfo;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AffiliateAndHigher)]
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
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<RegistrationModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<RegistrationModel, long>>> SearchAsync(
            [FromQuery] RegistrationSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();
            var response = await _registrationService.SearchAsync(new ()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                TenantId = tenantId,
                AffiliateId = request.AffiliateId
            });

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            return Ok(
                response.Registrations.Select(x => new RegistrationModel()
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
                        Status = x.Status,
                        GeneralInfo = new RegistrationGeneralInfo()
                        {
                            Email = x.GeneralInfo.Email,
                            CreatedAt = x.GeneralInfo.CreatedAt,
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