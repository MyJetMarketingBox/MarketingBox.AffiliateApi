using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Campaigns;
using MarketingBox.Affiliate.Service.Grpc.Requests.Campaigns;
using MarketingBox.AffiliateApi.Models.Campaigns;
using MarketingBox.AffiliateApi.Models.Campaigns.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class CampaignMapperProfile:Profile
    {
        public CampaignMapperProfile()
        {
            CreateMap<CampaignUpsertRequest, CampaignCreateRequest>();
            CreateMap<CampaignUpsertRequest, CampaignUpdateRequest>();
            CreateMap<Campaign, CampaignModel>();
        }
    }
}