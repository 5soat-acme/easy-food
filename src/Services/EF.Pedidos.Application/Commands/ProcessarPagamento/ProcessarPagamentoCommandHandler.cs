using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Application.DTOs.Adapters;
using EF.Pedidos.Application.Ports;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.ProcessarPagamento;

public class ProcessarPagamentoCommandHandler : CommandHandler,
    IRequestHandler<ProcessarPagamentoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPagamentoService _pagamentoService;
    private readonly IProdutoService _produtoService;

    public ProcessarPagamentoCommandHandler(IPedidoRepository pedidoRepository, IPagamentoService pagamentoService,
        IProdutoService produtoService)
    {
        _pedidoRepository = pedidoRepository;
        _pagamentoService = pagamentoService;
        _produtoService = produtoService;
    }

    public async Task<CommandResult> Handle(ProcessarPagamentoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

        var pagamento = new PagamentoDto
        {
            PedidoId = pedido.Id,
            TipoPagamento = request.TipoPagamento,
            Valor = pedido.ValorTotal,
            Cpf = pedido.Cpf?.Numero
        };

        if (!await _pagamentoService.ProcessarPagamento(pagamento))
        {
            AddError("Erro ao processar o pagamento");
            return CommandResult.Create(ValidationResult);
        }

        pedido.ConfirmarPagamento();
        pedido.AddEvent(await CriarEvento(pedido, request));

        _pedidoRepository.Atualizar(pedido);

        ValidationResult = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(ValidationResult, pedido.Id);
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

    private async Task<PagamentoProcessadoEvent> CriarEvento(Pedido pedido, ProcessarPagamentoCommand request)
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
            SessionId = request.SessionId,
            Itens = itens
        };
    }
}