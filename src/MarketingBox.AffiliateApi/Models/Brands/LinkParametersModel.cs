using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Brands
{
    public class LinkParametersModel
    {
        [Required]
        public string ClickId { get; set; }
        public string Language { get; set; }
        public string MPC_1 { get; set; }
        public string MPC_2 { get; set; }
        public string MPC_3 { get; set; }
        public string MPC_4 { get; set; }
    }
}