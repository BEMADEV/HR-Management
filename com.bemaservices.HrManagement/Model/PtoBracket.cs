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

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Model;
using Rock.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rock.Lava;

namespace com.bemaservices.HrManagement.Model
{
    /// <summary>
    /// A Reservation Location
    /// </summary>
    [Table( "_com_bemaservices_HrManagement_PtoBracket" )]
    [Rock.SystemGuid.EntityTypeGuid( "079275BF-4E79-4038-91E9-389A172DCA71" )]
    [DataContract]
    public class PtoBracket : Rock.Data.Model<PtoBracket>, Rock.Data.IRockEntity
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the pto tier identifier.
        /// </summary>
        /// <value>The pto tier identifier.</value>
        [Required]
        [DataMember]
        public int PtoTierId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        [Required]
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the minimum year.
        /// </summary>
        /// <value>The minimum year.</value>
        [Required]
        [DataMember]
        public int MinimumYear { get; set; }

        /// <summary>
        /// Gets or sets the maximum year.
        /// </summary>
        /// <value>The maximum year.</value>
        [DataMember]
        public int? MaximumYear { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the pto tier.
        /// </summary>
        /// <value>The pto tier.</value>
        [LavaVisibleAttribute]
        public virtual PtoTier PtoTier { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [LavaVisibleAttribute]
        public virtual string Name { get { return ToString(); } }

        /// <summary>
        /// Gets or sets the pto bracket types.
        /// </summary>
        /// <value>The pto bracket types.</value>
        [LavaVisibleAttribute]
        public virtual ICollection<PtoBracketType> PtoBracketTypes
        {
            get { return _ptoBracketTypes ?? ( _ptoBracketTypes = new Collection<PtoBracketType>() ); }
            set { _ptoBracketTypes = value; }
        }

        /// <summary>
        /// The pto bracket types
        /// </summary>
        private ICollection<PtoBracketType> _ptoBracketTypes;

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {

            if ( this.MaximumYear.HasValue )
            {
                return this.MinimumYear.ToString() + " to " + this.MaximumYear.Value.ToString() + " years";
            }
            else
            {
                return this.MinimumYear.ToString() + "+ years";
            }

        }
    }

    #region Entity Configuration

    /// <summary>
    /// The EF configuration class for the ReservationLocation.
    /// </summary>
    public partial class PtoBracketConfiguration : EntityTypeConfiguration<PtoBracket>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PtoBracketConfiguration" /> class.
        /// </summary>
        public PtoBracketConfiguration()
        {
            this.HasRequired( r => r.PtoTier ).WithMany( r => r.PtoBrackets ).HasForeignKey( r => r.PtoTierId ).WillCascadeOnDelete( false );

            // IMPORTANT!!
            this.HasEntitySetName( "PtoBracket" );
        }
    }

    #endregion
}
