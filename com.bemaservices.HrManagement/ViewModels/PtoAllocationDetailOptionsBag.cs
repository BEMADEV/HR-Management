using System.Collections.Generic;

using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The additional configuration options for the Pto Allocation Detail block.
    /// </summary>
    public class PtoAllocationDetailOptionsBag
    {
        /// <summary>
        /// Gets or sets the PTO types available for selection.
        /// </summary>
        public List<ListItemBag> PtoTypes { get; set; }
    }
}
