using AutoMapper;
using EF.Pedidos.Application.DTOs.Responses;
using EF.Pedidos.Application.UseCases.Interfaces;
using EF.Pedidos.Domain.Repository;

namespace EF.Pedidos.Application.UseCases;

public class ConsultarPedidoUseCase(IPedidoRepository respository, IMapper mapper) : IConsultarPedidoUseCase
{
    public async Task<PedidoDto?> ObterPedidoPorId(Guid pedidoId)
    {
        var pedido = await respository.ObterPorId(pedidoId);
        return mapper.Map<PedidoDto>(pedido);
    }
}