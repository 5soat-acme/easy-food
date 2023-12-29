using EF.Carrinho.Application.DTOs.Requests;
using EF.Domain.Commons.Communication;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoManipulacaoService
{
    Task<OperationResult> AdicionarItemCarrinho(AdicionarItemDto itemDto);
    Task<OperationResult> AtualizarItem(AtualizarItemDto itemDto);
    Task<OperationResult> RemoverItemCarrinho(Guid itemId);
    Task<OperationResult> AplicarCupom(string codigo);
}