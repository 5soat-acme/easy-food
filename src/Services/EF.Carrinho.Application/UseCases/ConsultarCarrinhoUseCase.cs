using AutoMapper;
using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Application.UseCases.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;

namespace EF.Carrinho.Application.UseCases;

public class ConsultarCarrinhoUseCase : IConsultarCarrinhoUseCase
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IMapper _mapper;

    public ConsultarCarrinhoUseCase(
        ICarrinhoRepository carrinhoRepository, IMapper mapper)
    {
        _carrinhoRepository = carrinhoRepository;
        _mapper = mapper;
    }

    public async Task<CarrinhoClienteDto> ConsultarCarrinho(CarrinhoSessaoDto carrinhoSessao)
    {
        var carrinho = await ObterCarrinho(carrinhoSessao);

        if (carrinho is null)
        {
            carrinho = new CarrinhoCliente();
            carrinho.AssociarCarrinho(carrinhoSessao.CarrinhoId);
        }

        if (carrinhoSessao.ClienteId.HasValue) carrinho.AssociarCliente(carrinhoSessao.ClienteId.Value);

        return _mapper.Map<CarrinhoClienteDto>(carrinho);
    }

    public async Task<CarrinhoCliente?> ObterCarrinho(CarrinhoSessaoDto carrinhoSessao)
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