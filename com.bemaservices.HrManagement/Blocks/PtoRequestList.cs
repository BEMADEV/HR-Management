using System;
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
using Rock.ViewModels.Core.Grid;
using Rock.ViewModels.Utility;
using Rock.Web.Cache;

using com.bemaservices.HrManagement.Model;
using com.bemaservices.HrManagement.ViewModels;

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

    #region Block Attributes

    [WorkflowTypeField( "PTO Request Workflow",
        Description = "The Workflow used to add, modify, and delete PTO Requests.",
        IsRequired = false,
        AllowMultiple = false,
        DefaultValue = "EBF1D986-8BBD-4888-8A7E-43AF5914751C",
        Key = AttributeKey.PtoRequestWorkflow,
        Order = 0 )]

    [TextField( "Workflow Entry Page Route",
        Description = "The route to the workflow entry page.",
        IsRequired = true,
        DefaultValue = "WorkflowEntry",
        Key = AttributeKey.WorkflowEntryPageRoute,
        Order = 1 )]

    #endregion

    [Rock.SystemGuid.EntityTypeGuid( "ef3a3fc7-5586-4eb1-83d1-87ef4f188abb" )]
    [Rock.SystemGuid.BlockTypeGuid( "2f76725e-ccf0-4e57-8f2f-54d4e63e6c99" )]
    [CustomizedGrid]
    public class PtoRequestList : RockBlockType
    {
        #region Keys

        private static class AttributeKey
        {
            public const string PtoRequestWorkflow = "PTORequestWorkflow";
            public const string WorkflowEntryPageRoute = "WorkflowEntryPageRoute";
        }

        private static class NavigationUrlKey
        {
            public const string WorkflowEntryPage = "WorkflowEntryPage";
        }

        private static class PageParameterKey
        {
            public const string PersonId = "PersonId";
        }

        #endregion Keys

        #region Properties

        /// <inheritdoc/>
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/ptoRequestList.obs";

        /// <summary>
        /// Gets the person from page parameter or context.
        /// </summary>
        private Person ContextPerson
        {
            get
            {
                // First try to get from context
                var contextEntity = RequestContext.GetContextEntity<Person>();
                if ( contextEntity != null )
                {
                    return contextEntity;
                }

                // Then try to get from page parameter
                var personId = RequestContext.GetPageParameter( PageParameterKey.PersonId ).AsIntegerOrNull();
                if ( personId.HasValue )
                {
                    return new PersonService( RockContext ).Get( personId.Value );
                }

                return null;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new ListBlockBox<PtoRequestListOptionsBag>();

            var canEdit = GetEditRights();
            box.IsAddEnabled = canEdit;
            box.IsDeleteEnabled = canEdit;
            box.ExpectedRowCount = null;
            box.NavigationUrls = GetBoxNavigationUrls();
            box.Options = GetBoxOptions();

            return box;
        }

        /// <summary>
        /// Gets the box options required for the component to render the list.
        /// </summary>
        /// <returns>The options that provide additional details to the block.</returns>
        private PtoRequestListOptionsBag GetBoxOptions()
        {
            var options = new PtoRequestListOptionsBag();

            // Check if we have a person context
            options.HasPersonContext = ContextPerson != null;
            if ( ContextPerson != null )
            {
                options.ContextPersonAliasGuid = ContextPerson.PrimaryAlias?.Guid.ToString();
            }

            return options;
        }

        /// <summary>
        /// Determines if the current user has edit rights.
        /// </summary>
        /// <returns>A boolean value that indicates if the user can edit.</returns>
        private bool GetEditRights()
        {
            var canEdit = BlockCache.IsAuthorized( Authorization.EDIT, RequestContext.CurrentPerson );

            if ( ContextPerson != null && RequestContext.CurrentPerson != null )
            {
                // User can edit their own requests
                if ( RequestContext.CurrentPerson.Id == ContextPerson.Id )
                {
                    canEdit = true;
                }

                // Supervisor can edit
                if ( !canEdit )
                {
                    ContextPerson.LoadAttributes( RockContext );
                    var supervisorAliasGuid = ContextPerson.GetAttributeValue( "Supervisor" ).AsGuidOrNull();
                    if ( supervisorAliasGuid != null )
                    {
                        var personAlias = new PersonAliasService( RockContext ).Get( supervisorAliasGuid.Value );
                        if ( personAlias != null && personAlias.PersonId == RequestContext.CurrentPerson.Id )
                        {
                            canEdit = true;
                        }
                    }
                }
            }

            return canEdit;
        }

        /// <summary>
        /// Gets the box navigation URLs required for the page to operate.
        /// </summary>
        /// <returns>A dictionary of key names and URL values.</returns>
        private Dictionary<string, string> GetBoxNavigationUrls()
        {
            var workflowTypeGuid = GetAttributeValue( AttributeKey.PtoRequestWorkflow ).AsGuidOrNull();
            var workflowEntryRoute = GetAttributeValue( AttributeKey.WorkflowEntryPageRoute );
            var workflowTypeId = workflowTypeGuid.HasValue ? WorkflowTypeCache.Get( workflowTypeGuid.Value )?.Id : null;

            var baseUrl = workflowTypeId.HasValue
                ? $"/{workflowEntryRoute}/{workflowTypeId}"
                : string.Empty;

            return new Dictionary<string, string>
            {
                [NavigationUrlKey.WorkflowEntryPage] = baseUrl
            };
        }

        #endregion

        #region Block Actions

        /// <summary>
        /// Gets the grid data for the PTO Request List.
        /// </summary>
        /// <returns>A GridDataBag containing request row data.</returns>
        [BlockAction]
        public BlockActionResult GetGridData()
        {
            using ( var rockContext = new RockContext() )
            {
                rockContext.Database.CommandTimeout = 90;

                var requestService = new PtoRequestService( rockContext );
                var qry = requestService.Queryable()
                    .Include( a => a.PtoAllocation )
                    .Include( a => a.PtoAllocation.PersonAlias )
                    .Include( a => a.PtoAllocation.PersonAlias.Person )
                    .Include( a => a.PtoAllocation.PtoType )
                    .AsNoTracking();

                // Filter by person context
                if ( ContextPerson != null )
                {
                    qry = qry.Where( a => a.PtoAllocation.PersonAlias.PersonId == ContextPerson.Id );
                }

                var sortedQry = qry
                    .OrderByDescending( b => b.RequestDate )
                    .ThenBy( b => b.PtoAllocation.PersonAlias.Person.LastName );

                var rows = sortedQry.ToList().Select( r => new Dictionary<string, object>
                {
                    ["idKey"] = r.IdKey,
                    ["guid"] = r.Guid.ToString(),
                    ["person"] = r.PtoAllocation?.PersonAlias?.Person?.FullName ?? string.Empty,
                    ["ptoType"] = r.PtoAllocation?.PtoType?.Name ?? string.Empty,
                    ["reason"] = r.Reason ?? string.Empty,
                    ["hours"] = r.Hours,
                    ["requestDate"] = r.RequestDate,
                    ["ptoRequestApprovalState"] = r.PtoRequestApprovalState.ConvertToString()
                } ).ToList();

                // DEBUG: Final row count
                System.Diagnostics.Debug.WriteLine( $"[PtoRequestList] Final rows returned: {rows.Count}" );

                var gridData = new GridDataBag
                {
                    Rows = rows
                };

                return ActionOk( gridData );
            }
        }

        /// <summary>
        /// Deletes the specified PTO request by redirecting to the workflow with cancel parameter.
        /// </summary>
        /// <param name="key">The identifier of the row to delete.</param>
        /// <returns>An empty result.</returns>
        [BlockAction]
        public BlockActionResult Delete( string key )
        {
            if ( !GetEditRights() )
            {
                return ActionBadRequest( "Not authorized to delete PTO requests." );
            }

            using ( var rockContext = new RockContext() )
            {
                var ptoRequestService = new PtoRequestService( rockContext );
                var ptoRequest = ptoRequestService.Get( key, !PageCache.Layout.Site.DisablePredictableIds );

                if ( ptoRequest == null )
                {
                    return ActionBadRequest( "PTO request not found." );
                }

                var workflowTypeGuid = GetAttributeValue( AttributeKey.PtoRequestWorkflow ).AsGuidOrNull();
                if ( !workflowTypeGuid.HasValue )
                {
                    return ActionBadRequest( "PTO Request Workflow is not configured." );
                }

                var workflowType = WorkflowTypeCache.Get( workflowTypeGuid.Value );
                if ( workflowType == null )
                {
                    return ActionBadRequest( "PTO Request Workflow not found." );
                }

                var workflowEntryRoute = GetAttributeValue( AttributeKey.WorkflowEntryPageRoute );
                var url = $"/{workflowEntryRoute}/{workflowType.Id}?PTORequest={ptoRequest.Guid}&CancelRequest=Yes";

                return ActionOk( new { redirectUrl = url } );
            }
        }

        #endregion
    }
}
