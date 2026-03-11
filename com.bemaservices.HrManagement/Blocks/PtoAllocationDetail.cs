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
    /// Displays the details of a particular pto allocation.
    /// </summary>

    [DisplayName( "Pto Allocation Detail" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Displays the details of a particular pto allocation." )]
    [IconCssClass( "fa fa-question" )]
    [SupportedSiteTypes( SiteType.Web )]

    #region Block Attributes

    #endregion

    [Rock.SystemGuid.EntityTypeGuid( "80ed296c-00de-4981-9a4f-3e29383c5125" )]
    [Rock.SystemGuid.BlockTypeGuid( "a81082a2-8f37-4eec-ae6e-2ed09f93a350" )]
    public class PtoAllocationDetail : RockEntityDetailBlockType<PtoAllocation, PtoAllocationBag>
    {
        #region Keys

        private static class PageParameterKey
        {
            public const string PtoAllocationId = "PtoAllocationId";
        }

        private static class NavigationUrlKey
        {
            public const string ParentPage = "ParentPage";
        }

        #endregion Keys

        #region Properties

        /// <inheritdoc/>
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoAllocationDetail.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new DetailBlockBox<PtoAllocationBag, PtoAllocationDetailOptionsBag>();

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
        private PtoAllocationDetailOptionsBag GetBoxOptions( bool isEditable )
        {
            var options = new PtoAllocationDetailOptionsBag();

            return options;
        }

        /// <summary>
        /// Validates the PtoAllocation for any final information that might not be
        /// valid after storing all the data from the client.
        /// </summary>
        /// <param name="ptoAllocation">The PtoAllocation to be validated.</param>
        /// <param name="errorMessage">On <c>false</c> return, contains the error message.</param>
        /// <returns><c>true</c> if the PtoAllocation is valid, <c>false</c> otherwise.</returns>
        private bool ValidatePtoAllocation( PtoAllocation ptoAllocation, out string errorMessage )
        {
            errorMessage = null;

            return true;
        }

        /// <summary>
        /// Sets the initial entity state of the box. Populates the Entity or
        /// ErrorMessage properties depending on the entity and permissions.
        /// </summary>
        /// <param name="box">The box to be populated.</param>
        private void SetBoxInitialEntityState( DetailBlockBox<PtoAllocationBag, PtoAllocationDetailOptionsBag> box )
        {
            var entity = GetInitialEntity();

            if ( entity == null )
            {
                box.ErrorMessage = $"The {PtoAllocation.FriendlyTypeName} was not found.";
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
                    box.ErrorMessage = EditModeMessage.NotAuthorizedToView( PtoAllocation.FriendlyTypeName );
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
                    box.ErrorMessage = EditModeMessage.NotAuthorizedToEdit( PtoAllocation.FriendlyTypeName );
                }
            }

            PrepareDetailBox( box, entity );
        }

        /// <summary>
        /// Gets the entity bag that is common between both view and edit modes.
        /// </summary>
        /// <param name="entity">The entity to be represented as a bag.</param>
        /// <returns>A <see cref="PtoAllocationBag"/> that represents the entity.</returns>
        private PtoAllocationBag GetCommonEntityBag( PtoAllocation entity )
        {
            if ( entity == null )
            {
                return null;
            }

            return new PtoAllocationBag
            {
                IdKey = entity.IdKey,
                EndDate = entity.EndDate,
                Hours = entity.Hours,
                Note = entity.Note,
                PersonAlias = entity.PersonAlias.ToListItemBag(),
                PtoAllocationSourceType = entity.PtoAllocationSourceType,
                PtoAllocationStatus = entity.PtoAllocationStatus,
                PtoType = entity.PtoType.ToListItemBag(),
                StartDate = entity.StartDate
            };
        }

        /// <inheritdoc/>
        protected override PtoAllocationBag GetEntityBagForView( PtoAllocation entity )
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
        protected override PtoAllocationBag GetEntityBagForEdit( PtoAllocation entity )
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
        protected override bool UpdateEntityFromBox( PtoAllocation entity, ValidPropertiesBox<PtoAllocationBag> box )
        {
            if ( box.ValidProperties == null )
            {
                return false;
            }

            box.IfValidProperty( nameof( box.Bag.EndDate ),
                () => entity.EndDate = box.Bag.EndDate );

            box.IfValidProperty( nameof( box.Bag.Hours ),
                () => entity.Hours = box.Bag.Hours );

            box.IfValidProperty( nameof( box.Bag.Note ),
                () => entity.Note = box.Bag.Note );

            box.IfValidProperty( nameof( box.Bag.PersonAlias ),
                () => entity.PersonAliasId = box.Bag.PersonAlias.GetEntityId<PersonAlias>( RockContext ).Value );

            box.IfValidProperty( nameof( box.Bag.PtoAllocationSourceType ),
                () => entity.PtoAllocationSourceType = box.Bag.PtoAllocationSourceType );

            box.IfValidProperty( nameof( box.Bag.PtoAllocationStatus ),
                () => entity.PtoAllocationStatus = box.Bag.PtoAllocationStatus );

            box.IfValidProperty( nameof( box.Bag.PtoType ),
                () => entity.PtoTypeId = box.Bag.PtoType.GetEntityId<PtoType>( RockContext ).Value );

            box.IfValidProperty( nameof( box.Bag.StartDate ),
                () => entity.StartDate = box.Bag.StartDate );

            box.IfValidProperty( nameof( box.Bag.AttributeValues ),
                () =>
                {
                    entity.LoadAttributes( RockContext );

                    entity.SetPublicAttributeValues( box.Bag.AttributeValues, RequestContext.CurrentPerson, enforceSecurity: true );
                } );

            return true;
        }

        /// <inheritdoc/>
        protected override PtoAllocation GetInitialEntity()
        {
            return GetInitialEntity<PtoAllocation, PtoAllocationService>( RockContext, PageParameterKey.PtoAllocationId );
        }

        /// <summary>
        /// Gets the box navigation URLs required for the page to operate.
        /// </summary>
        /// <returns>A dictionary of key names and URL values.</returns>
        private Dictionary<string, string> GetBoxNavigationUrls()
        {
            return new Dictionary<string, string>
            {
                [NavigationUrlKey.ParentPage] = this.GetParentPageUrl()
            };
        }

        /// <inheritdoc/>
        protected override bool TryGetEntityForEditAction( string idKey, out PtoAllocation entity, out BlockActionResult error )
        {
            var entityService = new PtoAllocationService( RockContext );
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
                entity = new PtoAllocation();
                entityService.Add( entity );
            }

            if ( entity == null )
            {
                error = ActionBadRequest( $"{PtoAllocation.FriendlyTypeName} not found." );
                return false;
            }

            if ( !entity.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                error = ActionBadRequest( $"Not authorized to edit ${PtoAllocation.FriendlyTypeName}." );
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

            return ActionOk( new ValidPropertiesBox<PtoAllocationBag>
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
        public BlockActionResult Save( ValidPropertiesBox<PtoAllocationBag> box )
        {
            var entityService = new PtoAllocationService( RockContext );

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
            if ( !ValidatePtoAllocation( entity, out var validationMessage ) )
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
                    [PageParameterKey.PtoAllocationId] = entity.IdKey
                } ) );
            }

            // Ensure navigation properties will work now.
            entity = entityService.Get( entity.Id );
            entity.LoadAttributes( RockContext );

            var bag = GetEntityBagForEdit( entity );

            return ActionOk( new ValidPropertiesBox<PtoAllocationBag>
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
            var entityService = new PtoAllocationService( RockContext );

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
