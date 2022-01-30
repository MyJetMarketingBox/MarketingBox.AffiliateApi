using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.Postback.Service.Grpc;
using MarketingBox.Postback.Service.Grpc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AffiliateAndHigher)]
    [ApiController]
    [Route("/api/[controller]/")]
    public class PostbackLogsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PostbackLogsController> _logger;
        private readonly IEventReferenceLogService _eventReferenceLogService;

        public PostbackLogsController(
            IEventReferenceLogService eventReferenceLogService,
            IMapper mapper,
            ILogger<PostbackLogsController> logger)
        {
            _eventReferenceLogService = eventReferenceLogService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult<List<Models.PostbackLogs.EventReferenceLog>>> GetLogs()
        {
            try
            {
                var affiliateId = this.GetAffiliateId();
                var response = await _eventReferenceLogService.GetLogsAsync(
                    new ByAffiliateIdRequest
                    {
                        AffiliateId = affiliateId
                    });

                if (!response.Success)
                {
                    ModelState.AddModelError("Error", response.ErrorMessage);
                    return BadRequest(ModelState);
                }

                return Ok(
                    response.Data is null
                    ? Enumerable.Empty<Models.PostbackLogs.EventReferenceLog>()
                    : response.Data.Select(_mapper.Map<Models.PostbackLogs.EventReferenceLog>));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
