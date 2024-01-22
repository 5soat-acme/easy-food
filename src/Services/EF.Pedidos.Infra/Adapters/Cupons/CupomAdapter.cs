using AutoMapper;
using EF.Cupons.Application.Queries.Interfaces;
using EF.Pedidos.Application.DTOs.Adapters;
using EF.Pedidos.Application.Ports;

namespace EF.Pedidos.Infra.Adapters.Cupons;

public class CupomAdapter : ICupomService
{
    private readonly ICupomQuery _cupomQuery;
    private readonly IMapper _mapper;

    public CupomAdapter(IMapper mapper, ICupomQuery cupomQuery)
    {
        _mapper = mapper;
        _cupomQuery = cupomQuery;
    }

    public async Task<CupomDto?> ObterCupomPorCodigo(string codigo)
    {
        var cupom = await _cupomQuery.ObterCupom(codigo);
        return _mapper.Map<CupomDto>(cupom);
    }
}