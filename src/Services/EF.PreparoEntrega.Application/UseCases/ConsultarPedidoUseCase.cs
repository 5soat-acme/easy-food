using AutoMapper;
using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Application.UseCases.Interfaces;
using EF.PreparoEntrega.Domain.Models;
using EF.PreparoEntrega.Domain.Repository;

namespace EF.PreparoEntrega.Application.UseCases;

public class ConsultarPedidoUseCase : IConsultarPedidoUseCase
{
    private readonly IMapper _mapper;
    private readonly IPedidoRepository _pedidoRepository;

    public ConsultarPedidoUseCase(IPedidoRepository pedidoRepository, IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _mapper = mapper;
    }

    public async Task<PedidoPreparoDto> ObterPedidoPorId(Guid id)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorId(id);
        var result = _mapper.Map<PedidoPreparoDto>(pedido);
        return result;
    }

    public async Task<IEnumerable<PedidoPreparoDto>> ObterPedidos(StatusPreparo? status)
    {
        var pedidos = await _pedidoRepository.ObterPedidos(status);
        var result = _mapper.Map<IEnumerable<PedidoPreparoDto>>(pedidos);
        return result.OrderBy(p => p.DataCriacao);
    }

    public async Task<IEnumerable<PedidoMonitorDto>?> ObterPedidosMonitor()
    {
        var pedidos = await _pedidoRepository.ObterPedidosEmAberto();
        var result = _mapper.Map<IEnumerable<PedidoMonitorDto>>(pedidos);
        return result.OrderByDescending(p => p.Codigo);
    }
}