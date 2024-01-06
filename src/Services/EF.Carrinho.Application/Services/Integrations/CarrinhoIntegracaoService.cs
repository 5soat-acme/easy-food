using EF.Carrinho.Application.Services.Interfaces;
using EF.Domain.Commons.Messages.Integrations;
using MediatR;

namespace EF.Carrinho.Application.Services.Integrations;

public class CarrinhoIntegracaoService : INotificationHandler<PedidoCriadoEvent>
{
    private readonly ICarrinhoManipulacaoService _service;

    public CarrinhoIntegracaoService(ICarrinhoManipulacaoService service)
    {
        _service = service;
    }

    public async Task Handle(PedidoCriadoEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Tratamento de erros com transaçõs de compensação
        await _service.RemoverCarrinho(notification.SessionId);
    }
}