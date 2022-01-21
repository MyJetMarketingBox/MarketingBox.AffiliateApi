using Autofac;
using MarketingBox.Postback.Service.Client;

namespace MarketingBox.AffiliateApi.Modules
{
    public class ClientModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterPostbackServiceClient(Program.Settings.PostbackServiceUrl);
        }
    }
}
