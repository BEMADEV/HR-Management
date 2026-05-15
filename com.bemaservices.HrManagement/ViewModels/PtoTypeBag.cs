using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The item details for the PTO Type list modal editor.
    /// </summary>
    public class PtoTypeBag : EntityBagBase
    {
        public string Color { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsNegativeTimeBalanceAllowed { get; set; }

        public string Name { get; set; }

        public ListItemBag WorkflowType { get; set; }
    }
}
