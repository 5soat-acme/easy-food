using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Domain.Commons.ValueObjects;
using EF.Pagamentos.Application.Commands;
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
    private readonly IEstoqueService _estoqueService;
    private readonly ICupomService _cupomService;
    private readonly IProdutoService _produtoService;


    public CriarPedidoCommandHandler(IMediatorHandler mediator, IPedidoRepository pedidoRepository,
        IEstoqueService estoqueService,
        ICupomService cupomService, IProdutoService produtoService)
    {
        _mediator = mediator;
        _pedidoRepository = pedidoRepository;
        _estoqueService = estoqueService;
        _cupomService = cupomService;
        _produtoService = produtoService;
    }

    public async Task<CommandResult> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = MapearPedido(request);

        if (!string.IsNullOrEmpty(request.CodigoCupom))
        {
            pedido = await AplicarCupom(request.CodigoCupom, pedido);
        }

        pedido.CalcularValorTotal();

        if (!await ValidarPedido(pedido, request)) return CommandResult.Create(ValidationResult);

        if (!await ProcessarPagamento(pedido, request.MetodoPagamento)) return CommandResult.Create(ValidationResult);

        pedido.AddEvent(new PedidoCriadoEvent
        {
            AggregateId = pedido.Id
        });

        _pedidoRepository.Criar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }

    private async Task<bool> ProcessarPagamento(Pedido pedido, string metodoPagamento)
    {
        var result = await _mediator.Send(new CriarPagamentoCommand
        {
            PedidoId = pedido.Id,
            AggregateId = pedido.Id,
            MetodoPagamento = metodoPagamento,
            Valor = pedido.ValorTotal
        });

        if (!result.IsValid())
        {
            AddError("Erro ao processar pagamento");
            return false;
        }

        return true;
    }

    private Pedido MapearPedido(CriarPedidoCommand request)
    {
        var pedido = new Pedido();

        if (string.IsNullOrEmpty(request.ClienteCpf))
        {
            var cpf = new Cpf(request.ClienteCpf);
            pedido.AssociarCpf(cpf);
        }

        foreach (var itemAdd in request.Itens)
        {
            var item = new Item(itemAdd.ProdutoId, itemAdd.Nome, itemAdd.ValorUnitario, itemAdd.Quantidade);
            pedido.AdicionarItem(item);
        }

        return pedido;
    }

    private async Task<Pedido> AplicarCupom(string codigoCupom, Pedido pedido)
    {
        var cupom = await _cupomService.ObterCupomDesconto(codigoCupom);
        pedido.AssociarCupom(cupom.Id);

        foreach (var item in pedido.Itens)
        {
            pedido.AplicarDescontoItem(item.Id, cupom.Desconto);
        }

        return pedido;
    }

    private async Task<bool> ValidarPedido(Pedido pedido, CriarPedidoCommand request)
    {
        if (pedido.ValorTotal != request.ValorTotal)
            AddError("O valor total do pedido não confere com o valor informado");

        foreach (var item in pedido.Itens)
        {
            if (!await ValidarEstoque(item))
                AddError($"Estoque insuficiente", item.Id.ToString());

            if (!await ValidarProduto(item))
                AddError($"O valor do item não confere com o valor informado", item.Id.ToString());
        }

        return ValidationResult.IsValid;
    }

    private async Task<bool> ValidarEstoque(Item item)
    {
        var estoque = await _estoqueService.ObterEstoquePorProdutoId(item.ProdutoId);

        if (estoque is null || estoque.Quantidade < item.Quantidade) return false;

        return true;
    }

    private async Task<bool> ValidarProduto(Item item)
    {
        var produto = await _produtoService.ObterProdutoPorId(item.ProdutoId);

        if (produto is null || produto.ValorUnitario != item.ValorUnitario) return false;

        return true;
    }
}