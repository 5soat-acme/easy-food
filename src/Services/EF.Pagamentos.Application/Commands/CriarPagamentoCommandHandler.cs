using EF.Domain.Commons.Messages;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Repository;
using MediatR;

namespace EF.Pagamentos.Application.Commands;

public class CriarPagamentoCommandHandler : CommandHandler,
    IRequestHandler<CriarPagamentoCommand, CommandResult>
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IFormaPagamentoRepository _formaPagamentoRepository;

    public CriarPagamentoCommandHandler(IPagamentoRepository pagamentoRepository,
        IFormaPagamentoRepository formaPagamentoRepository)
    {
        _pagamentoRepository = pagamentoRepository;
        _formaPagamentoRepository = formaPagamentoRepository;
    }

    public async Task<CommandResult> Handle(CriarPagamentoCommand command, CancellationToken cancellationToken)
    {
        var formaPagamento = await GetFormaPagamento(command.TipoFormaPagamento, cancellationToken);
        if (formaPagamento is null)
        {
            AddError("Forma de pagamento informada não existe.");
            return CommandResult.Create(ValidationResult);
        }

        var pagamento = new Pagamento(command.PedidoId, formaPagamento.Id, DateTime.Now, command.Valor);
        await _pagamentoRepository.Criar(pagamento, cancellationToken);
        var result = await PersistData(_pagamentoRepository.UnitOfWork);

        return CommandResult.Create(result, pagamento.Id);
    }

    private async Task<FormaPagamento?> GetFormaPagamento(TipoFormaPagamento tipoFormaPagamento, CancellationToken cancellationToken)
    {
        return await _formaPagamentoRepository.BuscarPorTipo(tipoFormaPagamento, cancellationToken);
    }
}