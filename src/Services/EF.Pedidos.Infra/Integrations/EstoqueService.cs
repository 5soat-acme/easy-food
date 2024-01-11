using AutoMapper;
using EF.Estoques.Application.DTOs.Responses;
using EF.Estoques.Application.Queries.Interfaces;
using EF.Pedidos.Application.DTOs.Integrations;
using EF.Pedidos.Application.Ports;

namespace EF.Pedidos.Infra.Integrations;

public class EstoqueService : IEstoqueService
{
    private readonly IMapper _mapper;
    private readonly IEstoqueQuery estoqueQuery;

    public EstoqueService(IMapper mapper, IEstoqueQuery estoqueQuery)
    {
        _mapper = mapper;
        this.estoqueQuery = estoqueQuery;
    }

    public async Task<EstoqueProdutoDto?> ObterEstoquePorProdutoId(Guid produtoId)
    {
        //  TODO: Retirar
        //var estoque = await estoqueQuery.ObterEstoqueProduto(produtoId, CancellationToken.None);
        var estoque = new EstoqueDto
        {
            ProdutoId = produtoId,
            Quantidade = 1000
        };
        return _mapper.Map<EstoqueProdutoDto>(estoque);
    }
}