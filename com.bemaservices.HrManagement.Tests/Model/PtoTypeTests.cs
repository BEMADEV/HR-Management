using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.bemaservices.HrManagement.Model;
using System;

namespace com.bemaservices.HrManagement.Tests.Model
{
    [TestClass]
    public class PtoTypeTests
    {
        [TestMethod]
        public void PtoType_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var ptoType = new PtoType();

            // Assert
            Assert.IsTrue( ptoType.IsActive, "New PtoType should be active by default" );
            Assert.IsNull( ptoType.Name, "Name should be null by default" );
            Assert.IsNull( ptoType.Description, "Description should be null by default" );
        }

        [TestMethod]
        public void PtoType_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var ptoType = new PtoType();
            var expectedName = "Vacation";
            var expectedDescription = "Annual vacation days";
            var expectedColor = "#00FF00";

            // Act
            ptoType.Name = expectedName;
            ptoType.Description = expectedDescription;
            ptoType.Color = expectedColor;
            ptoType.IsActive = true;

            // Assert
            Assert.AreEqual( expectedName, ptoType.Name );
            Assert.AreEqual( expectedDescription, ptoType.Description );
            Assert.AreEqual( expectedColor, ptoType.Color );
            Assert.IsTrue( ptoType.IsActive );
        }

        [TestMethod]
        public void PtoType_FriendlyTypeName_ShouldReturnCorrectValue()
        {
            // Assert
            Assert.AreEqual( "Pto Type", PtoType.FriendlyTypeName );
        }
    }
}
