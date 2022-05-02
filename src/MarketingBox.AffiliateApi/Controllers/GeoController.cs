using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Requests.Country;
using MarketingBox.AffiliateApi.Models.Country;
using MarketingBox.AffiliateApi.Models.Country.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class GeoController : ControllerBase
    {
        private readonly IGeoService _geoService;
        private readonly IMapper _mapper;

        public GeoController(IGeoService geoService, IMapper mapper)
        {
            _geoService = geoService;
            _mapper = mapper;
        }

        [HttpPost("search")]
        public async Task<ActionResult<Paginated<GeoModel, int?>>> SearchAsync(
            [FromBody] SearchRequest paginationRequest)
        {
            var request = new GeoSearchRequest()
            {
                Asc = paginationRequest.Order == PaginationOrder.Asc,
                Cursor = paginationRequest.Cursor,
                Take = paginationRequest.Limit,
                Name = paginationRequest.Name,
                CountryIds = paginationRequest.CountryIds,
                GeoId = paginationRequest.GeoId
            };
            var response = await _geoService.SearchAllAsync(request);
            return this.ProcessResult(
                response,
                response.Data?
                    .Select(_mapper.Map<GeoModel>)
                    .ToArray()
                    .Paginate(paginationRequest, Url, response.Total ?? default, x => x.Id));
        }

        [HttpPost]
        public async Task<ActionResult<GeoModel>> CreateAsync([FromBody] GeoUpsertRequest upsertRequest)
        {
            var response = await _geoService.CreateAsync(_mapper.Map<GeoCreateRequest>(upsertRequest));
            return this.ProcessResult(response, _mapper.Map<GeoModel>(response.Data));
        }

        [HttpPut("{geoId}")]
        public async Task<ActionResult<GeoModel>> UpdateAsync(
            [FromRoute] int geoId,
            [FromBody] GeoUpsertRequest updateUpsertRequest)
        {
            var request = _mapper.Map<GeoUpdateRequest>(updateUpsertRequest);
            request.Id = geoId;
            var response = await _geoService.UpdateAsync(request);
            return this.ProcessResult(response, _mapper.Map<GeoModel>(response.Data));
        }

        [HttpDelete("{geoId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int geoId)
        {
            var response = await _geoService.DeleteAsync(new GeoByIdRequest {GeoId = geoId});
            return this.ProcessResult(response);
        }
    }
}