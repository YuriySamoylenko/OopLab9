using OopProjectPartB.Core;

namespace OopProjectPartB.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private StringWriter _consoleOut;
        private StringReader _consoleIn;

        [TestInitialize]
        public void Setup()
        {
            _consoleOut = new StringWriter();
            Console.SetOut(_consoleOut);
            _consoleIn = null;
        }

        [TestCleanup]
        public void Cleanup()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            _consoleOut?.Dispose();
            _consoleIn?.Dispose();
        }

        private void SetInput(params string[] lines)
        {
            _consoleIn = new StringReader(string.Join(Environment.NewLine, lines));
            Console.SetIn(_consoleIn);
        }

        private List<Apartment> CreateTestDatabase()
        {
            return new List<Apartment>();
        }

        [TestMethod]
        public void CreateApartment_AddsApartmentWithCorrectRoomCount()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("3"); // 3 bedrooms

            // Act
            Program.CreateApartment(db);

            // Assert
            Assert.AreEqual(1, db.Count);
            Assert.AreEqual(5, db[0].GetRooms().Count); // 1 Bathroom + 1 Kitchen + 3 Bedrooms
            Assert.IsTrue(db[0].GetRooms().Any(r => r is Bathroom));
            Assert.IsTrue(db[0].GetRooms().Any(r => r is Kitchen));
            Assert.AreEqual(3, db[0].GetRooms().Count(r => r is Bedroom));
        }

        [TestMethod]
        public void FillApartmentsWithFurniture_AddsFurniture_RespectsBathroomAndKitchenRules()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("1");
            Program.CreateApartment(db);

            // Act
            Program.FillApartmentsWithFurniture(db);

            // Assert
            var apartment = db[0];
            var bathroom = apartment.GetRooms().OfType<Bathroom>().First();
            var kitchen = apartment.GetRooms().OfType<Kitchen>().First();
            var bedroom = apartment.GetRooms().OfType<Bedroom>().First();

            // Ліжко НЕ додається у ванну та кухню → виняток
            var output = _consoleOut.ToString();
            Assert.IsTrue(output.Contains("can't put Bed"));

            // Але додається в спальню
            Assert.IsTrue(bedroom.GetFurniture().Any(f => f is Bed));

            // Стільці та столи додаються всюди (окрім ліжка в ванну/кухню)
            Assert.IsTrue(kitchen.GetFurniture().Any(f => f is Table));
            Assert.IsTrue(bathroom.GetFurniture().Any(f => f is Chair));
            Assert.AreEqual(3, kitchen.GetFurniture().Count(f => f is Chair)); // 3 стільці
        }

        [TestMethod]
        public void AddFurnitureToRoom_AddsCorrectFurnitureType()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("1"); // create apartment
            Program.CreateApartment(db);

            SetInput("0", "0", "0"); // apartment 0 → room 0 (Bathroom) → furniture type 0 (Bed) → має бути виняток

            // Act
            Program.AddFurnitureToRoom(db);

            // Assert
            var output = _consoleOut.ToString();
            Assert.IsTrue(output.Contains("can't put Bed")); // Bed в Bathroom → заборонено
        }

        [TestMethod]
        public void UseAllFurniture_PrintsCorrectUseMessages()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("1");
            Program.CreateApartment(db);
            Program.FillApartmentsWithFurniture(db);

            _consoleOut = new StringWriter();
            Console.SetOut(_consoleOut);

            // Act
            Program.UseAllFurniture(db);

            // Assert
            var output = _consoleOut.ToString();
            Assert.IsTrue(output.Contains("Lie on bed"));
            Assert.IsTrue(output.Contains("Sit on chair"));
            Assert.IsTrue(output.Contains("Put something on table"));
        }

        [TestMethod]
        public void MoveAllFurnitureTo_ChangesAllFurniturePlace()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("1");
            Program.CreateApartment(db);
            Program.FillApartmentsWithFurniture(db);

            // Act
            Program.MoveAllFurnitureTo(Place.East, db);

            // Assert
            foreach (var apartment in db)
                foreach (var room in apartment.GetRooms())
                    foreach (var furniture in room.GetFurniture())
                    {
                        Assert.AreEqual(Place.East, furniture.Place);
                    }
        }

        [TestMethod]
        public void RemoveFurniture_RemovesCorrectFurniture()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("1");
            Program.CreateApartment(db);
            Program.FillApartmentsWithFurniture(db);

            var firstBedroom = db[0].GetRooms().OfType<Bedroom>().First();
            var initialCount = firstBedroom.GetFurniture().Count;

            SetInput("0", "2", "0"); // apartment 0 → room 2 (Bedroom) → furniture 0 (перший предмет)

            // Act
            Program.RemoveFurniture(db);

            // Assert
            Assert.AreEqual(initialCount - 1, firstBedroom.GetFurniture().Count);
            var output = _consoleOut.ToString();
            Assert.IsTrue(output.Contains("removed"));
        }

        [TestMethod]
        public void RemoveApartment_RemovesCorrectApartment()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("1", "1"); // два рази створюємо квартиру
            Program.CreateApartment(db);
            Program.CreateApartment(db);

            SetInput("0"); // видаляємо першу

            // Act
            Program.RemoveApartment(db);

            // Assert
            Assert.AreEqual(1, db.Count);
            var output = _consoleOut.ToString();
            Assert.IsTrue(output.Contains("removed"));
        }

        [TestMethod]
        public void MoveFurniture_ChangesFurniturePlace()
        {
            // Arrange
            var db = CreateTestDatabase();
            SetInput("1");
            Program.CreateApartment(db);
            Program.FillApartmentsWithFurniture(db);

            var firstChair = db[0].GetRooms().First().GetFurniture().First();

            SetInput("0", "0", "0", "2"); // apartment 0 → room 0 → furniture 0 → place 2 (East)

            // Act
            Program.MoveFurniture(db);

            // Assert
            Assert.AreEqual(Place.East, firstChair.Place);
        }

        [TestMethod]
        public void ReadUserNumber_AcceptsValidInput()
        {
            SetInput("42");
            var result = Program.ReadUserNumber("Test: ", 0, 100);
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void ReadUserNumber_RejectsInvalidAndLoops()
        {
            SetInput("abc", "-5", "1000", "7");
            var result = Program.ReadUserNumber("Enter: ", 0, 10);

            Assert.AreEqual(7, result);

            var output = _consoleOut.ToString();
            Assert.IsTrue(output.Contains("Invalid format"));
            Assert.IsTrue(output.Contains("out of range"));
        }
    }
}
