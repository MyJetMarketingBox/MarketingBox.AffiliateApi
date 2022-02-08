namespace MarketingBox.AffiliateApi.Models.Registrations
{
    public class RegistrationModel
    {
        public long RegistrationId { get; set; }

        public RegistrationGeneralInfo GeneralInfo { get; set; }

        public RegistrationRouteInfo RouteInfo { get; set; }

        public RegistrationAdditionalInfo AdditionalInfo { get; set; }

        public RegistrationStatus Status { get; set; }

    }

    public class RegistrationModelForAffiliate
    {
        public long RegistrationId { get; set; }

        public RegistrationGeneralInfoForAffiliate GeneralInfo { get; set; }

        public RegistrationStatus Status { get; set; }

    }
}
