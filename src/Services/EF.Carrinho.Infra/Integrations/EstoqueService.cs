using AutoMapper;
using EF.Carrinho.Application.DTOs.Integrations;
using EF.Carrinho.Application.Ports;
using EF.Estoques.Application.Queries.Interfaces;

namespace EF.Carrinho.Infra.Integrations;

public class EstoqueService(IEstoqueQuery estoqueQuery, IMapper mapper) : IEstoqueService
{
    public async Task<ProdutoEstoqueDto?> ObterEstoquePorProdutoId(Guid produtoId)
    {
        var estoque = await estoqueQuery.ObterEstoqueProduto(produtoId, CancellationToken.None);
        return mapper.Map<ProdutoEstoqueDto>(estoque);
    }
}