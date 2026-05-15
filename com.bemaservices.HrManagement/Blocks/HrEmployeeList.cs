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

using com.bemaservices.HrManagement.Enums;
using com.bemaservices.HrManagement.Model;
using com.bemaservices.HrManagement.ViewModels;

namespace com.bemaservices.HrManagement.Blocks
{
    /// <summary>
    /// Lists all the employees along with their PTO information.
    /// </summary>

    [DisplayName( "HR Employee List" )]
    [Category( "BEMA Software Services > Hr Management" )]
    [Description( "Lists all the employees along with their PTO information." )]
    [IconCssClass( "fa fa-users" )]
    [SupportedSiteTypes( SiteType.Web )]

    #region Block Attributes

    [AttributeField( Rock.SystemGuid.EntityType.PERSON,
        "Person Hired Date Attribute",
        Description = "The Person Attribute that contains the Person's Hired Date. This will be used to determine if the person is currently staff or not.",
        IsRequired = true,
        AllowMultiple = false,
        Key = AttributeKey.HireDate,
        Order = 0 )]

    [AttributeField( Rock.SystemGuid.EntityType.PERSON,
        "Person Fired Date Attribute",
        Description = "The Person Attribute that contains the Person's Fired Date. This will be used to determine if the person is currently staff or not.",
        IsRequired = true,
        AllowMultiple = false,
        Key = AttributeKey.FireDate,
        Order = 1 )]

    [AttributeField( Rock.SystemGuid.EntityType.PERSON,
        "Person Supervisor Attribute",
        Description = "The Person Attribute that contains the Person's Supervisor.",
        IsRequired = true,
        AllowMultiple = false,
        Key = AttributeKey.Supervisor,
        Order = 2 )]

    [AttributeField( Rock.SystemGuid.EntityType.PERSON,
        "Person Ministry Area Attribute",
        Description = "The Person Attribute that contains the Person's Ministry Area.",
        IsRequired = false,
        AllowMultiple = false,
        Key = AttributeKey.MinistryArea,
        Order = 3 )]

    [LinkedPage( "Detail Page",
        Description = "The page that will show the employee details.",
        Key = AttributeKey.DetailPage,
        Order = 4 )]

    #endregion

    [Rock.SystemGuid.EntityTypeGuid( "7b3a4d65-8f2e-4c9a-b1d3-e5f6a7b8c9d0" )]
    [Rock.SystemGuid.BlockTypeGuid( "8c5d6e7f-9a1b-2c3d-4e5f-6a7b8c9d0e1f" )]
    public class HrEmployeeList : RockBlockType
    {
        #region Keys

        private static class AttributeKey
        {
            public const string HireDate = "HireDate";
            public const string FireDate = "FireDate";
            public const string Supervisor = "Supervisor";
            public const string MinistryArea = "MinistryArea";
            public const string DetailPage = "DetailPage";
        }

        private static class NavigationUrlKey
        {
            public const string DetailPage = "DetailPage";
        }

        #endregion Keys

        #region Properties

        /// <inheritdoc/>
        public override string ObsidianFileUrl => "~/Plugins/com_bemaservices/HrManagement/hrEmployeeList.obs";

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            var box = new ListBlockBox<HrEmployeeListOptionsBag>();

            box.IsAddEnabled = false;
            box.IsDeleteEnabled = false;
            box.ExpectedRowCount = null;
            box.NavigationUrls = GetBoxNavigationUrls();
            box.Options = GetBoxOptions();

            return box;
        }

        /// <summary>
        /// Gets the box options required for the component to render the list.
        /// </summary>
        /// <returns>The options that provide additional details to the block.</returns>
        private HrEmployeeListOptionsBag GetBoxOptions()
        {
            var options = new HrEmployeeListOptionsBag();

            // Get PTO Types
            var ptoTypes = new PtoTypeService( RockContext )
                .Queryable()
                .AsNoTracking()
                .Where( x => x.IsActive == true )
                .OrderBy( x => x.Name )
                .ToList()
                .Select( p => new ListItemBag
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                } )
                .ToList();

