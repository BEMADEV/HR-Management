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
using System;

namespace com.bemaservices.HrManagement.Blocks
{
    /// <summary>
    /// Displays a list of pto requests.
    /// </summary>

    [DisplayName( "Pto Request List" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Displays a list of pto requests." )]
    [IconCssClass( "fa fa-list" )]
    [SupportedSiteTypes( SiteType.Web )]

    [LinkedPage( "Detail Page",
        Description = "The page that will show the pto request details.",
        Key = AttributeKey.DetailPage )]

    [Rock.SystemGuid.EntityTypeGuid( "ef3a3fc7-5586-4eb1-83d1-87ef4f188abb" )]
    [Rock.SystemGuid.BlockTypeGuid( "2f76725e-ccf0-4e57-8f2f-54d4e63e6c99" )]
    [CustomizedGrid]
    public class PtoRequestList : RockEntityListBlockType<PtoRequest>
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
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoRequestList.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new ListBlockBox<PtoRequestListOptionsBag>();
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
        private PtoRequestListOptionsBag GetBoxOptions()
        {
            var options = new PtoRequestListOptionsBag();

            return options;
        }

        /// <summary>
        /// Determines if the add button should be enabled in the grid.
        /// </summary>
        /// <returns>A boolean value that indicates if the add button should be enabled.</returns>
        private bool GetIsAddEnabled()
        {
            var entity = new PtoRequest();

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
                [NavigationUrlKey.DetailPage] = this.GetLinkedPageUrl( AttributeKey.DetailPage, "PtoRequestId", "((Key))" )
            };
        }

        /// <inheritdoc/>
        protected override IQueryable<PtoRequest> GetListQueryable( RockContext rockContext )
        {
            return base.GetListQueryable( rockContext )
                .Include( a => a.PtoAllocation );
        }

        /// <inheritdoc/>
        protected override GridBuilder<PtoRequest> GetGridBuilder()
        {
            return new GridBuilder<PtoRequest>()
                .AddTextField( "idKey", a => a.IdKey )
                .AddField( "hours", a => a.Hours )
                .AddTextField( "ptoAllocation", a => throw new NotSupportedException() )
                .AddField( "ptoRequestApprovalState", a => a.PtoRequestApprovalState )
                .AddField( "reason", a => a.Reason )
                .AddDateTimeField( "requestDate", a => a.RequestDate )
                .AddAttributeFields( GetGridAttributes() );
        }

        #endregion

        #region Block Actions

        #endregion
    }
}
