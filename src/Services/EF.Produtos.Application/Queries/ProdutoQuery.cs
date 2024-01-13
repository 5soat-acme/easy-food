using AutoMapper;
using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Application.Queries.Interfaces;
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

    public async Task<ProdutoDto> ObterPorId(Guid id)
    {
        // TODO: Descomentar quando o repositório estiver pronto
        // var produto = await _produtoRepository.BuscarPorId(id);
        // return _mapper.Map<ProdutoDto>(produto);

        // TODO: Retirar este mock
        return new ProdutoDto
        {
            Id = id,
            Nome = "Produto Teste",
            ValorUnitario = 35.53m,
            TempoPreparoEstimado = 15,
            Descricao = "Descrição do produto"
        };
    }
}