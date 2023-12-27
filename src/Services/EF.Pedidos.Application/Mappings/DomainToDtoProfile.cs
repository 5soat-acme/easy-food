using AutoMapper;
using EF.Pedidos.Application.DTOs;
using EF.Pedidos.Domain.Models;

namespace EF.Pedidos.Application.Mappings;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Pedido, PedidoDto>();
        CreateMap<Item, ItemPedidoDto>();
    }
}