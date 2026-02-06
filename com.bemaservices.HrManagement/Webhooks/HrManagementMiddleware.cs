using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.bemaservices.HrManagement.Model;
using Microsoft.Owin;
using Rock.Data;
using Rock.Model;
using Rock;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.Net;
using PuppeteerSharp;
using Quartz;
using TimeZoneConverter;
using com.bemaservices.HrManagement.Utility.RockInternalMethods;

namespace com.bemaservices.HrManagement.Webhooks
{
    public class HrManagementMiddleware : OwinMiddleware
    {
        public HrManagementMiddleware( OwinMiddleware next )
            : base( next )
        {
        }

        /// <inheritdoc/>
        public override async Task Invoke( IOwinContext context )
        {
            var path = context.Request.Uri.AbsolutePath;

            if ( !path.EndsWith( "/GetPtoCalendarFeed.ashx", StringComparison.OrdinalIgnoreCase ) )
            {
                await Next.Invoke( context );
                return;
            }

            try
            {
                var request = context.Request;
                var response = context.Response;
                var interactionDeviceType = InteractionDeviceType.GetClientType( request.Headers["User-Agent"] );

                RockContext rockContext = new RockContext();
                PtoCalendarProps ptoCalendarProps = ValidateRequestData( context );

                if ( ptoCalendarProps == null )
                {
                    SendBadRequest( context );
                    return;
                }

                string icalendarString = CreateICalendar( ptoCalendarProps, interactionDeviceType );

                response.Headers.Clear();
                response.Headers.Add( "Content-Disposition", new[] { string.Format( "attachment; filename={0}_ical.ics", DateTime.Now.ToString( "yyyy-MM-dd_hhmmss" ) ) } );
                response.ContentType = "text/calendar";
                await response.WriteAsync( icalendarString );
            }
            catch ( Exception ex )
            {
                ExceptionLogService.LogException( ex, System.Web.HttpContext.Current );
                SendBadRequest( context );
            }
        }

        private void SendNotAuthorized( IOwinContext context )
        {
            context.Response.StatusCode = ( int ) HttpStatusCode.Forbidden;
            context.Response.ReasonPhrase = "Not authorized to view reservation type.";
        }

        private void SendBadRequest( IOwinContext context, string addlInfo = "" )
        {
            context.Response.StatusCode = ( int ) HttpStatusCode.BadRequest;
            context.Response.ReasonPhrase = "Request is invalid or malformed. " + addlInfo;
        }

