using EF.Carrinho.Domain.Models;
using EF.Domain.Commons.Repository;

namespace EF.Carrinho.Domain.Repository;

public interface ICarrinhoRepository : IRepository<CarrinhoCliente>
{
    Task<CarrinhoCliente?> ObterPorCliente(Guid clienteId);
    Task<CarrinhoCliente?> ObterPorId(Guid id);
    CarrinhoCliente Criar(CarrinhoCliente carrinho);
    CarrinhoCliente Atualizar(CarrinhoCliente carrinho);
    void Remover(CarrinhoCliente carrinho);
}