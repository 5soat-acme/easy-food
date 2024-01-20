using AutoMapper;
using EF.Pedidos.Application.DTOs.Adapters;
using static EF.Pedidos.Application.DTOs.Adapters.CupomDto;

namespace EF.Pedidos.Infra.Adapters.Cupons;

public class CumpomToDomainProfile : Profile
{
    public CumpomToDomainProfile()
    {
        CreateMap<EF.Cupons.Application.DTOs.Responses.CupomDto, CupomDto?>();
        CreateMap<EF.Cupons.Application.DTOs.Responses.CupomProdutoDto, CupomProdutoDto?>();
    }
}