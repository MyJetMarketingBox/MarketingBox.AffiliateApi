using System;
using System.Collections.Generic;
using System.Linq;
using MarketingBox.AffiliateApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
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

        public RegistrationsController(IRegistrationService registrationService, 
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
        [HttpPost("search")]
        [ProducesResponseType(typeof(Paginated<RegistrationModel, long?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Paginated<RegistrationModelForAffiliate, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<RegistrationModel, long?>>> SearchAsync(
            [FromBody] RegistrationSearchRequest request)
        {
            var tenantId = this.GetTenantId();
            
            var response = await _registrationService.SearchAsync(new()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                TenantId = tenantId,
                AffiliateIds = request.AffiliateIds,
                Type = request.Type ?? RegistrationsReportType.All,
                CountryIds = request.CountryIds,
                Statuses = request.Statuses,
                CrmStatuses = request.CrmStatuses,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                RegistrationIds = request.RegistrationIds,
                BrandBoxIds = request.BrandBoxIds,
                Email = request.Email,
                Phone = request.Phone,
                BrandIds = request.BrandIds,
                CampaignIds = request.CampaignIds,
                FirstName = request.FirstName,
                IntegrationIds = request.IntegrationIds,
                LastName = request.LastName
            });

            return this.ProcessResult(response,
                response.Data?
                    .Select(Map)
                    .ToArray()
                    .Paginate(request, Url, response.Total ?? default, x => x.RegistrationId));
        }

        [HttpPut("update-status")]
        public async Task<ActionResult<Deposit>> UpdateStatusAsync(
            [FromQuery] long registrationId,
            [FromQuery] RegistrationStatus newStatus, 
            [FromQuery] string comment)
        {
            try
            {
                var response = await _depositService.UpdateDepositStatusAsync(new UpdateDepositStatusRequest()
                {
                    Mode = DepositUpdateMode.Manually,
                    Comment = comment,
                    NewStatus = newStatus,
                    RegistrationId = registrationId,
                    TenantId = this.GetTenantId(),
                    UserId = this.GetUserId()
                });
                    
                return this.ProcessResult(response, _mapper.Map<Deposit>(response.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }
        }
        [HttpGet("status-log")]
        public async Task<ActionResult<List<StatusChangeLog>>> GetStatusLogAsync(
            [FromQuery] long? userId, 
            [FromQuery] long? registrationId, 
            [FromQuery] DepositUpdateMode? mode)
        {
            try
            {
                var response = await _depositService.GetStatusChangeLogAsync(new GetStatusChangeLogRequest()
                {
                    Mode = mode,
                    RegistrationId = registrationId,
                    UserId = userId
                });
                    
                return this.ProcessResult(response, _mapper.Map<List<StatusChangeLog>>(response.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }
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
                    CampaignId = registrationDetails.CampaignId,
                    IntegrationIdId = registrationDetails.IntegrationId,
                    BrandId = registrationDetails.BrandId
                }
            };
        }
    }
}