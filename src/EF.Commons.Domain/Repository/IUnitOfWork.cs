namespace EF.Domain.Commons.Repository;

public interface IUnitOfWork
{
    Task<bool> Commit();
}