using MarketingBox.AffiliateApi.Models.Partners;
using MarketingBox.AffiliateApi.Models.Partners.Requests;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using MarketingBox.AffiliateApi.Authorization;
using AffiliateCreateRequest = MarketingBox.AffiliateApi.Models.Partners.Requests.AffiliateCreateRequest;
using AffiliateSearchRequest = MarketingBox.AffiliateApi.Models.Partners.Requests.AffiliateSearchRequest;
using AffiliateUpdateRequest = MarketingBox.AffiliateApi.Models.Partners.Requests.AffiliateUpdateRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ApiController]
    [Route("/api/affiliates")]
    public class AffiliateController : ControllerBase
    {
        private readonly IAffiliateService _affiliateService;

        public AffiliateController(IAffiliateService affiliateService)
        {
            _affiliateService = affiliateService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<AffiliateModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<AffiliateModel, long>>> SearchAsync(
            [FromQuery] AffiliateSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();
            var role = request.Role?.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateRole>();

            var response = await _affiliateService.SearchAsync(new ()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Name = request.Name,
                AffiliateId = request.Id,
                CreatedAt = request.CreatedAt?.DateTime ?? default,
                Email = request.Email,
                Role = role,
                Username = request.Username,
                Take = request.Limit,
                TenantId = tenantId
            });

            return Ok(
                response.Affiliates.Select(Map)
                    .ToArray()
                    .Paginate(request, Url, x => x.AffiliateId));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{affiliateId}")]
        [ProducesResponseType(typeof(AffiliateModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<AffiliateModel>> GetAsync(
            [FromRoute, Required] long affiliateId)
        {
            var response = await _affiliateService.GetAsync(new ()
            {
                AffiliateId = affiliateId
            });


            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(AffiliateModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateModel>> CreateAsync(
            [FromBody] AffiliateCreateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateService.CreateAsync(new ()
            {
                TenantId = tenantId,
                Bank = new ()
                {
                    AccountNumber = request.Bank.AccountNumber,
                    BankAddress = request.Bank.BankAddress,
                    BankName = request.Bank.BankName,
                    BeneficiaryAddress = request.Bank.BeneficiaryAddress,
                    BeneficiaryName = request.Bank.BeneficiaryName,
                    Iban = request.Bank.Iban,
                    Swift = request.Bank.Swift
                },
                Company = new ()
                {
                    Address = request.Company.Address,
                    Name = request.Company.Name,
                    RegNumber = request.Company.RegNumber,
                    VatId = request.Company.VatId
                },
                GeneralInfo = new ()
                {
                    CreatedAt = request.GeneralInfo.CreatedAt,
                    Currency = request.GeneralInfo.Currency.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Email = request.GeneralInfo.Email,
                    Password = request.GeneralInfo.Password,
                    Phone = request.GeneralInfo.Phone,
                    Role = request.GeneralInfo.Role.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateRole>(),
                    Skype = request.GeneralInfo.Skype,
                    State = request.GeneralInfo.State.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateState>(),
                    Username = request.GeneralInfo.Username,
                    ZipCode = request.GeneralInfo.ZipCode,
                    ApiKey = request.GeneralInfo.ApiKey
                },
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{affiliateId}")]
        [ProducesResponseType(typeof(AffiliateModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AffiliateModel>> UpdateAsync(
            [Required, FromRoute] long affiliateId,
            [FromBody] AffiliateUpdateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _affiliateService.UpdateAsync(new ()
            {
                Sequence = request.Sequence,
                AffiliateId = affiliateId,
                TenantId = tenantId,
                Bank = new ()
                {
                    AccountNumber = request.Bank.AccountNumber,
                    BankAddress = request.Bank.BankAddress,
                    BankName = request.Bank.BankName,
                    BeneficiaryAddress = request.Bank.BeneficiaryAddress,
                    BeneficiaryName = request.Bank.BeneficiaryName,
                    Iban = request.Bank.Iban,
                    Swift = request.Bank.Swift
                },
                Company = new ()
                {
                    Address = request.Company.Address,
                    Name = request.Company.Name,
                    RegNumber = request.Company.RegNumber,
                    VatId = request.Company.VatId
                },
                GeneralInfo = new ()
                {
                    CreatedAt = request.GeneralInfo.CreatedAt,
                    Currency = request.GeneralInfo.Currency.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Email = request.GeneralInfo.Email,
                    Password = request.GeneralInfo.Password,
                    Phone = request.GeneralInfo.Phone,
                    Role = request.GeneralInfo.Role.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateRole>(),
                    Skype = request.GeneralInfo.Skype,
                    State = request.GeneralInfo.State.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateState>(),
                    Username = request.GeneralInfo.Username,
                    ZipCode = request.GeneralInfo.ZipCode,
                    ApiKey = request.GeneralInfo.ApiKey
                },
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{affiliateId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [Required, FromRoute] long affiliateId)
        {
            var response = await _affiliateService.DeleteAsync(new ()
            {
                AffiliateId = affiliateId,
            });

            return MapToResponseEmpty(response);
        }

        private ActionResult MapToResponse(Affiliate.Service.Grpc.Models.Affiliates.AffiliateResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Affiliate == null)
                return NotFound();

            return Ok(new AffiliateModel()
            {
                AffiliateId = response.Affiliate.AffiliateId,
                Bank = new AffiliateBank()
                {
                    AccountNumber = response.Affiliate.Bank.AccountNumber,
                    BankAddress = response.Affiliate.Bank.BankAddress,
                    BankName = response.Affiliate.Bank.BankName,
                    BeneficiaryAddress = response.Affiliate.Bank.BeneficiaryAddress,
                    BeneficiaryName = response.Affiliate.Bank.BeneficiaryName,
                    Iban = response.Affiliate.Bank.Iban,
                    Swift = response.Affiliate.Bank.Swift
                },
                Company = new AffiliateCompany()
                {
                    Address = response.Affiliate.Company.Address,
                    Name = response.Affiliate.Company.Name,
                    RegNumber = response.Affiliate.Company.RegNumber,
                    VatId = response.Affiliate.Company.VatId
                },
                GeneralInfo = new AffiliateGeneralInfo()
                {
                    CreatedAt = response.Affiliate.GeneralInfo.CreatedAt,
                    Currency = response.Affiliate.GeneralInfo.Currency.MapEnum<Currency>(),
                    Email = response.Affiliate.GeneralInfo.Email,
                    Password = response.Affiliate.GeneralInfo.Password,
                    Phone = response.Affiliate.GeneralInfo.Phone,
                    Role = response.Affiliate.GeneralInfo.Role.MapEnum<AffiliateRole>(),
                    Skype = response.Affiliate.GeneralInfo.Skype,
                    State = response.Affiliate.GeneralInfo.State.MapEnum<AffiliateState>(),
                    Username = response.Affiliate.GeneralInfo.Username,
                    ZipCode = response.Affiliate.GeneralInfo.ZipCode,
                    ApiKey = response.Affiliate.GeneralInfo.ApiKey
                },
                Sequence = response.Affiliate.Sequence
            });
        }

        private static AffiliateModel Map(Affiliate.Service.Grpc.Models.Affiliates.Affiliate affiliate)
        {
            return new AffiliateModel()
            {
                AffiliateId = affiliate.AffiliateId,
                Bank = new AffiliateBank()
                {
                    AccountNumber = affiliate.Bank.AccountNumber,
                    BankAddress = affiliate.Bank.BankAddress,
                    BankName = affiliate.Bank.BankName,
                    BeneficiaryAddress = affiliate.Bank.BeneficiaryAddress,
                    BeneficiaryName = affiliate.Bank.BeneficiaryName,
                    Iban = affiliate.Bank.Iban,
                    Swift = affiliate.Bank.Swift
                },
                Company = new AffiliateCompany()
                {
                    Address = affiliate.Company.Address,
                    Name = affiliate.Company.Name,
                    RegNumber = affiliate.Company.RegNumber,
                    VatId = affiliate.Company.VatId
                },
                GeneralInfo = new AffiliateGeneralInfo()
                {
                    CreatedAt = affiliate.GeneralInfo.CreatedAt,
                    Currency = affiliate.GeneralInfo.Currency.MapEnum<Currency>(),
                    Email = affiliate.GeneralInfo.Email,
                    Password = affiliate.GeneralInfo.Password,
                    Phone = affiliate.GeneralInfo.Phone,
                    Role = affiliate.GeneralInfo.Role.MapEnum<AffiliateRole>(),
                    Skype = affiliate.GeneralInfo.Skype,
                    State = affiliate.GeneralInfo.State.MapEnum<AffiliateState>(),
                    Username = affiliate.GeneralInfo.Username,
                    ZipCode = affiliate.GeneralInfo.ZipCode,
                    ApiKey = affiliate.GeneralInfo.ApiKey
                },
                Sequence = affiliate.Sequence
            };
        }

        private ActionResult MapToResponseEmpty(Affiliate.Service.Grpc.Models.Affiliates.AffiliateResponse response)
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