            options.PtoTypes = ptoTypes;

            // Get Fiscal Years
            var fiscalYears = new List<ListItemBag>();
            for ( int yearOffset = -5; yearOffset <= 5; yearOffset++ )
            {
                var year = RockDateTime.Now.AddYears( yearOffset ).Year;
                fiscalYears.Add( new ListItemBag
                {
                    Value = year.ToString(),
                    Text = year.ToString()
                } );
            }

            options.FiscalYears = fiscalYears;

            // Check if supervisor attribute is configured
            var supervisorAttributeGuid = GetAttributeValue( AttributeKey.Supervisor ).AsGuidOrNull();
            options.ShowSupervisorFilter = supervisorAttributeGuid.HasValue;

            // Check if ministry area attribute is configured
            var ministryAreaAttributeGuid = GetAttributeValue( AttributeKey.MinistryArea ).AsGuidOrNull();
            options.ShowMinistryAreaFilter = ministryAreaAttributeGuid.HasValue;

            return options;
        }

        /// <summary>
        /// Gets the box navigation URLs required for the page to operate.
        /// </summary>
        /// <returns>A dictionary of key names and URL values.</returns>
        private Dictionary<string, string> GetBoxNavigationUrls()
        {
            return new Dictionary<string, string>
            {
                [NavigationUrlKey.DetailPage] = this.GetLinkedPageUrl( AttributeKey.DetailPage, "PersonId", "((Key))" )
            };
        }

        /// <summary>
        /// Gets the fiscal dates based on the fiscal year end.
        /// </summary>
        /// <param name="fiscalYearEnd">The fiscal year end.</param>
        /// <returns>A tuple containing the fiscal start and end dates.</returns>
        private (DateTime StartDate, DateTime EndDate) GetFiscalDates( int? fiscalYearEnd )
        {
            var fiscalStartDateValue = GlobalAttributesCache.Value( AttributeCache.Get( SystemGuid.Attribute.FISCAL_YEAR_START_DATE_ATTRIBUTE ).Key );
            var fiscalStartDate = DateTime.Parse( fiscalStartDateValue );

            var fiscalStartMonth = fiscalStartDate.Month;
            var fiscalStartDay = fiscalStartDate.Day;

            DateTime calculatedFiscalStartDate;
            DateTime calculatedFiscalEndDate;

            if ( fiscalYearEnd.HasValue && fiscalYearEnd != 0 )
            {
                calculatedFiscalStartDate = new DateTime( fiscalYearEnd.Value - 1, fiscalStartMonth, fiscalStartDay );
                calculatedFiscalEndDate = calculatedFiscalStartDate.AddYears( 1 ).AddDays( -1 );
            }
            else
            {
                var todayOffset = RockDateTime.Now;
                var fiscalStartYear = todayOffset.AddMonths( ( fiscalStartMonth - 1 ) * -1 ).AddDays( ( fiscalStartDay - 1 ) * -1 ).Year;
                calculatedFiscalStartDate = new DateTime( fiscalStartYear, fiscalStartMonth, fiscalStartDay );
                calculatedFiscalEndDate = calculatedFiscalStartDate.AddYears( 1 ).AddDays( -1 );
            }

            return (calculatedFiscalStartDate, calculatedFiscalEndDate);
        }

        #endregion

        #region Block Actions

        /// <summary>
        /// Gets the grid data for the HR Employee List.
        /// </summary>
        /// <param name="fiscalYearEnd">The fiscal year end filter.</param>
        /// <param name="ptoTypeId">The PTO type filter.</param>
        /// <param name="supervisorId">The supervisor filter.</param>
        /// <param name="ministryArea">The ministry area filter.</param>
        /// <param name="showUnallocatedPtoTypes">Whether to show unallocated PTO types.</param>
        /// <returns>A GridDataBag containing employee row data.</returns>
        [BlockAction]
        public BlockActionResult GetGridData( int? fiscalYearEnd, int? ptoTypeId, int? supervisorId, string ministryArea, bool showUnallocatedPtoTypes )
        {
            using ( var rockContext = new RockContext() )
            {
                rockContext.Database.CommandTimeout = 90;

                var personService = new PersonService( rockContext );
                var ptoAllocationService = new PtoAllocationService( rockContext );
                var ptoTypeService = new PtoTypeService( rockContext );
                var personAliasService = new PersonAliasService( rockContext );

                var (calculatedFiscalStartDate, calculatedFiscalEndDate) = GetFiscalDates( fiscalYearEnd );

                var hireDateAttributeGuid = GetAttributeValue( AttributeKey.HireDate ).AsGuidOrNull();
                var fireDateAttributeGuid = GetAttributeValue( AttributeKey.FireDate ).AsGuidOrNull();
                var supervisorAttributeGuid = GetAttributeValue( AttributeKey.Supervisor ).AsGuidOrNull();
                var ministryAreaAttributeGuid = GetAttributeValue( AttributeKey.MinistryArea ).AsGuidOrNull();

                // Get employees during specified year
                var hiredPeople = new List<int>();
                var firedPeople = new List<int>();

                if ( hireDateAttributeGuid.HasValue )
                {
                    hiredPeople = personService.Queryable().AsNoTracking()
                        .WhereAttributeValue( rockContext, x => x.Attribute.Guid == hireDateAttributeGuid.Value && x.ValueAsDateTime <= calculatedFiscalEndDate )
                        .Select( p => p.Id )
                        .ToList();
                }

                if ( fireDateAttributeGuid.HasValue )
                {
                    firedPeople = personService.Queryable().AsNoTracking()
                        .WhereAttributeValue( rockContext, x => x.Attribute.Guid == fireDateAttributeGuid.Value && x.ValueAsDateTime <= calculatedFiscalStartDate )
                        .Select( p => p.Id )
                        .ToList();
                }

                var qry = personService.Queryable().AsNoTracking();
                qry = qry.Where( p => hiredPeople.Contains( p.Id ) );

                if ( firedPeople.Any() )
                {
                    qry = qry.Where( p => !firedPeople.Contains( p.Id ) );
                }

                // Filter by supervisor
                if ( supervisorAttributeGuid.HasValue && supervisorId.HasValue )
                {
                    qry = qry.WhereAttributeValue( rockContext, x => x.Attribute.Guid == supervisorAttributeGuid.Value && x.ValueAsPersonId == supervisorId.Value );
                }

                // Filter by ministry area
                if ( ministryAreaAttributeGuid.HasValue && ministryArea.IsNotNullOrWhiteSpace() )
                {
                    qry = qry.WhereAttributeValue( rockContext, x => x.Attribute.Guid == ministryAreaAttributeGuid.Value && x.Value == ministryArea );
                }

                var sortedQry = qry
                    .OrderBy( b => b.LastName )
                    .ThenBy( p => p.FirstName );

                var people = sortedQry.ToList();

                // Get PTO types
                var ptoTypesQry = ptoTypeService.Queryable().AsNoTracking().Where( pto => pto.IsActive == true );
                if ( ptoTypeId.HasValue && ptoTypeId.Value > 0 )
                {
                    ptoTypesQry = ptoTypesQry.Where( p => p.Id == ptoTypeId.Value );
                }
                var ptoTypes = ptoTypesQry.ToList();
                var ptoTypeIds = ptoTypes.Select( pto => pto.Id ).ToList();

                var rows = new List<Dictionary<string, object>>();

                foreach ( var person in people )
                {
                    person.LoadAttributes( rockContext );

                    var row = new Dictionary<string, object>
                    {
                        ["idKey"] = person.IdKey,
                        ["name"] = person.FullNameReversed
                    };

                    // Get supervisor
                    if ( supervisorAttributeGuid.HasValue )
                    {
                        var supervisorAttribute = AttributeCache.Get( supervisorAttributeGuid.Value );
                        if ( supervisorAttribute != null )
                        {
                            var supervisorAliasGuid = person.GetAttributeValue( supervisorAttribute.Key ).AsGuid();
                            var supervisorAlias = personAliasService.Get( supervisorAliasGuid );
                            row["supervisor"] = supervisorAlias?.Person?.FullNameReversed ?? string.Empty;
                        }
                    }

                    // Get ministry area
                    if ( ministryAreaAttributeGuid.HasValue )
                    {
                        var ministryAreaAttribute = AttributeCache.Get( ministryAreaAttributeGuid.Value );
                        if ( ministryAreaAttribute != null )
                        {
                            row["ministryArea"] = person.GetAttributeValue( ministryAreaAttribute.Key ) ?? string.Empty;
                        }
                    }

                    // Get allocations
                    var allocationQry = ptoAllocationService
                        .Queryable()
                        .AsNoTracking()
                        .Where( a => ( !ptoTypeIds.Any() || ptoTypeIds.Contains( a.PtoTypeId ) ) &&
                                    a.PersonAlias.PersonId == person.Id &&
                                    a.PtoAllocationStatus == PtoAllocationStatus.Active );

                    // Filter by fiscal year
                    if ( fiscalYearEnd.HasValue && fiscalYearEnd != 0 )
                    {
                        allocationQry = allocationQry.Where( a =>
                            ( a.StartDate >= calculatedFiscalStartDate && a.StartDate <= calculatedFiscalEndDate ) ||
                            ( a.EndDate.HasValue && a.EndDate >= calculatedFiscalStartDate && a.EndDate <= calculatedFiscalEndDate ) ||
                            ( !a.EndDate.HasValue && a.StartDate <= calculatedFiscalEndDate )
                        );
                    }

                    var allocations = allocationQry
                        .Include( a => a.PtoType )
                        .Include( a => a.PtoRequests )
                        .OrderByDescending( b => b.StartDate )
                        .ThenByDescending( b => b.EndDate )
                        .ToList();

                    var allocationPtoTypes = allocations.GroupBy( a => a.PtoTypeId );

                    var allocationItems = allocations
                        .Select( a => string.Format( "{0}: {1} - {2} ({3} hrs)",
                            a.PtoType.Name,
                            a.StartDate.ToShortDateString(),
                            a.EndDate.HasValue ? a.EndDate.Value.ToShortDateString() : "N/A",
                            a.Hours ) )
                        .ToList();

                    var accruedItems = new List<string>();
                    var takenItems = new List<string>();
                    var remainingItems = new List<object>();

                    foreach ( var ptoType in ptoTypes )
                    {
                        var name = ptoType.Name;
                        decimal accruedHours = 0;
                        decimal takenHours = 0;
                        decimal remainingHours = 0;

                        var allocationPtoType = allocationPtoTypes.Where( a => a.Key == ptoType.Id ).FirstOrDefault();
                        if ( allocationPtoType != null )
                        {
                            accruedHours = allocationPtoType.Sum( pt => pt.Hours );
                            takenHours = allocationPtoType.Sum( pt => pt.PtoRequests
                                .Where( pr => pr.PtoRequestApprovalState != PtoRequestApprovalState.Cancelled &&
                                             pr.PtoRequestApprovalState != PtoRequestApprovalState.Denied )
                                .Sum( pr => pr.Hours ) );
                            remainingHours = accruedHours - takenHours;
                        }

                        if ( showUnallocatedPtoTypes || accruedHours > 0 || takenHours > 0 )
                        {
                            accruedItems.Add( string.Format( "{0}: {1}", name, accruedHours ) );
                            takenItems.Add( string.Format( "{0}: {1}", name, takenHours ) );
                            remainingItems.Add( new
                            {
                                text = string.Format( "{0}: {1}", name, remainingHours ),
                                isNegative = remainingHours < 0
                            } );
                        }
                    }

                    row["allocations"] = allocationItems;
                    row["totalAccrued"] = accruedItems;
                    row["totalTaken"] = takenItems;
                    row["remaining"] = remainingItems;

                    rows.Add( row );
                }

                var gridData = new GridDataBag
                {
                    Rows = rows
                };

                return ActionOk( gridData );
            }
        }

        #endregion
    }
}
