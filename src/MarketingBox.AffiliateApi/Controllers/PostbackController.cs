using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Postback.Requests;
using MarketingBox.Postback.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MarketingBox.Postback.Service.Domain.Models;
using MarketingBox.Sdk.Common.Extensions;
using Reference = MarketingBox.AffiliateApi.Models.Postback.Reference;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AffiliateAndHigher)]
    [ApiController]
    [Route("/api/postback/")]
    public class PostbackController : ControllerBase
    {
        private readonly IPostbackService _postbackService;
        private readonly IMapper _mapper;
        private readonly ILogger<PostbackController> _logger;

        public PostbackController(
            IPostbackService postbackService,
            IMapper mapper,
            ILogger<PostbackController> logger)
        {
            _postbackService = postbackService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Reference>> GetAsync()
        {
            var affiliateId = this.GetAffiliateId();
            var result = await _postbackService.GetAsync(
                new ByAffiliateIdRequest {AffiliateId = affiliateId});

            return this.ProcessResult(result, _mapper.Map<Reference>(result.Data));
        }

        [HttpPost]
        public async Task<ActionResult<Reference>> CreateAsync(
            [FromBody] ReferenceRequest request)
        {
            request.AffiliateId = this.GetAffiliateId();
            var result = await _postbackService.CreateAsync(
                _mapper.Map<MarketingBox.Postback.Service.Domain.Models.Reference>(request));
            return this.ProcessResult(result, _mapper.Map<Reference>(result.Data));
        }

        [HttpPut]
        public async Task<ActionResult<Reference>> UpdateAsync(
            [FromBody] ReferenceRequest request)
        {
            request.AffiliateId = this.GetAffiliateId();
            var result = await _postbackService.UpdateAsync(
                _mapper.Map<MarketingBox.Postback.Service.Domain.Models.Reference>(request));

            return this.ProcessResult(result, _mapper.Map<Reference>(result.Data));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            var affiliateId = this.GetAffiliateId();
            var result = await _postbackService.DeleteAsync(
                new ByAffiliateIdRequest {AffiliateId = affiliateId});
            this.ProcessResult(result, true);
            return Ok();
        }
    }
}