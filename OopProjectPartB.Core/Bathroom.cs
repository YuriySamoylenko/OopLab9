namespace OopProjectPartB.Core
{
    public class Bathroom : Room
    {
        public Bathroom(float area) : base(area)
        {
        }

        public override void AddFurniture(Furniture furniture)
        {
            if (furniture is Bed)
            {
                throw new ArgumentException($"You can't put {nameof(Bed)} to {nameof(Bathroom)}");
            }

            base.AddFurniture(furniture);
        }
    }
}
