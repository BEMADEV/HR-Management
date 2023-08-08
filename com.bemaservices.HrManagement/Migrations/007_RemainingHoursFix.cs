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
    [MigrationNumber( 7, "1.11.2" )]
    public class RemainingHoursFix : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.AddActionTypeAttributeValue( "4556A8F7-7B85-4246-892D-BF7EA5CB4FB8", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign requestHours = Workflow | Attribute:'RequestedHours' %}

{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ptoAllocationGuid}}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState == 1 %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
        {% assign remainingHours = totalHours | Minus:takenHours %}
    {% endfor %}
{% endptoallocation %}
{% assign remainingHours = remainingHours | Minus:requestHours %}

{{ remainingHours }}" ); // PTO Request:Add / Modify Request:Check Remaining Hours:Lava
           
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
        }
    }
}
