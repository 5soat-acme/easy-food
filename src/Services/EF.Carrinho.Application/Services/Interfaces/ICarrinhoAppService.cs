using EF.Carrinho.Domain.Models;
using EF.Domain.Commons.Communication;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoAppService
{
    Task<CarrinhoCliente?> ObterCarrinhoCliente();
    Task LimparCarrinho();
    Task AdicionarItemCarrinho(Item item);
    Task<Result<Item>> AtualizarItem(Item item);
    Task RemoverItemCarrinho(Item item);
    
}