namespace CafeManager.Core.Common.Interfaces;

public interface IEntity<TId> where TId : struct
{
    TId Id { get; }
}