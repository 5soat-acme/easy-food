using EF.Domain.Commons.DomainObjects;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Domain.Commons.ValueObjects;
using EF.Pagamentos.Application.Commands;
using EF.Pedidos.Application.DTOs.Adapters;
using EF.Pedidos.Application.Ports;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.CriarPedido;

public class CriarPedidoCommandHandler : CommandHandler,
    IRequestHandler<CriarPedidoCommand, CommandResult>
{
    private readonly IMediatorHandler _mediator;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ICupomService _cupomService;
    private readonly IEstoqueService _estoqueService;
    private readonly IProdutoService _produtoService;

    public CriarPedidoCommandHandler(IMediatorHandler mediator, IPedidoRepository pedidoRepository,
        IEstoqueService estoqueService, ICupomService cupomService, IProdutoService produtoService)
    {
        _mediator = mediator;
        _pedidoRepository = pedidoRepository;
        _estoqueService = estoqueService;
        _cupomService = cupomService;
        _produtoService = produtoService;
    }

    public async Task<CommandResult> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
    {
        var produtos = await ObterProdutosPedido(request);

        var pedido = await MapearPedido(request, produtos);

        if (!string.IsNullOrEmpty(request.CodigoCupom)) pedido = await AplicarCupom(request.CodigoCupom, pedido);

        pedido.CalcularValorTotal();

        if (!await ValidarPedido(pedido, request)) return CommandResult.Create(ValidationResult);

        if (!await ProcessarPagamento(pedido, request.TipoPagamento)) return CommandResult.Create(ValidationResult);

        pedido.AddEvent(CriarEvento(pedido, request, produtos));

        _pedidoRepository.Criar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result, pedido.Id);
    }

    private async Task<bool> ProcessarPagamento(Pedido pedido, string tipoPagamento)
    {
        var result = await _mediator.Send(new AutorizarPagamentoCommand
        {
            PedidoId = pedido.Id,
            AggregateId = pedido.Id,
            TipoPagamento = tipoPagamento,
            Valor = pedido.ValorTotal,
            Cpf = pedido.Cpf?.Numero
        });

        if (!result.IsValid())
        {
            AddError("Erro ao processar pagamento");
            return false;
        }

        return true;
    }

    private async Task<Pedido> MapearPedido(CriarPedidoCommand request, List<ProdutoDto> produtos)
    {
        var pedido = new Pedido();

        if (request.ClienteId.HasValue) pedido.AssociarCliente(request.ClienteId.Value);

        if (!string.IsNullOrEmpty(request.ClienteCpf))
        {
            var cpf = new Cpf(request.ClienteCpf);
            pedido.AssociarCpf(cpf);
        }

        foreach (var itemAdd in request.Itens)
        {
            var produto = produtos.FirstOrDefault(p => p.ProdutoId == itemAdd.ProdutoId);

            var item = new Item(produto!.ProdutoId, produto.Nome, produto.ValorUnitario, itemAdd.Quantidade);
            pedido.AdicionarItem(item);
        }

        return pedido;
    }

    private async Task<Pedido> AplicarCupom(string codigoCupom, Pedido pedido)
    {
        var cupom = await _cupomService.OpterCupomPorCodigo(codigoCupom);
        if (cupom is not null)
        {
            pedido.AssociarCupom(cupom.Id);

            foreach (var item in pedido.Itens)
            {
                if (cupom.Produtos.Exists(produtoId => produtoId == item.ProdutoId))
                    pedido.AplicarDescontoItem(item.Id, cupom.Desconto);
            }
        }

        return pedido;
    }

    private async Task<bool> ValidarPedido(Pedido pedido, CriarPedidoCommand request)
    {
        if (pedido.ValorTotal != request.ValorTotal)
            AddError("O valor total do pedido não confere com o valor informado");

        foreach (var item in pedido.Itens)
            if (!await ValidarEstoque(item))
                AddError("Estoque insuficiente", item.Id.ToString());

        return ValidationResult.IsValid;
    }

    private async Task<bool> ValidarEstoque(Item item)
    {
        return await _estoqueService.VerificarEstoque(item.ProdutoId, item.Quantidade);
    }

    private PedidoCriadoEvent CriarEvento(Pedido pedido, CriarPedidoCommand request, List<ProdutoDto> produtos)
    {
        List<PedidoCriadoEvent.ItemPedido> itens = new();

        foreach (var item in pedido.Itens)
        {
            var produto = produtos.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
            itens.Add(new PedidoCriadoEvent.ItemPedido
            {
                Quantidade = item.Quantidade,
                ProdutoId = item.ProdutoId,
                NomeProduto = produto!.Nome,
                TempoPreparoEstimado = produto.TempoPreparoEstimado
            });
        }

        return new PedidoCriadoEvent
        {
            AggregateId = pedido.Id,
            ClientId = pedido.ClienteId,
            SessionId = request.SessionId,
            Itens = itens
        };
    }

    private async Task<List<ProdutoDto>> ObterProdutosPedido(CriarPedidoCommand request)
    {
        List<ProdutoDto> produtos = new();
        foreach (var item in request.Itens)
        {
            var produto = await _produtoService.ObterPorId(item.ProdutoId);
            if (produto is null) throw new DomainException("Produto inválido");
            produtos.Add(produto);
        }

        return produtos;
    }
}