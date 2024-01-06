using AutoMapper;
using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Application.Queries.Interfaces;
using EF.PreparoEntrega.Domain.Repository;

namespace EF.PreparoEntrega.Application.Queries;

public class PreparoEntregaQuery : IPreparoEntregaQuery
{
    private readonly IMapper _mapper;
    private readonly IPedidoRepository _pedidoRepository;

    public PreparoEntregaQuery(IPedidoRepository pedidoRepository, IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PedidoPreparoDto>> ObterPedidos()
    {
        var pedidos = await _pedidoRepository.ObterPedidosEmAberto();
        return _mapper.Map<IEnumerable<PedidoPreparoDto>>(pedidos);
    }

    public async Task<IEnumerable<PedidoMonitorDto>?> ObterPedidosMonitor()
    {
        var pedidos = await _pedidoRepository.ObterPedidosEmAberto();
        return _mapper.Map<IEnumerable<PedidoMonitorDto>>(pedidos);
    }
}