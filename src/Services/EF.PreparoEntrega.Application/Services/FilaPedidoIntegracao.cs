// using EF.Domain.Commons.Mediator;
// using EF.Domain.Commons.Messages.Integrations;
// using MediatR;
//
// namespace EF.PreparoEntrega.Application.Services;
//
// public class FilaPedidoIntegracao  : INotificationHandler
// {
//     private readonly IMediatorHandler _mediator;
//     public FilaPedidoIntegracao(IMediatorHandler mediator)
//     {
//         _mediator = mediator;
//     }
//     public async Task Handle(CheckoutIniciadoEvent notification, CancellationToken cancellationToken)
//     {
//         try
//         {
//             _mediator.Send();
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
// }

