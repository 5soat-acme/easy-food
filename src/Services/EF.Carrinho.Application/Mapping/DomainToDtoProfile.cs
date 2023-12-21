using AutoMapper;
using EF.Carrinho.Application.DTOs;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Mapping;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<CarrinhoCliente, CarrinhoClienteDto>();
        CreateMap<Item, ItemDto>();
    }
}