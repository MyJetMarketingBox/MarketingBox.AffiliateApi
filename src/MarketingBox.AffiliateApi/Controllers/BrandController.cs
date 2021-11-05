using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.Campaigns.Requests;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Campaigns;
using MarketingBox.AffiliateApi.Models.Campaigns.Requests;
using MarketingBox.AffiliateApi.Models.Partners;
using Microsoft.AspNetCore.Authorization;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/brands")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<BrandModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<BrandModel, long>>> SearchAsync(
            [FromQuery] BrandsSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should not be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();
            var status = request.Status?.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Brands.BrandStatus>();

            var response = await _brandService.SearchAsync(new ()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                BrandId = request.Id,
                IntegrationId = request.IntegrationId,
                Name = request.Name,
                Status = status,
                Take = request.Limit,
                TenantId = tenantId
            });

            return Ok(
                response.Campaigns.Select(Map)
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet("{brandId}")]
        [ProducesResponseType(typeof(BrandModel), StatusCodes.Status200OK)]

        public async Task<ActionResult<BrandModel>> GetAsync(
            [FromRoute, Required] long brandId)
        {
            var tenantId = this.GetTenantId();
            var response =await _brandService.GetAsync(new () {BrandId = brandId});

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(BrandModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<BrandModel>> CreateAsync(
            [FromBody] BrandCreateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandService.CreateAsync(new ()
            {
                IntegrationId = request.IntegrationId,
                Name = request.Name,
                TenantId = tenantId,
                Payout = new ()
                {
                    Currency = request.Payout.Currency.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Amount = request.Payout.Amount,
                    Plan = request.Payout.Plan.MapEnum< MarketingBox.Affiliate.Service.Domain.Models.Brands.Plan>()
                },
                Status = request.Status.MapEnum<  MarketingBox.Affiliate.Service.Domain.Models.Brands.BrandStatus>(),
                Privacy = request.Privacy.MapEnum< MarketingBox.Affiliate.Service.Domain.Models.Brands.BrandPrivacy >(),
                Revenue = new ()
                {
                    Currency = request.Revenue.Currency.MapEnum< MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Amount = request.Revenue.Amount,
                    Plan = request.Revenue.Plan.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Brands.Plan>()
                }
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpPut("{brandId}")]
        [ProducesResponseType(typeof(BrandModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<BrandModel>> UpdateAsync(
            [Required, FromRoute] long brandId,
            [FromBody] BrandUpdateRequest request)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandService.UpdateAsync(new ()
            {
                Id = brandId,
                Sequence = request.Sequence,
                IntegrationId = request.IntegrationId,
                Name = request.Name,
                TenantId = tenantId,
                Payout = new ()
                {
                    Currency = request.Payout.Currency.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Amount = request.Payout.Amount,
                    Plan = request.Payout.Plan.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Brands.Plan>()
                },
                Status = request.Status.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Brands.BrandStatus>(),
                Privacy = request.Privacy.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Brands.BrandPrivacy>(),
                Revenue = new ()
                {
                    Currency = request.Revenue.Currency.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Common.Currency>(),
                    Amount = request.Revenue.Amount,
                    Plan = request.Revenue.Plan.MapEnum<MarketingBox.Affiliate.Service.Domain.Models.Brands.Plan>()
                }
            });

            return MapToResponse(response);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpDelete("{brandId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(
            [Required, FromRoute] long brandId)
        {
            var tenantId = this.GetTenantId();
            var response = await _brandService.DeleteAsync(new ()
            {
                BrandId = brandId
            });

            return MapToResponseEmpty(response);
        }

        private ActionResult MapToResponse(Affiliate.Service.Grpc.Models.Brands.BrandResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Brand == null)
                return NotFound();

            return Ok(Map(response.Brand));
        }

        private static BrandModel Map(Affiliate.Service.Grpc.Models.Brands.Brand brand)
        {
            return new BrandModel()
            {
                Name = brand.Name,
                IntegrationId = brand.IntegrationId,
                Id = brand.Id,
                Payout = new Payout()
                {
                    Currency = brand.Payout.Currency.MapEnum<Currency>(),
                    Amount = brand.Payout.Amount,
                    Plan = brand.Payout.Plan.MapEnum<Plan>()
                },
                Privacy = brand.Privacy.MapEnum<BrandPrivacy>(),
                Revenue = new Revenue()
                {
                    Currency = brand.Revenue.Currency.MapEnum<Currency>(),
                    Amount = brand.Revenue.Amount,
                    Plan = brand.Revenue.Plan.MapEnum<Plan>()
                },
                Status = brand.Status.MapEnum<BrandStatus>(),
                Sequence = brand.Sequence
            };
        }

        private ActionResult MapToResponseEmpty(Affiliate.Service.Grpc.Models.Brands.BrandResponse response)
        {
            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}