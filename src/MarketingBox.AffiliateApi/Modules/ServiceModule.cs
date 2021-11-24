using Autofac;
using MarketingBox.Affiliate.Service.Client;
using MarketingBox.Affiliate.Service.Messages;
using MarketingBox.Affiliate.Service.Messages.Affiliates;
using MarketingBox.Registration.Service.Client;
using MarketingBox.Reporting.Service.Client;
using MyJetWallet.Sdk.ServiceBus;

namespace MarketingBox.AffiliateApi.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAffiliateServiceClient(Program.Settings.AffiliateServiceUrl);
            builder.RegisterReportingServiceClient(Program.Settings.ReportingServiceUrl);
            builder.RegisterRegistrationServiceClient(Program.Settings.RegistrationServiceUrl);
            
            var serviceBusClient = builder
                .RegisterMyServiceBusTcpClient(
                    Program.ReloadedSettings(e => e.MarketingBoxServiceBusHostPort),
                    Program.LogFactory);
            builder.RegisterMyServiceBusPublisher<AffiliateDeleteMessage>(serviceBusClient, Topics.AffiliateInitDeleteTopic, false);
        }
    }
}
