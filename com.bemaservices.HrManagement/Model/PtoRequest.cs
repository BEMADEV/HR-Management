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
using System;
using Rock.Lava;

namespace com.bemaservices.HrManagement.Model
{
    /// <summary>
    /// A Reservation Location
    /// </summary>
    [Table( "_com_bemaservices_HrManagement_PtoRequest" )]
    [Rock.SystemGuid.EntityTypeGuid( "F38B7BA2-1289-4CFE-AFD4-74DF942280D8" )]
    [DataContract]
    public class PtoRequest : Rock.Data.Model<PtoRequest>, Rock.Data.IRockEntity
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>The request date.</value>
        [Required]
        [DataMember]
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        /// <value>The hours.</value>
        [Required]
        [DataMember]
        public decimal Hours { get; set; }

        /// <summary>
        /// Gets or sets the pto allocation identifier.
        /// </summary>
        /// <value>The pto allocation identifier.</value>
        [DataMember]
        public int PtoAllocationId { get; set; }

        /// <summary>
        /// Gets or sets the approver person alias identifier.
        /// </summary>
        /// <value>The approver person alias identifier.</value>
        [DataMember]
        public int? ApproverPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the state of the pto request approval.
        /// </summary>
        /// <value>The state of the pto request approval.</value>
        [Required]
        [DataMember]
        public PtoRequestApprovalState PtoRequestApprovalState { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>The reason.</value>
        [Required]
        [DataMember]
        public string Reason { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the approver person alias.
        /// </summary>
        /// <value>The approver person alias.</value>
        [LavaVisibleAttribute]
        public virtual PersonAlias ApproverPersonAlias { get; set; }

        /// <summary>
        /// Gets or sets the pto allocation.
        /// </summary>
        /// <value>The pto allocation.</value>
        [LavaVisibleAttribute]
        public virtual PtoAllocation PtoAllocation { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        [NotMapped]
        public virtual string Name
        {
            get
            {
                // Use the SuffixValueId and DefinedValue cache instead of referencing SuffixValue property so
                // that if FullName is used in datagrid, the SuffixValue is not lazy-loaded for each row
                return this.PtoAllocation.PersonAlias.Person.FullName + "(" + this.RequestDate.ToString( "M/d/yyyy" ) + ")";
            }

            private set
            {
                // intentionally blank
            }
        }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// The EF configuration class for the ReservationLocation.
    /// </summary>
    public partial class PtoRequestConfiguration : EntityTypeConfiguration<PtoRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PtoBracketTypeConfiguration" /> class.
        /// </summary>
        public PtoRequestConfiguration()
        {
            this.HasRequired( r => r.ApproverPersonAlias ).WithMany().HasForeignKey( r => r.ApproverPersonAliasId ).WillCascadeOnDelete( false );
            this.HasRequired( r => r.PtoAllocation ).WithMany( r => r.PtoRequests ).HasForeignKey( r => r.PtoAllocationId ).WillCascadeOnDelete( false );

            // IMPORTANT!!
            this.HasEntitySetName( "PtoRequest" );
        }
    }

    #endregion

    #region Enumerations
    /// <summary>
    /// Enum PtoRequestApprovalState
    /// </summary>
    public enum PtoRequestApprovalState
    {
        /// <summary>
        /// The pending
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The approved
        /// </summary>
        Approved = 1,

        /// <summary>
        /// The denied
        /// </summary>
        Denied = 2,

        /// <summary>
        /// The cancelled
        /// </summary>
        Cancelled = 3
    }

    #endregion
}
