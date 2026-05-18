using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.bemaservices.HrManagement.Model;
using com.bemaservices.HrManagement.Enums;
using System;

namespace com.bemaservices.HrManagement.Tests.Model
{
    [TestClass]
    public class PtoAllocationTests
    {
        [TestMethod]
        public void PtoAllocation_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var allocation = new PtoAllocation();

            // Assert
            Assert.AreEqual( 0, allocation.PtoTypeId );
            Assert.AreEqual( 0, allocation.PersonAliasId );
            Assert.AreEqual( 0m, allocation.Hours );
            Assert.IsNull( allocation.EndDate );
            Assert.IsNull( allocation.LastProcessedDate );
            Assert.IsNull( allocation.Note );
        }

        [TestMethod]
        public void PtoAllocation_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var allocation = new PtoAllocation();
            var startDate = new DateTime( 2024, 1, 1 );
            var endDate = new DateTime( 2024, 12, 31 );
            var hours = 80m;
            var note = "Annual vacation allocation";

            // Act
            allocation.PtoTypeId = 1;
            allocation.PersonAliasId = 100;
            allocation.StartDate = startDate;
            allocation.EndDate = endDate;
            allocation.Hours = hours;
            allocation.Note = note;
            allocation.PtoAllocationStatus = PtoAllocationStatus.Active;
            allocation.PtoAllocationSourceType = PtoAllocationSourceType.Manual;
            allocation.PtoAccrualSchedule = PtoAccrualSchedule.Monthly;

            // Assert
            Assert.AreEqual( 1, allocation.PtoTypeId );
            Assert.AreEqual( 100, allocation.PersonAliasId );
            Assert.AreEqual( startDate, allocation.StartDate );
            Assert.AreEqual( endDate, allocation.EndDate );
            Assert.AreEqual( hours, allocation.Hours );
            Assert.AreEqual( note, allocation.Note );
            Assert.AreEqual( PtoAllocationStatus.Active, allocation.PtoAllocationStatus );
            Assert.AreEqual( PtoAllocationSourceType.Manual, allocation.PtoAllocationSourceType );
            Assert.AreEqual( PtoAccrualSchedule.Monthly, allocation.PtoAccrualSchedule );
        }

        [TestMethod]
        public void PtoAllocation_ToString_WithEndDate_ShouldIncludeDateRange()
        {
            // Arrange
            var allocation = new PtoAllocation
            {
                StartDate = new DateTime( 2024, 1, 1 ),
                EndDate = new DateTime( 2024, 12, 31 ),
                PtoType = new PtoType { Name = "Vacation" }
            };

            // Act
            var result = allocation.ToString();

            // Assert
            Assert.IsTrue( result.Contains( "Vacation" ) );
            Assert.IsTrue( result.Contains( "1/2024" ) );
            Assert.IsTrue( result.Contains( "12/2024" ) );
        }

        [TestMethod]
        public void PtoAllocation_ToString_WithoutEndDate_ShouldOnlyShowStartDate()
        {
            // Arrange
            var allocation = new PtoAllocation
            {
                StartDate = new DateTime( 2024, 6, 1 ),
                EndDate = null,
                PtoType = new PtoType { Name = "Sick Leave" }
            };

            // Act
            var result = allocation.ToString();

            // Assert
            Assert.IsTrue( result.Contains( "Sick Leave" ) );
            Assert.IsTrue( result.Contains( "6/2024" ) );
            Assert.IsFalse( result.Contains( " - " ) );
        }

        [TestMethod]
        public void PtoAllocation_AllocationStatus_TransitionsCorrectly()
        {
            // Arrange
            var allocation = new PtoAllocation
            {
                PtoAllocationStatus = PtoAllocationStatus.Pending
            };

            // Act & Assert - Verify status can be changed
            allocation.PtoAllocationStatus = PtoAllocationStatus.Active;
            Assert.AreEqual( PtoAllocationStatus.Active, allocation.PtoAllocationStatus );

            allocation.PtoAllocationStatus = PtoAllocationStatus.Denied;
            Assert.AreEqual( PtoAllocationStatus.Denied, allocation.PtoAllocationStatus );

            allocation.PtoAllocationStatus = PtoAllocationStatus.Inactive;
            Assert.AreEqual( PtoAllocationStatus.Inactive, allocation.PtoAllocationStatus );
        }
    }
}
