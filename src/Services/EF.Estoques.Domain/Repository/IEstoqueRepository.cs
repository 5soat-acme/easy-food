using EF.Domain.Commons.Repository;
using EF.Estoques.Domain.Models;

namespace EF.Estoques.Domain.Repository;

public interface IEstoqueRepository : IRepository<Estoque>
{
    Task<Estoque?> Buscar(Guid produtoId, CancellationToken cancellationToken);
    Task<Estoque> Salvar(Estoque estoque, CancellationToken cancellationToken);
}