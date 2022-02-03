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
using MarketingBox.AffiliateApi.Helpers;

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
        public async Task<ActionResult<Reference>> GetReferenceAsync()
        {
            try
            {
                var affiliateId = this.GetAffiliateId();
                var result = await _postbackService.GetReferenceAsync(
                    new ByAffiliateIdRequest { AffiliateId = affiliateId });
                return this.ProcessResult(result,_mapper.Map<Reference>(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
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
                return this.ProcessResult(result,_mapper.Map<Reference>(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Reference>> UpdateReferenceAsync(
            [FromBody] ReferenceRequest request)
        {
            try
            {
                request.AffiliateId = this.GetAffiliateId();
                var result = await _postbackService.UpdateReferenceAsync(
                    _mapper.Map<FullReferenceRequest>(request));
                return this.ProcessResult(result,_mapper.Map<Reference>(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteReferenceAsync()
        {
            try
            {
                var affiliateId = this.GetAffiliateId();
                var result = await _postbackService.DeleteReferenceAsync(
                    new ByAffiliateIdRequest { AffiliateId = affiliateId });
                switch (result.StatusCode)
                {
                    case Postback.Service.Grpc.Models.StatusCode.Ok:
                        return Ok();
                    case Postback.Service.Grpc.Models.StatusCode.NotFound:
                        ModelState.AddModelError("Error", result.ErrorMessage);
                        return NotFound(ModelState);
                    case Postback.Service.Grpc.Models.StatusCode.BadRequest:
                        ModelState.AddModelError("Error", result.ErrorMessage);
                        return BadRequest(ModelState);
                    case Postback.Service.Grpc.Models.StatusCode.InternalError:
                        return StatusCode(500);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
