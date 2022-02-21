using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.Postback.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Helpers;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Postback.Service.Domain.Models;
using EventReferenceLog = MarketingBox.AffiliateApi.Models.PostbackLogs.EventReferenceLog;

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

        [HttpGet]
        public async Task<ActionResult<Paginated<EventReferenceLog, long>>> GetLogs(
            [FromQuery] PaginationRequest<long> paginationRequest)
        {
            try
            {
                var request = new ByAffiliateIdPaginatedRequest
                {
                    Asc = paginationRequest.Order == PaginationOrder.Asc,
                    Cursor = paginationRequest.Cursor,
                    Take = paginationRequest.Limit,
                    AffiliateId = this.GetAffiliateId()
                };
                var response = await _eventReferenceLogService.GetLogsAsync(request);
                return this.ProcessResult(
                    response,
                response.Data is null ?
                    Paginated.Empty<EventReferenceLog,long>(paginationRequest) :
                    response.Data.Select(_mapper.Map<EventReferenceLog>)
                        .ToArray().Paginate(paginationRequest, Url, x => x.Id));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
