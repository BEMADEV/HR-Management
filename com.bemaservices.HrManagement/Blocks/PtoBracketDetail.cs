using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Rock;
using Rock.Attribute;
using Rock.Blocks;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.ViewModels.Blocks;
using Rock.ViewModels.Utility;
using Rock.Web.Cache;

using com.bemaservices.HrManagement.Model;
using com.bemaservices.HrManagement.ViewModels;

namespace com.bemaservices.HrManagement.Blocks
{
    /// <summary>
    /// Displays the details of a particular pto bracket.
    /// </summary>

    [DisplayName( "Pto Bracket Detail" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Displays the details of a particular pto bracket." )]
    [IconCssClass( "fa fa-question" )]
    [SupportedSiteTypes( SiteType.Web )]

    #region Block Attributes

    #endregion

    [Rock.SystemGuid.EntityTypeGuid( "60f8c7d1-4a84-4330-97a9-ebc4b433ec80" )]
    [Rock.SystemGuid.BlockTypeGuid( "3d8f1189-a48f-4913-8218-68b78a00f6c7" )]
    public class PtoBracketDetail : RockEntityDetailBlockType<PtoBracket, PtoBracketBag>
    {
        #region Keys

        private static class PageParameterKey
        {
            public const string PtoBracketId = "PtoBracketId";
            public const string PtoTierId = "PtoTierId";
        }

        private static class NavigationUrlKey
        {
            public const string ParentPage = "ParentPage";
        }

        #endregion Keys

        #region Properties

        /// <inheritdoc/>
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoBracketDetail.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new DetailBlockBox<PtoBracketBag, PtoBracketDetailOptionsBag>();

            SetBoxInitialEntityState( box );

            box.NavigationUrls = GetBoxNavigationUrls();
            box.Options = GetBoxOptions( box.IsEditable );

            return box;
        }

        /// <summary>
        /// Gets the box options required for the component to render the view
        /// or edit the entity.
        /// </summary>
        /// <param name="isEditable"><c>true</c> if the entity is editable; otherwise <c>false</c>.</param>
        /// <returns>The options that provide additional details to the block.</returns>
        private PtoBracketDetailOptionsBag GetBoxOptions( bool isEditable )
        {
            var options = new PtoBracketDetailOptionsBag
            {
                PtoTypeOptions = new PtoTypeService( RockContext )
                    .Queryable()
                    .OrderBy( pt => pt.Name )
                    .ToList()
                    .Select( pt => pt.ToListItemBag() )
                    .ToList()
            };

            return options;
        }

        /// <summary>
        /// Validates the PtoBracket for any final information that might not be
        /// valid after storing all the data from the client.
        /// </summary>
        /// <param name="ptoBracket">The PtoBracket to be validated.</param>
        /// <param name="errorMessage">On <c>false</c> return, contains the error message.</param>
        /// <returns><c>true</c> if the PtoBracket is valid, <c>false</c> otherwise.</returns>
        private bool ValidatePtoBracket( PtoBracket ptoBracket, out string errorMessage )
        {
            var hasBracketTypes = ptoBracket.Id == 0
                ? ptoBracket.PtoBracketTypes.Any()
                : new PtoBracketTypeService( RockContext ).Queryable().Any( pbt => pbt.PtoBracketId == ptoBracket.Id );

            if ( !hasBracketTypes )
            {
                errorMessage = "At least one PTO Type needs to be configured.";
                return false;
            }

            errorMessage = null;

            return true;
        }

        /// <summary>
        /// Sets the initial entity state of the box. Populates the Entity or
        /// ErrorMessage properties depending on the entity and permissions.
        /// </summary>
        /// <param name="box">The box to be populated.</param>
        private void SetBoxInitialEntityState( DetailBlockBox<PtoBracketBag, PtoBracketDetailOptionsBag> box )
        {
            var entity = GetInitialEntity();

            if ( entity == null )
            {
                box.ErrorMessage = $"The {PtoBracket.FriendlyTypeName} was not found.";
                return;
            }

            var isViewable = entity.IsAuthorized( Authorization.VIEW, RequestContext.CurrentPerson );
            box.IsEditable = entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson );

            if ( entity.Id != 0 )
            {
                // Existing entity was found, prepare for view mode by default.
                if ( isViewable )
                {
                    box.Entity = GetEntityBagForView( entity );
                }
                else
                {
                    box.ErrorMessage = EditModeMessage.NotAuthorizedToView( PtoBracket.FriendlyTypeName );
                }
            }
            else
            {
                // New entity is being created, prepare for edit mode by default.
                if ( box.IsEditable )
                {
                    box.Entity = GetEntityBagForEdit( entity );
                }
                else
                {
                    box.ErrorMessage = EditModeMessage.NotAuthorizedToEdit( PtoBracket.FriendlyTypeName );
                }
            }

            PrepareDetailBox( box, entity );
        }

        /// <summary>
        /// Gets the entity bag that is common between both view and edit modes.
        /// </summary>
        /// <param name="entity">The entity to be represented as a bag.</param>
        /// <returns>A <see cref="PtoBracketBag"/> that represents the entity.</returns>
        private PtoBracketBag GetCommonEntityBag( PtoBracket entity )
        {
            if ( entity == null )
            {
                return null;
            }

            return new PtoBracketBag
            {
                IdKey = entity.IdKey,
                IsActive = entity.IsActive,
                MaximumYear = entity.MaximumYear,
                MinimumYear = entity.MinimumYear,
                PtoBracketTypes = ( entity.Id == 0
                    ? new List<PtoBracketType>()
                    : new PtoBracketTypeService( RockContext ).Queryable()
                        .Where( pbt => pbt.PtoBracketId == entity.Id )
                        .ToList() )
                    .Select( pbt => new PtoBracketTypeBag
                    {
                        DefaultHours = pbt.DefaultHours,
                        Guid = pbt.Guid.ToString(),
                        IsActive = pbt.IsActive,
                        PtoType = pbt.PtoType?.ToListItemBag()
                    } )
                    .ToList()
            };
        }

        /// <inheritdoc/>
        protected override PtoBracketBag GetEntityBagForView( PtoBracket entity )
        {
            if ( entity == null )
            {
                return null;
            }

            var bag = GetCommonEntityBag( entity );

            if ( entity.Attributes == null )
            {
                entity.LoadAttributes( RockContext );
            }

            bag.LoadAttributesAndValuesForPublicView( entity, RequestContext.CurrentPerson, enforceSecurity: true );

            return bag;
        }

        /// <inheritdoc/>
        protected override PtoBracketBag GetEntityBagForEdit( PtoBracket entity )
        {
            if ( entity == null )
            {
                return null;
            }

            var bag = GetCommonEntityBag( entity );

            if ( entity.Attributes == null )
            {
                entity.LoadAttributes( RockContext );
            }

            bag.LoadAttributesAndValuesForPublicEdit( entity, RequestContext.CurrentPerson, enforceSecurity: true );

            return bag;
        }

        /// <inheritdoc/>
        protected override bool UpdateEntityFromBox( PtoBracket entity, ValidPropertiesBox<PtoBracketBag> box )
        {
            if ( box.ValidProperties == null )
            {
                return false;
            }

            box.IfValidProperty( nameof( box.Bag.IsActive ),
                () => entity.IsActive = box.Bag.IsActive );

            box.IfValidProperty( nameof( box.Bag.MaximumYear ),
                () => entity.MaximumYear = box.Bag.MaximumYear );

            box.IfValidProperty( nameof( box.Bag.MinimumYear ),
                () => entity.MinimumYear = box.Bag.MinimumYear );

            box.IfValidProperty( nameof( box.Bag.PtoBracketTypes ),
                () =>
                {
                    var ptoBracketTypeService = new PtoBracketTypeService( RockContext );
                    var incomingPtoBracketTypes = box.Bag.PtoBracketTypes ?? new List<PtoBracketTypeBag>();
                    var existingPtoBracketTypes = entity.Id == 0
                        ? new List<PtoBracketType>()
                        : ptoBracketTypeService.Queryable().Where( pbt => pbt.PtoBracketId == entity.Id ).ToList();

                    var incomingGuids = incomingPtoBracketTypes
                        .Select( pbt => pbt.Guid.AsGuidOrNull() )
                        .Where( g => g.HasValue )
                        .Select( g => g.Value )
                        .ToList();

                    foreach ( var existingPtoBracketType in existingPtoBracketTypes.Where( pbt => !incomingGuids.Contains( pbt.Guid ) ).ToList() )
                    {
                        ptoBracketTypeService.Delete( existingPtoBracketType );
                    }

                    foreach ( var incomingPtoBracketType in incomingPtoBracketTypes )
                    {
                        var incomingGuid = incomingPtoBracketType.Guid.AsGuidOrNull() ?? Guid.NewGuid();
                        var existingPtoBracketType = existingPtoBracketTypes.FirstOrDefault( pbt => pbt.Guid == incomingGuid );

                        if ( existingPtoBracketType == null )
                        {
                            existingPtoBracketType = new PtoBracketType
                            {
                                Guid = incomingGuid,
                                PtoBracket = entity
                            };

                            ptoBracketTypeService.Add( existingPtoBracketType );
                        }

                        var ptoTypeId = incomingPtoBracketType.PtoType.GetEntityId<PtoType>( RockContext );

                        existingPtoBracketType.PtoTypeId = ptoTypeId.GetValueOrDefault();
                        existingPtoBracketType.DefaultHours = incomingPtoBracketType.DefaultHours;
                        existingPtoBracketType.IsActive = incomingPtoBracketType.IsActive;
                    }
                } );

            box.IfValidProperty( nameof( box.Bag.AttributeValues ),
                () =>
                {
                    entity.LoadAttributes( RockContext );

                    entity.SetPublicAttributeValues( box.Bag.AttributeValues, RequestContext.CurrentPerson, enforceSecurity: true );
                } );

            return true;
        }

        /// <inheritdoc/>
        protected override PtoBracket GetInitialEntity()
        {
            return GetInitialEntity<PtoBracket, PtoBracketService>( RockContext, PageParameterKey.PtoBracketId );
        }

        /// <summary>
        /// Gets the box navigation URLs required for the page to operate.
        /// </summary>
        /// <returns>A dictionary of key names and URL values.</returns>
        private Dictionary<string, string> GetBoxNavigationUrls()
        {
            var ptoTierId = PageParameter( PageParameterKey.PtoTierId );

            var qryParams = new Dictionary<string, string>();

            if ( ptoTierId.IsNotNullOrWhiteSpace() )
            {
                qryParams.Add( PageParameterKey.PtoTierId, ptoTierId );
            }

            return new Dictionary<string, string>
            {
                [NavigationUrlKey.ParentPage] = this.GetParentPageUrl( qryParams )
            };
        }

        /// <inheritdoc/>
        protected override bool TryGetEntityForEditAction( string idKey, out PtoBracket entity, out BlockActionResult error )
        {
            var entityService = new PtoBracketService( RockContext );
            error = null;

            // Determine if we are editing an existing entity or creating a new one.
            if ( idKey.IsNotNullOrWhiteSpace() )
            {
                // If editing an existing entity then load it and make sure it
                // was found and can still be edited.
                entity = entityService.Get( idKey, !PageCache.Layout.Site.DisablePredictableIds );
            }
            else
            {
                // Create a new entity.
                entity = new PtoBracket();
                entityService.Add( entity );
            }

            if ( entity == null )
            {
                error = ActionBadRequest( $"{PtoBracket.FriendlyTypeName} not found." );
                return false;
            }

            if ( !entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                error = ActionBadRequest( $"Not authorized to edit ${PtoBracket.FriendlyTypeName}." );
                return false;
            }

            return true;
        }

        #endregion

        #region Block Actions

        /// <summary>
        /// Gets the box that will contain all the information needed to begin
        /// the edit operation.
        /// </summary>
        /// <param name="key">The identifier of the entity to be edited.</param>
        /// <returns>A box that contains the entity and any other information required.</returns>
        [BlockAction]
        public BlockActionResult Edit( string key )
        {
            if ( !TryGetEntityForEditAction( key, out var entity, out var actionError ) )
            {
                return actionError;
            }

            entity.LoadAttributes( RockContext );

            var bag = GetEntityBagForEdit( entity );

            return ActionOk( new ValidPropertiesBox<PtoBracketBag>
            {
                Bag = bag,
                ValidProperties = bag.GetType().GetProperties().Select( p => p.Name ).ToList()
            } );
        }

        /// <summary>
        /// Saves the entity contained in the box.
        /// </summary>
        /// <param name="box">The box that contains all the information required to save.</param>
        /// <returns>A new entity bag to be used when returning to view mode, or the URL to redirect to after creating a new entity.</returns>
        [BlockAction]
        public BlockActionResult Save( ValidPropertiesBox<PtoBracketBag> box )
        {
            var entityService = new PtoBracketService( RockContext );

            if ( !TryGetEntityForEditAction( box.Bag.IdKey, out var entity, out var actionError ) )
            {
                return actionError;
            }

            // Update the entity instance from the information in the bag.
            if ( !UpdateEntityFromBox( entity, box ) )
            {
                return ActionBadRequest( "Invalid data." );
            }

            // Ensure everything is valid before saving.
            if ( !ValidatePtoBracket( entity, out var validationMessage ) )
            {
                return ActionBadRequest( validationMessage );
            }

            var isNew = entity.Id == 0;

            RockContext.WrapTransaction( () =>
            {
                RockContext.SaveChanges();
                entity.SaveAttributeValues( RockContext );
            } );

            if ( isNew )
            {
                return ActionContent( System.Net.HttpStatusCode.Created, this.GetCurrentPageUrl( new Dictionary<string, string>
                {
                    [PageParameterKey.PtoBracketId] = entity.IdKey
                } ) );
            }

            // Ensure navigation properties will work now.
            entity = entityService.Get( entity.Id );
            entity.LoadAttributes( RockContext );

            var bag = GetEntityBagForEdit( entity );

            return ActionOk( new ValidPropertiesBox<PtoBracketBag>
            {
                Bag = bag,
                ValidProperties = bag.GetType().GetProperties().Select( p => p.Name ).ToList()
            } );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="key">The identifier of the entity to be deleted.</param>
        /// <returns>A string that contains the URL to be redirected to on success.</returns>
        [BlockAction]
        public BlockActionResult Delete( string key )
        {
            var entityService = new PtoBracketService( RockContext );

            if ( !TryGetEntityForEditAction( key, out var entity, out var actionError ) )
            {
                return actionError;
            }

            entityService.Delete( entity );
            RockContext.SaveChanges();

            return ActionOk( this.GetParentPageUrl() );
        }

        #endregion
    }
}
