namespace OopProjectPartB.Core
{
    public class Kitchen : Room
    {
        public Kitchen(float area) : base(area)
        {
        }

        public override void AddFurniture(Furniture furniture)
        {
            if (furniture is Bed)
            {
                throw new ArgumentException($"You can't put {nameof(Bed)} to {nameof(Kitchen)}");
            }

            base.AddFurniture(furniture);
        }
    }
}
