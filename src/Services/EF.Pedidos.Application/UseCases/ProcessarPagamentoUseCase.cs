using EF.Core.Commons.Communication;
using EF.Core.Commons.DomainObjects;
using EF.Core.Commons.Messages.Integrations;
using EF.Core.Commons.UseCases;
using EF.Pedidos.Application.DTOs.Gateways;
using EF.Pedidos.Application.DTOs.Requests;
using EF.Pedidos.Application.Gateways;
using EF.Pedidos.Application.UseCases.Interfaces;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;

namespace EF.Pedidos.Application.UseCases;

public class ProcessarPagamentoUseCase : CommonUseCase, IProcessarPagamentoUseCase
{
    private readonly IPagamentoService _pagamentoService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoService _produtoService;

    public ProcessarPagamentoUseCase(IPedidoRepository pedidoRepository, IPagamentoService pagamentoService,
        IProdutoService produtoService)
    {
        _pedidoRepository = pedidoRepository;
        _pagamentoService = pagamentoService;
        _produtoService = produtoService;
    }

    public async Task<OperationResult<Guid>> Handle(ProcessarPagamentoDto processarPagamentoDto)
    {
        var pedido = await _pedidoRepository.ObterPorId(processarPagamentoDto.PedidoId);
        if (pedido is null) throw new DomainException("Pedido inválido");

        var pagamento = new PagamentoDto
        {
            PedidoId = pedido.Id,
            TipoPagamento = processarPagamentoDto.TipoPagamento,
            Valor = pedido.ValorTotal,
            Cpf = pedido.Cpf?.Numero
        };

        if (!await _pagamentoService.ProcessarPagamento(pagamento))
        {
            AddError("Erro ao processar o pagamento");
            return OperationResult<Guid>.Failure(ValidationResult.GetErrorMessages());
        }

        pedido.ConfirmarPagamento();
        pedido.AddEvent(await CriarEvento(pedido, processarPagamentoDto));

        _pedidoRepository.Atualizar(pedido);
        ValidationResult = await PersistData(_pedidoRepository.UnitOfWork);

        if (!ValidationResult.IsValid) return OperationResult<Guid>.Failure(ValidationResult.GetErrorMessages());

        return OperationResult<Guid>.Success(pedido.Id);
    }

    private async Task<List<ProdutoDto>> ObterProdutosPedido(Pedido pedido)
    {
        List<ProdutoDto> produtos = new();
        foreach (var item in pedido.Itens)
        {
            var produto = await _produtoService.ObterPorId(item.ProdutoId);
            produtos.Add(produto);
        }

        return produtos;
    }

    private async Task<PagamentoProcessadoEvent> CriarEvento(Pedido pedido, ProcessarPagamentoDto processarPagamentoDto)
    {
        var produtos = await ObterProdutosPedido(pedido);
        List<PagamentoProcessadoEvent.ItemPedido> itens = new();

        foreach (var item in pedido.Itens)
        {
            var produto = produtos.FirstOrDefault(p => p.Id == item.ProdutoId);
            itens.Add(new PagamentoProcessadoEvent.ItemPedido
            {
                Quantidade = item.Quantidade,
                ProdutoId = item.ProdutoId,
                NomeProduto = produto!.Nome,
                TempoPreparoEstimado = produto.TempoPreparoEstimado
            });
        }

        return new PagamentoProcessadoEvent
        {
            AggregateId = pedido.Id,
            ClientId = pedido.ClienteId,
            SessionId = processarPagamentoDto.SessionId,
            Itens = itens
        };
    }
}