using AutoMapper;
using EF.Carrinho.Application.DTOs;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Mappings;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<CarrinhoCliente, CarrinhoClienteDto>();
        CreateMap<Item, ItemCarrinhoDto>();
        CreateMap<CarrinhoCliente, ResumoCarrinhoDto>();
        CreateMap<Item, ResumoItemDto>();
    }
}