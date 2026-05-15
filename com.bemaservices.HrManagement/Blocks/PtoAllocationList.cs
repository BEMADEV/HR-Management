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
                .AddTextField( "person", a => a.PersonAlias != null && a.PersonAlias.Person != null
                    ? a.PersonAlias.Person.NickName + " " + a.PersonAlias.Person.LastName
                    : string.Empty )
                .AddTextField( "ptoType", a => a.PtoType?.Name )
                .AddTextField( "ptoAllocationSourceType", a => a.PtoAllocationSourceType.ToString() )
                .AddField( "hours", a => a.Hours )
                .AddDateTimeField( "startDate", a => a.StartDate )
                .AddDateTimeField( "endDate", a => a.EndDate )
                .AddTextField( "accrualSchedule", a => a.PtoAccrualSchedule.ToString() )
                .AddTextField( "ptoAllocationStatus", a => a.PtoAllocationStatus.ConvertToString() )
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

        /// <summary>
        /// Updates the status of multiple allocations.
        /// </summary>
        /// <param name="keys">The identifiers of the entities to be updated.</param>
        /// <param name="action">The action to perform (ACTIVATE or INACTIVATE).</param>
        /// <returns>A result that indicates if the operation succeeded.</returns>
        [BlockAction]
        public BlockActionResult BulkStatusUpdate( List<string> keys, string action )
        {
            if ( keys == null || !keys.Any() )
            {
                return ActionBadRequest( "No allocations selected." );
            }

            var entityService = new PtoAllocationService( RockContext );
            var newStatus = action == "ACTIVATE" ? Enums.PtoAllocationStatus.Active : Enums.PtoAllocationStatus.Inactive;
            var updatedCount = 0;

            foreach ( var key in keys )
            {
                var entity = entityService.Get( key, !PageCache.Layout.Site.DisablePredictableIds );

                if ( entity != null && entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
                {
                    if ( entity.PtoAllocationStatus != newStatus )
                    {
                        entity.PtoAllocationStatus = newStatus;
                        updatedCount++;
                    }
                }
            }

            RockContext.SaveChanges();

            return ActionOk( new { count = updatedCount, status = newStatus.ToString() } );
        }

        #endregion
    }
}
