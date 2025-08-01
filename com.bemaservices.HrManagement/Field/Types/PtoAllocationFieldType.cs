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
using System.Data;
using System.Linq;
using Rock;
using Rock.Attribute;
using Rock.Enums.Controls;
using Rock.Field.Types;
using Rock.SystemGuid;
using Rock.ViewModels.Utility;

namespace com.bemaservices.HrManagement.Field.Types
{
    /// <summary>
    /// Field Type used to display a list of options as checkboxes.  Value is saved as a | delimited list
    /// </summary>
    [Serializable]
    [FieldTypeGuid( "C3206242-E213-4038-99EB-1A563B375997" )]
    [CustomDropdownListField( "Control Type",
        Description = "The type of control to use for selecting a single value from the list.",
        IsRequired = true,
        ListSource = "ddl^Drop Down List,ddl_enhanced^Drop Down List (Enhanced for Long Lists),rb^Radio Buttons",
        DefaultValue = "ddl",
        Key = FIELDTYPE_KEY,
        Order = 0
        )]
    [IntegerField( "Columns",
        Description = "Select how many columns the list should use before going to the next row. If blank or 0 then 4 columns will be displayed. There is no enforced upper limit however the block this control is used in might add contraints due to available space.",
        IsRequired = false,
        Key = REPEAT_COLUMNS,
        Order = 1
        )]
    public class PtoAllocationFieldType : UniversalItemPickerFieldType
    {
        /// <summary>
        /// Gets a value to determine if this field type supports multiple
        /// selection or only single selection.
        /// </summary>
        /// <value><c>true</c> if this field type supports selecting multiple items.</value>
        protected override bool IsMultipleSelection => false;

        #region AttributeKeys

        /// <summary>
        /// The fieldtype key
        /// </summary>
        private const string FIELDTYPE_KEY = "fieldtype";
        /// <summary>
        /// The repeat columns
        /// </summary>
        private const string REPEAT_COLUMNS = "repeatColumns";

        #endregion

        #region Constants 

        /// <summary>
        /// The list source lava
        /// </summary>
        private const string ListSourceLava = @"{% include '~/Plugins/com_bemaservices/HrManagement/Assets/Lava/PtoAllocationFieldTypeLava.lava'  %}";

        #endregion 

        /// <summary>
        /// Gets the display style to use when rendering the edit control.
        /// </summary>
        /// <param name="privateConfigurationValues">The configuration values that describe the field type.</param>
        /// <returns>The style to display the edit control in.</returns>
        protected override UniversalItemValuePickerDisplayStyle GetDisplayStyle( Dictionary<string, string> privateConfigurationValues )
        {
            var fieldType = privateConfigurationValues.GetValueOrDefault( FIELDTYPE_KEY, string.Empty );
            if ( fieldType == "rb" )
            {
                return UniversalItemValuePickerDisplayStyle.List;
            }
            else
            {
                return UniversalItemValuePickerDisplayStyle.Condensed;
            }
        }

        /// <summary>
        /// Gets a value that indicates if the edit control should be rendered
        /// with enhanced selection mode. This provides search capabilities to
        /// the list of items in condensed display mode.
        /// </summary>
        /// <param name="privateConfigurationValues">The configuration values that describe the field type.</param>
        /// <returns><c>true</c> if the list of items should provide search capabilities; otherwise <c>false</c>.</returns>
        protected override bool GetEnhanceForLongLists( Dictionary<string, string> privateConfigurationValues )
        {
            var fieldType = privateConfigurationValues.GetValueOrDefault( FIELDTYPE_KEY, string.Empty );
            if ( fieldType == "ddl_enhanced" )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of columns to use when displaying the values in list
        /// display mode.
        /// </summary>
        /// <param name="privateConfigurationValues">The configuration values that describe the field type.</param>
        /// <returns>An integer that contains the number of columns to use or <c>null</c> to use the default.</returns>
        protected override int? GetColumnCount( Dictionary<string, string> privateConfigurationValues )
        {
            var fieldType = privateConfigurationValues.GetValueOrDefault( FIELDTYPE_KEY, string.Empty );
            var columnCount = privateConfigurationValues.GetValueOrDefault( REPEAT_COLUMNS, string.Empty ).AsIntegerOrNull();
            if ( fieldType == "rb" )
            {
                return columnCount;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the list of items to be displayed in the picker.
        /// </summary>
        /// <param name="privateConfigurationValues">The configuration values that describe the field type.</param>
        /// <returns>A list of item bags that will be rendered in the picker.</returns>
        protected override List<ListItemBag> GetListItems( Dictionary<string, string> privateConfigurationValues )
        {
            var configuredValues = GetConfiguredAllocationValues();

            return configuredValues.Select( item => new ListItemBag
            {
                Value = item.Key,
                Text = item.Value
            } )
            .ToList();
        }

        /// <summary>
        /// Gets the item bags for the values. If an item is not found
        /// (for example, no longer exists), then it should not be included
        /// in the returned list.
        /// </summary>
        /// <param name="values">The individual values that should be retrieved.</param>
        /// <param name="privateConfigurationValues">The private (database) configuration values.</param>
        /// <returns>A list of <see cref="T:Rock.ViewModels.Utility.ListItemBag" /> objects that have the <see cref="P:Rock.ViewModels.Utility.ListItemBag.Value" /> and <see cref="P:Rock.ViewModels.Utility.ListItemBag.Text" /> properties filled in.</returns>
        protected override List<ListItemBag> GetItemBags( IEnumerable<string> values, Dictionary<string, string> privateConfigurationValues )
        {
            var configuredValues = GetConfiguredAllocationValues();
            return configuredValues
            .Where( item => values.Contains( item.Key ) )
            .Select( item => new ListItemBag
            {
                Value = item.Key,
                Text = item.Value
            } )
            .ToList();
        }

        /// <summary>
        /// Gets the configured allocation values.
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public Dictionary<string, string> GetConfiguredAllocationValues()
        {
            var items = new Dictionary<string, string>();

            var listSource = ListSourceLava;

            var options = new Rock.Lava.CommonMergeFieldsOptions();
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( null, null, options );

            listSource = listSource.ResolveMergeFields( mergeFields, "RockEntity" );

            if ( listSource.ToUpper().Contains( "SELECT" ) && listSource.ToUpper().Contains( "FROM" ) )
            {
                var tableValues = new List<string>();
                DataTable dataTable = Rock.Data.DbService.GetDataTable( listSource, CommandType.Text, null );
                if ( dataTable != null && dataTable.Columns.Contains( "Value" ) && dataTable.Columns.Contains( "Text" ) )
                {
                    foreach ( DataRow row in dataTable.Rows )
                    {
                        items.AddOrReplace( row["value"].ToString(), row["text"].ToString() );
                    }
                }
            }

            else
            {
                foreach ( string keyvalue in listSource.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
                {
                    var keyValueArray = keyvalue.Split( new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries );
                    if ( keyValueArray.Length > 0 )
                    {
                        items.AddOrReplace( keyValueArray[0].Trim(), keyValueArray.Length > 1 ? keyValueArray[1].Trim() : keyValueArray[0].Trim() );
                    }
                }
            }


            return items;
        }
    }
}