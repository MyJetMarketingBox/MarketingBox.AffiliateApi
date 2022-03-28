using System;
using System.Collections.Generic;
using System.IO;
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
using MarketingBox.Redistribution.Service.Domain.Models;
using MarketingBox.Redistribution.Service.Grpc;
using MarketingBox.Redistribution.Service.Grpc.Models;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.Extensions.Logging;
using RegistrationStatus = MarketingBox.AffiliateApi.Models.Registrations.RegistrationStatus;
using ValidationError = MarketingBox.Sdk.Common.Models.ValidationError;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/registrations")]
    public class RegistrationsController : ControllerBase
    {
        private readonly ILogger<RegistrationsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRegistrationService _registrationService;
        private readonly IRegistrationImporter _registrationImporter;

        public RegistrationsController(IRegistrationService registrationService, 
            IRegistrationImporter registrationImporter, 
            ILogger<RegistrationsController> logger, 
            IMapper mapper)
        {
            _registrationService = registrationService;
            _registrationImporter = registrationImporter;
            _logger = logger;
            _mapper = mapper;
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
            var masterAffiliateId = this.GetUserId();
            
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
        
        [HttpPost("upload-file")]
        public async Task<ActionResult> UploadFileAsync(IFormFile file)
        {
            try
            {
                await using var s = file.OpenReadStream();
                using var br = new BinaryReader(s);
                var bytes = br.ReadBytes((int)s.Length);

                await _registrationImporter.ImportAsync(new ImportRequest()
                {
                    RegistrationsFile = bytes,
                    UserId = this.GetUserId()
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }
        }
        
        [HttpGet("files")]
        public async Task<ActionResult<GetRegistrationFilesResponse>> GetFilesAsync()
        {
            try
            {
                var result = await _registrationImporter.GetRegistrationFilesAsync();

                return this.ProcessResult(result, _mapper.Map<GetRegistrationFilesResponse>(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }
        }
        
        [HttpGet("parse-file")]
        public async Task<ActionResult<List<RegistrationFromFile>>> GetRegistrationsFromFileAsync([FromQuery] long fileId)
        {
            try
            {
                var result = await _registrationImporter.GetRegistrationsFromFileAsync(new GetRegistrationsFromFileRequest()
                {
                    FileId = fileId
                });

                return this.ProcessResult(result, _mapper.Map<List<RegistrationFromFile>>(result.Data));
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