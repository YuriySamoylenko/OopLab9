namespace OopProjectPartB.Core
{
    public interface IFurniture
    {
        int NumberOfLegs { get; }

        Place Place { get; }

        void Move(Place place);
    }
}
