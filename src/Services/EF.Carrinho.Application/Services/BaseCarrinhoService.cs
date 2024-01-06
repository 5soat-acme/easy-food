using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;

namespace EF.Carrinho.Application.Services;

public abstract class BaseCarrinhoService
{
    protected readonly ICarrinhoRepository _carrinhoRepository;

    public BaseCarrinhoService(ICarrinhoRepository carrinhoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
    }

    protected async Task<CarrinhoCliente?> ObterCarrinho(CarrinhoSessaoDto carrinhoSessao)
    {
        CarrinhoCliente? carrinho;

        if (carrinhoSessao.ClienteId.HasValue)
        {
            carrinho = await _carrinhoRepository.ObterPorClienteId(carrinhoSessao.ClienteId.Value);
            carrinho?.AssociarCliente(carrinhoSessao.ClienteId.Value);
        }
        else
        {
            carrinho = await _carrinhoRepository.ObterPorId(carrinhoSessao.CarrinhoId);
        }

        carrinho?.AssociarCarrinho(carrinhoSessao.CarrinhoId);

        return carrinho;
    }
}