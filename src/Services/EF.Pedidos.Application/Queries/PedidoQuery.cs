using AutoMapper;
using EF.Pedidos.Application.DTOs;
using EF.Pedidos.Application.Queries.Interfaces;
using EF.Pedidos.Domain.Repository;

namespace EF.Pedidos.Application.Queries;

public class PedidoQuery(IPedidoRepository respository, IMapper mapper) : IPedidoQuery
{
    public async Task<PedidoDto> ObterPedidoPorId(Guid pedidoId)
    {
        var pedido = await respository.ObterPorId(pedidoId);
        return mapper.Map<PedidoDto>(pedido);
    }

    public async Task<PedidoDto> ObterPedidoPorCorrelacaoId(Guid correlacaoId)
    {
        var pedido = await respository.ObterPorCorrelacaoId(correlacaoId);
        return mapper.Map<PedidoDto>(pedido);
    }
}