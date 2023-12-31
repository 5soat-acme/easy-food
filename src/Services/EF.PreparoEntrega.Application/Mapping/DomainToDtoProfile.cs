using AutoMapper;
using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Domain.Models;

namespace EF.PreparoEntrega.Application.Mapping;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Pedido, PedidoMonitorDto>();
        CreateMap<Pedido, PedidoPreparoDto>();
        CreateMap<Item, ItemPreparoDto>();
    }
}