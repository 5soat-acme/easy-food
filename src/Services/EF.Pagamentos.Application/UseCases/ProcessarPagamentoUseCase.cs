using EF.Core.Commons.Communication;
using EF.Core.Commons.UseCases;
using EF.Pagamentos.Application.Config;
using EF.Pagamentos.Application.DTOs.Requests;
using EF.Pagamentos.Application.UseCases.Interfaces;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Repository;

namespace EF.Pagamentos.Application.UseCases;

public class ProcessarPagamentoUseCase : CommonUseCase, IProcessarPagamentoUseCase
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly PagamentoServiceResolver _resolver;

    public ProcessarPagamentoUseCase(IPagamentoRepository pagamentoRepository, PagamentoServiceResolver resolver)
    {
        _pagamentoRepository = pagamentoRepository;
        _resolver = resolver;
    }

    public async Task<OperationResult> Handle(ProcessarPagamentoDto processarPagamentoDto)
    {
        var tipoPagamento = Enum.Parse<Tipo>(processarPagamentoDto.TipoPagamento);

        var pagamento = new Pagamento(processarPagamentoDto.PedidoId, tipoPagamento, processarPagamentoDto.ValorTotal);

        var pagamentoService = _resolver.GetService(tipoPagamento);
        await pagamentoService.AutorizarPagamento(pagamento);

        _pagamentoRepository.Criar(pagamento);
        ValidationResult = await PersistData(_pagamentoRepository.UnitOfWork);

        if (!ValidationResult.IsValid) return OperationResult<Guid>.Failure(ValidationResult.GetErrorMessages());

        return OperationResult.Success();
    }
}