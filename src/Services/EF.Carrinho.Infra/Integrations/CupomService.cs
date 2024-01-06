using AutoMapper;
using EF.Carrinho.Application.DTOs.Integrations;
using EF.Carrinho.Application.Ports;
using EF.Cupons.Application.Queries.Interfaces;

namespace EF.Carrinho.Infra.Integrations;

public class CupomService(ICupomQuery cupomQuery, IMapper mapper) : ICupomService
{
    public async Task<CupomDescontoDto?> ObterDescontoCupom(string codigo)
    {
        var cupom = await cupomQuery.ObterCupom(codigo, CancellationToken.None);
        return mapper.Map<CupomDescontoDto>(cupom);
    }
}