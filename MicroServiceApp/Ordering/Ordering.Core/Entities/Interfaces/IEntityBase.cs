namespace Ordering.Core.Entities.Interfaces
{
    public interface IEntityBase<out TId>
    {
        TId Id { get; }
    }
}
