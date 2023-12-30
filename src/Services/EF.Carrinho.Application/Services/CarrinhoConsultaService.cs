using AutoMapper;
using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;

namespace EF.Carrinho.Application.Services;

public class CarrinhoConsultaService : BaseCarrinhoService, ICarrinhoConsultaService
{
    private readonly IMapper _mapper;

    public CarrinhoConsultaService(
        ICarrinhoRepository carrinhoRepository,
        IMapper mapper) : base(carrinhoRepository)
    {
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
}