using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoAppService
{
    Task<CarrinhoCliente> ObterCarrinhoPorCliente(Guid clienteId);
    Task<CarrinhoCliente> ObterCarrinhoPorId(Guid id);
    Task LimparCarrinho(Guid id);
    Task AdicionarItemCarrinho(Guid id, Item item);
    Task RemoverItemCarrinho(Guid id, Item item);
    Task AtualizarQuantidadeItemCarrinho(Guid id, Guid itemId, int quantidade);
}