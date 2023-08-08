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
    [MigrationNumber( 8, "1.11.0" )]
    public class HrUpdate111 : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql( @"
                ALTER TABLE [_com_bemaservices_HrManagement_PtoTier] ADD DaysOfWeek [nvarchar](max) NULL;
                    " );            

            #region EntityTypes

            RockMigrationHelper.UpdateEntityType( "Rock.Model.Workflow", "3540E9A7-FE30-43A9-8B0A-A372B63DFC93", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActivity", "2CB52ED0-CB06-4D62-9E2C-73B60AFA4C9F", true, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Model.WorkflowActionType", "23E3273A-B137-48A3-9AFF-C8DC832DDCA6", true, true );
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate", "546C6C01-5C8B-449E-A16A-580D92D0317B", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.ActivateActivity", "38907A90-1634-4A93-8017-619326A4A582", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.CompleteWorkflow", "EEDA4318-F014-4A46-9C76-4C052EF81AA1", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.PersistWorkflow", "F1A39347-6FE0-43D4-89FB-544195088ECF", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.Redirect", "F4FB0FB4-B2B3-4FC4-BEEA-E9B846A63293", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.RunLava", "BC21E57A-1477-44B3-A7C2-61A806118945", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SendEmail", "66197B01-D1F0-4924-A315-47AD54E030DE", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetAttributeFromEntity", "972F19B9-598B-474B-97A4-50E56E7B59D2", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetAttributeToCurrentPerson", "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.SetAttributeValue", "C789E457-0783-44B3-9D8F-2EBAB5F11110", false, true );
            RockMigrationHelper.UpdateEntityType( "Rock.Workflow.Action.ShowHtml", "497746B4-8B0C-4D7B-9AF7-B42CEDD6C37C", false, true );
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "DE9CB292-4785-4EA3-976D-3826F91E9E98" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Person Attribute", "PersonAttribute", "The attribute to set to the currently logged in person.", 0, @"", "BBED8A83-8BB2-4D35-BAFB-05F67DCAD112" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Person Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "89E9BCED-91AB-47B0-AD52-D78B0B7CB9E8" ); // Rock.Workflow.Action.SetAttributeToCurrentPerson:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "38907A90-1634-4A93-8017-619326A4A582", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "E8ABD802-372C-47BE-82B1-96F50DB5169E" ); // Rock.Workflow.Action.ActivateActivity:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "38907A90-1634-4A93-8017-619326A4A582", "739FD425-5B8C-4605-B775-7E4D9D4C11DB", "Activity", "Activity", "The activity type to activate", 0, @"", "02D5A7A5-8781-46B4-B9FC-AF816829D240" ); // Rock.Workflow.Action.ActivateActivity:Activity
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "38907A90-1634-4A93-8017-619326A4A582", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "3809A78C-B773-440C-8E3F-A8E81D0DAE08" ); // Rock.Workflow.Action.ActivateActivity:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "497746B4-8B0C-4D7B-9AF7-B42CEDD6C37C", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "HTML", "HTML", "The HTML to show. <span class='tip tip-lava'></span>", 0, @"", "640FBD13-FEEB-4313-B6AC-6E5CF6E005DF" ); // Rock.Workflow.Action.ShowHtml:HTML
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "497746B4-8B0C-4D7B-9AF7-B42CEDD6C37C", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "05673872-1E8D-42CD-9517-7CAFBC6976F9" ); // Rock.Workflow.Action.ShowHtml:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "497746B4-8B0C-4D7B-9AF7-B42CEDD6C37C", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Hide Status Message", "HideStatusMessage", "Whether or not to hide the built-in status message.", 1, @"False", "46ACD91A-9455-41D2-8849-C2305F364418" ); // Rock.Workflow.Action.ShowHtml:Hide Status Message
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "497746B4-8B0C-4D7B-9AF7-B42CEDD6C37C", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "B3B530F2-602D-44AB-A7AB-F0839F2B0754" ); // Rock.Workflow.Action.ShowHtml:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "DD610F3E-2E83-41AE-B63B-9B163B87F82E" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Existing Pto Request", "PTO_REQUEST_ATTRIBUTE_KEY", "The Pto Request to update.", 0, @"", "C957F777-F0FE-4D05-BB22-10D7C7A5C437" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Existing Pto Request
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Allocation|Attribute Value", "ALLOCATION_KEY", "The allocation or an attribute that contains the allocation of the pto request. <span class='tip tip-lava'></span>", 1, @"", "EC01344E-61BF-4E22-88E3-36051BCAABE7" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Allocation|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Approval State|Attribute Value", "APPROVAL_STATE_KEY", "The Approval State or an attribute that contains the Approval State of the pto request. <span class='tip tip-lava'></span>", 7, @"", "080025FD-9E80-4158-8D7F-FBF3ED12A2E1" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Approval State|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Approver|Attribute Value", "APPROVER_KEY", "The approver or an attribute that contains the approver of the pto request. <span class='tip tip-lava'></span>", 6, @"", "A781A20B-4F21-47CA-9BCF-1654565DB5F6" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Approver|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "End Date|Attribute Value", "ENDDATE_KEY", "The end date or an attribute that contains the end date of the pto request. <span class='tip tip-lava'></span>", 3, @"", "8304DE14-DA5C-41FD-BA30-026D91A492C7" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:End Date|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Exclude Weekends|Attribute Value", "EXCLUDE_WEEKENDS_KEY", "Whether to Include weekends, or an attribute that contains whether or not to incldue weekends. <span class='tip tip-lava'></span>", 8, @"False", "552610AA-C128-4A6F-AAB6-20ACC0C5F060" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Exclude Weekends|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Hours|Attribute Value", "HOURS_KEY", "The hours per day or an attribute that contains the hours per day of the pto request. <span class='tip tip-lava'></span>", 4, @"", "858BFCA2-E793-446E-B146-87D5FC6783A0" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Hours|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Reason|Attribute Value", "PTO_REASON_KEY", "The reason or an attribute that contains the reason of the pto request. <span class='tip tip-lava'></span>", 5, @"", "C6A51AEB-18CB-4591-BDF8-D4017CF38DCF" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Reason|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Start Date|Attribute Value", "STARTDATE_KEY", "The start date or an attribute that contains the start date of the pto request. <span class='tip tip-lava'></span>", 2, @"", "3C5F03BD-2CDD-41D7-9ED1-5AAC62AF733D" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Start Date|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "546C6C01-5C8B-449E-A16A-580D92D0317B", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "A9B5EAF8-9CC9-4521-9FC1-480875B11CAA" ); // com.bemaservices.HrManagement.Workflow.Action.PtoRequestUpdate:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Body", "Body", "The body of the email that should be sent. <span class='tip tip-lava'></span> <span class='tip tip-html'></span>", 4, @"", "4D245B9E-6B03-46E7-8482-A51FBA190E4D" ); // Rock.Workflow.Action.SendEmail:Body
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "36197160-7D3D-490D-AB42-7E29105AFE91" ); // Rock.Workflow.Action.SendEmail:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Save Communication History", "SaveCommunicationHistory", "Should a record of this communication be saved to the recipient's profile?", 10, @"False", "1BDC7ACA-9A0B-4C8A-909E-8B4143D9C2A3" ); // Rock.Workflow.Action.SendEmail:Save Communication History
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attachment One", "AttachmentOne", "Workflow attribute that contains the email attachment. Note file size that can be sent is limited by both the sending and receiving email services typically 10 - 25 MB.", 7, @"", "C2C7DA55-3018-4645-B9EE-4BCD11855F2C" ); // Rock.Workflow.Action.SendEmail:Attachment One
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attachment Three", "AttachmentThree", "Workflow attribute that contains the email attachment. Note file size that can be sent is limited by both the sending and receiving email services typically 10 - 25 MB.", 9, @"", "A059767A-5592-4926-948A-1065AF4E9748" ); // Rock.Workflow.Action.SendEmail:Attachment Three
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attachment Two", "AttachmentTwo", "Workflow attribute that contains the email attachment. Note file size that can be sent is limited by both the sending and receiving email services typically 10 - 25 MB.", 8, @"", "FFD9193A-451F-40E6-9776-74D5DCAC1450" ); // Rock.Workflow.Action.SendEmail:Attachment Two
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Send to Group Role", "GroupRole", "An optional Group Role attribute to limit recipients to if the 'Send to Email Addresses' is a group or security role.", 2, @"", "E3667110-339F-4FE3-B6B7-084CF9633580" ); // Rock.Workflow.Action.SendEmail:Send to Group Role
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "BCC Email Addresses|BCC Attribute", "BCC", "The email addresses or an attribute that contains the person, email address, group or security role that the email should be BCC'd (blind carbon copied) to. Any address in this field will be copied on the email sent to every recipient. <span class='tip tip-lava'></span>", 6, @"", "3A131021-CB73-44A8-A142-B42832B77F60" ); // Rock.Workflow.Action.SendEmail:BCC Email Addresses|BCC Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "CC Email Addresses|CC Attribute", "CC", "The email addresses or an attribute that contains the person, email address, group or security role that the email should be CC'd (carbon copied) to. Any address in this field will be copied on the email sent to every recipient. <span class='tip tip-lava'></span>", 5, @"", "99FFD423-2AB6-481B-8749-B4793A16B620" ); // Rock.Workflow.Action.SendEmail:CC Email Addresses|CC Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "From Email Address|From Attribute", "From", "The email address or an attribute that contains the person or email address that email should be sent from (will default to organization email). <span class='tip tip-lava'></span>", 0, @"", "9F5F7CEC-F369-4FDF-802A-99074CE7A7FC" ); // Rock.Workflow.Action.SendEmail:From Email Address|From Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Send To Email Addresses|To Attribute", "To", "The email addresses or an attribute that contains the person, email address, group or security role that the email should be sent to. <span class='tip tip-lava'></span>", 1, @"", "0C4C13B8-7076-4872-925A-F950886B5E16" ); // Rock.Workflow.Action.SendEmail:Send To Email Addresses|To Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Subject", "Subject", "The subject that should be used when sending email. <span class='tip tip-lava'></span>", 3, @"", "5D9B13B6-CD96-4C7C-86FA-4512B9D28386" ); // Rock.Workflow.Action.SendEmail:Subject
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "66197B01-D1F0-4924-A315-47AD54E030DE", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "D1269254-C15A-40BD-B784-ADCC231D3950" ); // Rock.Workflow.Action.SendEmail:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Lava Template", "LavaTemplate", "By default this action will set the attribute value equal to the guid (or id) of the entity that was passed in for processing. If you include a lava template here, the action will instead set the attribute value to the output of this template. The mergefield to use for the entity is 'Entity.' For example, use {{ Entity.Name }} if the entity has a Name property. <span class='tip tip-lava'></span>", 4, @"", "7D79FC31-D0ED-4DB0-AB7D-60F4F98A1199" ); // Rock.Workflow.Action.SetAttributeFromEntity:Lava Template
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "9392E3D7-A28B-4CD8-8B03-5E147B102EF1" ); // Rock.Workflow.Action.SetAttributeFromEntity:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Entity Is Required", "EntityIsRequired", "Should an error be returned if the entity is missing or not a valid entity type?", 2, @"True", "B524B00C-29CB-49E9-9896-8BB60F209783" ); // Rock.Workflow.Action.SetAttributeFromEntity:Entity Is Required
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Use Id instead of Guid", "UseId", "Most entity attribute field types expect the Guid of the entity (which is used by default). Select this option if the entity's Id should be used instead (should be rare).", 3, @"False", "1246C53A-FD92-4E08-ABDE-9A6C37E70C7B" ); // Rock.Workflow.Action.SetAttributeFromEntity:Use Id instead of Guid
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attribute", "Attribute", "The attribute to set the value of.", 1, @"", "61E6E1BC-E657-4F00-B2E9-769AAA25B9F7" ); // Rock.Workflow.Action.SetAttributeFromEntity:Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "972F19B9-598B-474B-97A4-50E56E7B59D2", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "AD4EFAC4-E687-43DF-832F-0DC3856ABABB" ); // Rock.Workflow.Action.SetAttributeFromEntity:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "BC21E57A-1477-44B3-A7C2-61A806118945", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Lava", "Value", "The <span class='tip tip-lava'></span> to run.", 0, @"", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4" ); // Rock.Workflow.Action.RunLava:Lava
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "BC21E57A-1477-44B3-A7C2-61A806118945", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "F1924BDC-9B79-4018-9D4A-C3516C87A514" ); // Rock.Workflow.Action.RunLava:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "BC21E57A-1477-44B3-A7C2-61A806118945", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attribute", "Attribute", "The attribute to store the result in.", 1, @"", "431273C6-342D-4030-ADC7-7CDEDC7F8B27" ); // Rock.Workflow.Action.RunLava:Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "BC21E57A-1477-44B3-A7C2-61A806118945", "4BD9088F-5CC6-89B1-45FC-A2AAFFC7CC0D", "Enabled Lava Commands", "EnabledLavaCommands", "The Lava commands that should be enabled for this action.", 2, @"", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5" ); // Rock.Workflow.Action.RunLava:Enabled Lava Commands
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "BC21E57A-1477-44B3-A7C2-61A806118945", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "1B833F48-EFC2-4537-B1E3-7793F6863EAA" ); // Rock.Workflow.Action.RunLava:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C789E457-0783-44B3-9D8F-2EBAB5F11110", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "D7EAA859-F500-4521-9523-488B12EAA7D2" ); // Rock.Workflow.Action.SetAttributeValue:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C789E457-0783-44B3-9D8F-2EBAB5F11110", "33E6DF69-BDFA-407A-9744-C175B60643AE", "Attribute", "Attribute", "The attribute to set the value of.", 0, @"", "44A0B977-4730-4519-8FF6-B0A01A95B212" ); // Rock.Workflow.Action.SetAttributeValue:Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C789E457-0783-44B3-9D8F-2EBAB5F11110", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Text Value|Attribute Value", "Value", "The text or attribute to set the value from. <span class='tip tip-lava'></span>", 1, @"", "E5272B11-A2B8-49DC-860D-8D574E2BC15C" ); // Rock.Workflow.Action.SetAttributeValue:Text Value|Attribute Value
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "C789E457-0783-44B3-9D8F-2EBAB5F11110", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "57093B41-50ED-48E5-B72B-8829E62704C8" ); // Rock.Workflow.Action.SetAttributeValue:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "EEDA4318-F014-4A46-9C76-4C052EF81AA1", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "0CA0DDEF-48EF-4ABC-9822-A05E225DE26C" ); // Rock.Workflow.Action.CompleteWorkflow:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "EEDA4318-F014-4A46-9C76-4C052EF81AA1", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Status|Status Attribute", "Status", "The status to set the workflow to when marking the workflow complete. <span class='tip tip-lava'></span>", 0, @"Completed", "385A255B-9F48-4625-862B-26231DBAC53A" ); // Rock.Workflow.Action.CompleteWorkflow:Status|Status Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "EEDA4318-F014-4A46-9C76-4C052EF81AA1", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "25CAD4BE-5A00-409D-9BAB-E32518D89956" ); // Rock.Workflow.Action.CompleteWorkflow:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "50B01639-4938-40D2-A791-AA0EB4F86847" ); // Rock.Workflow.Action.PersistWorkflow:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Persist Immediately", "PersistImmediately", "This action will normally cause the workflow to be persisted (saved) once all the current activities/actions have completed processing. Set this flag to true, if the workflow should be persisted immediately. This is only required if a subsequent action needs a persisted workflow with a valid id.", 0, @"False", "82744A46-0110-4728-BD3D-66C85C5FCB2F" ); // Rock.Workflow.Action.PersistWorkflow:Persist Immediately
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F1A39347-6FE0-43D4-89FB-544195088ECF", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "86F795B0-0CB6-4DA4-9CE4-B11D0922F361" ); // Rock.Workflow.Action.PersistWorkflow:Order
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F4FB0FB4-B2B3-4FC4-BEEA-E9B846A63293", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Active", "Active", "Should Service be used?", 0, @"False", "1DAA899B-634B-4DD5-A30A-69BAC235B383" ); // Rock.Workflow.Action.Redirect:Active
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F4FB0FB4-B2B3-4FC4-BEEA-E9B846A63293", "3B1D93D7-9414-48F9-80E5-6A3FC8F94C20", "Url|Url Attribute", "Url", "The full URL to redirect to, for example: http://www.rockrms.com  <span class='tip tip-lava'></span>", 0, @"", "051BD491-817F-45DD-BBAC-875BA79E3644" ); // Rock.Workflow.Action.Redirect:Url|Url Attribute
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F4FB0FB4-B2B3-4FC4-BEEA-E9B846A63293", "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", "Processing Options", "ProcessingOptions", "How should workflow continue processing?", 1, @"0", "581736CE-76CF-46CE-A401-60A9E9EBCC1A" ); // Rock.Workflow.Action.Redirect:Processing Options
            RockMigrationHelper.UpdateWorkflowActionEntityAttribute( "F4FB0FB4-B2B3-4FC4-BEEA-E9B846A63293", "A75DFC58-7A1B-4799-BF31-451B2BBE38FF", "Order", "Order", "The order that this service should be used (priority)", 0, @"", "66A0A14E-45EC-45CD-904E-F0AC4344E1DB" ); // Rock.Workflow.Action.Redirect:Order

            #endregion

            #region Categories

            RockMigrationHelper.UpdateCategory( "C9F3C4A5-1526-474D-803F-D6C7A45CBBAE", "PTO Requests", "fa fa-clock", "", "CD21D1FD-B9DB-4122-B252-86E8FD85CEEC", 0 ); // PTO Requests

            #endregion

            #region Auto-Approve PTO Request

            RockMigrationHelper.UpdateWorkflowType( false, true, "Auto-Approve PTO Request", "", "CD21D1FD-B9DB-4122-B252-86E8FD85CEEC", "PTO Request", "fa fa-list-ol", 28800, false, 0, "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", 0 ); // Auto-Approve PTO Request
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Person", "Person", "", 0, @"", "E87CD011-20BC-4B2F-97B8-587A7C349FA0", false ); // Auto-Approve PTO Request:Person
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "6B6AA175-4758-453F-8D83-FCD8044B5F36", "Start Date", "StartDate", "", 1, @"", "24E36999-59A5-4332-A960-CF0162E2ACC8", false ); // Auto-Approve PTO Request:Start Date
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "6B6AA175-4758-453F-8D83-FCD8044B5F36", "End Date", "EndDate", "", 2, @"", "A1BC9859-1A57-4540-8892-2B4EE6194DE1", false ); // Auto-Approve PTO Request:End Date
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "C3206242-E213-4038-99EB-1A563B375997", "PTO Allocation", "PTOAllocation", "", 3, @"", "162FF669-668E-41D6-BEF5-F17B9FC3C60C", false ); // Auto-Approve PTO Request:PTO Allocation
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", "Approval State", "ApprovalState", "", 4, @"0", "640D63E4-B2C9-4A18-8D6F-4CDEBD3662C2", false ); // Auto-Approve PTO Request:Approval State
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "90ECF283-5344-4168-9224-E0D26E9B7ECB", "PTO Request", "PTORequest", "", 5, @"", "1C104818-B563-4FA2-97CD-A3225811A13D", false ); // Auto-Approve PTO Request:PTO Request
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Supervisor", "Supervisor", "", 6, @"", "36CFE4C9-BA6F-49E3-8B83-D545B561D4CB", false ); // Auto-Approve PTO Request:Supervisor
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "99B090AA-4D7E-46D8-B393-BF945EA1BA8B", "Supervisor Attribute", "SupervisorAttribute", "", 7, @"67afd5a3-28f3-404f-a3b8-88630061f294", "12C191AE-7483-4AA8-A871-F94AA9B8B632", false ); // Auto-Approve PTO Request:Supervisor Attribute
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "9C204CD0-1233-41C5-818A-C5DA439445AA", "HasViewRights", "HasViewRights", "", 8, @"", "7164E1D7-DEEF-43EC-BCFF-B7CC6AE7CFCF", false ); // Auto-Approve PTO Request:HasViewRights
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "9C204CD0-1233-41C5-818A-C5DA439445AA", "HasReviewRights", "HasReviewRights", "", 9, @"", "15E9CC36-DAA7-4986-9884-DCB791A2E226", false ); // Auto-Approve PTO Request:HasReviewRights
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", "Hours / Day", "HoursDay", "", 10, @"", "792DAFEE-A653-4803-B604-A086880E553C", false ); // Auto-Approve PTO Request:Hours / Day
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "C28C7BF3-A552-4D77-9408-DEDCF760CED0", "Reason", "Reason", "", 11, @"", "C52D901A-3051-4FE5-918E-CAE01FCE2042", false ); // Auto-Approve PTO Request:Reason
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "E4EAB7B2-0B76-429B-AFE4-AD86D7428C70", "Approver", "Approver", "", 12, @"", "7B99CE21-BDBB-4502-8FC4-734A944475C6", false ); // Auto-Approve PTO Request:Approver
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "C757A554-3009-4214-B05D-CEA2B2EA6B8F", "Remaining Hours", "RemainingHours", "", 13, @"0", "2041DF84-5E4B-476E-A4F5-636C9AAD42DE", false ); // Auto-Approve PTO Request:Remaining Hours
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "C757A554-3009-4214-B05D-CEA2B2EA6B8F", "Requested Hours", "RequestedHours", "", 14, @"", "5F4E96AA-C6A3-45B1-A3A5-893F067AB300", false ); // Auto-Approve PTO Request:Requested Hours
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Exclude Weekends", "ExcludeWeekends", "", 15, @"", "43059232-A145-4098-B5B2-A60D73AD763F", false ); // Auto-Approve PTO Request:Exclude Weekends
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", "Cancel Request", "CancelRequest", "", 16, @"", "711A41B3-C660-454A-8487-957941EDB90C", false ); // Auto-Approve PTO Request:Cancel Request
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Time Frame Validation Error", "TimeFrameValidationError", "", 17, @"", "3F0B2CED-0E8A-4B40-B7A9-093C13FF4D6D", false ); // Auto-Approve PTO Request:Time Frame Validation Error
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "7BD25DC9-F34A-478D-BEF9-0C787F5D39B8", "Human Resources", "HumanResources", "", 18, @"6f8aaba3-5bc8-468b-90dd-f0686f38e373", "D32AD4BC-3448-46A6-9438-87D63512DB60", false ); // Auto-Approve PTO Request:Human Resources
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Requested Hours YTD", "RequestedHoursYTD", "", 19, @"", "48CFC4A3-171D-4984-8664-C991FD3BEC25", false ); // Auto-Approve PTO Request:Requested Hours YTD
            RockMigrationHelper.UpdateWorkflowTypeAttribute( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Reason for Cancellation", "CancelReason", "", 20, @"", "7BBF2F20-FB40-4B59-8FD3-22946CF93D7F", false ); // Auto-Approve PTO Request:Reason for Cancellation
            RockMigrationHelper.AddAttributeQualifier( "E87CD011-20BC-4B2F-97B8-587A7C349FA0", "EnableSelfSelection", @"False", "A57E0EC6-8A06-4D31-82C4-3C35177FB2EC" ); // Auto-Approve PTO Request:Person:EnableSelfSelection
            RockMigrationHelper.AddAttributeQualifier( "24E36999-59A5-4332-A960-CF0162E2ACC8", "datePickerControlType", @"Date Picker", "31E64553-50C4-4FE2-8FF8-40A8F8C7B72F" ); // Auto-Approve PTO Request:Start Date:datePickerControlType
            RockMigrationHelper.AddAttributeQualifier( "24E36999-59A5-4332-A960-CF0162E2ACC8", "displayCurrentOption", @"False", "EAB44FB9-E381-4BE5-AC36-A96B57927259" ); // Auto-Approve PTO Request:Start Date:displayCurrentOption
            RockMigrationHelper.AddAttributeQualifier( "24E36999-59A5-4332-A960-CF0162E2ACC8", "displayDiff", @"False", "BD522418-6779-4CE0-BE92-09EDB99B754E" ); // Auto-Approve PTO Request:Start Date:displayDiff
            RockMigrationHelper.AddAttributeQualifier( "24E36999-59A5-4332-A960-CF0162E2ACC8", "format", @"", "00A9BA7C-31F8-43EA-9238-459D820EA4D1" ); // Auto-Approve PTO Request:Start Date:format
            RockMigrationHelper.AddAttributeQualifier( "24E36999-59A5-4332-A960-CF0162E2ACC8", "futureYearCount", @"", "CC0759F7-EDEB-4022-A66F-98063BA11C0E" ); // Auto-Approve PTO Request:Start Date:futureYearCount
            RockMigrationHelper.AddAttributeQualifier( "A1BC9859-1A57-4540-8892-2B4EE6194DE1", "datePickerControlType", @"Date Picker", "0A30C1AF-D6F2-4D84-9072-11BBF3AE6C1B" ); // Auto-Approve PTO Request:End Date:datePickerControlType
            RockMigrationHelper.AddAttributeQualifier( "A1BC9859-1A57-4540-8892-2B4EE6194DE1", "displayCurrentOption", @"False", "9FC5AE3A-04E1-4C3B-AC51-2895846F8949" ); // Auto-Approve PTO Request:End Date:displayCurrentOption
            RockMigrationHelper.AddAttributeQualifier( "A1BC9859-1A57-4540-8892-2B4EE6194DE1", "displayDiff", @"False", "E91D9CD8-BF97-46BB-B441-19444ACF1104" ); // Auto-Approve PTO Request:End Date:displayDiff
            RockMigrationHelper.AddAttributeQualifier( "A1BC9859-1A57-4540-8892-2B4EE6194DE1", "format", @"", "63909250-DADC-4CAE-8587-1241C150A22D" ); // Auto-Approve PTO Request:End Date:format
            RockMigrationHelper.AddAttributeQualifier( "A1BC9859-1A57-4540-8892-2B4EE6194DE1", "futureYearCount", @"", "6EBD1D83-78DD-4D08-920B-B3A79C989E92" ); // Auto-Approve PTO Request:End Date:futureYearCount
            RockMigrationHelper.AddAttributeQualifier( "162FF669-668E-41D6-BEF5-F17B9FC3C60C", "fieldtype", @"ddl", "CB573FBE-9E1C-44ED-AC4F-E59A691AB292" ); // Auto-Approve PTO Request:PTO Allocation:fieldtype
            RockMigrationHelper.AddAttributeQualifier( "162FF669-668E-41D6-BEF5-F17B9FC3C60C", "repeatColumns", @"", "5597F730-2BD6-4590-9CA8-196498D428A4" ); // Auto-Approve PTO Request:PTO Allocation:repeatColumns
            RockMigrationHelper.AddAttributeQualifier( "640D63E4-B2C9-4A18-8D6F-4CDEBD3662C2", "fieldtype", @"ddl", "30C655F1-4097-4DBC-9DDE-29E445BD3A1D" ); // Auto-Approve PTO Request:Approval State:fieldtype
            RockMigrationHelper.AddAttributeQualifier( "640D63E4-B2C9-4A18-8D6F-4CDEBD3662C2", "repeatColumns", @"", "2654DB28-D768-4344-BA1F-C323530138A0" ); // Auto-Approve PTO Request:Approval State:repeatColumns
            RockMigrationHelper.AddAttributeQualifier( "640D63E4-B2C9-4A18-8D6F-4CDEBD3662C2", "values", @"0^Pending, 1^Approved, 2^Denied,3^Cancelled", "1E38BC98-F492-4697-A557-BAC0BCEEF6E8" ); // Auto-Approve PTO Request:Approval State:values
            RockMigrationHelper.AddAttributeQualifier( "36CFE4C9-BA6F-49E3-8B83-D545B561D4CB", "EnableSelfSelection", @"False", "85F65737-070F-4F21-A189-30D71B9F2FB1" ); // Auto-Approve PTO Request:Supervisor:EnableSelfSelection
            RockMigrationHelper.AddAttributeQualifier( "12C191AE-7483-4AA8-A871-F94AA9B8B632", "allowmultiple", @"False", "CBA7D362-7223-4BAA-8070-70627264DF9A" ); // Auto-Approve PTO Request:Supervisor Attribute:allowmultiple
            RockMigrationHelper.AddAttributeQualifier( "12C191AE-7483-4AA8-A871-F94AA9B8B632", "entitytype", @"72657ed8-d16e-492e-ac12-144c5e7567e7", "E0A881F8-2BEF-490D-82A0-67C04D58EBEA" ); // Auto-Approve PTO Request:Supervisor Attribute:entitytype
            RockMigrationHelper.AddAttributeQualifier( "12C191AE-7483-4AA8-A871-F94AA9B8B632", "qualifierColumn", @"", "64D39A45-4728-4727-A710-B9236363760E" ); // Auto-Approve PTO Request:Supervisor Attribute:qualifierColumn
            RockMigrationHelper.AddAttributeQualifier( "12C191AE-7483-4AA8-A871-F94AA9B8B632", "qualifierValue", @"", "ECE5799E-DF9C-4EC6-872F-4F34074EDEA3" ); // Auto-Approve PTO Request:Supervisor Attribute:qualifierValue
            RockMigrationHelper.AddAttributeQualifier( "7164E1D7-DEEF-43EC-BCFF-B7CC6AE7CFCF", "ispassword", @"False", "4A712857-ACE4-4A31-B6EB-5CF8B2D780EA" ); // Auto-Approve PTO Request:HasViewRights:ispassword
            RockMigrationHelper.AddAttributeQualifier( "7164E1D7-DEEF-43EC-BCFF-B7CC6AE7CFCF", "maxcharacters", @"", "1A63AAF8-6257-42F8-BFCE-1B6E5AECEFCE" ); // Auto-Approve PTO Request:HasViewRights:maxcharacters
            RockMigrationHelper.AddAttributeQualifier( "7164E1D7-DEEF-43EC-BCFF-B7CC6AE7CFCF", "showcountdown", @"False", "3046E721-1188-4C57-9631-2A81F59729FB" ); // Auto-Approve PTO Request:HasViewRights:showcountdown
            RockMigrationHelper.AddAttributeQualifier( "15E9CC36-DAA7-4986-9884-DCB791A2E226", "ispassword", @"False", "E589A983-EF66-49E6-8797-8A71FEC970D3" ); // Auto-Approve PTO Request:HasReviewRights:ispassword
            RockMigrationHelper.AddAttributeQualifier( "15E9CC36-DAA7-4986-9884-DCB791A2E226", "maxcharacters", @"", "F751F847-3EA6-4976-A0EE-336BC65B5D6B" ); // Auto-Approve PTO Request:HasReviewRights:maxcharacters
            RockMigrationHelper.AddAttributeQualifier( "15E9CC36-DAA7-4986-9884-DCB791A2E226", "showcountdown", @"False", "8AD19EF6-D2F6-452C-B53B-C13142A7BA7B" ); // Auto-Approve PTO Request:HasReviewRights:showcountdown
            RockMigrationHelper.AddAttributeQualifier( "792DAFEE-A653-4803-B604-A086880E553C", "fieldtype", @"ddl", "A45F98A0-942E-443E-8DB8-792B7CAA2619" ); // Auto-Approve PTO Request:Hours / Day:fieldtype
            RockMigrationHelper.AddAttributeQualifier( "792DAFEE-A653-4803-B604-A086880E553C", "repeatColumns", @"", "43BB4A5B-78B1-4ECF-A094-E54025ED458C" ); // Auto-Approve PTO Request:Hours / Day:repeatColumns
            RockMigrationHelper.AddAttributeQualifier( "792DAFEE-A653-4803-B604-A086880E553C", "values", @"0.5,1.0,1.5,2.0,2.5,3.0,3.5,4.0,4.5,5.0,5.5,6.0,6.5,7.0,7.5,8.0", "A6C3D835-D1EC-4F84-9093-3DD1947142BE" ); // Auto-Approve PTO Request:Hours / Day:values
            RockMigrationHelper.AddAttributeQualifier( "C52D901A-3051-4FE5-918E-CAE01FCE2042", "allowhtml", @"False", "C535F38E-C4FB-4BA4-A07C-0AC3FC153A9A" ); // Auto-Approve PTO Request:Reason:allowhtml
            RockMigrationHelper.AddAttributeQualifier( "C52D901A-3051-4FE5-918E-CAE01FCE2042", "maxcharacters", @"", "66EB6460-ACD0-46FA-B5BA-75866493BD9B" ); // Auto-Approve PTO Request:Reason:maxcharacters
            RockMigrationHelper.AddAttributeQualifier( "C52D901A-3051-4FE5-918E-CAE01FCE2042", "numberofrows", @"", "1DC7A7EB-DE9C-4A5C-998D-8C5CC9D13785" ); // Auto-Approve PTO Request:Reason:numberofrows
            RockMigrationHelper.AddAttributeQualifier( "C52D901A-3051-4FE5-918E-CAE01FCE2042", "showcountdown", @"False", "A17FB4C7-C4BD-43F3-844C-F96D18610DF3" ); // Auto-Approve PTO Request:Reason:showcountdown
            RockMigrationHelper.AddAttributeQualifier( "7B99CE21-BDBB-4502-8FC4-734A944475C6", "EnableSelfSelection", @"False", "340BCF3F-7033-447B-ADCB-8B3F10DD9EF2" ); // Auto-Approve PTO Request:Approver:EnableSelfSelection
            RockMigrationHelper.AddAttributeQualifier( "43059232-A145-4098-B5B2-A60D73AD763F", "falsetext", @"No", "50CC45EA-743E-4546-AC36-CD481481823A" ); // Auto-Approve PTO Request:Exclude Weekends:falsetext
            RockMigrationHelper.AddAttributeQualifier( "43059232-A145-4098-B5B2-A60D73AD763F", "truetext", @"Yes", "79EC6C56-22CB-4F64-9837-78EB82A679BB" ); // Auto-Approve PTO Request:Exclude Weekends:truetext
            RockMigrationHelper.AddAttributeQualifier( "711A41B3-C660-454A-8487-957941EDB90C", "fieldtype", @"ddl", "962D4DD8-F869-4B7B-ACDD-A26D73796FC1" ); // Auto-Approve PTO Request:Cancel Request:fieldtype
            RockMigrationHelper.AddAttributeQualifier( "711A41B3-C660-454A-8487-957941EDB90C", "repeatColumns", @"", "D2E995FD-CE5C-4ED5-8E4B-DAB0E2425616" ); // Auto-Approve PTO Request:Cancel Request:repeatColumns
            RockMigrationHelper.AddAttributeQualifier( "711A41B3-C660-454A-8487-957941EDB90C", "values", @"Yes,No", "99541515-3579-4C00-9468-9B660641D4FE" ); // Auto-Approve PTO Request:Cancel Request:values
            RockMigrationHelper.AddAttributeQualifier( "3F0B2CED-0E8A-4B40-B7A9-093C13FF4D6D", "ispassword", @"False", "ECE26C8D-5B4C-424A-81E2-6D274A1B23BF" ); // Auto-Approve PTO Request:Time Frame Validation Error:ispassword
            RockMigrationHelper.AddAttributeQualifier( "3F0B2CED-0E8A-4B40-B7A9-093C13FF4D6D", "maxcharacters", @"", "033D3450-0A38-41DB-B5E3-5A2C025C7B6A" ); // Auto-Approve PTO Request:Time Frame Validation Error:maxcharacters
            RockMigrationHelper.AddAttributeQualifier( "3F0B2CED-0E8A-4B40-B7A9-093C13FF4D6D", "showcountdown", @"False", "D6935B1A-6271-4E74-B5EB-3AA7E3A02579" ); // Auto-Approve PTO Request:Time Frame Validation Error:showcountdown
            RockMigrationHelper.AddAttributeQualifier( "48CFC4A3-171D-4984-8664-C991FD3BEC25", "ispassword", @"False", "F9AEB70F-11C1-45BD-BCB4-2B96C7E8EE91" ); // Auto-Approve PTO Request:Requested Hours YTD:ispassword
            RockMigrationHelper.AddAttributeQualifier( "48CFC4A3-171D-4984-8664-C991FD3BEC25", "maxcharacters", @"", "399381DB-8360-43FE-A599-F053A14D9A1B" ); // Auto-Approve PTO Request:Requested Hours YTD:maxcharacters
            RockMigrationHelper.AddAttributeQualifier( "48CFC4A3-171D-4984-8664-C991FD3BEC25", "showcountdown", @"False", "81E618CB-D098-4380-AF39-8BFE2950A6B9" ); // Auto-Approve PTO Request:Requested Hours YTD:showcountdown
            RockMigrationHelper.AddAttributeQualifier( "7BBF2F20-FB40-4B59-8FD3-22946CF93D7F", "ispassword", @"False", "56F62C1F-64D3-4DA2-B717-9CD885CD152E" ); // Auto-Approve PTO Request:Reason for Cancellation:ispassword
            RockMigrationHelper.AddAttributeQualifier( "7BBF2F20-FB40-4B59-8FD3-22946CF93D7F", "maxcharacters", @"", "F7870FE1-A690-4C5D-A752-1D36DBCE23BA" ); // Auto-Approve PTO Request:Reason for Cancellation:maxcharacters
            RockMigrationHelper.AddAttributeQualifier( "7BBF2F20-FB40-4B59-8FD3-22946CF93D7F", "showcountdown", @"False", "B6282A46-464F-4E1A-A729-98B5B017AB64" ); // Auto-Approve PTO Request:Reason for Cancellation:showcountdown
            RockMigrationHelper.UpdateWorkflowActivityType( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", true, "Start", "", true, 0, "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F" ); // Auto-Approve PTO Request:Start
            RockMigrationHelper.UpdateWorkflowActivityType( "600A8E4C-AC45-496D-90D9-05CF39DF7A1A", true, "Review Request", "", false, 1, "1F605736-304A-4A03-90AB-9F9DCCECAB1C" ); // Auto-Approve PTO Request:Review Request
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set PTO Request from Entity", 0, "972F19B9-598B-474B-97A4-50E56E7B59D2", true, false, "", "", 1, "", "020DE802-DCB4-4908-95F3-52079B7FD0EB" ); // Auto-Approve PTO Request:Start:Set PTO Request from Entity
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Person", 1, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "1C104818-B563-4FA2-97CD-A3225811A13D", 64, "", "725562E7-637B-4AE0-94FC-1E5FE1BA5389" ); // Auto-Approve PTO Request:Start:Set Person
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Person To Current Person If Blank", 2, "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", true, false, "", "E87CD011-20BC-4B2F-97B8-587A7C349FA0", 32, "", "BED56195-2FDF-4923-8DC3-5409A14C789C" ); // Auto-Approve PTO Request:Start:Set Person To Current Person If Blank
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Supervisor", 3, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 1, "", "FCCD4C63-5F5E-467E-89F8-94BE1C89DAB8" ); // Auto-Approve PTO Request:Start:Set Supervisor
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set HasReviewRights", 4, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 1, "", "4CC0B5A9-BB17-4898-ADE6-70BC4CC7329C" ); // Auto-Approve PTO Request:Start:Set HasReviewRights
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set HasViewRights", 5, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 1, "", "2EAC17B2-A244-442E-95AE-7CA0C9DFB867" ); // Auto-Approve PTO Request:Start:Set HasViewRights
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Error message if not Authorized", 6, "497746B4-8B0C-4D7B-9AF7-B42CEDD6C37C", true, true, "", "7164E1D7-DEEF-43EC-BCFF-B7CC6AE7CFCF", 8, "false", "DE49D888-B6AD-4223-94C0-532A7144ECA9" ); // Auto-Approve PTO Request:Start:Error message if not Authorized
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Activate Complete Workflow if no existing PTO Request", 7, "EEDA4318-F014-4A46-9C76-4C052EF81AA1", true, true, "", "1C104818-B563-4FA2-97CD-A3225811A13D", 32, "", "4F3D796B-38A2-47EC-A648-995801A1584F" ); // Auto-Approve PTO Request:Start:Activate Complete Workflow if no existing PTO Request
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Pto Allocation", 8, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 64, "", "E3D7AAD9-D1BA-4A63-B307-45DF56353325" ); // Auto-Approve PTO Request:Start:Set Pto Allocation
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Start Date", 9, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 64, "", "9875FC5B-E8E4-435F-AA5C-727E1630D8D0" ); // Auto-Approve PTO Request:Start:Set Start Date
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Reason", 10, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 64, "", "643E6788-B2CB-456E-9480-83BF3D942338" ); // Auto-Approve PTO Request:Start:Set Reason
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Hours", 11, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 64, "", "C9B60F95-B258-4DE7-B5CC-9F7C5FB0314F" ); // Auto-Approve PTO Request:Start:Set Hours
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Set Approval State", 12, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 1, "", "6DCC55A9-83F0-4803-90E7-38ECE24BACB3" ); // Auto-Approve PTO Request:Start:Set Approval State
            RockMigrationHelper.UpdateWorkflowActionType( "C301EDE2-CBE7-4EAA-AB68-2412EB0F273F", "Activate Review Request Activity", 13, "38907A90-1634-4A93-8017-619326A4A582", true, true, "", "", 32, "Yes", "CD099E90-CEAC-4552-8E23-962CF284157F" ); // Auto-Approve PTO Request:Start:Activate Review Request Activity
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Form", 0, "C789E457-0783-44B3-9D8F-2EBAB5F11110", true, false, "", "", 1, "", "FC67BF44-2963-467A-96CD-8AEA52795455" ); // Auto-Approve PTO Request:Review Request:Form
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Persist Workflow", 1, "F1A39347-6FE0-43D4-89FB-544195088ECF", true, false, "", "", 1, "", "3A602D5A-029E-4BFD-9906-C66A80B36089" ); // Auto-Approve PTO Request:Review Request:Persist Workflow
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Set Approver", 2, "24B7D5E6-C30F-48F4-9D7E-AF45A342CF3A", true, false, "", "", 1, "", "C745EC52-51DD-4BA7-91C7-BBBB669D63DD" ); // Auto-Approve PTO Request:Review Request:Set Approver
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Update Request", 3, "546C6C01-5C8B-449E-A16A-580D92D0317B", true, false, "", "", 1, "", "784A3171-5559-44C6-A2C3-527B2DDC9286" ); // Auto-Approve PTO Request:Review Request:Update Request
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Redirect", 4, "F4FB0FB4-B2B3-4FC4-BEEA-E9B846A63293", true, false, "", "", 1, "", "AB687DFC-B531-47EF-B5B7-2CB3C4A8FFAF" ); // Auto-Approve PTO Request:Review Request:Redirect
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Set Requested Hours YTD", 5, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 1, "", "14A9F948-D901-4849-8063-0ED9190D5CC7" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours YTD
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Set Requested Hours", 6, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 1, "", "666A1662-52EF-4C64-B3E2-F01DE0749CBD" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Set Remaining Hours", 7, "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "", "", 1, "", "CD1F2017-175F-4CD4-B448-F3A9EFA1A134" ); // Auto-Approve PTO Request:Review Request:Set Remaining Hours
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Send Email to Person", 8, "66197B01-D1F0-4924-A315-47AD54E030DE", true, false, "", "", 1, "", "92399294-3AEA-427B-9581-1E40B5A5BEB1" ); // Auto-Approve PTO Request:Review Request:Send Email to Person
            RockMigrationHelper.UpdateWorkflowActionType( "1F605736-304A-4A03-90AB-9F9DCCECAB1C", "Complete Workflow", 9, "EEDA4318-F014-4A46-9C76-4C052EF81AA1", true, true, "", "", 1, "", "B24A1693-775B-4F58-BEFC-DF9F9AB1EAA5" ); // Auto-Approve PTO Request:Review Request:Complete Workflow
            RockMigrationHelper.AddActionTypeAttributeValue( "020DE802-DCB4-4908-95F3-52079B7FD0EB", "9392E3D7-A28B-4CD8-8B03-5E147B102EF1", @"False" ); // Auto-Approve PTO Request:Start:Set PTO Request from Entity:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "020DE802-DCB4-4908-95F3-52079B7FD0EB", "61E6E1BC-E657-4F00-B2E9-769AAA25B9F7", @"1c104818-b563-4fa2-97cd-a3225811a13d" ); // Auto-Approve PTO Request:Start:Set PTO Request from Entity:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "020DE802-DCB4-4908-95F3-52079B7FD0EB", "B524B00C-29CB-49E9-9896-8BB60F209783", @"True" ); // Auto-Approve PTO Request:Start:Set PTO Request from Entity:Entity Is Required
            RockMigrationHelper.AddActionTypeAttributeValue( "020DE802-DCB4-4908-95F3-52079B7FD0EB", "1246C53A-FD92-4E08-ABDE-9A6C37E70C7B", @"False" ); // Auto-Approve PTO Request:Start:Set PTO Request from Entity:Use Id instead of Guid
            RockMigrationHelper.AddActionTypeAttributeValue( "725562E7-637B-4AE0-94FC-1E5FE1BA5389", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoRequestGuid = Workflow | Attribute:'PTORequest','RawValue' %}
{% ptorequest where:'Guid == ""{{ptoRequestGuid}}""'%}
    {% for ptoRequest in ptorequestItems %}
        {{ptoRequest.PtoAllocation.PersonAlias.Guid}}
    {% endfor %}
{% endptorequest %}" ); // Auto-Approve PTO Request:Start:Set Person:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "725562E7-637B-4AE0-94FC-1E5FE1BA5389", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set Person:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "725562E7-637B-4AE0-94FC-1E5FE1BA5389", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"e87cd011-20bc-4b2f-97b8-587a7c349fa0" ); // Auto-Approve PTO Request:Start:Set Person:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "725562E7-637B-4AE0-94FC-1E5FE1BA5389", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set Person:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "BED56195-2FDF-4923-8DC3-5409A14C789C", "DE9CB292-4785-4EA3-976D-3826F91E9E98", @"False" ); // Auto-Approve PTO Request:Start:Set Person To Current Person If Blank:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "BED56195-2FDF-4923-8DC3-5409A14C789C", "BBED8A83-8BB2-4D35-BAFB-05F67DCAD112", @"e87cd011-20bc-4b2f-97b8-587a7c349fa0" ); // Auto-Approve PTO Request:Start:Set Person To Current Person If Blank:Person Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "FCCD4C63-5F5E-467E-89F8-94BE1C89DAB8", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign person = Workflow | Attribute:'Person','Object' %}
{% assign supervisorAttribute = Workflow | Attribute:'SupervisorAttribute','Object' %}
{% assign supervisor = person | Attribute:supervisorAttribute.Key, 'Object' %}
{{{supervisor.PrimaryAlias.Guid}}" ); // Auto-Approve PTO Request:Start:Set Supervisor:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "FCCD4C63-5F5E-467E-89F8-94BE1C89DAB8", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set Supervisor:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "FCCD4C63-5F5E-467E-89F8-94BE1C89DAB8", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"36cfe4c9-ba6f-49e3-8b83-d545b561d4cb" ); // Auto-Approve PTO Request:Start:Set Supervisor:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "4CC0B5A9-BB17-4898-ADE6-70BC4CC7329C", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign viewer = CurrentPerson %}
{% assign canReview = false %}
{% group where:'Guid == ""6F8AABA3-5BC8-468B-90DD-F0686F38E373""' %}
    {% capture groupIdString %}{{group.Id}}{% endcapture %}
    {% assign groupMembers = viewer | Group:groupIdString %}
    {% for groupMember in groupMembers %}
        {% assign canReview = true %}
    {% endfor %}
{% endgroup %}

{% group where:'Guid == """"628C51A8-4613-43ED-A18D-4A6FB999273E""""' %}
    {% capture groupIdString %}{{group.Id}}{% endcapture %}
    {% assign groupMembers = viewer | Group:groupIdString %}
    {% for groupMember in groupMembers %}
        {% assign canReview = true %}
    {% endfor %}
{% endgroup %}

{% if canReview == false %}
    {% assign person = Workflow | Attribute:'Person','Object' %}
    {% assign supervisorAttribute = Workflow | Attribute:'SupervisorAttribute','Object' %}
    {% assign supervisor = person | Attribute:supervisorAttribute.Key, 'Object' %}
    {% if supervisor.Id == viewer.Id %}
        {% assign canReview = true %}
    {% endif %}
{% endif %}

{{canReview}}" ); // Auto-Approve PTO Request:Start:Set HasReviewRights:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "4CC0B5A9-BB17-4898-ADE6-70BC4CC7329C", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set HasReviewRights:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "4CC0B5A9-BB17-4898-ADE6-70BC4CC7329C", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"15e9cc36-daa7-4986-9884-dcb791a2e226" ); // Auto-Approve PTO Request:Start:Set HasReviewRights:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "4CC0B5A9-BB17-4898-ADE6-70BC4CC7329C", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set HasReviewRights:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "2EAC17B2-A244-442E-95AE-7CA0C9DFB867", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign viewer = CurrentPerson %}
{% assign canView = false %}
{% group where:'Guid == ""6F8AABA3-5BC8-468B-90DD-F0686F38E373""' %}
    {% capture groupIdString %}{{group.Id}}{% endcapture %}
    {% assign groupMembers = viewer | Group:groupIdString %}
    {% for groupMember in groupMembers %}
        {% assign canView = true %}
    {% endfor %}
{% endgroup %}

{% group where:'Guid == """"628C51A8-4613-43ED-A18D-4A6FB999273E""""' %}
    {% capture groupIdString %}{{group.Id}}{% endcapture %}
    {% assign groupMembers = viewer | Group:groupIdString %}
    {% for groupMember in groupMembers %}
        {% assign canView = true %}
    {% endfor %}
{% endgroup %}

{% if canView == false %}
    {% assign person = Workflow | Attribute:'Person','Object' %}
    {% assign supervisorAttribute = Workflow | Attribute:'SupervisorAttribute','Object' %}
    {% assign supervisor = person | Attribute:supervisorAttribute.Key, 'Object' %}
    {% if supervisor.Id == viewer.Id %}
        {% assign canView = true %}
    {% endif %}
{% endif %}

{% if canView == false %}
    {% assign person = Workflow | Attribute:'Person','Object' %}
    {% if person.Id == viewer.Id %}
        {% assign canView = true %}
    {% endif %}
{% endif %}

{{canView}}" ); // Auto-Approve PTO Request:Start:Set HasViewRights:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "2EAC17B2-A244-442E-95AE-7CA0C9DFB867", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set HasViewRights:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "2EAC17B2-A244-442E-95AE-7CA0C9DFB867", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"7164e1d7-deef-43ec-bcff-b7cc6ae7cfcf" ); // Auto-Approve PTO Request:Start:Set HasViewRights:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "2EAC17B2-A244-442E-95AE-7CA0C9DFB867", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set HasViewRights:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "DE49D888-B6AD-4223-94C0-532A7144ECA9", "640FBD13-FEEB-4313-B6AC-6E5CF6E005DF", @"<div class='alert alert-warning'>
    You are not authorized to view this PTO Request.
</div>" ); // Auto-Approve PTO Request:Start:Error message if not Authorized:HTML
            RockMigrationHelper.AddActionTypeAttributeValue( "DE49D888-B6AD-4223-94C0-532A7144ECA9", "05673872-1E8D-42CD-9517-7CAFBC6976F9", @"False" ); // Auto-Approve PTO Request:Start:Error message if not Authorized:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "DE49D888-B6AD-4223-94C0-532A7144ECA9", "46ACD91A-9455-41D2-8849-C2305F364418", @"True" ); // Auto-Approve PTO Request:Start:Error message if not Authorized:Hide Status Message
            RockMigrationHelper.AddActionTypeAttributeValue( "4F3D796B-38A2-47EC-A648-995801A1584F", "0CA0DDEF-48EF-4ABC-9822-A05E225DE26C", @"False" ); // Auto-Approve PTO Request:Start:Activate Complete Workflow if no existing PTO Request:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "4F3D796B-38A2-47EC-A648-995801A1584F", "385A255B-9F48-4625-862B-26231DBAC53A", @"Completed" ); // Auto-Approve PTO Request:Start:Activate Complete Workflow if no existing PTO Request:Status|Status Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "E3D7AAD9-D1BA-4A63-B307-45DF56353325", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoRequestGuid = Workflow | Attribute:'PTORequest','RawValue' %}
{% ptorequest where:'Guid == ""{{ptoRequestGuid}}""'%}
    {% for ptoRequest in ptorequestItems %}
        {{ptoRequest.PtoAllocation.Guid}}
    {% endfor %}
{% endptorequest %}" ); // Auto-Approve PTO Request:Start:Set Pto Allocation:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "E3D7AAD9-D1BA-4A63-B307-45DF56353325", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set Pto Allocation:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "E3D7AAD9-D1BA-4A63-B307-45DF56353325", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"162ff669-668e-41d6-bef5-f17b9fc3c60c" ); // Auto-Approve PTO Request:Start:Set Pto Allocation:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "E3D7AAD9-D1BA-4A63-B307-45DF56353325", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set Pto Allocation:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "9875FC5B-E8E4-435F-AA5C-727E1630D8D0", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoRequestGuid = Workflow | Attribute:'PTORequest','RawValue' %}
{% ptorequest where:'Guid == ""{{ptoRequestGuid}}""'%}
    {% for ptoRequest in ptorequestItems %}
        {{ptoRequest.RequestDate}}
    {% endfor %}
{% endptorequest %}" ); // Auto-Approve PTO Request:Start:Set Start Date:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "9875FC5B-E8E4-435F-AA5C-727E1630D8D0", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set Start Date:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "9875FC5B-E8E4-435F-AA5C-727E1630D8D0", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"24e36999-59a5-4332-a960-cf0162e2acc8" ); // Auto-Approve PTO Request:Start:Set Start Date:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "9875FC5B-E8E4-435F-AA5C-727E1630D8D0", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set Start Date:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "643E6788-B2CB-456E-9480-83BF3D942338", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoRequestGuid = Workflow | Attribute:'PTORequest','RawValue' %}
{% ptorequest where:'Guid == ""{{ptoRequestGuid}}""'%}
    {% for ptoRequest in ptorequestItems %}
        {{ptoRequest.Reason}}
    {% endfor %}
{% endptorequest %}" ); // Auto-Approve PTO Request:Start:Set Reason:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "643E6788-B2CB-456E-9480-83BF3D942338", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set Reason:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "643E6788-B2CB-456E-9480-83BF3D942338", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"c52d901a-3051-4fe5-918e-cae01fce2042" ); // Auto-Approve PTO Request:Start:Set Reason:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "643E6788-B2CB-456E-9480-83BF3D942338", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set Reason:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "C9B60F95-B258-4DE7-B5CC-9F7C5FB0314F", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoRequestGuid = Workflow | Attribute:'PTORequest','RawValue' %}
{% ptorequest where:'Guid == ""{{ptoRequestGuid}}""'%}
    {% for ptoRequest in ptorequestItems %}
        {{ptoRequest.Hours| Format:'#0.0' }}
    {% endfor %}
{% endptorequest %}" ); // Auto-Approve PTO Request:Start:Set Hours:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "C9B60F95-B258-4DE7-B5CC-9F7C5FB0314F", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set Hours:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "C9B60F95-B258-4DE7-B5CC-9F7C5FB0314F", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"792dafee-a653-4803-b604-a086880e553c" ); // Auto-Approve PTO Request:Start:Set Hours:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "C9B60F95-B258-4DE7-B5CC-9F7C5FB0314F", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set Hours:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "6DCC55A9-83F0-4803-90E7-38ECE24BACB3", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoRequestGuid = Workflow | Attribute:'PTORequest','RawValue' %}
{% ptorequest where:'Guid == ""{{ptoRequestGuid}}""'%}
    {% for ptoRequest in ptorequestItems %}
        {% case ptoRequest.PtoRequestApprovalState %}
        {% when 'Pending' %}
            0
        {% when 'Approved' %}
            1
        {% else %}
            2
        {% endcase %}
    {% endfor %}
{% endptorequest %}" ); // Auto-Approve PTO Request:Start:Set Approval State:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "6DCC55A9-83F0-4803-90E7-38ECE24BACB3", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Start:Set Approval State:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "6DCC55A9-83F0-4803-90E7-38ECE24BACB3", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"640d63e4-b2c9-4a18-8d6f-4cdebd3662c2" ); // Auto-Approve PTO Request:Start:Set Approval State:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "6DCC55A9-83F0-4803-90E7-38ECE24BACB3", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Start:Set Approval State:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "CD099E90-CEAC-4552-8E23-962CF284157F", "E8ABD802-372C-47BE-82B1-96F50DB5169E", @"False" ); // Auto-Approve PTO Request:Start:Activate Review Request Activity:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "CD099E90-CEAC-4552-8E23-962CF284157F", "02D5A7A5-8781-46B4-B9FC-AF816829D240", @"1F605736-304A-4A03-90AB-9F9DCCECAB1C" ); // Auto-Approve PTO Request:Start:Activate Review Request Activity:Activity
            RockMigrationHelper.AddActionTypeAttributeValue( "FC67BF44-2963-467A-96CD-8AEA52795455", "D7EAA859-F500-4521-9523-488B12EAA7D2", @"False" ); // Auto-Approve PTO Request:Review Request:Form:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "FC67BF44-2963-467A-96CD-8AEA52795455", "44A0B977-4730-4519-8FF6-B0A01A95B212", @"640d63e4-b2c9-4a18-8d6f-4cdebd3662c2" ); // Auto-Approve PTO Request:Review Request:Form:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "FC67BF44-2963-467A-96CD-8AEA52795455", "E5272B11-A2B8-49DC-860D-8D574E2BC15C", @"1" ); // Auto-Approve PTO Request:Review Request:Form:Text Value|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "3A602D5A-029E-4BFD-9906-C66A80B36089", "50B01639-4938-40D2-A791-AA0EB4F86847", @"False" ); // Auto-Approve PTO Request:Review Request:Persist Workflow:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "3A602D5A-029E-4BFD-9906-C66A80B36089", "82744A46-0110-4728-BD3D-66C85C5FCB2F", @"True" ); // Auto-Approve PTO Request:Review Request:Persist Workflow:Persist Immediately
            RockMigrationHelper.AddActionTypeAttributeValue( "C745EC52-51DD-4BA7-91C7-BBBB669D63DD", "DE9CB292-4785-4EA3-976D-3826F91E9E98", @"False" ); // Auto-Approve PTO Request:Review Request:Set Approver:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "C745EC52-51DD-4BA7-91C7-BBBB669D63DD", "BBED8A83-8BB2-4D35-BAFB-05F67DCAD112", @"7b99ce21-bdbb-4502-8fc4-734a944475c6" ); // Auto-Approve PTO Request:Review Request:Set Approver:Person Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "DD610F3E-2E83-41AE-B63B-9B163B87F82E", @"False" ); // Auto-Approve PTO Request:Review Request:Update Request:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "C957F777-F0FE-4D05-BB22-10D7C7A5C437", @"1c104818-b563-4fa2-97cd-a3225811a13d" ); // Auto-Approve PTO Request:Review Request:Update Request:Existing Pto Request
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "EC01344E-61BF-4E22-88E3-36051BCAABE7", @"162ff669-668e-41d6-bef5-f17b9fc3c60c" ); // Auto-Approve PTO Request:Review Request:Update Request:Allocation|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "3C5F03BD-2CDD-41D7-9ED1-5AAC62AF733D", @"24e36999-59a5-4332-a960-cf0162e2acc8" ); // Auto-Approve PTO Request:Review Request:Update Request:Start Date|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "8304DE14-DA5C-41FD-BA30-026D91A492C7", @"a1bc9859-1a57-4540-8892-2b4ee6194de1" ); // Auto-Approve PTO Request:Review Request:Update Request:End Date|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "858BFCA2-E793-446E-B146-87D5FC6783A0", @"792dafee-a653-4803-b604-a086880e553c" ); // Auto-Approve PTO Request:Review Request:Update Request:Hours|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "C6A51AEB-18CB-4591-BDF8-D4017CF38DCF", @"c52d901a-3051-4fe5-918e-cae01fce2042" ); // Auto-Approve PTO Request:Review Request:Update Request:Reason|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "A781A20B-4F21-47CA-9BCF-1654565DB5F6", @"7b99ce21-bdbb-4502-8fc4-734a944475c6" ); // Auto-Approve PTO Request:Review Request:Update Request:Approver|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "080025FD-9E80-4158-8D7F-FBF3ED12A2E1", @"640d63e4-b2c9-4a18-8d6f-4cdebd3662c2" ); // Auto-Approve PTO Request:Review Request:Update Request:Approval State|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "784A3171-5559-44C6-A2C3-527B2DDC9286", "552610AA-C128-4A6F-AAB6-20ACC0C5F060", @"False" ); // Auto-Approve PTO Request:Review Request:Update Request:Exclude Weekends|Attribute Value
            RockMigrationHelper.AddActionTypeAttributeValue( "AB687DFC-B531-47EF-B5B7-2CB3C4A8FFAF", "1DAA899B-634B-4DD5-A30A-69BAC235B383", @"False" ); // Auto-Approve PTO Request:Review Request:Redirect:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "AB687DFC-B531-47EF-B5B7-2CB3C4A8FFAF", "051BD491-817F-45DD-BBAC-875BA79E3644", @"/Person/{{ Workflow | Attribute:'Person','Id' }}/HR" ); // Auto-Approve PTO Request:Review Request:Redirect:Url|Url Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "AB687DFC-B531-47EF-B5B7-2CB3C4A8FFAF", "581736CE-76CF-46CE-A401-60A9E9EBCC1A", @"0" ); // Auto-Approve PTO Request:Review Request:Redirect:Processing Options
            RockMigrationHelper.AddActionTypeAttributeValue( "14A9F948-D901-4849-8063-0ED9190D5CC7", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign ptoAllocationGuid = Workflow | Attribute:'PTOAllocation','RawValue' %}
{% if ptoAllocationGuid == empty %}
    {% assign ptoRequest = Workflow | Attribute:'PTORequest','Object' %}
    {% assign ptoAllocationGuid = ptoRequest.PtoAllocation.Guid %}
{% endif %}

{% ptoallocation where:'Guid == ""{{ ptoAllocationGuid }}""' %}
    {% for ptoAllocation in ptoallocationItems %}
        {% assign totalHours = ptoAllocation.Hours %}
        {% assign takenHours = 0.0 %}
        {% for request in ptoAllocation.PtoRequests %}
            {% if request.PtoRequestApprovalState != 0 %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
    {% endfor %}
{% endptoallocation %}
{{ takenHours }}" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours YTD:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "14A9F948-D901-4849-8063-0ED9190D5CC7", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours YTD:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "14A9F948-D901-4849-8063-0ED9190D5CC7", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"48cfc4a3-171d-4984-8664-c991fd3bec25" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours YTD:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "14A9F948-D901-4849-8063-0ED9190D5CC7", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours YTD:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "666A1662-52EF-4C64-B3E2-F01DE0749CBD", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign startDate = Workflow | Attribute:'StartDate' %}
{% assign endDate = Workflow | Attribute:'EndDate' %}
{% assign hoursPerDay = Workflow | Attribute:'HoursDay' %}
{% assign excludeWeekends = Workflow | Attribute:'ExcludeWeekends' %}
{% assign requestDate = startDate %}
{% assign totalRequestHours = 0 %}

{% if endDate != empty %}
    {% assign dayCount = startDate | DateDiff:endDate, 'd' %}
    {% for i in (0...dayCount) %}
        {% assign requestDate = startDate | DateAdd:i,'d' %}
        {% assign dayCounts = true %}
        
        {% if excludeWeekends == 'True' %}
            {% assign dayOfWeek = requestDate | Date:'dddd' %}
            {% if dayOfWeek == 'Sunday' or dayOfWeek == 'Saturday' %}
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

{{totalRequestHours }}" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "666A1662-52EF-4C64-B3E2-F01DE0749CBD", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "666A1662-52EF-4C64-B3E2-F01DE0749CBD", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"5f4e96aa-c6a3-45b1-a3a5-893f067ab300" ); // Auto-Approve PTO Request:Review Request:Set Requested Hours:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "CD1F2017-175F-4CD4-B448-F3A9EFA1A134", "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4", @"{% assign requestHours = Workflow | Attribute:'RequestedHours' %}

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
            {% if request.PtoRequestApprovalState != 0 %}
                {% assign takenHours = takenHours | Plus:request.Hours %}
            {% endif %}
        {% endfor %}
        {% assign remainingHours = totalHours | Minus:takenHours %}
    {% endfor %}
{% endptoallocation %}
{% assign remainingHours = remainingHours | Minus:requestHours %}

{{ remainingHours }}" ); // Auto-Approve PTO Request:Review Request:Set Remaining Hours:Lava
            RockMigrationHelper.AddActionTypeAttributeValue( "CD1F2017-175F-4CD4-B448-F3A9EFA1A134", "F1924BDC-9B79-4018-9D4A-C3516C87A514", @"False" ); // Auto-Approve PTO Request:Review Request:Set Remaining Hours:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "CD1F2017-175F-4CD4-B448-F3A9EFA1A134", "431273C6-342D-4030-ADC7-7CDEDC7F8B27", @"2041df84-5e4b-476e-a4f5-636c9aad42de" ); // Auto-Approve PTO Request:Review Request:Set Remaining Hours:Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "CD1F2017-175F-4CD4-B448-F3A9EFA1A134", "F3E380BF-AAC8-4015-9ADC-0DF56B5462F5", @"RockEntity" ); // Auto-Approve PTO Request:Review Request:Set Remaining Hours:Enabled Lava Commands
            RockMigrationHelper.AddActionTypeAttributeValue( "92399294-3AEA-427B-9581-1E40B5A5BEB1", "36197160-7D3D-490D-AB42-7E29105AFE91", @"False" ); // Auto-Approve PTO Request:Review Request:Send Email to Person:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "92399294-3AEA-427B-9581-1E40B5A5BEB1", "0C4C13B8-7076-4872-925A-F950886B5E16", @"e87cd011-20bc-4b2f-97b8-587a7c349fa0" ); // Auto-Approve PTO Request:Review Request:Send Email to Person:Send To Email Addresses|To Attribute
            RockMigrationHelper.AddActionTypeAttributeValue( "92399294-3AEA-427B-9581-1E40B5A5BEB1", "5D9B13B6-CD96-4C7C-86FA-4512B9D28386", @"{{Workflow | Attribute:'ApprovalState'}}: {{ Workflow | Attribute:'Type' }} Time Off Request for {{Workflow | Attribute:'Person'}} ({{Workflow | Attribute:'StartDate'}} - {{Workflow | Attribute:'EndDate'}})" ); // Auto-Approve PTO Request:Review Request:Send Email to Person:Subject
            RockMigrationHelper.AddActionTypeAttributeValue( "92399294-3AEA-427B-9581-1E40B5A5BEB1", "4D245B9E-6B03-46E7-8482-A51FBA190E4D", @"{% capture reviewLink %}{{ 'Global' | Attribute:'InternalApplicationRoot' }}Person/{{ Workflow | Attribute:'Person','Id' }}/HR{% endcapture %}
{% capture reviewText %}Review Request{% endcapture %}
{% capture endDate %}{{ Workflow | Attribute:'EndDate'}}{% endcapture %}

{{ 'Global' | Attribute:'EmailHeader' }}

    Your {{ Workflow | Attribute:'Type' }} Time Off Request has been {{Workflow | Attribute:'ApprovalState'}} by {{ Workflow | Attribute:'Approver' }}. <br /> <br />

    {% if endDate != empty %}
        <strong>Date(s):</strong> {{ Workflow | Attribute:'StartDate' | Date:'dddd, MMM d, yyyy' }} - {{ Workflow | Attribute:'EndDate' | Date:'dddd, MMM d, yyyy'}} <br /> <br />
    {% else %}
        <strong>Date:</strong> {{ Workflow | Attribute:'StartDate' | Date:'dddd, MMM d, yyyy' }} <br /> <br />
    {% endif %}
        
    
    <strong>Request Hours:</strong> {{ Workflow | Attribute:'RequestedHours' }}<br />
    <strong>Reason:</strong> {{ Workflow | Attribute:'Reason' }}<br /><br />
    
    <strong>Total Requested Hours YTD:</strong> {{ Workflow | Attribute:'RequestedHoursYTD' }}<br />
    <strong>Remaining Hours:</strong> {{ Workflow | Attribute:'RemainingHours' }}<br />
            

Thank you!<br/>
<br/>
<table align=""left"" style=""width: 29%; min-width: 190px; margin-bottom: 12px;"" cellpadding=""0"" cellspacing=""0"">
 <tr>
   <td>

		<div><!--[if mso]>
		  <v:roundrect xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:w=""urn:schemas-microsoft-com:office:word"" href=""{{ reviewLink }}"" style=""height:38px;v-text-anchor:middle;width:175px;"" arcsize=""11%"" strokecolor=""#e76812"" fillcolor=""#ee7624"">
			<w:anchorlock/>
			<center style=""color:#ffffff;font-family:sans-serif;font-size:13px;font-weight:normal;"">{{reviewText}}</center>
		  </v:roundrect>
		<![endif]--><a href=""{{ reviewLink }}""
		style=""background-color:#ee7624;border:1px solid #e76812;border-radius:4px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:normal;line-height:38px;text-align:center;text-decoration:none;width:175px;-webkit-text-size-adjust:none;mso-hide:all;"">{{reviewText}}</a></div>

	</td>
 </tr>
</table>
{{ 'Global' | Attribute:'EmailFooter' }}
" ); // Auto-Approve PTO Request:Review Request:Send Email to Person:Body
            RockMigrationHelper.AddActionTypeAttributeValue( "92399294-3AEA-427B-9581-1E40B5A5BEB1", "1BDC7ACA-9A0B-4C8A-909E-8B4143D9C2A3", @"True" ); // Auto-Approve PTO Request:Review Request:Send Email to Person:Save Communication History
            RockMigrationHelper.AddActionTypeAttributeValue( "B24A1693-775B-4F58-BEFC-DF9F9AB1EAA5", "0CA0DDEF-48EF-4ABC-9822-A05E225DE26C", @"False" ); // Auto-Approve PTO Request:Review Request:Complete Workflow:Active
            RockMigrationHelper.AddActionTypeAttributeValue( "B24A1693-775B-4F58-BEFC-DF9F9AB1EAA5", "385A255B-9F48-4625-862B-26231DBAC53A", @"Completed" ); // Auto-Approve PTO Request:Review Request:Complete Workflow:Status|Status Attribute

            #endregion
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            Sql( @"
                ALTER TABLE [_com_bemaservices_HrManagement_PtoTier] DROP COLUMN ApprovalGroupId
            " );
        }
    }
}
