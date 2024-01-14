using EF.Carrinho.Application.Ports;
using EF.Estoques.Application.Queries.Interfaces;

namespace EF.Carrinho.Infra.Adapters.Estoque;

public class EstoqueAdapter : IEstoqueService
{
    private readonly IEstoqueQuery _estoqueQuery;

    public EstoqueAdapter(IEstoqueQuery estoqueQuery)
    {
        _estoqueQuery = estoqueQuery;
    }

    public async Task<bool> VerificarEstoque(Guid produtoId, int quantidade)
    {
        return await _estoqueQuery.ValidarEstoque(produtoId, quantidade);
    }
}