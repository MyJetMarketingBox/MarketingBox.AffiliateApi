using System;
using System.Linq;
using AutoMapper;
using MarketingBox.AffiliateApi.Models.Postback.Requests;
using MarketingBox.Postback.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MarketingBox.Postback.Service.Domain.Models.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Reference = MarketingBox.AffiliateApi.Models.Postback.Reference;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class PostbacksController : ControllerBase
    {
        private readonly IPostbackService _postbackService;
        private readonly IMapper _mapper;
        private readonly ILogger<PostbacksController> _logger;

        public PostbacksController(
            IPostbackService postbackService,
            IMapper mapper,
            ILogger<PostbacksController> logger)
        {
            _postbackService = postbackService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{postbackId}")]
        public async Task<ActionResult<Reference>> GetAsync([FromRoute] long postbackId)
        {
            var affiliateId = this.GetUserId();
            var tenant = this.GetTenantId();
            var result = await _postbackService.GetAsync(
                new ByIdRequest()
                {
                    PostbackId = postbackId,
                    RequestedBy = affiliateId,
                    TenantId = tenant
                });

            return this.ProcessResult(result, _mapper.Map<Reference>(result.Data));
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<Paginated<Reference, long?>>> SearchAsync(
            [FromQuery] MarketingBox.AffiliateApi.Models.Postback.Requests.SearchReferenceRequest paginationLogsRequest)
        {
            var tenantId = this.GetTenantId();
            var response = await _postbackService.SearchAsync(new() 
            {
                Asc = paginationLogsRequest.Order == PaginationOrder.Asc,
                Cursor = paginationLogsRequest.Cursor,
                Take = paginationLogsRequest.Limit,
                
                AffiliateName = paginationLogsRequest.AffiliateName,
                AffiliateIds = paginationLogsRequest.AffiliateIds.Parse<long>(),
                HttpQueryType = paginationLogsRequest.HttpQueryType,
                TenantId = tenantId
            });

            return this.ProcessResult(
                response,
                (response.Data?
                    .Select(_mapper.Map<Reference>)
                    .ToArray() ?? Array.Empty<Reference>())
                    .Paginate(paginationLogsRequest, Url, response.Total ?? default, x => x.Id));
        }

        [HttpPost]
        public async Task<ActionResult<Reference>> CreateAsync(
            [FromBody] ReferenceCreateRequest request)
        {
            var grpcRequest =
                _mapper.Map<CreateReferenceRequest>(request);
            var userId = this.GetUserId();
            
            grpcRequest.TenantId = this.GetTenantId();
            grpcRequest.CreatedBy = userId;
            grpcRequest.AffiliateId ??= userId;
            var result = await _postbackService.CreateAsync(grpcRequest);
            return this.ProcessResult(result, _mapper.Map<Reference>(result.Data));
        }

        [HttpPut("{postbackId}")]
        public async Task<ActionResult<Reference>> UpdateAsync(
            [FromRoute] long postbackId,
            [FromBody] ReferenceUpdateRequest request)
        {
            var grpcRequest =
                _mapper.Map<UpdateReferenceRequest>(request);
            grpcRequest.PostbackId = postbackId;
            grpcRequest.RequestedBy = this.GetUserId();
            grpcRequest.TenantId = this.GetTenantId();
            var result = await _postbackService.UpdateAsync(grpcRequest);

            return this.ProcessResult(result, _mapper.Map<Reference>(result.Data));
        }

        [HttpDelete("{postbackId}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] long postbackId)
        {
            var affiliateId = this.GetUserId();
            var tenant = this.GetTenantId();
            var result = await _postbackService.DeleteAsync(
                new ByIdRequest()
                {
                    PostbackId = postbackId,
                    RequestedBy = affiliateId,
                    TenantId = tenant
                });
            this.ProcessResult(result, true);
            return Ok();
        }
    }
}