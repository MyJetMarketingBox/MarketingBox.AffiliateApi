using Autofac;
using MarketingBox.Postback.Service.Client;
using MarketingBox.Redistribution.Service.Client;

namespace MarketingBox.AffiliateApi.Modules
{
    public class ClientModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterPostbackServiceClient(Program.Settings.PostbackServiceUrl);
            builder.RegisterRedistributionServiceClient(Program.Settings.RedistributionServiceUrl);
        }
    }
}
