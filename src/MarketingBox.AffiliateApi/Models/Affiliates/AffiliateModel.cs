using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketingBox.AffiliateApi.Models.Partners
{
    public class AffiliateModel
    {
        public long AffiliateId { get; set; }

        public AffiliateGeneralInfo GeneralInfo { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
        public long Sequence { get; set; }
    }
}
