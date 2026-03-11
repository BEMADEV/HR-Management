using System;

using com.bemaservices.HrManagement.Enums;

using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The item details for the Pto Allocation Detail block.
    /// </summary>
    public class PtoAllocationBag : EntityBagBase
    {
        public DateTime? EndDate { get; set; }

        public decimal Hours { get; set; }

        public string Note { get; set; }

        public ListItemBag PersonAlias { get; set; }

        public PtoAllocationSourceType PtoAllocationSourceType { get; set; }

        public PtoAllocationStatus PtoAllocationStatus { get; set; }

        public ListItemBag PtoType { get; set; }

        public DateTime StartDate { get; set; }
    }
}
