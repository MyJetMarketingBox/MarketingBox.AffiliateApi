using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.Postback.Service.Grpc;
using MarketingBox.Postback.Service.Grpc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IEventReferenceLogService _eventReferenceLogService;

        public PostbackLogsController(
            IEventReferenceLogService eventReferenceLogService,
            IMapper mapper)
        {
            _eventReferenceLogService = eventReferenceLogService;
            _mapper = mapper;
        }

        [HttpGet("{affiliateId}")]
        public async Task<ActionResult<List<Models.PostbackLogs.EventReferenceLog>>> GetLogs(
            [FromRoute]long affiliateId)
        {
            var response = await _eventReferenceLogService.GetLogsAsync(
                new ByAffiliateIdRequest
                {
                    AffiliateId = affiliateId
                });

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(
                response.Data is null 
                ? Enumerable.Empty<Models.PostbackLogs.EventReferenceLog>()
                : response.Data.Select(_mapper.Map<Models.PostbackLogs.EventReferenceLog>));
        }
    }
}
