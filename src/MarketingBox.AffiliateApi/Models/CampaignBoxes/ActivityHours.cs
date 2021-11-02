using System;

namespace MarketingBox.AffiliateApi.Models.CampaignBoxes
{
    public class ActivityHours
    {
        public DayOfWeek Day { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan? From { get; set; }

        public TimeSpan? To { get; set; }
    }
}