using EF.Core.Commons.Communication;
using EF.Core.Commons.DomainObjects;
using EF.Core.Commons.UseCases;
using EF.Pagamentos.Application.Config;
using EF.Pagamentos.Application.DTOs.Requests;
using EF.Pagamentos.Application.UseCases.Interfaces;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Repository;

namespace EF.Pagamentos.Application.UseCases;

public class AutorizarPagamentoUseCase : CommonUseCase, IAutorizarPagamentoUseCase
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly PagamentoServiceResolver _resolver;

    public AutorizarPagamentoUseCase(IPagamentoRepository pagamentoRepository, PagamentoServiceResolver resolver)
    {
        _pagamentoRepository = pagamentoRepository;
        _resolver = resolver;
    }

    public async Task<OperationResult> Handle(AutorizarPagamentoDto autorizarPagamentoDto)
    {
        if (!Enum.IsDefined(typeof(Tipo), autorizarPagamentoDto.TipoPagamento))
            throw new DomainException("Tipo de Pagamento inválido");

        var tipoPagamento = Enum.Parse<Tipo>(autorizarPagamentoDto.TipoPagamento);
        var pagamentoService = _resolver.GetService(tipoPagamento);
        var transacao =
            await pagamentoService.AutorizarPagamento(new Pagamento(autorizarPagamentoDto.PedidoId, tipoPagamento,
                autorizarPagamentoDto.Valor));
        var pagamento = new Pagamento(autorizarPagamentoDto.PedidoId, tipoPagamento, autorizarPagamentoDto.Valor);
        pagamento.AdicionarTransacao(transacao);
        await _pagamentoRepository.Criar(pagamento);
        await PersistData(_pagamentoRepository.UnitOfWork);

        if (!ValidationResult.IsValid) return OperationResult.Failure(ValidationResult);

        return OperationResult.Success();
    }
}