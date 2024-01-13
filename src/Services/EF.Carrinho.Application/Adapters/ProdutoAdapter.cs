using AutoMapper;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Domain.Models;
using EF.Produtos.Application.Queries.Interfaces;

namespace EF.Carrinho.Application.Adapters;

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
        // TODO: Retirar
        // var produto = _produtoQuery.ObterProdutoPorId(id);
        // return _mapper.Map<Item>(produto);

        return new Item(id, "Produto Teste", 35.50m, 15);
    }
}