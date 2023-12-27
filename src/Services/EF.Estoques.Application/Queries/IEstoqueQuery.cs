using EF.Estoques.Application.DTOs;

namespace EF.Estoques.Application.Queries;

public interface IEstoqueQuery
{
    public Task<EstoqueDto?> ObterEstoqueProduto(Guid produtoId, CancellationToken cancellationToken);
}