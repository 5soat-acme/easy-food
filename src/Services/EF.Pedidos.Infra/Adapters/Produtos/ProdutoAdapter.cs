using AutoMapper;
using EF.Pedidos.Application.DTOs.Adapters;
using EF.Pedidos.Application.Ports;
using EF.Produtos.Application.Queries.Interfaces;

namespace EF.Pedidos.Infra.Adapters.Produtos;

public class ProdutoAdapter : IProdutoService
{
    private readonly IProdutoQuery _produtoQuery;
    private readonly IMapper _mapper;

    public ProdutoAdapter(IProdutoQuery produtoQuery, IMapper mapper)
    {
        _produtoQuery = produtoQuery;
        _mapper = mapper;
    }

    public async Task<ProdutoDto> ObterPorId(Guid id)
    {
        var produto = await _produtoQuery.BuscarPorId(id);
        return _mapper.Map<ProdutoDto>(produto);
    }
}