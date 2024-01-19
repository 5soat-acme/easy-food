using AutoMapper;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Domain.Models;
using EF.Produtos.Application.Queries.Interfaces;

namespace EF.Carrinho.Infra.Adapters.Produtos;

public class ProdutoAdapter : IProdutoService
{
    private readonly IProdutoQuery _produtoQuery;
    private readonly IMapper _mapper;

    public ProdutoAdapter(IProdutoQuery produtoQuery, IMapper mapper)
    {
        _produtoQuery = produtoQuery;
        _mapper = mapper;
    }

    public async Task<Item> ObterItemPorProdutoId(Guid id)
    {
        var produto = await _produtoQuery.BuscarPorId(id);
        return _mapper.Map<Item>(produto);
    }
}