        /// <summary>
        /// Creates the iCalendar object and populates it with events
        /// </summary>
        /// <param name="calendarProps">The calendar props.</param>
        /// <returns></returns>
        private string CreateICalendar( PtoCalendarProps ptoCalendarProps, string interactionDeviceType )
        {
            // Get a list of PTO Requests filtered by ptoCalendarProps
            List<PtoRequest> ptoRequests = GetPtoRequests( ptoCalendarProps );

            // Create the iCalendar
            var iCalendar = new Ical.Net.Calendar();

            // Specify the calendar timezone using the Internet Assigned Numbers Authority (IANA) identifier, because most third-party applications
            // require this to interpret event times correctly.
            var timeZoneId = TZConvert.WindowsToIana( RockDateTime.OrgTimeZoneInfo.Id );

            var setEventDescription = ( interactionDeviceType != "Outlook" );

            // Keep track of the earliest event date/time, so we can use it to set the calendar's time zone info below.
            var earliestEventDateTime = RockDateTime.Now;

            // Create each of the events for the calendar(s)
            foreach ( PtoRequest ptoRequest in ptoRequests )
            {
                if ( ptoRequest.RequestDate < earliestEventDateTime )
                {
                    earliestEventDateTime = ptoRequest.RequestDate;
                }

                var calendarEvent = new CalendarEvent();
                calendarEvent.IsAllDay = true;
                var calDateTime = new CalDateTime( ptoRequest.RequestDate, timeZoneId );
                calDateTime.HasTime = true;
                calendarEvent.DtStart = calDateTime;
                calendarEvent.End = null; // all day event

                // Create a new calendar event copy to prevent thread-safety issues. This might not be a legitimate
                // concern, but we've historically done this, so it doesn't hurt to leave this behavior in place.
                calendarEvent = EventCalendarServiceOverrides.CopyCalendarEvent( calendarEvent );

                // Rock has more descriptions than iCal so lets concatenate them
                string description = CreatePtoDescription( ptoRequest );

                calendarEvent.Summary = description;

                // Don't set the description prop for outlook to force it to use the X-ALT-DESC property which can have markup.
                if ( setEventDescription)
                {
                    calendarEvent.Description = description.ConvertBrToCrLf()
                                                        .Replace( "</P>", "" )
                                                        .Replace( "</p>", "" )
                                                        .Replace( "<P>", Environment.NewLine )
                                                        .Replace( "<p>", Environment.NewLine )
                                                        .Replace( "&nbsp;", " " )
                                                        .SanitizeHtml();
                }

                // HTML version of the description for outlook
                calendarEvent.AddProperty( "X-ALT-DESC;FMTTYPE=text/html", "<html>" + description + "</html>" );

                // classification: "PUBLIC", "PRIVATE", "CONFIDENTIAL"
                calendarEvent.Class = "PUBLIC";

                var person = ptoRequest.PtoAllocation.PersonAlias.Person;
                // add contact info if it exists
                if ( person != null )
                {
                    calendarEvent.Organizer = new Organizer( string.Format( "MAILTO:{0}", person.Email ) );
                    calendarEvent.Organizer.CommonName = person.FullName;

                    // Outlook doesn't seems to use Contacts or Comments
                    string contactName = !string.IsNullOrEmpty( person.FullName ) ? "Name: " + person.FullName : string.Empty;
                    string contactInfo = contactName;

                    calendarEvent.Contacts.Add( contactInfo );
                    calendarEvent.Comments.Add( contactInfo );
                }

                iCalendar.Events.Add( calendarEvent );
            }

            // Find a non-DST date to use as the earliest supported timezone date, also ensuring that it is not a leap-day.
            // This is necessary to work around a bug in the iCal.Net framework (v4.2.0).
            // See https://github.com/rianjs/ical.net/issues/439.
            var tzInfo = TZConvert.GetTimeZoneInfo( timeZoneId );
            if ( tzInfo.SupportsDaylightSavingTime )
            {
                for ( var i = 0; i < 365; i++ )
                {
                    if ( !tzInfo.IsDaylightSavingTime( earliestEventDateTime ) )
                    {
                        break;
                    }
                    earliestEventDateTime = earliestEventDateTime.AddDays( -1 );
                };
            }

            // Ensure that the target date is not a leap-day.
            // This is necessary to work around a bug in the iCal.Net framework (v4.2.0).
            if ( earliestEventDateTime.Month == 2 && earliestEventDateTime.Day == 29 )
            {
                earliestEventDateTime = earliestEventDateTime.AddDays( -1 );
            }

            iCalendar.AddTimeZone( VTimeZone.FromDateTimeZone( timeZoneId, earliestEventDateTime, includeHistoricalData: true ) );

            // Return a serialized iCalendar.
            var iCalendarString = InetCalendarHelperOverrides.SerializeCalendarForExport( iCalendar );

            return iCalendarString;
        }

        /// <summary>
        /// Creates the event description from the lava template. Default is used if one is not specified in the request.
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        /// <param name="occurrence">The occurrence.</param>
        /// <returns></returns>
        private string CreatePtoDescription( PtoRequest ptoRequest )
        {
            return String.Format( "{0}: {1}", ptoRequest.PtoAllocation.PersonAlias.Person.FullName, ptoRequest.PtoAllocation.PtoType.Name );
        }

