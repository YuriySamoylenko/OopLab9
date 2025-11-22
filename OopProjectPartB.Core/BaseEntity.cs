namespace OopProjectPartB.Core
{
    public abstract class BaseEntity
    {
        public int Id { get; }

        public BaseEntity()
        {
            this.Id = IdGenerator.NewId();
        }
    }
}
