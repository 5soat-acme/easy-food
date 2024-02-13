using AutoMapper;
using EF.Cupons.Application.DTOs.Responses;
using EF.Cupons.Application.UseCases.Interfaces;
using EF.Cupons.Domain.Repository;

namespace EF.Cupons.Application.UseCases;

public class ConsultarCupomUseCase : IConsultarCupomUseCase
{
    private readonly ICupomRepository _cupomRepository;
    private readonly IMapper _mapper;

    public ConsultarCupomUseCase(ICupomRepository cupomRepository, IMapper mapper)
    {
        _cupomRepository = cupomRepository;
        _mapper = mapper;
    }

    public async Task<CupomDto?> ObterCupom(string codigoCupom, CancellationToken cancellationToken = default)
    {
        var cupom = await _cupomRepository.BuscarCupomVigente(codigoCupom, cancellationToken);
        return _mapper.Map<CupomDto>(cupom);
    }
}