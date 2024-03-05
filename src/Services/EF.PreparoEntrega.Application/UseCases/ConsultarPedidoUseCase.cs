using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Application.Mapping;
using EF.PreparoEntrega.Application.UseCases.Interfaces;
using EF.PreparoEntrega.Domain.Models;
using EF.PreparoEntrega.Domain.Repository;

namespace EF.PreparoEntrega.Application.UseCases;

public class ConsultarPedidoUseCase : IConsultarPedidoUseCase
{
    private readonly IPedidoRepository _pedidoRepository;

    public ConsultarPedidoUseCase(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<PedidoPreparoDto?> ObterPedidoPorId(Guid id)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorId(id);
        return DomainToDtoMapper.Map(pedido);
    }

    public async Task<IEnumerable<PedidoPreparoDto>> ObterPedidos(StatusPreparo? status)
    {
        var pedidos = await _pedidoRepository.ObterPedidos(status);
        var result = DomainToDtoMapper.MapToList(pedidos);
        return result.OrderByDescending(p => p.Status).ThenBy(p => p.DataCriacao);
    }

    public async Task<IEnumerable<PedidoMonitorDto>?> ObterPedidosMonitor()
    {
        var pedidos = await _pedidoRepository.ObterPedidosEmAberto();
        var result = DomainToDtoMapper.MapToMonitorList(pedidos);
        return result.OrderByDescending(p => p.Status).ThenByDescending(p => p.Codigo);
    }
}