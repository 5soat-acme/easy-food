using AutoMapper;
using EF.Carrinho.Application.DTOs.Integrations;
using EF.Carrinho.Application.Ports;
using EF.Estoques.Application.DTOs.Responses;
using EF.Estoques.Application.Queries.Interfaces;

namespace EF.Carrinho.Infra.Integrations;

public class EstoqueService(IEstoqueQuery estoqueQuery, IMapper mapper) : IEstoqueService
{
    public async Task<EstoqueProdutoDto?> ObterEstoquePorProdutoId(Guid produtoId)
    {
        var estoque = await estoqueQuery.ObterEstoqueProduto(produtoId, CancellationToken.None);

        //TODO: Retirar mock
        return mapper.Map<EstoqueProdutoDto>(new EstoqueDto
        {   
            ProdutoId = produtoId,
            Quantidade = 10
        });
    }
}