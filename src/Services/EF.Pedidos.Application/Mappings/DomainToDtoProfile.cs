using AutoMapper;
using EF.Pedidos.Application.DTOs.Responses;
using EF.Pedidos.Domain.Models;

namespace EF.Pedidos.Application.Mappings;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Pedido, PedidoDto>()
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf.Numero));
        CreateMap<Item, ItemPedidoDto>();
    }
}