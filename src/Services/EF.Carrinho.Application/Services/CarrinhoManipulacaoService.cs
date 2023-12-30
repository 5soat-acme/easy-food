using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.Domain.Commons.Communication;

namespace EF.Carrinho.Application.Services;

public class CarrinhoManipulacaoService : BaseCarrinhoService, ICarrinhoManipulacaoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly ICupomService _cupomService;
    private readonly IEstoqueService _estoqueService;
    private readonly IProdutoService _produtoService;

    public CarrinhoManipulacaoService(
        ICarrinhoRepository carrinhoRepository,
        IProdutoService produtoService,
        IEstoqueService estoqueService,
        ICupomService cupomService) : base(carrinhoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoService = produtoService;
        _estoqueService = estoqueService;
        _cupomService = cupomService;
    }

    public async Task<OperationResult> AdicionarItemCarrinho(AdicionarItemDto itemDto, CarrinhoSessaoDto carrinhoSessao)
    {
        var carrinho = await ObterCarrinho(carrinhoSessao);

        if (carrinho is null)
        {
            carrinho = await AdicionarItemCarrinhoNovo(itemDto, carrinhoSessao);
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

    public async Task<OperationResult> AtualizarItem(AtualizarItemDto itemDto, CarrinhoSessaoDto carrinhoSessao)
    {
        var carrinho = await ObterCarrinho(carrinhoSessao);

        if (carrinho is null) return OperationResult.Failure("O carrinho está vazio");

        carrinho.AtualizarQuantidadeItem(itemDto.ItemId, itemDto.Quantidade);
        var item = carrinho.ObterItemPorId(itemDto.ItemId);

        if (!await ValidarEstoque(item!)) return OperationResult.Failure("Produto sem estoque");

        _carrinhoRepository.Atualizar(carrinho);

        await PersistirDados();

        return OperationResult.Success();
    }

    public async Task<OperationResult> RemoverItemCarrinho(Guid itemId, CarrinhoSessaoDto carrinhoSessao)
    {
        var carrinho = await ObterCarrinho(carrinhoSessao);

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

    public async Task RemoverCarrinho(Guid carrinhoId)
    {
        var carrinho = await _carrinhoRepository.ObterPorId(carrinhoId);
        _carrinhoRepository.Remover(carrinho);
        await PersistirDados();
    }

    private CarrinhoCliente CriarCarrinhoCliente(CarrinhoSessaoDto carrinhoSessao)
    {
        var carrinho = new CarrinhoCliente();

        if (carrinhoSessao.ClienteId.HasValue) carrinho.AssociarCliente(carrinhoSessao.ClienteId.Value);

        carrinho.AssociarCarrinho(carrinhoSessao.CarrinhoId);

        return carrinho;
    }

    private async Task<CarrinhoCliente> AdicionarItemCarrinhoNovo(AdicionarItemDto itemDto,
        CarrinhoSessaoDto carrinhoSessao)
    {
        var item = await _produtoService.ObterItemPorProdutoId(itemDto.ProdutoId);
        item.AtualizarQuantidade(itemDto.Quantidade);
        var carrinho = CriarCarrinhoCliente(carrinhoSessao);
        carrinho.AdicionarItem(item);
        return carrinho;
    }

    private async Task<CarrinhoCliente> AdicionarItemCarrinhoExistente(CarrinhoCliente carrinho,
        AdicionarItemDto itemDto)
    {
        var produtoExiste = carrinho.ProdutoExiste(itemDto.ProdutoId);
        
        if (produtoExiste)
        {
            var itemExistente = carrinho.ObterItemPorProdutoId(itemDto.ProdutoId);
            itemExistente.AtualizarQuantidade(itemDto.Quantidade);
            carrinho.AdicionarItem(itemExistente);
            _carrinhoRepository.AtualizarItem(itemExistente);
        }
        else
        {
            var itemNovo = await _produtoService.ObterItemPorProdutoId(itemDto.ProdutoId);
            carrinho.AdicionarItem(itemNovo);
            _carrinhoRepository.AdicionarItem(itemNovo);
        }

        return carrinho;
    }

    protected async Task<bool> ValidarEstoque(Item item)
    {
        // TODO: Testar quando o estoque possuir dados
        // if (item is null) throw new ArgumentNullException(nameof(item));
        //
        // var estoque = await _estoqueService.ObterEstoquePorProdutoId(item.ProdutoId);
        //
        // if (estoque is null || estoque.Quantidade < item.Quantidade) return false;

        return true;
    }

    protected async Task PersistirDados()
    {
        await _carrinhoRepository.UnitOfWork.Commit();
    }
}