        /// <summary>
        /// Uses the filter information in the CalendarProps object to get a list of events
        /// </summary>
        /// <param name="calendarProps">The calendar props.</param>
        /// <returns></returns>
        private List<PtoRequest> GetPtoRequests( PtoCalendarProps ptoCalendarProps )
        {
            RockContext rockContext = new RockContext();

            PtoRequestService ptoRequestService = new PtoRequestService( rockContext );
            var ptoRequestQueryable = ptoRequestService
                .Queryable()
                .Where( p => p.RequestDate <= ptoCalendarProps.EndDate && ptoCalendarProps.StartDate <= p.RequestDate );

            // For PTO Type
            if ( ptoCalendarProps.PtoTypeGuids.Any() || ptoCalendarProps.PtoTypeIds.Any() )
            {
                ptoRequestQueryable = ptoRequestQueryable.Where( p => ptoCalendarProps.PtoTypeIds.Contains( p.PtoAllocation.PtoTypeId ) || ptoCalendarProps.PtoTypeGuids.Contains( p.PtoAllocation.PtoType.Guid ) );
            }

            // For Employee
            if ( ptoCalendarProps.EmployeeIds.Any() )
            {
                ptoRequestQueryable = ptoRequestQueryable.Where( p => ptoCalendarProps.EmployeeIds.Contains( p.PtoAllocation.PersonAlias.PersonId ) );
            }

            // For Approval State
            if ( ptoCalendarProps.ApprovalStates.Any() )
            {
                ptoRequestQueryable = ptoRequestQueryable.Where( p => ptoCalendarProps.ApprovalStates.Contains( p.PtoRequestApprovalState ) );
            }
            else
            {
                ptoRequestQueryable = ptoRequestQueryable.Where( p => p.PtoRequestApprovalState == PtoRequestApprovalState.Approved );
            }

            return ptoRequestQueryable.ToList();
        }

        /// <summary>
        /// Validates the request data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private PtoCalendarProps ValidateRequestData( IOwinContext context )
        {
            var query = context.Request.Query;

            PtoCalendarProps calendarProps = new PtoCalendarProps();


            string ptoTypeGuidQueryString = query.Get( "ptotypeguids" ) != null ? query.Get( "ptotypeguids" ) : string.Empty;
            calendarProps.PtoTypeGuids = ParseGuids( ptoTypeGuidQueryString );

            string ptoTypeIdQueryString = query.Get( "ptotypeids" ) != null ? query.Get( "ptotypeids" ) : string.Empty;
            calendarProps.PtoTypeIds = ParseIds( ptoTypeIdQueryString );

            string employeeIdQueryString = query.Get( "employeeids" ) != null ? query.Get( "employeeids" ) : string.Empty;
            calendarProps.EmployeeIds = ParseIds( employeeIdQueryString );

            string approvalStateIdQueryString = query.Get( "approvalstates" ) != null ? query.Get( "approvalstates" ) : string.Empty;
            calendarProps.ApprovalStates = ParseApprovalStates( approvalStateIdQueryString );

            string startDate = query.Get( "startdate" );
            if ( !string.IsNullOrWhiteSpace( startDate ) )
            {
                calendarProps.StartDate = DateTime.ParseExact( startDate, "yyyyMMdd", CultureInfo.InvariantCulture );
            }

            string endDate = query.Get( "enddate" );
            if ( !string.IsNullOrWhiteSpace( endDate ) )
            {
                calendarProps.EndDate = DateTime.ParseExact( endDate, "yyyyMMdd", CultureInfo.InvariantCulture );
            }

            return calendarProps;
        }

