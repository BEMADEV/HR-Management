using System.Collections.Generic;

using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The additional configuration options for the HR Employee List block.
    /// </summary>
    public class HrEmployeeListOptionsBag
    {
        /// <summary>
        /// Gets or sets the list of active PTO types for filtering.
        /// </summary>
        public List<ListItemBag> PtoTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of fiscal year options.
        /// </summary>
        public List<ListItemBag> FiscalYears { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the supervisor filter should be shown.
        /// </summary>
        public bool ShowSupervisorFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ministry area filter should be shown.
        /// </summary>
        public bool ShowMinistryAreaFilter { get; set; }
    }
}
