using EF.Core.Commons.Messages;
using EF.Core.Commons.Messages.Integrations;
using EF.Pedidos.Application.DTOs.Requests;
using EF.Pedidos.Application.UseCases.Interfaces;
using EF.Pedidos.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Pedidos.Application.Events;

public class PedidoEventHandler : IEventHandler<PreparoPedidoIniciadoEvent>,
    IEventHandler<PreparoPedidoFinalizadoEvent>, IEventHandler<EntregaRealizadaEvent>,
    IEventHandler<PagamentoAutorizadoEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PedidoEventHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Handle(EntregaRealizadaEvent notification)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var atualizarPedidoUseCase = scope.ServiceProvider.GetRequiredService<IAtualizarPedidoUseCase>();

        await atualizarPedidoUseCase.Handle(new AtualizarPedidoDto
        {
            PedidoId = notification.PedidoCorrelacaoId,
            Status = Status.Finalizado
        });
    }

    public async Task Handle(PreparoPedidoFinalizadoEvent notification)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var atualizarPedidoUseCase = scope.ServiceProvider.GetRequiredService<IAtualizarPedidoUseCase>();

        await atualizarPedidoUseCase.Handle(new AtualizarPedidoDto
        {
            PedidoId = notification.PedidoCorrelacaoId,
            Status = Status.Pronto
        });
    }

    public async Task Handle(PreparoPedidoIniciadoEvent notification)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var atualizarPedidoUseCase = scope.ServiceProvider.GetRequiredService<IAtualizarPedidoUseCase>();

        await atualizarPedidoUseCase.Handle(new AtualizarPedidoDto
        {
            PedidoId = notification.PedidoCorrelacaoId,
            Status = Status.EmPreparacao
        });
    }

    public async Task Handle(PagamentoAutorizadoEvent notification)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var atualizarPedidoUseCase = scope.ServiceProvider.GetRequiredService<IReceberPedidoUsecase>();

        await atualizarPedidoUseCase.Handle(new ReceberPedidoDto
        {
            PedidoId = notification.PedidoId
        });
    }
}