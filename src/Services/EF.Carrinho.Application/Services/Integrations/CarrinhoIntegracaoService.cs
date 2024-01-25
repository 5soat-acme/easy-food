using EF.Carrinho.Application.Services.Interfaces;
using EF.Domain.Commons.Messages.Integrations;
using MediatR;

namespace EF.Carrinho.Application.Services.Integrations;

public class CarrinhoIntegracaoService : INotificationHandler<PagamentoProcessadoEvent>
{
    private readonly ICarrinhoManipulacaoService _service;

    public CarrinhoIntegracaoService(ICarrinhoManipulacaoService service)
    {
        _service = service;
    }

    public async Task Handle(PagamentoProcessadoEvent notification, CancellationToken cancellationToken)
    {
        if (notification.ClientId.HasValue)
        {
            await _service.RemoverCarrinhoPorClienteId(notification.ClientId.Value);
            return;
        }

        await _service.RemoverCarrinho(notification.SessionId);
    }
}