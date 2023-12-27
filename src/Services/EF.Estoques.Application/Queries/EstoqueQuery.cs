using EF.Estoques.Application.DTOs;
using EF.Estoques.Domain.Repository;

namespace EF.Estoques.Application.Queries;

public class EstoqueQuery : IEstoqueQuery
{
    private readonly IEstoqueRepository _estoqueRepository;

    public EstoqueQuery(IEstoqueRepository estoqueRepository)
    {
        _estoqueRepository = estoqueRepository;
    }

    public async Task<EstoqueDto?> ObterEstoqueProduto(Guid produtoId, CancellationToken cancellationToken)
    {
        var estoque = await _estoqueRepository.Buscar(produtoId, cancellationToken);
        return estoque is null
            ? null
            : new EstoqueDto
            {
                ProdutoId = estoque.ProdutoId,
                Quantidade = estoque.Quantidade
            };
    }
}