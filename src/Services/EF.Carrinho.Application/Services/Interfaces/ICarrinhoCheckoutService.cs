using EF.Carrinho.Application.DTOs.Responses;
using EF.Domain.Commons.Communication;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoCheckoutService
{
    Task<OperationResult<CheckoutRespostaDto>> IniciarCheckout();
}