using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp
{
    [TestClass]
    public class LSPTests
    {
        [TestMethod]
        public void Square_SetHeight_DoesNotAffectWidth()
        {
            //arrange
            var square = new Square();

            //act and assert
            SetHeight_DoesNotAffectWidth(square);
        }

        [TestMethod]
        public void Rectangle_SetHeight_DoesNotAffectWidth()
        {
            //arrange
            var rectangle = new Rectangle();

            //act and assert
            SetHeight_DoesNotAffectWidth(rectangle);
        }

        public void SetHeight_DoesNotAffectWidth(Rectangle rectangle)
        {
            //arrange
            var expectedWidth = 4;
            rectangle.Width = expectedWidth;

            //act
            rectangle.Height = 7;

            //assert
            Assert.AreEqual(expectedWidth, rectangle.Width);
        }
    }
}
