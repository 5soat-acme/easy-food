using AutoMapper;
using EF.Cupons.Application.DTOs.Responses;
using EF.Cupons.Application.Queries.Interfaces;
using EF.Cupons.Domain.Repository;

namespace EF.Cupons.Application.Queries;

public class CupomQuery : ICupomQuery
{
    private readonly ICupomRepository _cupomRepository;
    private readonly IMapper _mapper;

    public CupomQuery(ICupomRepository cupomRepository, IMapper mapper)
    {
        _cupomRepository = cupomRepository;
        _mapper = mapper;
    }

    public async Task<CupomDto?> ObterCupom(string codigoCupom, CancellationToken cancellationToken)
    {
        var cupom = await _cupomRepository.BuscarCupomVigente(codigoCupom, cancellationToken);
        return _mapper.Map<CupomDto>(cupom);
    }
}