using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.CampaignRows;
using MarketingBox.Affiliate.Service.Grpc.Requests.CampaignRows;
using MarketingBox.AffiliateApi.Models.CampaignRows;
using MarketingBox.AffiliateApi.Models.CampaignRows.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class CampaignRowMapperProfile:Profile
    {
        public CampaignRowMapperProfile()
        {
            CreateMap<CampaignRowUpsertRequest, CampaignRowCreateRequest>();
            CreateMap<CampaignRowUpsertRequest, CampaignRowUpdateRequest>();
            CreateMap<CampaignRow, CampaignRowModel>()
                .ForMember(x => x.CampaignRowId, x => x.MapFrom(z => z.Id))
                .ForMember(x => x.GeoId, x => x.MapFrom(z => z.Geo.Id))
                .ForMember(x => x.GeoName, x => x.MapFrom(z => z.Geo.Name));
        }
    }
}