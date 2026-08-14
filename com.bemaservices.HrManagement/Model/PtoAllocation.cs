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
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using com.bemaservices.HrManagement.Enums;
using Rock.Data;
using Rock.Lava;
using Rock.Model;

namespace com.bemaservices.HrManagement.Model
{
    /// <summary>
    /// A Reservation Location
    /// </summary>
    [Table( "_com_bemaservices_HrManagement_PtoAllocation" )]
    [Rock.SystemGuid.EntityTypeGuid( "198A76A9-9EA0-4030-B8B2-526ADE69C268" )]
    [DataContract]
    public class PtoAllocation : Rock.Data.Model<PtoAllocation>, Rock.Data.IRockEntity
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the pto type identifier.
        /// </summary>
        /// <value>The pto type identifier.</value>
        [Required]
        [DataMember]
        public int PtoTypeId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [Required]
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        /// <value>The hours.</value>
        [DataMember]
        public decimal Hours { get; set; }

        /// <summary>
        /// Gets or sets the pto accrual schedule.
        /// </summary>
        /// <value>The pto accrual schedule.</value>
        [DataMember]
        public PtoAccrualSchedule PtoAccrualSchedule { get; set; }

        /// <summary>
        /// Gets or sets the type of the pto allocation source.
        /// </summary>
        /// <value>The type of the pto allocation source.</value>
        [Required]
        [DataMember]
        public PtoAllocationSourceType PtoAllocationSourceType { get; set; }

        /// <summary>
        /// Gets or sets the last processed date.
        /// </summary>
        /// <value>The last processed date.</value>
        [DataMember]
        public DateTime? LastProcessedDate { get; set; }

        /// <summary>
        /// Gets or sets the pto allocation status.
        /// </summary>
        /// <value>The pto allocation status.</value>
        [Required]
        [DataMember]
        public PtoAllocationStatus PtoAllocationStatus { get; set; }

        /// <summary>
        /// Gets or sets the person alias identifier.
        /// </summary>
        /// <value>The person alias identifier.</value>
        [Required]
        [DataMember]
        public int PersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        [DataMember]
        public string Note { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var ptoTypeName = PtoType?.Name ?? "PTO Allocation";

            return ptoTypeName + " " + this.StartDate.ToString( "M/yyyy" ) + ( this.EndDate.HasValue ? " - " + this.EndDate.Value.ToString( "M/yyyy" ) : string.Empty );
        }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the type of the pto.
        /// </summary>
        /// <value>The type of the pto.</value>
        [LavaVisibleAttribute]
        public virtual PtoType PtoType { get; set; }

        /// <summary>
        /// Gets or sets the person alias.
        /// </summary>
        /// <value>The person alias.</value>
        [LavaVisibleAttribute]
        public virtual PersonAlias PersonAlias { get; set; }

        /// <summary>
        /// Gets or sets the pto requests.
        /// </summary>
        /// <value>The pto requests.</value>
        [LavaVisibleAttribute]
        public virtual ICollection<PtoRequest> PtoRequests
        {
            get { return _ptoRequests ?? ( _ptoRequests = new Collection<PtoRequest>() ); }
            set { _ptoRequests = value; }
        }

        /// <summary>
        /// The pto requests
        /// </summary>
        private ICollection<PtoRequest> _ptoRequests;
        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// The EF configuration class for the ReservationLocation.
    /// </summary>
    public partial class PtoAllocationConfiguration : EntityTypeConfiguration<PtoAllocation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PtoAllocationConfiguration" /> class.
        /// </summary>
        public PtoAllocationConfiguration()
        {
            this.HasRequired( r => r.PtoType ).WithMany().HasForeignKey( r => r.PtoTypeId ).WillCascadeOnDelete( false );

            // IMPORTANT!!
            this.HasEntitySetName( "PtoAllocation" );
        }
    }

    #endregion
}
