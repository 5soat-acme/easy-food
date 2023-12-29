using AutoMapper;
using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Repository;
using EF.WebApi.Commons.Users;

namespace EF.Carrinho.Application.Services;

public class CarrinhoConsultaService : BaseCarrinhoService, ICarrinhoConsultaService
{
    private readonly IMapper _mapper;

    public CarrinhoConsultaService(
        ICarrinhoRepository carrinhoRepository,
        IEstoqueService estoqueService,
        IUserApp user,
        IMapper mapper) : base(user, carrinhoRepository, estoqueService)
    {
        _mapper = mapper;
    }

    public async Task<CarrinhoClienteDto?> ObterCarrinhoCliente()
    {
        var carrinho = await ObterCarrinho();

        if (carrinho is null)
        {
            carrinho = new CarrinhoCliente(_carrinhoId);
            carrinho.AssociarCliente(_clienteId);
        }

        return _mapper.Map<CarrinhoClienteDto>(carrinho);
    }
}