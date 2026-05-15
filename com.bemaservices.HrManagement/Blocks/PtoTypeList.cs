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

    [Rock.SystemGuid.EntityTypeGuid( "ffa7da36-fdea-49d7-a40f-227e9b4b2b60" )]
    [Rock.SystemGuid.BlockTypeGuid( "79d68e47-6d76-47bc-b528-fcbf570bb801" )]
    [CustomizedGrid]
    public class PtoTypeList : RockEntityListBlockType<PtoType>
    {

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
            var ptoTypeIdList = new PtoTypeService( RockContext ).Queryable()
                .AsNoTracking()
                .Where( pt => pt.IsActive )
                .Select( pt => pt.Id )
                .ToList()
                .AsDelimited( "," );

            var publicApplicationRoot = GlobalAttributesCache.Get()
                .GetValue( "PublicApplicationRoot" )
                .EnsureTrailingForwardslash();

            var options = new PtoTypeListOptionsBag
            {
                PtoCalendarFeedUrl = $"{publicApplicationRoot}Webhooks/GetPtoCalendarFeed.ashx?PtoTypeIds={ptoTypeIdList}"
            };

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
            };
        }

        /// <summary>
        /// Gets the bag for editing the PTO type.
        /// </summary>
        /// <param name="entity">The entity to be represented for editing purposes.</param>
        /// <returns>A <see cref="PtoTypeBag"/> that represents the entity.</returns>
        private PtoTypeBag GetEntityBagForEdit( PtoType entity )
        {
            if ( entity == null )
            {
                return null;
            }

            var bag = new PtoTypeBag
            {
                Id = entity.Id,
                IdKey = entity.IdKey,
                Color = entity.Color,
                Description = entity.Description,
                IsActive = entity.IsActive,
                IsNegativeTimeBalanceAllowed = entity.IsNegativeTimeBalanceAllowed,
                Name = entity.Name,
                WorkflowType = entity.WorkflowTypeId.HasValue
                    ? new WorkflowTypeService( RockContext ).Get( entity.WorkflowTypeId.Value )?.ToListItemBag()
                    : null
            };

            bag.LoadAttributesAndValuesForPublicEdit( entity, RequestContext.CurrentPerson, enforceSecurity: true );

            return bag;
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
                .AddField( "name", a => a.Name )
                .AddField( "description", a => a.Description )
                .AddField( "color", a => a.Color )
                .AddField( "isActive", a => a.IsActive )
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

            if ( !entityService.CanDelete( entity, out var errorMessage ) )
            {
                return ActionBadRequest( errorMessage );
            }

            entityService.Delete( entity );
            RockContext.SaveChanges();

            return ActionOk();
        }

        /// <summary>
        /// Gets the specified entity for editing.
        /// </summary>
        /// <param name="key">The identifier of the entity to be edited.</param>
        /// <returns>An object containing the entity values required for editing.</returns>
        [BlockAction]
        public BlockActionResult Edit( string key )
        {
            var entityService = new PtoTypeService( RockContext );

            if ( !new PtoType().IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                return ActionBadRequest( $"Not authorized to edit ${PtoType.FriendlyTypeName}." );
            }

            var entity = entityService.Get( key, !PageCache.Layout.Site.DisablePredictableIds );

            if ( entity == null )
            {
                entity = new PtoType
                {
                    Id = 0,
                    IsActive = true
                };
            }

            entity.LoadAttributes( RockContext );

            return ActionOk( GetEntityBagForEdit( entity ) );
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="bag">The bag that contains all the information required to save.</param>
        /// <returns>An empty result that indicates if the operation succeeded.</returns>
        [BlockAction]
        public BlockActionResult Save( PtoTypeBag bag )
        {
            var entityService = new PtoTypeService( RockContext );
            PtoType entity;

            if ( !new PtoType().IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                return ActionBadRequest( $"Not authorized to edit ${PtoType.FriendlyTypeName}." );
            }

            if ( bag.IdKey.IsNullOrWhiteSpace() )
            {
                entity = new PtoType
                {
                    Id = 0
                };
            }
            else
            {
                entity = entityService.Get( bag.IdKey, !PageCache.Layout.Site.DisablePredictableIds );
            }

            if ( entity == null )
            {
                return ActionBadRequest( $"{PtoType.FriendlyTypeName} not found." );
            }

            entity.LoadAttributes( RockContext );

            entity.Color = bag.Color;
            entity.Description = bag.Description;
            entity.IsActive = bag.IsActive;
            entity.IsNegativeTimeBalanceAllowed = bag.IsNegativeTimeBalanceAllowed;
            entity.Name = bag.Name;
            entity.WorkflowTypeId = bag.WorkflowType.GetEntityId<WorkflowType>( RockContext );

            if ( bag.AttributeValues != null )
            {
                entity.SetPublicAttributeValues( bag.AttributeValues, RequestContext.CurrentPerson, enforceSecurity: true );
            }

            if ( !entity.IsValid )
            {
                return ActionBadRequest( entity.ValidationResults.Select( r => r.ErrorMessage ).FirstOrDefault() );
            }

            RockContext.WrapTransaction( () =>
            {
                if ( entity.Id == 0 )
                {
                    entityService.Add( entity );
                }

                RockContext.SaveChanges();

                entity.SaveAttributeValues( RockContext );
            } );

            return ActionOk();
        }

        #endregion
    }
}
