using AutoMapper;
using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Domain.Models;

namespace EF.PreparoEntrega.Application.Mapping;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Pedido, PedidoMonitorDto>()
            .ForMember(dest => dest.TempoEspera, opt => opt.MapFrom(src => FormatTempoDecorrido(src.DataCriacao)));
        CreateMap<Pedido, PedidoPreparoDto>();
        CreateMap<Item, ItemPreparoDto>();
    }

    private string FormatTempoDecorrido(DateTime dataCriacao)
    {
        DateTime agoraUtc = DateTime.UtcNow;
        TimeSpan tempoDecorrido = agoraUtc - dataCriacao;
        tempoDecorrido = tempoDecorrido.Duration();
        return $"{tempoDecorrido.Hours:D2}:{tempoDecorrido.Minutes:D2}:{tempoDecorrido.Seconds:D2}";
    }
}