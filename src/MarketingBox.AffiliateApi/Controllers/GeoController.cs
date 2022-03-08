using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.Country;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;
using Geo = MarketingBox.AffiliateApi.Models.Country.Geo;
using ApiModel = MarketingBox.AffiliateApi.Models.Country.Requests;
using GrpcModel = MarketingBox.Affiliate.Service.Grpc.Models.Country;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Route("/api/geo")]
    public class GeoController : ControllerBase
    {
        private readonly IGeoService _geoService;
        private readonly IMapper _mapper;

        public GeoController(IGeoService geoService, IMapper mapper)
        {
            _geoService = geoService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<Paginated<Geo, int>>> GetAllAsync(
            [FromQuery] PaginationRequest<int> paginationRequest)
        {
            var asc = paginationRequest.Order == PaginationOrder.Asc;
            if (!asc && paginationRequest.Cursor == 0)
            {
                paginationRequest.Cursor = paginationRequest.Limit;
            }

            var request = new GetAllRequest
            {
                Asc = asc,
                Cursor = paginationRequest.Cursor,
                Take = paginationRequest.Limit
            };
            var response = await _geoService.GetAllAsync(request);
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<Geo>)
                    .ToArray()
                    .Paginate(paginationRequest, Url, x => x.Id));
        }
        
        [HttpPost]
        public async Task<ActionResult<Geo>> CreateAsync([FromBody] ApiModel.GeoRequest request)
        {
            var response = await _geoService.CreateAsync(_mapper.Map<GeoCreateRequest>(request));
            return this.ProcessResult(response, _mapper.Map<Geo>(response.Data));
        }

        [HttpPut("/api/geo/{geoId}")]
        public async Task<ActionResult<Geo>> UpdateAsync(
            [FromRoute] int geoId,
            [FromBody] ApiModel.GeoRequest updateRequest)
        {
            var request = _mapper.Map<GeoUpdateRequest>(updateRequest);
            request.Id = geoId;
            var response = await _geoService.UpdateAsync(request);
            return this.ProcessResult(response, _mapper.Map<Geo>(response.Data));
        }

        [HttpDelete("/api/geo/{geoId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int geoId)
        {
            var response = await _geoService.DeleteAsync(new GeoByIdRequest {CountryBoxId = geoId});
            return this.ProcessResult(response);
        }
    }
}