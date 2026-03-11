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
    /// Displays a list of pto allocations.
    /// </summary>

    [DisplayName( "Pto Allocation List" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Displays a list of pto allocations." )]
    [IconCssClass( "fa fa-list" )]
    [SupportedSiteTypes( SiteType.Web )]

    [LinkedPage( "Detail Page",
        Description = "The page that will show the pto allocation details.",
        Key = AttributeKey.DetailPage )]

    [Rock.SystemGuid.EntityTypeGuid( "d2bfe182-0d87-4d3a-8489-106556c00167" )]
    [Rock.SystemGuid.BlockTypeGuid( "0ee8b9f0-9cca-46ce-b4a2-dab79e51df9c" )]
    [CustomizedGrid]
    public class PtoAllocationList : RockEntityListBlockType<PtoAllocation>
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
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoAllocationList.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new ListBlockBox<PtoAllocationListOptionsBag>();
            var builder = GetGridBuilder();

            box.IsAddEnabled = GetIsAddEnabled();
            box.IsDeleteEnabled = true;
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
        private PtoAllocationListOptionsBag GetBoxOptions()
        {
            var options = new PtoAllocationListOptionsBag();

            return options;
        }

        /// <summary>
        /// Determines if the add button should be enabled in the grid.
        /// </summary>
        /// <returns>A boolean value that indicates if the add button should be enabled.</returns>
        private bool GetIsAddEnabled()
        {
            var entity = new PtoAllocation();

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
                [NavigationUrlKey.DetailPage] = this.GetLinkedPageUrl( AttributeKey.DetailPage, "PtoAllocationId", "((Key))" )
            };
        }

        /// <inheritdoc/>
        protected override IQueryable<PtoAllocation> GetListQueryable( RockContext rockContext )
        {
            return base.GetListQueryable( rockContext )
                .Include( a => a.PersonAlias )
                .Include( a => a.PtoType );
        }

        /// <inheritdoc/>
        protected override GridBuilder<PtoAllocation> GetGridBuilder()
        {
            return new GridBuilder<PtoAllocation>()
                .WithBlock( this )
                .AddTextField( "idKey", a => a.IdKey )
                .AddDateTimeField( "endDate", a => a.EndDate )
                .AddField( "hours", a => a.Hours )
                .AddPersonField( "", a => a.PersonAlias?.Person )
                .AddField( "ptoAllocationSourceType", a => a.PtoAllocationSourceType )
                .AddField( "ptoAllocationStatus", a => a.PtoAllocationStatus )
                .AddTextField( "ptoType", a => a.PtoType?.Name )
                .AddDateTimeField( "startDate", a => a.StartDate )
                .AddAttributeFields( GetGridAttributes() );
        }

        #endregion

        #region Block Actions

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="key">The identifier of the entity to be deleted.</param>
        /// <returns>An empty result that indicates if the operation succeeded.</returns>
        [BlockAction]
        public BlockActionResult Delete( string key )
        {
            var entityService = new PtoAllocationService( RockContext );
            var entity = entityService.Get( key, !PageCache.Layout.Site.DisablePredictableIds );

            if ( entity == null )
            {
                return ActionBadRequest( $"{PtoAllocation.FriendlyTypeName} not found." );
            }

            if ( !entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                return ActionBadRequest( $"Not authorized to delete {PtoAllocation.FriendlyTypeName}." );
            }

            entityService.Delete( entity );
            RockContext.SaveChanges();

            return ActionOk();
        }

        #endregion
    }
}
