// <copyright>
// Copyright by BEMA Software Services
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
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using com.bemaservices.HrManagement.SystemGuid;
using Rock;
using Rock.Lava.Blocks;
using Rock.Plugin;
using Rock.Web.Cache;

namespace com.bemaservices.HrManagement.Migrations
{
    /// <summary>
    /// Class PtoCancellationFix.
    /// Implements the <see cref="Migration" />
    /// </summary>
    /// <seealso cref="Migration" />
    [MigrationNumber( 12, "1.17.6" )]
    public class ObsidianConversion : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            AddNewBlockTypes();
            SwitchBlockTypes();
            UpdatePersonDetailLayout();
        }

        private void UpdatePersonDetailLayout()
        {
            RockMigrationHelper.UpdatePageLayout( "34A9F1E4-8249-4F39-B861-F28210E8C70A", "6AD84AFC-B3A1-4E30-B53B-C6E57B513839" );
        }

        private void SwitchBlockTypes()
        {
            Sql( @"
-- =============================================
-- HR Management Webforms to Obsidian Migration Script
-- This script updates existing HR Management blocks from webforms to Obsidian
-- =============================================

BEGIN TRANSACTION;

-- =============================================
-- BLOCK TYPE MAPPING
-- =============================================
DECLARE @BlockTypeMapping TABLE (
    OldBlockTypeGuid UNIQUEIDENTIFIER,
    OldBlockTypeName NVARCHAR(200),
    NewBlockTypeGuid UNIQUEIDENTIFIER,
    NewBlockTypeName NVARCHAR(200),
    OldBlockTypeId INT,
    NewBlockTypeId INT
);

-- Insert block type mappings
INSERT INTO @BlockTypeMapping (OldBlockTypeGuid, OldBlockTypeName, NewBlockTypeGuid, NewBlockTypeName)
VALUES 
    ('601c2443-c67d-4d4f-9ca2-d42af186d5eb', 'HR Employee List (Old)', '8c5d6e7f-9a1b-2c3d-4e5f-6a7b8c9d0e1f', 'HR Employee List'),
    ('9a69f6d7-0c72-40a0-9d34-841c6ecc0e19', 'HR Employee List (PowerTools)', '8c5d6e7f-9a1b-2c3d-4e5f-6a7b8c9d0e1f', 'HR Employee List'),
    ('d59a0a08-0a20-406e-95ce-03882446f70c', 'Pto Allocation Detail (Old)', 'a81082a2-8f37-4eec-ae6e-2ed09f93a350', 'Pto Allocation Detail'),
    ('88eb8f0f-1c74-4b10-a43a-d9958272b8a2', 'PTO Allocation Detail (PowerTools)', 'a81082a2-8f37-4eec-ae6e-2ed09f93a350', 'Pto Allocation Detail'),
    ('9a7fdff1-21de-4bf5-9598-690224e98ffc', 'Pto Allocation List (Old)', '0ee8b9f0-9cca-46ce-b4a2-dab79e51df9c', 'Pto Allocation List'),
    ('5cf3ec7b-650a-4fd0-b7ba-2963c962409f', 'PTO Allocation List (PowerTools)', '0ee8b9f0-9cca-46ce-b4a2-dab79e51df9c', 'Pto Allocation List'),
    ('0801c3f9-019e-4b8f-b5d5-4d38d48d11b2', 'Pto Bracket Detail (Old)', '3d8f1189-a48f-4913-8218-68b78a00f6c7', 'Pto Bracket Detail'),
    ('2dfbd8a3-0154-426f-8171-d2e0e766545e', 'PTO Bracket Detail (PowerTools)', '3d8f1189-a48f-4913-8218-68b78a00f6c7', 'Pto Bracket Detail'),
    ('786ae09b-d8e7-4ec2-8488-1b104cb2c0a0', 'Pto Bracket List (Old)', '09b6ad11-e215-4bd0-a955-b19b38c05dff', 'Pto Bracket List'),
    ('58b30320-3a9b-4297-a4a4-62122d84e8f1', 'PTO Bracket List (PowerTools)', '09b6ad11-e215-4bd0-a955-b19b38c05dff', 'Pto Bracket List'),
    ('7dbc964f-c656-4015-8e5b-e99c3a65ae26', 'Pto Request List (Old)', '2f76725e-ccf0-4e57-8f2f-54d4e63e6c99', 'Pto Request List'),
    ('1be582c9-2059-476b-b896-30997e45de89', 'PTO Request List (PowerTools)', '2f76725e-ccf0-4e57-8f2f-54d4e63e6c99', 'Pto Request List'),
    ('4736490b-bf41-42ab-aa4b-88defcfb6cae', 'Pto Teir Detail (Old)', '6355f631-5116-419b-a972-d822073e94ad', 'Pto Tier Detail'),
    ('f34e373b-d090-4d7a-9a4f-8e6db2d7ede9', 'PTO Tier Detail (PowerTools)', '6355f631-5116-419b-a972-d822073e94ad', 'Pto Tier Detail'),
    ('2bc393d8-123a-42e3-8eb5-766ededefc2a', 'Pto Teir List (Old)', '37f2dda9-cb72-4fac-950f-b260a6796230', 'Pto Tier List'),
    ('6c0430bd-2ffd-40c5-838c-60f625c7fe08', 'PTO Tier List (PowerTools)', '37f2dda9-cb72-4fac-950f-b260a6796230', 'Pto Tier List'),
    ('39d2b37d-801b-408a-b726-da962eb5f85f', 'PTO Type List (Old)', '79d68e47-6d76-47bc-b528-fcbf570bb801', 'Pto Type List'),
    ('c306cef4-9fbb-4c4b-b8a9-bb1be244f224', 'PTO Type List (PowerTools)', '79d68e47-6d76-47bc-b528-fcbf570bb801', 'Pto Type List');

-- Update with actual IDs
UPDATE @BlockTypeMapping
SET OldBlockTypeId = bt.[Id]
FROM [BlockType] bt
WHERE bt.[Guid] = [@BlockTypeMapping].OldBlockTypeGuid;

UPDATE @BlockTypeMapping
SET NewBlockTypeId = bt.[Id]
FROM [BlockType] bt
WHERE bt.[Guid] = [@BlockTypeMapping].NewBlockTypeGuid;

-- =============================================
-- ATTRIBUTE MAPPING (Auto-generated by matching keys)
-- =============================================
DECLARE @AttributeMapping TABLE (
    BlockTypeName NVARCHAR(200),
    AttributeKey NVARCHAR(200),
    OldAttributeId INT,
    NewAttributeId INT,
    OldAttributeGuid UNIQUEIDENTIFIER,
    NewAttributeGuid UNIQUEIDENTIFIER
);

-- Automatically populate attribute mappings by matching keys
INSERT INTO @AttributeMapping (BlockTypeName, AttributeKey, OldAttributeId, NewAttributeId, OldAttributeGuid, NewAttributeGuid)
SELECT 
    btm.NewBlockTypeName,
    a_old.[Key],
    a_old.[Id] AS OldAttributeId,
    a_new.[Id] AS NewAttributeId,
    a_old.[Guid] AS OldAttributeGuid,
    a_new.[Guid] AS NewAttributeGuid
FROM @BlockTypeMapping btm
INNER JOIN [Attribute] a_old 
    ON a_old.[EntityTypeQualifierColumn] = 'BlockTypeId'
    AND a_old.[EntityTypeQualifierValue] = CAST(btm.OldBlockTypeId AS NVARCHAR(50))
INNER JOIN [Attribute] a_new 
    ON a_new.[EntityTypeQualifierColumn] = 'BlockTypeId'
    AND a_new.[EntityTypeQualifierValue] = CAST(btm.NewBlockTypeId AS NVARCHAR(50))
    AND a_new.[Key] = a_old.[Key]  -- Match attributes by key
WHERE btm.OldBlockTypeId IS NOT NULL 
  AND btm.NewBlockTypeId IS NOT NULL;

-- =============================================
-- STEP 1: UPDATE BLOCKS TO NEW BLOCK TYPES
-- =============================================
UPDATE b
SET b.[BlockTypeId] = btm.NewBlockTypeId,
    b.[ModifiedDateTime] = GETDATE()
FROM [Block] b
INNER JOIN @BlockTypeMapping btm ON b.[BlockTypeId] = btm.OldBlockTypeId
WHERE btm.NewBlockTypeId IS NOT NULL;

-- =============================================
-- STEP 2: UPDATE ATTRIBUTE IDS ON ATTRIBUTE VALUES
-- =============================================
UPDATE av
SET av.[AttributeId] = am.NewAttributeId,
    av.[ModifiedDateTime] = GETDATE()
FROM [AttributeValue] av
INNER JOIN [Block] b ON b.[Id] = av.[EntityId]
INNER JOIN [BlockType] bt ON bt.[Id] = b.[BlockTypeId]
INNER JOIN @AttributeMapping am ON am.OldAttributeId = av.[AttributeId]
INNER JOIN @BlockTypeMapping btm ON btm.NewBlockTypeId = bt.[Id]
WHERE am.NewAttributeId IS NOT NULL;

-- =============================================
-- STEP 3: DELETE OLD ATTRIBUTES
-- =============================================
DELETE a
FROM [Attribute] a
INNER JOIN @BlockTypeMapping btm 
    ON a.[EntityTypeQualifierColumn] = 'BlockTypeId'
    AND a.[EntityTypeQualifierValue] = CAST(btm.OldBlockTypeId AS NVARCHAR(50))
WHERE btm.OldBlockTypeId IS NOT NULL;

-- =============================================
-- STEP 4: DELETE OLD BLOCK TYPES
-- =============================================
DELETE bt
FROM [BlockType] bt
INNER JOIN @BlockTypeMapping btm ON bt.[Guid] = btm.OldBlockTypeGuid
WHERE btm.OldBlockTypeId IS NOT NULL;

COMMIT TRANSACTION;
" );

        }

        private void AddNewBlockTypes()
        {

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoAllocationDetail
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoAllocationDetail", "Pto Allocation Detail", "com.bemaservices.HrManagement.Blocks.PtoAllocationDetail, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "80ED296C-00DE-4981-9A4F-3E29383C5125" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoAllocationList
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoAllocationList", "Pto Allocation List", "com.bemaservices.HrManagement.Blocks.PtoAllocationList, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "D2BFE182-0D87-4D3A-8489-106556C00167" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoBracketDetail
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoBracketDetail", "Pto Bracket Detail", "com.bemaservices.HrManagement.Blocks.PtoBracketDetail, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "60F8C7D1-4A84-4330-97A9-EBC4B433EC80" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoBracketList
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoBracketList", "Pto Bracket List", "com.bemaservices.HrManagement.Blocks.PtoBracketList, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "214745DC-9F60-47EF-955C-6B31359033DC" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoRequestList
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoRequestList", "Pto Request List", "com.bemaservices.HrManagement.Blocks.PtoRequestList, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "EF3A3FC7-5586-4EB1-83D1-87EF4F188ABB" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoTierDetail
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoTierDetail", "Pto Tier Detail", "com.bemaservices.HrManagement.Blocks.PtoTierDetail, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "546404D5-9367-4069-B8FE-3FCD625FA526" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoTierList
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoTierList", "Pto Tier List", "com.bemaservices.HrManagement.Blocks.PtoTierList, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "01DAC091-F36F-48B4-B02B-E65508A469AE" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoTypeList
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.PtoTypeList", "Pto Type List", "com.bemaservices.HrManagement.Blocks.PtoTypeList, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "FFA7DA36-FDEA-49D7-A40F-227E9B4B2B60" );

            // Add/Update Obsidian Block Entity Type
            //   EntityType:com.bemaservices.HrManagement.Blocks.HrEmployeeList
            RockMigrationHelper.UpdateEntityType( "com.bemaservices.HrManagement.Blocks.HrEmployeeList", "Hr Employee List", "com.bemaservices.HrManagement.Blocks.HrEmployeeList, com.bemaservices.HrManagement, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null", false, false, "7B3A4D65-8F2E-4C9A-B1D3-E5F6A7B8C9D0" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Allocation Detail
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoAllocationDetail
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Allocation Detail", "Displays the details of a particular pto allocation.", "com.bemaservices.HrManagement.Blocks.PtoAllocationDetail", "BEMA Software Services > Hr Management", "A81082A2-8F37-4EEC-AE6E-2ED09F93A350" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Allocation List
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoAllocationList
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Allocation List", "Displays a list of pto allocations.", "com.bemaservices.HrManagement.Blocks.PtoAllocationList", "BEMA Software Services > Hr Management", "0EE8B9F0-9CCA-46CE-B4A2-DAB79E51DF9C" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Bracket Detail
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoBracketDetail
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Bracket Detail", "Displays the details of a particular pto bracket.", "com.bemaservices.HrManagement.Blocks.PtoBracketDetail", "BEMA Software Services > Hr Management", "3D8F1189-A48F-4913-8218-68B78A00F6C7" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Bracket List
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoBracketList
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Bracket List", "Displays a list of pto brackets.", "com.bemaservices.HrManagement.Blocks.PtoBracketList", "BEMA Software Services > Hr Management", "09B6AD11-E215-4BD0-A955-B19B38C05DFF" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Request List
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoRequestList
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Request List", "Displays a list of pto requests.", "com.bemaservices.HrManagement.Blocks.PtoRequestList", "BEMA Software Services > Hr Management", "2F76725E-CCF0-4E57-8F2F-54D4E63E6C99" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Tier Detail
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoTierDetail
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Tier Detail", "Displays the details of a particular pto tier.", "com.bemaservices.HrManagement.Blocks.PtoTierDetail", "BEMA Software Services > Hr Management", "6355F631-5116-419B-A972-D822073E94AD" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Tier List
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoTierList
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Tier List", "Displays a list of pto tiers.", "com.bemaservices.HrManagement.Blocks.PtoTierList", "BEMA Software Services > Hr Management", "37F2DDA9-CB72-4FAC-950F-B260A6796230" );

            // Add/Update Obsidian Block Type
            //   Name:Pto Type List
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.PtoTypeList
            RockMigrationHelper.AddOrUpdateEntityBlockType( "Pto Type List", "Displays a list of pto types.", "com.bemaservices.HrManagement.Blocks.PtoTypeList", "BEMA Software Services > Hr Management", "79D68E47-6D76-47BC-B528-FCBF570BB801" );

            // Add/Update Obsidian Block Type
            //   Name:HR Employee List
            //   Category:BEMA Software Services > Hr Management
            //   EntityType:com.bemaservices.HrManagement.Blocks.HrEmployeeList
            RockMigrationHelper.AddOrUpdateEntityBlockType( "HR Employee List", "Lists all the employees along with their PTO information.", "com.bemaservices.HrManagement.Blocks.HrEmployeeList", "BEMA Software Services > Hr Management", "8C5D6E7F-9A1B-2C3D-4E5F-6A7B8C9D0E1F" );

            // Attribute for BlockType
            //   BlockType: Pto Allocation List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Detail Page
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "0EE8B9F0-9CCA-46CE-B4A2-DAB79E51DF9C", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPage", "Detail Page", @"The page that will show the pto allocation details.", 0, @"", "CA69CCBC-3674-4312-932E-39DE1C5B4429" );

            // Attribute for BlockType
            //   BlockType: Pto Allocation List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.CustomActionsConfigs
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "0EE8B9F0-9CCA-46CE-B4A2-DAB79E51DF9C", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "391C948F-8A30-4A08-9209-F3E7511F003A" );

            // Attribute for BlockType
            //   BlockType: Pto Allocation List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.EnableDefaultWorkflowLauncher
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "0EE8B9F0-9CCA-46CE-B4A2-DAB79E51DF9C", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "A349C58D-0C8B-4ADE-8560-155DEF896BD8" );

            // Attribute for BlockType
            //   BlockType: Pto Bracket List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Detail Page
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "09B6AD11-E215-4BD0-A955-B19B38C05DFF", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPage", "Detail Page", @"The page that will show the pto bracket details.", 0, @"", "CD3E0516-904C-42E8-B2D0-98BFBF6370D0" );

            // Attribute for BlockType
            //   BlockType: Pto Bracket List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.CustomActionsConfigs
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "09B6AD11-E215-4BD0-A955-B19B38C05DFF", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "3A2A71A7-00CF-4507-B32D-5DEAF59E36F9" );

            // Attribute for BlockType
            //   BlockType: Pto Bracket List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.EnableDefaultWorkflowLauncher
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "09B6AD11-E215-4BD0-A955-B19B38C05DFF", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "BFF2B62B-AE0B-4E1E-82DB-B0C764440A12" );

            // Attribute for BlockType
            //   BlockType: Pto Request List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.CustomActionsConfigs
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "2F76725E-CCF0-4E57-8F2F-54D4E63E6C99", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "2B0F76BD-6DF0-4789-8258-4244B9475ECC" );

            // Attribute for BlockType
            //   BlockType: Pto Request List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.EnableDefaultWorkflowLauncher
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "2F76725E-CCF0-4E57-8F2F-54D4E63E6C99", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "2987273E-6B2E-4A7B-A982-B6EC4C0D6FA4" );

            // Attribute for BlockType
            //   BlockType: Pto Request List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: PTO Request Workflow
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "2F76725E-CCF0-4E57-8F2F-54D4E63E6C99", "46A03F59-55D3-4ACE-ADD5-B4642225DD20", "PTO Request Workflow", "PTORequestWorkflow", "PTO Request Workflow", @"The Workflow used to add, modify, and delete PTO Requests.", 0, @"EBF1D986-8BBD-4888-8A7E-43AF5914751C", "88EF0E18-53D3-407F-B07C-BB018FB1C802" );

            // Attribute for BlockType
            //   BlockType: Pto Request List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Workflow Entry Page Route
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "2F76725E-CCF0-4E57-8F2F-54D4E63E6C99", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Workflow Entry Page Route", "WorkflowEntryPageRoute", "Workflow Entry Page Route", @"The route to the workflow entry page.", 1, @"WorkflowEntry", "1CB79854-79E9-4501-89EE-7F03DE2A45D8" );

            // Attribute for BlockType
            //   BlockType: Pto Tier List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Detail Page
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "37F2DDA9-CB72-4FAC-950F-B260A6796230", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPage", "Detail Page", @"The page that will show the pto tier details.", 0, @"", "DD9B55DA-B62F-4DFD-AD47-798E10BF7953" );

            // Attribute for BlockType
            //   BlockType: Pto Tier List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.CustomActionsConfigs
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "37F2DDA9-CB72-4FAC-950F-B260A6796230", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "FFB4EFB1-5B06-4DD9-95FD-BAF8025F39C5" );

            // Attribute for BlockType
            //   BlockType: Pto Tier List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.EnableDefaultWorkflowLauncher
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "37F2DDA9-CB72-4FAC-950F-B260A6796230", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "F17B5F3A-B080-45AD-8E0A-41D79D878F92" );

            // Attribute for BlockType
            //   BlockType: Pto Type List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.CustomActionsConfigs
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "79D68E47-6D76-47BC-B528-FCBF570BB801", "9C204CD0-1233-41C5-818A-C5DA439445AA", "core.CustomActionsConfigs", "core.CustomActionsConfigs", "core.CustomActionsConfigs", @"", 0, @"", "AE30562A-1521-4F6D-9D10-08501826C59D" );

            // Attribute for BlockType
            //   BlockType: Pto Type List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: core.EnableDefaultWorkflowLauncher
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "79D68E47-6D76-47BC-B528-FCBF570BB801", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", "core.EnableDefaultWorkflowLauncher", @"", 0, @"True", "D4987462-30BF-4F94-8B2B-B9C7F499A3CB" );

            // Attribute for BlockType
            //   BlockType: Lava Tester
            //   Category: com_centralaz > Utility
            //   Attribute: Enabled Lava Commands
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "E32C203C-8091-45C1-B7D2-9950A6FB480B", "4BD9088F-5CC6-89B1-45FC-A2AAFFC7CC0D", "Enabled Lava Commands", "EnabledLavaCommands", "Enabled Lava Commands", @"The Lava commands that should be enabled.", 0, @"", "008A9C51-DAB0-4C84-BD35-4D44134AABD6" );

            // Attribute for BlockType
            //   BlockType: HR Employee List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Person Hired Date Attribute
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "8C5D6E7F-9A1B-2C3D-4E5F-6A7B8C9D0E1F", "99B090AA-4D7E-46D8-B393-BF945EA1BA8B", "Person Hired Date Attribute", "HireDate", "Person Hired Date Attribute", @"The Person Attribute that contains the Person's Hired Date. This will be used to determine if the person is currently staff or not.", 0, @"", "0816B434-35E4-4A0E-BD3B-CBF60E488CFC" );

            // Attribute for BlockType
            //   BlockType: HR Employee List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Person Fired Date Attribute
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "8C5D6E7F-9A1B-2C3D-4E5F-6A7B8C9D0E1F", "99B090AA-4D7E-46D8-B393-BF945EA1BA8B", "Person Fired Date Attribute", "FireDate", "Person Fired Date Attribute", @"The Person Attribute that contains the Person's Fired Date. This will be used to determine if the person is currently staff or not.", 1, @"", "83768F43-A1FE-4212-8B37-DC095AE8B2FD" );

            // Attribute for BlockType
            //   BlockType: HR Employee List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Person Supervisor Attribute
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "8C5D6E7F-9A1B-2C3D-4E5F-6A7B8C9D0E1F", "99B090AA-4D7E-46D8-B393-BF945EA1BA8B", "Person Supervisor Attribute", "Supervisor", "Person Supervisor Attribute", @"The Person Attribute that contains the Person's Supervisor.", 2, @"", "C4A631EF-2CE9-4F41-A66A-E4245A8F74B5" );

            // Attribute for BlockType
            //   BlockType: HR Employee List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Person Ministry Area Attribute
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "8C5D6E7F-9A1B-2C3D-4E5F-6A7B8C9D0E1F", "99B090AA-4D7E-46D8-B393-BF945EA1BA8B", "Person Ministry Area Attribute", "MinistryArea", "Person Ministry Area Attribute", @"The Person Attribute that contains the Person's Ministry Area.", 3, @"", "EFFEB180-DA8B-4C63-9676-E3D3A07DCE8D" );

            // Attribute for BlockType
            //   BlockType: HR Employee List
            //   Category: BEMA Software Services > Hr Management
            //   Attribute: Detail Page
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "8C5D6E7F-9A1B-2C3D-4E5F-6A7B8C9D0E1F", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPage", "Detail Page", @"The page that will show the employee details.", 4, @"", "3D016A5A-D3D7-4D2D-AF16-149A25DE3484" );

            
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
        }
    }
}
