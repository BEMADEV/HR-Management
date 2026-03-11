using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The item details for the Pto Bracket Detail block.
    /// </summary>
    public class PtoBracketBag : EntityBagBase
    {
        public bool IsActive { get; set; }

        public int? MaximumYear { get; set; }

        public int MinimumYear { get; set; }
    }
}
