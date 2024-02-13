using AutoMapper;
using EF.Carrinho.Application.Gateways;
using EF.Carrinho.Domain.Models;
using EF.Produtos.Application.UseCases.Interfaces;

namespace EF.Carrinho.Infra.Adapters.Produtos;

public class ProdutoAdapter : IProdutoService
{
    private readonly IConsultarProdutoUseCase _consultarProdutoUseCase;
    private readonly IMapper _mapper;

    public ProdutoAdapter(IConsultarProdutoUseCase consultarProdutoUseCase, IMapper mapper)
    {
        _consultarProdutoUseCase = consultarProdutoUseCase;
        _mapper = mapper;
    }

    public async Task<Item> ObterItemPorProdutoId(Guid id)
    {
        var produto = await _consultarProdutoUseCase.BuscarPorId(id);
        return _mapper.Map<Item>(produto);
    }
}