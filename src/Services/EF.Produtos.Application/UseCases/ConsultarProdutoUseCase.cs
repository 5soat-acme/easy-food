using AutoMapper;
using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Application.UseCases.Interfaces;
using EF.Produtos.Domain.Models;
using EF.Produtos.Domain.Repository;

namespace EF.Produtos.Application.UseCases;

public class ConsultarProdutoUseCase : IConsultarProdutoUseCase
{
    private readonly IMapper _mapper;
    private readonly IProdutoRepository _produtoRepository;

    public ConsultarProdutoUseCase(IProdutoRepository produtoRepository, IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
    }

    public async Task<ProdutoDto> BuscarPorId(Guid id)
    {
        var produto = await _produtoRepository.BuscarPorId(id);
        return _mapper.Map<ProdutoDto>(produto);
    }

    public async Task<IEnumerable<ProdutoDto>> Buscar(ProdutoCategoria? categoria)
    {
        var produto = await _produtoRepository.Buscar(categoria);
        return _mapper.Map<IEnumerable<ProdutoDto>>(produto);
    }
}