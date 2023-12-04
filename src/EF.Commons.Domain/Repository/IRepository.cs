using EF.Domain.Commons.DomainObjects;

namespace EF.Domain.Commons.Repository;

public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}