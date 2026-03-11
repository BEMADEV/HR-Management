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
    /// Displays a list of pto types.
    /// </summary>

    [DisplayName( "Pto Type List" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Displays a list of pto types." )]
    [IconCssClass( "fa fa-list" )]
    [SupportedSiteTypes( SiteType.Web )]

    [LinkedPage( "Detail Page",
        Description = "The page that will show the pto type details.",
        Key = AttributeKey.DetailPage )]

    [Rock.SystemGuid.EntityTypeGuid( "ffa7da36-fdea-49d7-a40f-227e9b4b2b60" )]
    [Rock.SystemGuid.BlockTypeGuid( "79d68e47-6d76-47bc-b528-fcbf570bb801" )]
    [CustomizedGrid]
    public class PtoTypeList : RockEntityListBlockType<PtoType>
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
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoTypeList.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new ListBlockBox<PtoTypeListOptionsBag>();
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
        private PtoTypeListOptionsBag GetBoxOptions()
        {
            var options = new PtoTypeListOptionsBag();

            return options;
        }

        /// <summary>
        /// Determines if the add button should be enabled in the grid.
        /// </summary>
        /// <returns>A boolean value that indicates if the add button should be enabled.</returns>
        private bool GetIsAddEnabled()
        {
            var entity = new PtoType();

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
                [NavigationUrlKey.DetailPage] = this.GetLinkedPageUrl( AttributeKey.DetailPage, "PtoTypeId", "((Key))" )
            };
        }

        /// <inheritdoc/>
        protected override IQueryable<PtoType> GetListQueryable( RockContext rockContext )
        {
            return base.GetListQueryable( rockContext )
                .Include( a => a.WorkflowType );
        }

        /// <inheritdoc/>
        protected override GridBuilder<PtoType> GetGridBuilder()
        {
            return new GridBuilder<PtoType>()
                .WithBlock( this )
                .AddTextField( "idKey", a => a.IdKey )
                .AddField( "color", a => a.Color )
                .AddField( "description", a => a.Description )
                .AddField( "isActive", a => a.IsActive )
                .AddField( "name", a => a.Name )
                .AddTextField( "workflowType", a => a.WorkflowType?.Name )
                .AddField( "isSecurityDisabled", a => !a.IsAuthorized( Authorization.ADMINISTRATE, RequestContext.CurrentPerson ) )
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
            var entityService = new PtoTypeService( RockContext );
            var entity = entityService.Get( key, !PageCache.Layout.Site.DisablePredictableIds );

            if ( entity == null )
            {
                return ActionBadRequest( $"{PtoType.FriendlyTypeName} not found." );
            }

            if ( !entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                return ActionBadRequest( $"Not authorized to delete {PtoType.FriendlyTypeName}." );
            }

            entityService.Delete( entity );
            RockContext.SaveChanges();

            return ActionOk();
        }

        #endregion
    }
}
