using EF.Carrinho.Application.DTOs.Responses;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoConsultaService
{
    Task<CarrinhoClienteDto?> ObterCarrinhoCliente();
}