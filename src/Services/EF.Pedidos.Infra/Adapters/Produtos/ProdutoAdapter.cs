using AutoMapper;
using EF.Pedidos.Application.DTOs.Gateways;
using EF.Pedidos.Application.Gateways;
using EF.Produtos.Application.UseCases.Interfaces;

namespace EF.Pedidos.Infra.Adapters.Produtos;

public class ProdutoAdapter : IProdutoService
{
    private readonly IConsultarProdutoUseCase _consultarProdutoUseCase;
    private readonly IMapper _mapper;

    public ProdutoAdapter(IConsultarProdutoUseCase consultarProdutoUseCase, IMapper mapper)
    {
        _consultarProdutoUseCase = consultarProdutoUseCase;
        _mapper = mapper;
    }

    public async Task<ProdutoDto> ObterPorId(Guid id)
    {
        var produto = await _consultarProdutoUseCase.BuscarPorId(id);
        return _mapper.Map<ProdutoDto>(produto);
    }
}