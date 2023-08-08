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
using Rock;

namespace com.bemaservices.HrManagement.Migrations
{
    [MigrationNumber( 9, "1.11.0" )]
    public class RemainingHours : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.AddActionTypeAttributeValue( "88F975B5-7EAD-4823-A9B5-7EF55AEF1068", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
    {% endfor %}
{% endptoallocation %}
{{ takenHours }}" ); // PTO Request:Add / Modify Request:Set Requested Hours YTD:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "4556A8F7-7B85-4246-892D-BF7EA5CB4FB8", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign requestHours = Workflow | Attribute:'RequestedHours' %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ptoAllocationGuid}}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
        {% assign remainingHours = totalHours | Minus:takenHours %}
    {% endfor %}
{% endptoallocation %}
{% assign remainingHours = remainingHours | Minus:requestHours %}

{{ remainingHours }}" ); // PTO Request:Add / Modify Request:Check Remaining Hours:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "5DBCAEEF-AA8C-43FC-BC4B-D65284956AE0", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
{% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}

{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
    {% endfor %}
{% endptoallocation %}
{{ takenHours }}" ); // PTO Request:Review Request:Set Requested Hours YTD:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "A25B9E13-E915-4F7E-B99F-1D114A3CDC87", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign requestHours = Workflow | Attribute:'RequestedHours' %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ptoAllocationGuid}}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
        {% assign remainingHours = totalHours | Minus:takenHours %}
    {% endfor %}
{% endptoallocation %}
{% assign remainingHours = remainingHours | Minus:requestHours %}

{{ remainingHours }}" ); // PTO Request:Review Request:Set Remaining Hours:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "D5381E17-4D23-498B-98BB-8C7BCB5EFF88", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
    {% endfor %}
{% endptoallocation %}
{{ takenHours }}" ); // PTO Request:Cancel Request:Set Requested Hours YTD:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "AC83A176-224C-435F-A030-667AC09D7322", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign requestHours = Workflow | Attribute:'RequestedHours' %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ptoAllocationGuid}}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
        {% assign remainingHours = totalHours | Minus:takenHours %}
    {% endfor %}
{% endptoallocation %}
{% assign remainingHours = remainingHours | Minus:requestHours %}

{{ remainingHours }}" ); // PTO Request:Cancel Request:Set Remaining Hours:Lava


            RockMigrationHelper.AddActionTypeAttributeValue( "14A9F948-D901-4849-8063-0ED9190D5CC7", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
    {% endfor %}
{% endptoallocation %}
{{ takenHours }}" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours YTD:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "CD1F2017-175F-4CD4-B448-F3A9EFA1A134", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign requestHours = Workflow | Attribute:'RequestedHours' %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ptoAllocationGuid}}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 2 and request.PtoRequestApprovalState != 3 and request.Id != ptoRequest.Id %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
        {% assign remainingHours = totalHours | Minus:takenHours %}
    {% endfor %}
{% endptoallocation %}
{% assign remainingHours = remainingHours | Minus:requestHours %}

{{ remainingHours }}" ); // Auto-Approve PTO Request:Review Request:Set Remaining Hours:Lava
            
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {

        }
    }
}