        /// <summary>
        /// Parses a query string for a list of Ids
        /// </summary>
        /// <returns></returns>
        private List<int> ParseIds( string queryParamemter )
        {
            List<string> stringIdList = new List<string>();
            List<int> intIdList = new List<int>();

            if ( queryParamemter.IsNotNullOrWhiteSpace() )
            {
                stringIdList = queryParamemter.Split( ',' ).ToList();

                foreach ( string stringId in stringIdList )
                {
                    int intId;
                    if ( int.TryParse( stringId, out intId ) )
                    {
                        intIdList.Add( intId );
                    }
                }
            }

            return intIdList;
        }

        /// <summary>
        /// Parses a query string for a list of Ids
        /// </summary>
        /// <returns></returns>
        private List<Guid> ParseGuids( string queryParamemter )
        {
            List<string> stringGuidList = new List<string>();
            List<Guid> guidGuidList = new List<Guid>();

            if ( queryParamemter.IsNotNullOrWhiteSpace() )
            {
                stringGuidList = queryParamemter.Split( ',' ).ToList();

                foreach ( string stringGuid in stringGuidList )
                {
                    Guid guidGuid;
                    if ( Guid.TryParse( stringGuid, out guidGuid ) )
                    {
                        guidGuidList.Add( guidGuid );
                    }
                }
            }

            return guidGuidList;
        }

        /// <summary>
        /// Parses a query string for a list of Ids
        /// </summary>
        /// <returns></returns>
        private List<PtoRequestApprovalState> ParseApprovalStates( string queryParamemter )
        {
            List<string> stringEnumList = new List<string>();
            List<PtoRequestApprovalState> enumList = new List<PtoRequestApprovalState>();

            if ( queryParamemter.IsNotNullOrWhiteSpace() )
            {
                stringEnumList = queryParamemter.Split( ',' ).ToList();

                foreach ( string stringEnum in stringEnumList )
                {
                    PtoRequestApprovalState? enumVariable = stringEnum.ConvertToEnumOrNull<PtoRequestApprovalState>();

                    if ( enumVariable.HasValue )
                    {
                        enumList.Add( enumVariable.Value );
                    }
                }
            }

            return enumList;
        }

        /// <summary>
        /// PtoTypeIds, EmployeeIds, ApprovalStatuses, Startdate, and Enddate are optional.
        /// StartDate defaults to the current date, EndDate defaults to the currentDate + 2 months.
        /// </summary>
        private class PtoCalendarProps
        {
            private DateTime? _startDate;
            private DateTime? _endDate;

            /// <summary>
            /// Gets or sets the pto type guids. Leave empty to return all pto types
            /// </summary>
            /// <value>
            /// The pto type ids.
            /// </value>
            public List<Guid> PtoTypeGuids { get; set; }

            /// <summary>
            /// Gets or sets the pto type ids. Leave empty to return all pto types
            /// </summary>
            /// <value>
            /// The pto type ids.
            /// </value>
            public List<int> PtoTypeIds { get; set; }

            /// <summary>
            /// Gets or sets the employee id list. leave empty to return all employees
            /// </summary>
            /// <value>
            /// The employee ids.
            /// </value>
            public List<int> EmployeeIds { get; set; }

            /// <summary>
            /// Gets or sets the state list. leave empty to return all approved requests
            /// </summary>
            /// <value>
            /// The states.
            /// </value>
            public List<PtoRequestApprovalState> ApprovalStates { get; set; }

            /// <summary>
            /// Gets or sets the start date. if not explicitly set returns current date
            /// </summary>
            /// <value>
            /// The start date.
            /// </value>
            public DateTime StartDate
            {
                get
                {
                    return _startDate ?? DateTime.Now.AddMonths( -3 ).Date;
                }

                set
                {
                    _startDate = value;
                }
            }

            /// <summary>
            /// Gets or sets the end date. If not explicitly set returns two months from current date.
            /// </summary>
            /// <value>
            /// The end date.
            /// </value>
            public DateTime EndDate
            {
                get
                {
                    return _endDate ?? DateTime.Now.AddMonths( 12 ).Date;
                }

                set
                {
                    _endDate = value;
                }
            }
        }
    }
}
