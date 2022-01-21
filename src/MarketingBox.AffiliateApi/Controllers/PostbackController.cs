using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Models.Postback;
using MarketingBox.AffiliateApi.Models.Postback.Requests;
using MarketingBox.Postback.Service.Grpc;
using MarketingBox.Postback.Service.Grpc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.AffiliateAndHigher)]
    [ApiController]
    [Route("/api/postback/")]
    public class PostbackController : ControllerBase
    {
        private readonly IPostbackService _postbackService;
        private readonly IMapper _mapper;

        public PostbackController(
            IPostbackService postbackService,
            IMapper mapper)
        {
            _postbackService = postbackService;
            _mapper = mapper;
        }

        [HttpGet("affiliates/{affiliateId}")]
        public async Task<ActionResult<Reference>> GetReferenceAsync(
            [FromRoute] long affiliateId)
        {
            var result = await _postbackService.GetReferenceAsync(
                new ReferenceByAffiliateRequest { AffiliateId = affiliateId });
            if (!result.Success)
            {
                ModelState.AddModelError("Error", result.ErrorMessage);
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<Reference>(result.Data));
        }

        [HttpPost]
        public async Task<ActionResult<Reference>> CreateReferenceAsync(
            [FromBody] ReferenceCreateRequest request)
        {
            if (!request.AffiliateId.HasValue)
            {
                ModelState.AddModelError("Error", "AffilateId must be specified.");
                return BadRequest(ModelState);
            }
            var result = await _postbackService.CreateReferenceAsync(
                _mapper.Map<FullReferenceRequest>(request));
            if (!result.Success)
            {
                ModelState.AddModelError("Error", result.ErrorMessage);
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<Reference>(result.Data));
        }

        [HttpPut("affiliates/{affiliateId}")]
        public async Task<ActionResult<Reference>> UpdateReferenceAsync(
            [FromRoute] long affiliateId,
            [FromBody] ReferenceUpdateRequest request)
        {
            request.AffiliateId = affiliateId;
            var result = await _postbackService.UpdateReferenceAsync(
                _mapper.Map<FullReferenceRequest>(request));
            if (!result.Success)
            {
                ModelState.AddModelError("Error", result.ErrorMessage);
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<Reference>(result.Data));

        }

        [HttpDelete("affiliates/{affiliateId}")]
        public async Task<ActionResult> DeleteReferenceAsync(
            [FromRoute] long affiliateId)
        {
            var result = await _postbackService.DeleteReferenceAsync(
                new ReferenceByAffiliateRequest { AffiliateId = affiliateId });
            if (!result.Success)
            {
                ModelState.AddModelError("Error", result.ErrorMessage);
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}
