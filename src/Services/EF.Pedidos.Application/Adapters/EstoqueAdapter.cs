using EF.Estoques.Application.Queries.Interfaces;
using EF.Pedidos.Application.Ports;

namespace EF.Pedidos.Application.Adapters;

public class EstoqueAdapter : IEstoqueService
{
    private readonly IEstoqueQuery _estoqueQuery;

    public EstoqueAdapter(IEstoqueQuery estoqueQuery)
    {
        _estoqueQuery = estoqueQuery;
    }

    public async Task<bool> VerificarEstoque(Guid produtoId, int quantidade)
    {
        // TODO: O estoque deve retornar true o false. Essa regra é do contexto de estoque.
        // var estoque = await _estoqueQuery.ObterEstoqueProduto(produtoId);
        // return estoque.Quantidade > quantidade;

        // TODO: Retirar
        return true;
    }
}