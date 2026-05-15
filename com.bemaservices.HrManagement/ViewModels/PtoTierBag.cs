using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The item details for the Pto Tier Detail block.
    /// </summary>
    public class PtoTierBag : EntityBagBase
    {
        public string Color { get; set; }

        public string[] DaysOfWeek { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }
    }
}
