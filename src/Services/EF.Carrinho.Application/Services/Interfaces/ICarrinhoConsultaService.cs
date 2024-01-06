using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.DTOs.Responses;

namespace EF.Carrinho.Application.Services.Interfaces;

public interface ICarrinhoConsultaService
{
    Task<CarrinhoClienteDto> ConsultarCarrinho(CarrinhoSessaoDto carrinhoSessao);
}