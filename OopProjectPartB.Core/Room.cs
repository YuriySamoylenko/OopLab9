namespace OopProjectPartB.Core
{
    public abstract class Room : BaseEntity, IComparable<Room>
    {
        private string Name => this.GetType().Name;

        protected IList<Furniture> Furniture { get; } = new List<Furniture>();

        public float Area { get; }

        public Room(float area)
        {
            this.Area = area;
        }

        public IList<Furniture> GetFurniture()
        {
            return this.Furniture;
        }

        public virtual void AddFurniture(Furniture furniture)
        {
            this.Furniture.Add(furniture);
        }

        public virtual void RemoveFurniture(Furniture furniture)
        {
            this.Furniture.Remove(furniture);
        }

        public override string ToString()
        {
            var result = $"{this.Name} {this.Id}\n";
            foreach (var item in this.Furniture.Order())
            {
                result += $"\t\t {item}\n";
            }

            return result;
        }

        public int CompareTo(Room? other)
        {
            if (other == null) return 1;
            return this.Id.CompareTo(other.Id);
        }
    }
}
