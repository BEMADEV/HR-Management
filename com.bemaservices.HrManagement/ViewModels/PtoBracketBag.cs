using Rock.ViewModels.Utility;
using System.Collections.Generic;

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

        public List<PtoBracketTypeBag> PtoBracketTypes { get; set; }
    }
}
