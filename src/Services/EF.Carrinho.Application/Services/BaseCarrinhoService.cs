using EF.Carrinho.Application.Ports;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.WebApi.Commons.Users;

namespace EF.Carrinho.Application.Services;

public abstract class BaseCarrinhoService
{
    protected readonly Guid _carrinhoId;
    protected readonly ICarrinhoRepository _carrinhoRepository;
    protected readonly Guid _clienteId;
    protected readonly IEstoqueService _estoqueService;

    public BaseCarrinhoService(IUserApp user, ICarrinhoRepository carrinhoRepository, IEstoqueService estoqueService)
    {
        _carrinhoRepository = carrinhoRepository;
        _estoqueService = estoqueService;
        _carrinhoId = user.ObterCarrinhoId();
        _clienteId = user.GetUserId();
    }

    protected async Task<CarrinhoCliente?> ObterCarrinho()
    {
        if (_clienteId != Guid.Empty) return await _carrinhoRepository.ObterPorCliente(_clienteId);

        return await _carrinhoRepository.ObterPorId(_carrinhoId);
    }

    protected async Task<bool> ValidarEstoque(Item item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        var estoque = await _estoqueService.ObterEstoquePorProdutoId(item.ProdutoId);

        if (estoque is null || estoque.Quantidade < item.Quantidade) return false;

        return true;
    }

    protected async Task PersistirDados()
    {
        await _carrinhoRepository.UnitOfWork.Commit();
    }
}