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
using System.Collections.Generic;
using System.Web.UI.WebControls;

using com.bemaservices.HrManagement.Model;

using Rock;
using Rock.Web.UI.Controls;

namespace com.bemaservices.HrManagement.Web.UI.Controls.Pickers
{
    /// <summary>
    /// Class PtoTypePicker.
    /// Implements the <see cref="RockDropDownList" />
    /// </summary>
    /// <seealso cref="RockDropDownList" />
    public class PtoTypePicker : RockDropDownList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PtoTypePicker" /> class.
        /// </summary>
        public PtoTypePicker() : base()
        {
            Label = "PTO Type";
        }

        /// <summary>
        /// Gets or sets the Pto Types.
        /// </summary>
        /// <value>The Pto Types.</value>
        public List<PtoType> PtoTypes
        {
            set
            {
                this.Items.Clear();
                this.Items.Add( new ListItem() );

                foreach ( PtoType ptoType in value )
                {
                    this.Items.Add( new ListItem( ptoType.Name, ptoType.Id.ToString() ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected Pto Type identifier.
        /// </summary>
        /// <value>The selected Pto Type identifier.</value>
        public int? SelectedPtoTypeId
        {
            get
            {
                return this.SelectedValueAsId();
            }

            set
            {
                int id = value.HasValue ? value.Value : 0;
                var li = this.Items.FindByValue( id.ToString() );
                if( li != null )
                {
                    li.Selected = true;
                }
            }
        }
    }
}
