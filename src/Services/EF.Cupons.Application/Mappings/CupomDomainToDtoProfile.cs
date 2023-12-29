using AutoMapper;
using EF.Cupons.Application.DTOs.Responses;
using EF.Cupons.Domain.Models;

namespace EF.Cupons.Application.Mappings;

public class CupomDomainToDtoProfile : Profile
{
    public CupomDomainToDtoProfile()
    {
        CreateMap<Cupom, CupomDto>()
            .ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.CupomProdutos));

        CreateMap<CupomProduto, CupomProdutoDto>();
    }
}