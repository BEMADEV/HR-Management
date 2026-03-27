using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The details for a PTO allocation row under a PTO bracket.
    /// </summary>
    public class PtoBracketTypeBag
    {
        public int DefaultHours { get; set; }

        public string Guid { get; set; }

        public bool IsActive { get; set; }

        public ListItemBag PtoType { get; set; }
    }
}
