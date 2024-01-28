using AutoMapper;
using EF.Cupons.Application.DTOs.Responses;
using CupomProdutoDto = EF.Cupons.Application.DTOs.Responses.CupomProdutoDto;

namespace EF.Pedidos.Infra.Adapters.Cupons;

public class CumpomToDomainProfile : Profile
{
    public CumpomToDomainProfile()
    {
        CreateMap<CupomDto, Application.DTOs.Adapters.CupomDto?>();
        CreateMap<CupomProdutoDto, Application.DTOs.Adapters.CupomDto.CupomProdutoDto?>();
    }
}