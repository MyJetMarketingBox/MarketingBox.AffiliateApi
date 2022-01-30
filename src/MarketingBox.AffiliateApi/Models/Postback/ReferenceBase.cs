namespace MarketingBox.AffiliateApi.Models.Postback
{
    public class ReferenceBase
    {
        public string RegistrationReference { get; init; }

        public string RegistrationTGReference { get; init; }

        public string DepositReference { get; init; }

        public string DepositTGReference { get; init; }

        public HttpQueryType HttpQueryType { get; set; }
    }
}