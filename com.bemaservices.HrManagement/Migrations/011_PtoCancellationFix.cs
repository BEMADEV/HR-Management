using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rock.Plugin;

using com.bemaservices.HrManagement.SystemGuid;
using Rock.Web.Cache;
using Rock.Lava.Blocks;
using System.Security.AccessControl;

namespace com.bemaservices.HrManagement.Migrations
{
    [MigrationNumber( 11, "1.12.7" )]
    public class PtoCancellationFix : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.AddActionTypeAttributeValue( "6299C5B3-7233-4CD3-9FD6-91A1B286C5CC", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign startDate = Workflow | Attribute:'StartDate' %}
{% assign endDate = Workflow | Attribute:'EndDate' %}
{% assign hoursPerDay = Workflow | Attribute:'HoursDay' %}
{% assign excludeWeekends = Workflow | Attribute:'ExcludeWeekends' | AsBoolean %}
{% assign requestDate = startDate %}
{% assign totalRequestHours = 0 %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
{% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}

{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign person = ptoAllocation.PersonAlias.Person %}
        {% assign ptoTier =  person | Attribute:'PTOTier','Object' %}
        {% assign weekendDays = ptoTier.DaysOfWeek | Replace:'0','Sunday' | Replace:'1','Monday' | Replace:'2','Tuesday' | Replace:'3','Wednesday' | Replace:'4','Thursday' | Replace:'5','Friday' | Replace:'6','Saturday' %}

        {% if endDate != empty %}
            {% assign dayCount = startDate | DateDiff:endDate, 'd' %}
            {% for i in (0..dayCount) %}
                {% assign requestDate = startDate | DateAdd:i,'d' %}
                {% assign dayCounts = true %}
                
                {% if excludeWeekends == true  %}
                    {% assign dayOfWeek = requestDate | Date:'dddd' %}
                    {% if weekendDays contains dayOfWeek %}
                        {% assign dayCounts = false %}
                    {% endif %}
                {% endif %}
                
                {% if dayCounts %}
                    {% assign totalRequestHours = totalRequestHours | Plus:hoursPerDay %}
                {% endif %}
            {% endfor %}
        {% else %}
            {% assign totalRequestHours = hoursPerDay %}
        {% endif %}
    {% endfor %}
{% endptoallocation %}

{{totalRequestHours }}" ); // PTO Request:Add / Modify Request:Set Requested Hours:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "6D5A6A43-F2E5-40F9-ACEE-54F14C904787", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign startDate = Workflow | Attribute:'StartDate' %}
{% assign endDate = Workflow | Attribute:'EndDate' %}
{% assign hoursPerDay = Workflow | Attribute:'HoursDay' %}
{% assign excludeWeekends = Workflow | Attribute:'ExcludeWeekends' | AsBoolean %}
{% assign requestDate = startDate %}
{% assign totalRequestHours = 0 %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
{% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}

{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign person = ptoAllocation.PersonAlias.Person %}
        {% assign ptoTier =  person | Attribute:'PTOTier','Object' %}
        {% assign weekendDays = ptoTier.DaysOfWeek | Replace:'0','Sunday' | Replace:'1','Monday' | Replace:'2','Tuesday' | Replace:'3','Wednesday' | Replace:'4','Thursday' | Replace:'5','Friday' | Replace:'6','Saturday' %}

        {% if endDate != empty %}
            {% assign dayCount = startDate | DateDiff:endDate, 'd' %}
            {% for i in (0..dayCount) %}
                {% assign requestDate = startDate | DateAdd:i,'d' %}
                {% assign dayCounts = true %}
                
                {% if excludeWeekends == true  %}
                    {% assign dayOfWeek = requestDate | Date:'dddd' %}
                    {% if weekendDays contains dayOfWeek %}
                        {% assign dayCounts = false %}
                    {% endif %}
                {% endif %}
                
                {% if dayCounts %}
                    {% assign totalRequestHours = totalRequestHours | Plus:hoursPerDay %}
                {% endif %}
            {% endfor %}
        {% else %}
            {% assign totalRequestHours = hoursPerDay %}
        {% endif %}
    {% endfor %}
{% endptoallocation %}

{{totalRequestHours }}" ); // PTO Request:Review Request:Set Requested Hours:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "7CA58F02-2006-4F02-9566-17FA43FF4E42", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign startDate = Workflow | Attribute:'StartDate' %}
{% assign endDate = Workflow | Attribute:'EndDate' %}
{% assign hoursPerDay = Workflow | Attribute:'HoursDay' %}
{% assign excludeWeekends = Workflow | Attribute:'ExcludeWeekends' | AsBoolean %}
{% assign requestDate = startDate %}
{% assign totalRequestHours = 0 %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
{% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}

{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign person = ptoAllocation.PersonAlias.Person %}
        {% assign ptoTier =  person | Attribute:'PTOTier','Object' %}
        {% assign weekendDays = ptoTier.DaysOfWeek | Replace:'0','Sunday' | Replace:'1','Monday' | Replace:'2','Tuesday' | Replace:'3','Wednesday' | Replace:'4','Thursday' | Replace:'5','Friday' | Replace:'6','Saturday' %}

        {% if endDate != empty %}
            {% assign dayCount = startDate | DateDiff:endDate, 'd' %}
            {% for i in (0..dayCount) %}
                {% assign requestDate = startDate | DateAdd:i,'d' %}
                {% assign dayCounts = true %}
                
                {% if excludeWeekends == true  %}
                    {% assign dayOfWeek = requestDate | Date:'dddd' %}
                    {% if weekendDays contains dayOfWeek %}
                        {% assign dayCounts = false %}
                    {% endif %}
                {% endif %}
                
                {% if dayCounts %}
                    {% assign totalRequestHours = totalRequestHours | Plus:hoursPerDay %}
                {% endif %}
            {% endfor %}
        {% else %}
            {% assign totalRequestHours = hoursPerDay %}
        {% endif %}
    {% endfor %}
{% endptoallocation %}

{{totalRequestHours }}" ); // PTO Request:Cancel Request:Set Requested Hours:Lava
            
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
        }
    }
}
