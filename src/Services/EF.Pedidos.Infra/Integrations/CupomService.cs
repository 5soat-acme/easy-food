using AutoMapper;
using EF.Cupons.Application.Queries.Interfaces;
using EF.Pedidos.Application.DTOs.Integrations;
using EF.Pedidos.Application.Ports;

namespace EF.Pedidos.Infra.Integrations;

public class CupomService : ICupomService
{
    private readonly IMapper _mapper;
    private readonly ICupomQuery _cupomQuery;
    public CupomService(IMapper mapper, ICupomQuery cupomQuery)
    {
        _mapper = mapper;
        _cupomQuery = cupomQuery;
    }
    
    public async Task<CupomDescontoDto?> ObterCupomDesconto(string codigo)
    {
        var cupom = await _cupomQuery.ObterCupom(codigo, CancellationToken.None);
        return _mapper.Map<CupomDescontoDto>(cupom);
    }
}