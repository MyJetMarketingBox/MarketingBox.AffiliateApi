using Autofac;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Postback.Service.Client;
using MarketingBox.Redistribution.Service.Client;
using MarketingBox.Reporting.Service.Client;

namespace MarketingBox.AffiliateApi.Modules
{
    public class ClientModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterPostbackServiceClient(Program.Settings.PostbackServiceUrl);
            builder.RegisterRedistributionServiceClient(Program.Settings.RedistributionServiceUrl);
            builder.RegisterAffiliateServiceClient(Program.Settings.AffiliateServiceUrl);
            builder.RegisterReportingServiceClient(Program.Settings.ReportingServiceUrl);
        }
    }
}
