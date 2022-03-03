using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.Postback.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Postback.Service.Domain.Models;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
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
            var asc = paginationRequest.Order == PaginationOrder.Asc;
            if (!asc && paginationRequest.Cursor == 0)
            {
                paginationRequest.Cursor = paginationRequest.Limit;
            }

            var request = new ByAffiliateIdPaginatedRequest
            {
                Asc = asc,
                Cursor = paginationRequest.Cursor,
                Take = paginationRequest.Limit,
                AffiliateId = this.GetAffiliateId()
            };
            var response = await _eventReferenceLogService.GetAsync(request);
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<EventReferenceLog>)
                    .ToArray()
                    .Paginate(paginationRequest, Url, x => x.Id));
        }
    }
}