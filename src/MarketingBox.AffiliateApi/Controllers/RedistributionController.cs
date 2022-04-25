using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Redistribution;
using MarketingBox.Redistribution.Service.Domain.Models;
using MarketingBox.Redistribution.Service.Grpc;
using MarketingBox.Redistribution.Service.Grpc.Models;
using MarketingBox.Sdk.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/redistribution")]
    public class RedistributionController : Controller
    {
        private readonly IRedistributionService _redistributionService;
        private readonly ILogger<RedistributionController> _logger;
        private readonly IMapper _mapper;

        public RedistributionController(IRedistributionService redistributionService, 
            ILogger<RedistributionController> logger, 
            IMapper mapper)
        {
            _redistributionService = redistributionService;
            _logger = logger;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<RedistributionEntity>>> GetAsync(
            [FromQuery] GetRedistributionsRequest request)
        {
            var result = await _redistributionService.GetRedistributionsAsync(request);
            return this.ProcessResult(result, result.Data ?? new List<RedistributionEntity>());
        }

        [HttpPost]
        public async Task<ActionResult<RedistributionEntity>> CreateAsync(
            [FromBody] CreateRedistributionRequestHttp request)
        {
            var response = await _redistributionService.CreateRedistributionAsync(new CreateRedistributionRequest()
            {
                CreatedBy = this.GetUserId(),
                AffiliateId = request.AffiliateId,
                CampaignId = request.CampaignId,
                Frequency = request.Frequency,
                Status = RedistributionState.Disable,
                PortionLimit = request.PortionLimit,
                DayLimit = request.DayLimit,
                UseAutologin = request.UseAutologin,
                RegistrationsIds = request.RegistrationsIds,
                FilesIds = request.FilesIds,
                RegistrationSearchRequest = request.RegistrationSearchRequest
            });
            return this.ProcessResult(response, response.Data);
        }

        [HttpPut]
        public async Task<ActionResult<RedistributionEntity>> UpdateState(
            [FromBody] UpdateRedistributionStateRequest request)
        {
            var response = await _redistributionService.UpdateRedistributionStateAsync(request);
            return this.ProcessResult(response, response.Data);
        }
    }
}