using Autofac;
using MarketingBox.Affiliate.Service.Messages;
using MarketingBox.Affiliate.Service.Messages.Affiliates;
using MyJetWallet.Sdk.ServiceBus;

namespace MarketingBox.AffiliateApi.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var serviceBusClient = builder
                .RegisterMyServiceBusTcpClient(
                    Program.ReloadedSettings(e => e.MarketingBoxServiceBusHostPort),
                    Program.LogFactory);
            builder.RegisterMyServiceBusPublisher<AffiliateDeleteMessage>(serviceBusClient, Topics.AffiliateInitDeleteTopic, false);
        }
    }
}
