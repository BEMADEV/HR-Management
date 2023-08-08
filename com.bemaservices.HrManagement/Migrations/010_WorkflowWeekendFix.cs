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
    [MigrationNumber( 10, "1.11.2" )]
    public class WorkflowWeekendFix : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            PtoRequestWorkflow();
        }

        private void PtoRequestWorkflow()
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
            {% for i in (0...dayCount) %}
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
            RockMigrationHelper.AddActionTypeAttributeValue( "6299C5B3-7233-4CD3-9FD6-91A1B286C5CC", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // PTO Request:Add / Modify Request:Set Requested Hours:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "6299C5B3-7233-4CD3-9FD6-91A1B286C5CC", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"52166c99-8a84-437e-8e01-a0282cd6e5bc" ); // PTO Request:Add / Modify Request:Set Requested Hours:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "6299C5B3-7233-4CD3-9FD6-91A1B286C5CC", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // PTO Request:Add / Modify Request:Set Requested Hours:Enabled Lava Commands
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
            {% for i in (0...dayCount) %}
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
            RockMigrationHelper.AddActionTypeAttributeValue( "6D5A6A43-F2E5-40F9-ACEE-54F14C904787", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // PTO Request:Review Request:Set Requested Hours:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "6D5A6A43-F2E5-40F9-ACEE-54F14C904787", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"52166c99-8a84-437e-8e01-a0282cd6e5bc" ); // PTO Request:Review Request:Set Requested Hours:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "6D5A6A43-F2E5-40F9-ACEE-54F14C904787", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // PTO Request:Review Request:Set Requested Hours:Enabled Lava Commands
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
            {% for i in (0...dayCount) %}
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
            RockMigrationHelper.AddActionTypeAttributeValue( "7CA58F02-2006-4F02-9566-17FA43FF4E42", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // PTO Request:Cancel Request:Set Requested Hours:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "7CA58F02-2006-4F02-9566-17FA43FF4E42", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"52166c99-8a84-437e-8e01-a0282cd6e5bc" ); // PTO Request:Cancel Request:Set Requested Hours:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "7CA58F02-2006-4F02-9566-17FA43FF4E42", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // PTO Request:Cancel Request:Set Requested Hours:Enabled Lava Commands
            

            #region DefinedValue AttributeType qualifier helper

            Sql( @"
			UPDATE [aq] SET [key] = 'definedtype', [Value] = CAST( [dt].[Id] as varchar(5) )
			FROM [AttributeQualifier] [aq]
			INNER JOIN [Attribute] [a] ON [a].[Id] = [aq].[AttributeId]
			INNER JOIN [FieldType] [ft] ON [ft].[Id] = [a].[FieldTypeId]
			INNER JOIN [DefinedType] [dt] ON CAST([dt].[guid] AS varchar(50) ) = [aq].[value]
			WHERE [ft].[class] = 'Rock.Field.Types.DefinedValueFieldType'
			AND [aq].[key] = 'definedtypeguid'
            And ( Select top 1 Id from AttributeQualifier aq1 Where aq1.[Key] = 'definedtype' and aq1.AttributeId = aq.AttributeId ) is null
		" );

            #endregion
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
        }
    }
}
