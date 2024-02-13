using AutoMapper;
using EF.Estoques.Application.DTOs.Responses;
using EF.Estoques.Application.UseCases.Interfaces;
using EF.Estoques.Domain.Repository;

namespace EF.Estoques.Application.UseCases;

public class ConsultaEstoqueUseCase : IConsultaEstoqueUseCase
{
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IMapper _mapper;

    public ConsultaEstoqueUseCase(IEstoqueRepository estoqueRepository, IMapper mapper)
    {
        _estoqueRepository = estoqueRepository;
        _mapper = mapper;
    }

    public async Task<EstoqueDto?> ObterEstoqueProduto(Guid produtoId, CancellationToken cancellationToken)
    {
        var estoque = await _estoqueRepository.Buscar(produtoId, cancellationToken);
        return _mapper.Map<EstoqueDto>(estoque);
    }

    public async Task<bool> ValidarEstoque(Guid produtoId, int quantidade,
        CancellationToken cancellationToken = default)
    {
        var estoque = await _estoqueRepository.Buscar(produtoId, cancellationToken);
        return estoque?.Quantidade > quantidade;
    }
}