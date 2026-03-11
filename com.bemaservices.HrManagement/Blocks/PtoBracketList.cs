using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

using Rock;
using Rock.Attribute;
using Rock.Blocks;
using Rock.Data;
using Rock.Model;
using Rock.Obsidian.UI;
using Rock.Security;
using Rock.ViewModels.Blocks;
using Rock.Web.Cache;

using com.bemaservices.HrManagement.Model;
using com.bemaservices.HrManagement.ViewModels;

namespace com.bemaservices.HrManagement.Blocks
{
    /// <summary>
    /// Displays a list of pto brackets.
    /// </summary>

    [DisplayName( "Pto Bracket List" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Displays a list of pto brackets." )]
    [IconCssClass( "fa fa-list" )]
    [SupportedSiteTypes( SiteType.Web )]

    [LinkedPage( "Detail Page",
        Description = "The page that will show the pto bracket details.",
        Key = AttributeKey.DetailPage )]

    [Rock.SystemGuid.EntityTypeGuid( "214745dc-9f60-47ef-955c-6b31359033dc" )]
    [Rock.SystemGuid.BlockTypeGuid( "09b6ad11-e215-4bd0-a955-b19b38c05dff" )]
    [CustomizedGrid]
    public class PtoBracketList : RockEntityListBlockType<PtoBracket>
    {
        #region Keys

        private static class AttributeKey
        {
            public const string DetailPage = "DetailPage";
        }

        private static class NavigationUrlKey
        {
            public const string DetailPage = "DetailPage";
        }

        #endregion Keys

        #region Properties

        /// <inheritdoc/>
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoBracketList.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new ListBlockBox<PtoBracketListOptionsBag>();
            var builder = GetGridBuilder();

            box.IsAddEnabled = GetIsAddEnabled();
            box.IsDeleteEnabled = false;
            box.ExpectedRowCount = null;
            box.NavigationUrls = GetBoxNavigationUrls();
            box.Options = GetBoxOptions();
            box.GridDefinition = builder.BuildDefinition();

            return box;
        }

        /// <summary>
        /// Gets the box options required for the component to render the list.
        /// </summary>
        /// <returns>The options that provide additional details to the block.</returns>
        private PtoBracketListOptionsBag GetBoxOptions()
        {
            var options = new PtoBracketListOptionsBag();

            return options;
        }

        /// <summary>
        /// Determines if the add button should be enabled in the grid.
        /// </summary>
        /// <returns>A boolean value that indicates if the add button should be enabled.</returns>
        private bool GetIsAddEnabled()
        {
            var entity = new PtoBracket();

            return entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson );
        }

        /// <summary>
        /// Gets the box navigation URLs required for the page to operate.
        /// </summary>
        /// <returns>A dictionary of key names and URL values.</returns>
        private Dictionary<string, string> GetBoxNavigationUrls()
        {
            return new Dictionary<string, string>
            {
                [NavigationUrlKey.DetailPage] = this.GetLinkedPageUrl( AttributeKey.DetailPage, "PtoBracketId", "((Key))" )
            };
        }

        /// <inheritdoc/>
        protected override IQueryable<PtoBracket> GetListQueryable( RockContext rockContext )
        {
            return base.GetListQueryable( rockContext );
        }

        /// <inheritdoc/>
        protected override GridBuilder<PtoBracket> GetGridBuilder()
        {
            return new GridBuilder<PtoBracket>()
                .WithBlock( this )
                .AddTextField( "idKey", a => a.IdKey )
                .AddField( "isActive", a => a.IsActive )
                .AddField( "maximumYear", a => a.MaximumYear )
                .AddField( "minimumYear", a => a.MinimumYear )
                .AddAttributeFields( GetGridAttributes() );
        }

        #endregion

        #region Block Actions

        #endregion
    }
}
