namespace OopProjectPartB.Core
{
    public class Apartment : BaseEntity
    {
        private List<Room> Rooms = new List<Room>();

        public Apartment(int bedRoomCount)
        {
            this.Rooms.Add(new Bathroom(8));
            this.Rooms.Add(new Kitchen(10));
            for (int i = 0; i < bedRoomCount; i++)
            {
                this.Rooms.Add(new Bedroom(15));
            }
        }

        public List<Room> GetRooms()
        {
            return this.Rooms;
        }

        public override string ToString()
        {
            var result = $"Apartment {this.Id}\n";
            foreach (var item in this.Rooms.Order())
            {
                result += $"\t {item}";
            }

            return result;
        }
    }
}
