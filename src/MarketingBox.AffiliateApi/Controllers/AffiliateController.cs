using System;
using MarketingBox.AffiliateApi.Models.Partners;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.AffiliateApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using AutoWrapper.Wrappers;
using MarketingBox.Affiliate.Service.Messages.Affiliates;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.ServiceBus;
using AffiliateBank = MarketingBox.AffiliateApi.Models.Partners.AffiliateBank;
using AffiliateCompany = MarketingBox.AffiliateApi.Models.Partners.AffiliateCompany;
using AffiliateCreateRequest = MarketingBox.AffiliateApi.Models.Partners.Requests.AffiliateCreateRequest;
using AffiliateGeneralInfo = MarketingBox.AffiliateApi.Models.Partners.AffiliateGeneralInfo;
using AffiliateSearchRequest = MarketingBox.AffiliateApi.Models.Affiliates.Requests.AffiliateSearchRequest;
using AffiliateUpdateRequest = MarketingBox.AffiliateApi.Models.Partners.Requests.AffiliateUpdateRequest;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/affiliates")]
    public class AffiliateController : ControllerBase
    {
        private readonly ILogger<AffiliateController> _logger;
        private readonly IAffiliateService _affiliateService;
        private readonly IServiceBusPublisher<AffiliateDeleteMessage> _serviceBusPublisher;

        public AffiliateController(IAffiliateService affiliateService,
            IServiceBusPublisher<AffiliateDeleteMessage> serviceBusPublisher,
            ILogger<AffiliateController> logger)
        {
            _affiliateService = affiliateService;
            _serviceBusPublisher = serviceBusPublisher;
            _logger = logger;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<AffiliateModel, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<AffiliateModel, long?>>> SearchAsync(
            [FromQuery] AffiliateSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                throw new ApiException(new Error
                {
                    ErrorMessage = "validation error",
                    ValidationErrors = new()
                    {
                        new()
                        {
                            ParameterName = nameof(request.Limit),
                            ErrorMessage = "Should be in the range 1..1000"
                        }
                    }
                });
            }

            var tenantId = this.GetTenantId();
            var role = request.Role?.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateRole>();

            long? masterAffiliateId = null;


            var response = await _affiliateService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                AffiliateId = request.Id,
                CreatedAt = request.CreatedAt?.DateTime ?? default,
                Email = request.Email,
                Role = role,
                Username = request.Username,
                Take = request.Limit,
                TenantId = tenantId,
                MasterAffiliateId = masterAffiliateId
            });
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(Map)
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
            var response = await _affiliateService.GetAsync(new()
            {
                AffiliateId = affiliateId
            });

            return this.ProcessResult(response, Map(response.Data));
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
            var response = await _affiliateService.CreateAsync(new()
            {
                TenantId = tenantId,
                Bank = new()
                {
                    AccountNumber = request.Bank.AccountNumber,
                    BankAddress = request.Bank.BankAddress,
                    BankName = request.Bank.BankName,
                    BeneficiaryAddress = request.Bank.BeneficiaryAddress,
                    BeneficiaryName = request.Bank.BeneficiaryName,
                    Iban = request.Bank.Iban,
                    Swift = request.Bank.Swift
                },
                Company = new()
                {
                    Address = request.Company.Address,
                    Name = request.Company.Name,
                    RegNumber = request.Company.RegNumber,
                    VatId = request.Company.VatId
                },
                GeneralInfo = new()
                {
                    CreatedAt = request.GeneralInfo.CreatedAt,
                    Currency = request.GeneralInfo.Currency
                        .MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Email = request.GeneralInfo.Email,
                    Password = request.GeneralInfo.Password,
                    Phone = request.GeneralInfo.Phone,
                    Role = request.GeneralInfo.Role
                        .MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateRole>(),
                    Skype = request.GeneralInfo.Skype,
                    State = request.GeneralInfo.State
                        .MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateState>(),
                    Username = request.GeneralInfo.Username,
                    ZipCode = request.GeneralInfo.ZipCode,
                    ApiKey = request.GeneralInfo.ApiKey
                },
            });

            return this.ProcessResult(response, Map(response.Data));
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
            var response = await _affiliateService.UpdateAsync(new()
            {
                Sequence = request.Sequence,
                AffiliateId = affiliateId,
                TenantId = tenantId,
                Bank = new()
                {
                    AccountNumber = request.Bank.AccountNumber,
                    BankAddress = request.Bank.BankAddress,
                    BankName = request.Bank.BankName,
                    BeneficiaryAddress = request.Bank.BeneficiaryAddress,
                    BeneficiaryName = request.Bank.BeneficiaryName,
                    Iban = request.Bank.Iban,
                    Swift = request.Bank.Swift
                },
                Company = new()
                {
                    Address = request.Company.Address,
                    Name = request.Company.Name,
                    RegNumber = request.Company.RegNumber,
                    VatId = request.Company.VatId
                },
                GeneralInfo = new()
                {
                    CreatedAt = request.GeneralInfo.CreatedAt,
                    Currency = request.GeneralInfo.Currency
                        .MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Email = request.GeneralInfo.Email,
                    Password = request.GeneralInfo.Password,
                    Phone = request.GeneralInfo.Phone,
                    Role = request.GeneralInfo.Role
                        .MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateRole>(),
                    Skype = request.GeneralInfo.Skype,
                    State = request.GeneralInfo.State
                        .MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Affiliates.AffiliateState>(),
                    Username = request.GeneralInfo.Username,
                    ZipCode = request.GeneralInfo.ZipCode,
                    ApiKey = request.GeneralInfo.ApiKey
                },
            });


            return this.ProcessResult(response, Map(response.Data));
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
            try
            {
                await _serviceBusPublisher.PublishAsync(new AffiliateDeleteMessage()
                {
                    AffiliateId = affiliateId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }

            return Ok();
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
    }
}