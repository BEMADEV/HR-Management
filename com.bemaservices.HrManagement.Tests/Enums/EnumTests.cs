using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.bemaservices.HrManagement.Enums;

namespace com.bemaservices.HrManagement.Tests.Enums
{
    [TestClass]
    public class PtoAllocationStatusTests
    {
        [TestMethod]
        public void PtoAllocationStatus_ShouldHaveExpectedValues()
        {
            // Assert
            Assert.AreEqual( 0, ( int ) PtoAllocationStatus.Inactive );
            Assert.AreEqual( 1, ( int ) PtoAllocationStatus.Active );
            Assert.AreEqual( 2, ( int ) PtoAllocationStatus.Pending );
            Assert.AreEqual( 3, ( int ) PtoAllocationStatus.Denied );
        }

        [TestMethod]
        public void PtoAllocationStatus_AllValuesAreDefined()
        {
            // Assert all expected values are defined
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAllocationStatus ), PtoAllocationStatus.Inactive ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAllocationStatus ), PtoAllocationStatus.Active ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAllocationStatus ), PtoAllocationStatus.Pending ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAllocationStatus ), PtoAllocationStatus.Denied ) );
        }
    }

    [TestClass]
    public class PtoAllocationSourceTypeTests
    {
        [TestMethod]
        public void PtoAllocationSourceType_ShouldHaveExpectedValues()
        {
            // Assert
            Assert.AreEqual( 1, ( int ) PtoAllocationSourceType.Automatic );
            Assert.AreEqual( 2, ( int ) PtoAllocationSourceType.Manual );
            Assert.AreEqual( 3, ( int ) PtoAllocationSourceType.Request );
        }

        [TestMethod]
        public void PtoAllocationSourceType_AllValuesAreDefined()
        {
            // Assert all expected values are defined
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAllocationSourceType ), PtoAllocationSourceType.Automatic ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAllocationSourceType ), PtoAllocationSourceType.Manual ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAllocationSourceType ), PtoAllocationSourceType.Request ) );
        }
    }

    [TestClass]
    public class PtoAccrualScheduleTests
    {
        [TestMethod]
        public void PtoAccrualSchedule_ShouldHaveExpectedValues()
        {
            // Assert
            Assert.AreEqual( 0, ( int ) PtoAccrualSchedule.None );
            Assert.AreEqual( 1, ( int ) PtoAccrualSchedule.Yearly );
            Assert.AreEqual( 2, ( int ) PtoAccrualSchedule.Quarterly );
            Assert.AreEqual( 3, ( int ) PtoAccrualSchedule.Monthly );
            Assert.AreEqual( 4, ( int ) PtoAccrualSchedule.Weekly );
        }

        [TestMethod]
        public void PtoAccrualSchedule_AllValuesAreDefined()
        {
            // Assert all expected values are defined
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAccrualSchedule ), PtoAccrualSchedule.None ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAccrualSchedule ), PtoAccrualSchedule.Yearly ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAccrualSchedule ), PtoAccrualSchedule.Quarterly ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAccrualSchedule ), PtoAccrualSchedule.Monthly ) );
            Assert.IsTrue( System.Enum.IsDefined( typeof( PtoAccrualSchedule ), PtoAccrualSchedule.Weekly ) );
        }
    }
}
