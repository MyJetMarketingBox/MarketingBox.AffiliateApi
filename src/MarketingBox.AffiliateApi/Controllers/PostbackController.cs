using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Postback;
using MarketingBox.AffiliateApi.Models.Postback.Requests;
using MarketingBox.Postback.Service.Grpc;
using MarketingBox.Postback.Service.Grpc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

        [HttpGet()]
        public async Task<ActionResult<Reference>> GetReferenceAsync()
        {
            try
            {
                var affiliateId = this.GetAffiliateId();
                var result = await _postbackService.GetReferenceAsync(
                    new ByAffiliateIdRequest { AffiliateId = affiliateId });
                if (!result.Success)
                {
                    ModelState.AddModelError("Error", result.ErrorMessage);
                    return BadRequest(ModelState);
                }
                return Ok(_mapper.Map<Reference>(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Reference>> CreateReferenceAsync(
            [FromBody] ReferenceRequest request)
        {
            try
            {
                request.AffiliateId = this.GetAffiliateId();
                var result = await _postbackService.CreateReferenceAsync(
                    _mapper.Map<FullReferenceRequest>(request));
                if (!result.Success)
                {
                    ModelState.AddModelError("Error", result.ErrorMessage);
                    return BadRequest(ModelState);
                }
                return Ok(_mapper.Map<Reference>(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPut()]
        public async Task<ActionResult<Reference>> UpdateReferenceAsync(
            [FromBody] ReferenceRequest request)
        {
            try
            {
                request.AffiliateId = this.GetAffiliateId();
                var result = await _postbackService.UpdateReferenceAsync(
                    _mapper.Map<FullReferenceRequest>(request));
                if (!result.Success)
                {
                    ModelState.AddModelError("Error", result.ErrorMessage);
                    return BadRequest(ModelState);
                }
                return Ok(_mapper.Map<Reference>(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }

        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteReferenceAsync()
        {
            try
            {
                var affiliateId = this.GetAffiliateId();
                var result = await _postbackService.DeleteReferenceAsync(
                    new ByAffiliateIdRequest { AffiliateId = affiliateId });
                if (!result.Success)
                {
                    ModelState.AddModelError("Error", result.ErrorMessage);
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
