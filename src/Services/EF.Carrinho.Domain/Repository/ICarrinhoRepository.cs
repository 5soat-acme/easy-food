using EF.Carrinho.Domain.Models;
using EF.Domain.Commons.Repository;

namespace EF.Carrinho.Domain.Repository;

public interface ICarrinhoRepository : IRepository<CarrinhoCliente>
{
    Task<CarrinhoCliente?> ObterPorCliente(Guid clienteId);
    Task<CarrinhoCliente?> ObterPorId(Guid id);
    void Criar(CarrinhoCliente carrinho);
    void Atualizar(CarrinhoCliente carrinho);
    void Remover(CarrinhoCliente carrinho);
}