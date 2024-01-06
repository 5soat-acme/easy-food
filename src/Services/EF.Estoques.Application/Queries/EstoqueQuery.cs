using AutoMapper;
using EF.Estoques.Application.DTOs.Responses;
using EF.Estoques.Application.Queries.Interfaces;
using EF.Estoques.Domain.Repository;

namespace EF.Estoques.Application.Queries;

public class EstoqueQuery : IEstoqueQuery
{
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IMapper _mapper;

    public EstoqueQuery(IEstoqueRepository estoqueRepository, IMapper mapper)
    {
        _estoqueRepository = estoqueRepository;
        _mapper = mapper;
    }

    public async Task<EstoqueDto?> ObterEstoqueProduto(Guid produtoId, CancellationToken cancellationToken)
    {
        var estoque = await _estoqueRepository.Buscar(produtoId, cancellationToken);
        return _mapper.Map<EstoqueDto>(estoque);
    }
}