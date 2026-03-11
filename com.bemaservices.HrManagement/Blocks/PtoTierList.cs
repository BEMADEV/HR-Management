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
    /// Displays a list of pto tiers.
    /// </summary>

    [DisplayName( "Pto Tier List" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Displays a list of pto tiers." )]
    [IconCssClass( "fa fa-list" )]
    [SupportedSiteTypes( SiteType.Web )]

    [LinkedPage( "Detail Page",
        Description = "The page that will show the pto tier details.",
        Key = AttributeKey.DetailPage )]

    [Rock.SystemGuid.EntityTypeGuid( "01dac091-f36f-48b4-b02b-e65508a469ae" )]
    [Rock.SystemGuid.BlockTypeGuid( "37f2dda9-cb72-4fac-950f-b260a6796230" )]
    [CustomizedGrid]
    public class PtoTierList : RockEntityListBlockType<PtoTier>
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
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoTierList.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new ListBlockBox<PtoTierListOptionsBag>();
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
        private PtoTierListOptionsBag GetBoxOptions()
        {
            var options = new PtoTierListOptionsBag();

            return options;
        }

        /// <summary>
        /// Determines if the add button should be enabled in the grid.
        /// </summary>
        /// <returns>A boolean value that indicates if the add button should be enabled.</returns>
        private bool GetIsAddEnabled()
        {
            var entity = new PtoTier();

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
                [NavigationUrlKey.DetailPage] = this.GetLinkedPageUrl( AttributeKey.DetailPage, "PtoTierId", "((Key))" )
            };
        }

        /// <inheritdoc/>
        protected override IQueryable<PtoTier> GetListQueryable( RockContext rockContext )
        {
            return base.GetListQueryable( rockContext );
        }

        /// <inheritdoc/>
        protected override GridBuilder<PtoTier> GetGridBuilder()
        {
            return new GridBuilder<PtoTier>()
                .WithBlock( this )
                .AddTextField( "idKey", a => a.IdKey )
                .AddField( "name", a => a.Name )
                .AddAttributeFields( GetGridAttributes() );
        }

        #endregion

        #region Block Actions

        #endregion
    }
}
