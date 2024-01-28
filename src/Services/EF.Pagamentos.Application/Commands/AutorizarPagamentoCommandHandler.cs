using EF.Domain.Commons.DomainObjects;
using EF.Domain.Commons.Messages;
using EF.Pagamentos.Application.Config;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Repository;
using MediatR;

namespace EF.Pagamentos.Application.Commands;

public class AutorizarPagamentoCommandHandler : CommandHandler,
    IRequestHandler<AutorizarPagamentoCommand, CommandResult>
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly PagamentoServiceResolver _resolver;

    public AutorizarPagamentoCommandHandler(IPagamentoRepository pagamentoRepository, PagamentoServiceResolver resolver)
    {
        _pagamentoRepository = pagamentoRepository;
        _resolver = resolver;
    }

    public async Task<CommandResult> Handle(AutorizarPagamentoCommand command, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(Tipo), command.TipoPagamento))
            throw new DomainException("Tipo de Pagamento inválido");

        var tipoPagamento = Enum.Parse<Tipo>(command.TipoPagamento);
        var pagamentoService = _resolver.GetService(tipoPagamento);
        var transacao =
            await pagamentoService.AutorizarPagamento(new Pagamento(command.PedidoId, tipoPagamento, command.Valor));
        var pagamento = new Pagamento(command.PedidoId, tipoPagamento, command.Valor);
        pagamento.AdicionarTransacao(transacao);
        await _pagamentoRepository.Criar(pagamento, cancellationToken);
        var result = await PersistData(_pagamentoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}