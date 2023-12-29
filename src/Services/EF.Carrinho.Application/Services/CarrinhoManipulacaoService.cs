using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.Domain.Commons.Communication;
using EF.WebApi.Commons.Users;

namespace EF.Carrinho.Application.Services;

public class CarrinhoManipulacaoService : BaseCarrinhoService, ICarrinhoManipulacaoService
{
    private readonly ICupomService _cupomService;
    private readonly IProdutoService _produtoService;

    public CarrinhoManipulacaoService(
        IUserApp user,
        ICarrinhoRepository carrinhoRepository,
        IProdutoService produtoService,
        IEstoqueService estoqueService,
        ICupomService cupomService) : base(user, carrinhoRepository, estoqueService)
    {
        _produtoService = produtoService;
        _cupomService = cupomService;
    }

    public async Task<OperationResult> AdicionarItemCarrinho(AdicionarItemDto itemDto)
    {
        var carrinho = await ObterCarrinho();

        if (carrinho is null)
        {
            carrinho = await AdicionarItemCarrinhoNovo(itemDto);
            _carrinhoRepository.Criar(carrinho);
        }
        else
        {
            carrinho = await AdicionarItemCarrinhoExistente(carrinho, itemDto);
            _carrinhoRepository.Atualizar(carrinho);
        }

        var item = carrinho.ObterItemPorProdutoId(itemDto.ProdutoId);

        if (!await ValidarEstoque(item!)) return OperationResult.Failure("Produto sem estoque");

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task<OperationResult> AtualizarItem(AtualizarItemDto itemDto)
    {
        var carrinho = await ObterCarrinho();

        if (carrinho is null) return OperationResult.Failure("O carrinho está vazio");

        carrinho.AtualizarQuantidadeItem(itemDto.ItemId, itemDto.Quantidade);
        var item = carrinho.ObterItemPorId(itemDto.ItemId);

        if (!await ValidarEstoque(item!)) return OperationResult.Failure("Produto sem estoque");

        _carrinhoRepository.Atualizar(carrinho);

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task<OperationResult> RemoverItemCarrinho(Guid itemId)
    {
        var carrinho = await ObterCarrinho();

        if (carrinho is null) return OperationResult.Failure("Carrinho não encontrado");

        var item = carrinho.Itens.FirstOrDefault(f => f.Id == itemId);

        if (item is null) return OperationResult.Success();

        carrinho.RemoverItem(item);
        _carrinhoRepository.RemoverItem(item);

        if (!carrinho.Itens.Any())
            _carrinhoRepository.Remover(carrinho);
        else
            _carrinhoRepository.Atualizar(carrinho);

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task<OperationResult> AplicarCupom(string codigo)
    {
        var produtoDesconto = await _cupomService.ObterDescontoCupom(codigo);
        var carrinho = await ObterCarrinho();

        if (carrinho is null) return OperationResult.Failure("Carrinho não encontrado");

        foreach (var produtoId in produtoDesconto.Ids)
            carrinho.AplicarDescontoItem(produtoId, produtoDesconto.Desconto);

        return OperationResult.Success();
    }

    private CarrinhoCliente CriarCarrinhoCliente()
    {
        var carrinho = new CarrinhoCliente(_carrinhoId);

        if (_clienteId != Guid.Empty) carrinho.AssociarCliente(_clienteId);

        return carrinho;
    }

    private async Task<CarrinhoCliente> AdicionarItemCarrinhoNovo(AdicionarItemDto itemDto)
    {
        var item = await _produtoService.ObterItemPorProdutoId(itemDto.ProdutoId);
        var carrinho = CriarCarrinhoCliente();
        carrinho.AdicionarItem(item);
        return carrinho;
    }

    private async Task<CarrinhoCliente> AdicionarItemCarrinhoExistente(CarrinhoCliente carrinho,
        AdicionarItemDto itemDto)
    {
        var produtoExiste = carrinho.ProdutoExiste(itemDto.ProdutoId);

        var item = await _produtoService.ObterItemPorProdutoId(itemDto.ProdutoId);

        carrinho.AdicionarItem(item);

        if (produtoExiste)
            _carrinhoRepository.AtualizarItem(carrinho.ObterItemPorProdutoId(itemDto.ProdutoId)!);
        else
            _carrinhoRepository.AdicionarItem(item);

        return carrinho;
    }
}