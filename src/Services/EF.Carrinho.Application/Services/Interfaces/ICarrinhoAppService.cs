using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoAppService
{
    Task<CarrinhoCliente?> ObterCarrinhoCliente();
    Task LimparCarrinho();
    Task AdicionarItemCarrinho(Item item);
    Task AtualizarItem(Item item);
    Task RemoverItemCarrinho(Item item);
    
}