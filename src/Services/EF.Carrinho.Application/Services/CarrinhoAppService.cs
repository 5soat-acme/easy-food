using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;

namespace EF.Carrinho.Application.Services;

public class CarrinhoAppService : ICarrinhoAppService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    public CarrinhoAppService(ICarrinhoRepository carrinhoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
    }
    
    public Task<CarrinhoCliente> ObterCarrinhoPorCliente(Guid clienteId)
    {
        throw new NotImplementedException();
    }

    public Task<CarrinhoCliente> ObterCarrinhoPorId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task LimparCarrinho(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AdicionarItemCarrinho(Guid id, Item item)
    {
        throw new NotImplementedException();
    }

    public Task RemoverItemCarrinho(Guid id, Item item)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarQuantidadeItemCarrinho(Guid id, Guid itemId, int quantidade)
    {
        throw new NotImplementedException();
    }
}