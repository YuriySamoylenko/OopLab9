using OopProjectPartB.Core;

namespace OopProjectPartB.Tests
{
    [TestClass]
    public class FurnitureTests
    {
        [TestMethod]
        public void Furniture_DefaultNumberOfLegs_Is4()
        {
            var chair = new Chair();
            Assert.AreEqual(4, chair.NumberOfLegs);
        }

        [TestMethod]
        public void Move_ChangesPlaceCorrectly()
        {
            var table = new Table();
            table.Move(Place.South);
            Assert.AreEqual(Place.South, table.Place);
        }

        [TestMethod]
        public void Use_ReturnsCorrectMessage_ForEachType()
        {
            Assert.AreEqual("Sit on chair", new Chair().Use());
            Assert.AreEqual("Lie on bed", new Bed().Use());
            Assert.AreEqual("Put something on table", new Table().Use());
        }

        [TestMethod]
        public void ToString_ContainsNameAndId()
        {
            var bed = new Bed();
            var str = bed.ToString();
            Assert.IsTrue(str.Contains("Bed"));
            Assert.IsTrue(str.Contains(bed.Id.ToString()));
        }

        [TestMethod]
        public void CompareTo_SortsByTypeNameAlphabetically()
        {
            var furniture = new Furniture[]
            {
                new Table(), new Bed(), new Chair(), new Bed(), new Chair()
            };

            var sorted = furniture.OrderBy(f => f).ToArray();

            Assert.AreEqual(furniture[0], sorted[0]);
            Assert.AreEqual(furniture[1], sorted[1]);
            Assert.AreEqual(furniture[2], sorted[2]);
            Assert.AreEqual(furniture[3], sorted[3]);
            Assert.AreEqual(furniture[4], sorted[4]);
        }
    }
}
