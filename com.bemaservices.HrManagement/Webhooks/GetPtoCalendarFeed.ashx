<%@ WebHandler Language="C#" Class="com.bemaservices.Webhooks.GetPtoCalendarFeed" %>
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;

using RestSharp.Extensions;

using com.bemaservices.HrManagement.Model;

using System.Globalization;

namespace com.bemaservices.Webhooks
{
    /// <summary>
    /// Summary description for GetPtoCalendarFeed
    /// </summary>
    public class GetPtoCalendarFeed : IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;
        private string interactionDeviceType;

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public void ProcessRequest( HttpContext httpContext )
        {
            try
            {
                request = httpContext.Request;
                response = httpContext.Response;
                interactionDeviceType = InteractionDeviceType.GetClientType( request.UserAgent );

                RockContext rockContext = new RockContext();
                PtoCalendarProps ptoCalendarProps = ValidateRequestData( httpContext );

                if ( ptoCalendarProps == null )
                {
                    return;
                }

                iCalendar icalendar = CreateICalendar( ptoCalendarProps );

                iCalendarSerializer serializer = new iCalendarSerializer();
                string s = serializer.SerializeToString( icalendar );

                response.Clear();
                response.ClearHeaders();
                response.ClearContent();
                response.AddHeader( "content-disposition", string.Format( "attachment; filename={0}_ical.ics", DateTime.Now.ToString( "yyyy-MM-dd_hhmmss" ) ) );
                response.ContentType = "text/calendar";
                response.Write( s );
            }
            catch ( Exception ex )
            {
                ExceptionLogService.LogException( ex, httpContext );
                SendBadRequest( httpContext );
            }
        }

        /// <summary>
        /// Creates the iCalendar object and populates it with events
        /// </summary>
        /// <param name="calendarProps">The calendar props.</param>
        /// <returns></returns>
        private iCalendar CreateICalendar( PtoCalendarProps ptoCalendarProps )
        {
            // Get a list of PTO Requests filtered by ptoCalendarProps
            List<PtoRequest> ptoRequests = GetPtoRequests( ptoCalendarProps );

            // Create the iCalendar
            iCalendar icalendar = new iCalendar();
            icalendar.AddLocalTimeZone();

            // Create each of the events for the calendar(s)
            foreach ( PtoRequest ptoRequest in ptoRequests )
            {
                Event ievent = new Event();
                ievent.DTStart = new DDay.iCal.iCalDateTime( ptoRequest.RequestDate );
                ievent.DTStart.HasTime = true;
                // make a one second duration since a zero duration won't be included in occurrences
                ievent.Duration = new TimeSpan( 0, 0, 1 );

                // Rock has more descriptions than iCal so lets concatenate them
                string description = CreatePtoDescription( ptoRequest );

                ievent.Summary = description;

                ievent.DTStart.SetTimeZone( icalendar.TimeZones[0] );
                ievent.DTEnd.SetTimeZone( icalendar.TimeZones[0] );

                // Don't set the description prop for outlook to force it to use the X-ALT-DESC property which can have markup.
                if ( interactionDeviceType != "Outlook" )
                {
                    ievent.Description = description.ConvertBrToCrLf()
                                                        .Replace( "</P>", "" )
                                                        .Replace( "</p>", "" )
                                                        .Replace( "<P>", Environment.NewLine )
                                                        .Replace( "<p>", Environment.NewLine )
                                                        .Replace( "&nbsp;", " " )
                                                        .SanitizeHtml();
                }

                // HTML version of the description for outlook
                ievent.AddProperty( "X-ALT-DESC;FMTTYPE=text/html", "<html>" + description + "</html>" );

                // classification: "PUBLIC", "PRIVATE", "CONFIDENTIAL"
                ievent.Class = "PUBLIC";

                var person = ptoRequest.PtoAllocation.PersonAlias.Person;
                // add contact info if it exists
                if ( person != null )
                {
                    ievent.Organizer = new Organizer( string.Format( "MAILTO:{0}", person.Email ) );
                    ievent.Organizer.CommonName = person.FullName;

                    // Outlook doesn't seems to use Contacts or Comments
                    string contactName = !string.IsNullOrEmpty( person.FullName ) ? "Name: " + person.FullName : string.Empty;
                    string contactInfo = contactName;

                    ievent.Contacts.Add( contactInfo );
                    ievent.Comments.Add( contactInfo );
                }

                icalendar.Events.Add( ievent );
            }

            return icalendar;
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
        /// Sends the bad request response
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="addlInfo">The addl information.</param>
        private void SendBadRequest( HttpContext httpContext, string addlInfo = "" )
        {
            httpContext.Response.StatusCode = HttpStatusCode.BadRequest.ConvertToInt();
            httpContext.Response.StatusDescription = "Request is invalid or malformed. " + addlInfo;
            httpContext.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Validates the request data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private PtoCalendarProps ValidateRequestData( HttpContext context )
        {
            PtoCalendarProps calendarProps = new PtoCalendarProps();


            string ptoTypeGuidQueryString = request.QueryString["ptotypeguids"] != null ? request.QueryString["ptotypeguids"] : string.Empty;
            calendarProps.PtoTypeGuids = ParseGuids( ptoTypeGuidQueryString );

            string ptoTypeIdQueryString = request.QueryString["ptotypeids"] != null ? request.QueryString["ptotypeids"] : string.Empty;
            calendarProps.PtoTypeIds = ParseIds( ptoTypeIdQueryString );

            string employeeIdQueryString = request.QueryString["employeeids"] != null ? request.QueryString["employeeids"] : string.Empty;
            calendarProps.EmployeeIds = ParseIds( employeeIdQueryString );

            string approvalStateIdQueryString = request.QueryString["approvalstates"] != null ? request.QueryString["approvalstates"] : string.Empty;
            calendarProps.ApprovalStates = ParseApprovalStates( approvalStateIdQueryString );

            string startDate = request.QueryString["startdate"];
            if ( !string.IsNullOrWhiteSpace( startDate ) )
            {
                calendarProps.StartDate = DateTime.ParseExact( startDate, "yyyyMMdd", CultureInfo.InvariantCulture );
            }

            string endDate = request.QueryString["enddate"];
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