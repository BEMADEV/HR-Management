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

        private static class PageParameterKey
        {
            public const string PtoTierId = "PtoTierId";
            public const string PtoBracketId = "PtoBracketId";
        }

        #endregion Keys

        #region Fields

        /// <summary>
        /// The cached PTO Tier, should be accessed via the <see cref="GetPtoTier"/> method.
        /// </summary>
        private PtoTier _ptoTier = null;

        #endregion

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
            box.IsDeleteEnabled = GetIsDeleteEnabled();
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
            options.IsBlockVisible = GetPtoTier() != null;
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
        /// Determines if the delete button should be enabled in the grid.
        /// </summary>
        /// <returns>A boolean value that indicates if the delete button should be enabled.</returns>
        private bool GetIsDeleteEnabled()
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
            var ptoTier = GetPtoTier();

            var qryParams = new Dictionary<string, string>
            {
                { PageParameterKey.PtoTierId, ptoTier?.IdKey },
                { PageParameterKey.PtoBracketId, "((Key))" },
            };

            return new Dictionary<string, string>
            {
                [NavigationUrlKey.DetailPage] = this.GetLinkedPageUrl( AttributeKey.DetailPage, qryParams )
            };
        }

        /// <inheritdoc/>
        protected override IQueryable<PtoBracket> GetListQueryable( RockContext rockContext )
        {
            var ptoTier = GetPtoTier();

            if(ptoTier != null )
            {
                var qry = new PtoBracketService( rockContext )
                    .Queryable()
                    .AsNoTracking()
                    .Where( b => b.PtoTierId == ptoTier.Id );

                return qry;
            }
            else
            {
                return new List<PtoBracket>().AsQueryable();
            }
        }

        /// <inheritdoc/>
        protected override GridBuilder<PtoBracket> GetGridBuilder()
        {
            return new GridBuilder<PtoBracket>()
                .WithBlock( this )
                .AddTextField( "idKey", a => a.IdKey )
                .AddTextField( "name", a => a.Name )
                .AddTextField( "summary", a => a.PtoBracketTypes
                    .Select( b => $"{b.PtoType.Name}: {b.DefaultHours} hrs" )
                    .ToList()
                    .AsDelimited( "<br/>" ) )
                .AddTextField( "status", a => a.IsActive ? "Active" : "Inactive" )
                .AddAttributeFields( GetGridAttributes() );
        }

        private PtoTier GetPtoTier()
        {
            if ( _ptoTier != null )
            {
                return _ptoTier;
            }

            var ptoTierId = PageParameter( PageParameterKey.PtoTierId );

            if ( ptoTierId.IsNotNullOrWhiteSpace() )
            {
                _ptoTier = new PtoTierService( RockContext ).Get( ptoTierId, !PageCache.Layout.Site.DisablePredictableIds );
            }

            return _ptoTier;
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
            var entityService = new PtoBracketService( RockContext );
            var entity = entityService.Get( key, !PageCache.Layout.Site.DisablePredictableIds );

            if ( entity == null )
            {
                return ActionBadRequest( $"{PtoBracket.FriendlyTypeName} not found." );
            }

            if ( !entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                return ActionBadRequest( $"Not authorized to delete {PtoBracket.FriendlyTypeName}." );
            }

            // Delete any bracket types tied to this bracket
            var ptoBracketTypeService = new PtoBracketTypeService( RockContext );
            var bracketTypes = ptoBracketTypeService.Queryable().Where( bt => bt.PtoBracketId == entity.Id ).ToList();
            foreach ( var bracketType in bracketTypes )
            {
                ptoBracketTypeService.Delete( bracketType );
            }

            entityService.Delete( entity );
            RockContext.SaveChanges();

            return ActionOk();
        }

        #endregion
    }
}
