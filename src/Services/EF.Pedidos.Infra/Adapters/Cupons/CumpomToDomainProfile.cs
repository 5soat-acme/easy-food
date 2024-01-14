using AutoMapper;
using EF.Pedidos.Application.DTOs.Adapters;

namespace EF.Pedidos.Infra.Adapters.Cupons;

public class CumpomToDomainProfile : Profile
{
    public CumpomToDomainProfile()
    {
        CreateMap<EF.Cupons.Application.DTOs.Responses.CupomDto, CupomDto?>();
    }
}