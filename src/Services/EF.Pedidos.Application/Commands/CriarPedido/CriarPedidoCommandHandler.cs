using EF.Domain.Commons.DomainObjects;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.ValueObjects;
using EF.Pedidos.Application.DTOs.Adapters;
using EF.Pedidos.Application.Ports;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.CriarPedido;

public class CriarPedidoCommandHandler : CommandHandler,
    IRequestHandler<CriarPedidoCommand, CommandResult>
{
    private readonly ICupomService _cupomService;
    private readonly IEstoqueService _estoqueService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoService _produtoService;

    public CriarPedidoCommandHandler(IPedidoRepository pedidoRepository,
        IEstoqueService estoqueService, ICupomService cupomService, IProdutoService produtoService)
    {
        _pedidoRepository = pedidoRepository;
        _estoqueService = estoqueService;
        _cupomService = cupomService;
        _produtoService = produtoService;
    }

    public async Task<CommandResult> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await MapearPedido(request);

        if (!string.IsNullOrEmpty(request.CodigoCupom)) pedido = await AplicarCupom(request.CodigoCupom, pedido);

        pedido.CalcularValorTotal();

        if (!await ValidarPedido(pedido)) return CommandResult.Create(ValidationResult);

        _pedidoRepository.Criar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result, pedido.Id);
    }
    

    private async Task<Pedido> MapearPedido(CriarPedidoCommand request)
    {
        var produtos = await ObterProdutosPedido(request);
        var pedido = new Pedido();

        if (request.ClienteId.HasValue) pedido.AssociarCliente(request.ClienteId.Value);

        if (!string.IsNullOrEmpty(request.ClienteCpf))
        {
            var cpf = new Cpf(request.ClienteCpf);
            pedido.AssociarCpf(cpf);
        }

        foreach (var itemAdd in request.Itens)
        {
            var produto = produtos.FirstOrDefault(p => p.Id == itemAdd.ProdutoId);

            var item = new Item(produto!.Id, produto.Nome, produto.ValorUnitario, itemAdd.Quantidade);
            pedido.AdicionarItem(item);
        }

        return pedido;
    }

    private async Task<Pedido> AplicarCupom(string codigoCupom, Pedido pedido)
    {
        var cupom = await _cupomService.ObterCupomPorCodigo(codigoCupom);

        if (cupom is null) return pedido;

        pedido.AssociarCupom(cupom.Id);

        foreach (var item in pedido.Itens)
            if (cupom.Produtos.Exists(produto => produto.ProdutoId == item.ProdutoId))
                pedido.AplicarDescontoItem(item.Id, cupom.PorcentagemDesconto);

        return pedido;
    }

    private async Task<bool> ValidarPedido(Pedido pedido)
    {
        foreach (var item in pedido.Itens)
            if (!await ValidarEstoque(item))
                AddError("Estoque insuficiente", item.Id.ToString());

        return ValidationResult.IsValid;
    }

    private async Task<bool> ValidarEstoque(Item item)
    {
        return await _estoqueService.VerificarEstoque(item.ProdutoId, item.Quantidade);
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