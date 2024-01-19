using AutoMapper;
using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Application.Queries.Interfaces;
using EF.Produtos.Domain.Models;
using EF.Produtos.Domain.Repository;

namespace EF.Produtos.Application.Queries;

public class ProdutoQuery : IProdutoQuery
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMapper _mapper;

    public ProdutoQuery(IProdutoRepository produtoRepository, IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
    }

    public async Task<ProdutoDto> BuscarPorId(Guid id)
    {
        var produto = await _produtoRepository.BuscarPorId(id);
        return _mapper.Map<ProdutoDto>(produto);
    }

    public async Task<IEnumerable<ProdutoDto>> Buscar(ProdutoCategoria categoria)
    {
        var produto = await _produtoRepository.Buscar(categoria);
        return _mapper.Map<IEnumerable<ProdutoDto>>(produto);
    }
}