using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Deposits;
using MarketingBox.AffiliateApi.Models.Deposits.Requests;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Registration.Service.Grpc.Models.Common;
using MarketingBox.Registration.Service.Grpc.Models.Deposits.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AffiliateManagerAndHigher)]
    [ApiController]
    [Route("/api/deposits")]
    public class DepositsController : ControllerBase
    {
        private readonly MarketingBox.Reporting.Service.Grpc.IDepositService _depositsService;
        private readonly MarketingBox.Registration.Service.Grpc.IDepositService _registrationDepositService;

        public DepositsController(MarketingBox.Reporting.Service.Grpc.IDepositService depositsService, 
            MarketingBox.Registration.Service.Grpc.IDepositService registrationDepositService)
        {
            _depositsService = depositsService;
            _registrationDepositService = registrationDepositService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<DepositModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<DepositModel, long>>> SearchAsync(
            [FromQuery] DepositSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();
            var response = await _depositsService.SearchAsync(new ()
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
                response.Deposits.Select(x => new DepositModel()
                    {
                        AffiliateId = x.AffiliateId,
                        CampaignId = x.CampaignId,
                        BoxId = x.BoxId,
                        BrandId = x.BrandId,
                        CreatedAt = x.CreatedAt,
                        Email = x.Email,
                        LeadId = x.LeadId,
                        Sequence = x.Sequence,
                        BrandStatus = x.BrandStatus,
                        ConversionDate = x.ConversionDate,
                        Country = x.Country,
                        CustomerId = x.CustomerId,
                        RegisterDate = x.RegisterDate,
                        Type = x.Type.MapEnum<ApprovedType>(),
                        UniqueId = x.UniqueId,
                    })
                    .ToArray()
                    .Paginate(request, Url, x => x.LeadId));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost("{registrationId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<DepositModel, long>>> ApproveAsync(
            [FromRoute, Required] long registrationId)
        {
            var tenantId = this.GetTenantId();
            var response = await _registrationDepositService.ApproveDepositAsync(new DepositApproveRequest()
            {
                RegistrationId = registrationId,
                Mode = ApproveMode.ApproveManually,
                TenantId = tenantId
            });

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}