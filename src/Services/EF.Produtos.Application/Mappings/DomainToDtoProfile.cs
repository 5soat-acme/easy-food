using AutoMapper;
using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Domain.Models;

namespace EF.Produtos.Application.Mappings;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Produto, ProdutoDto>();
    }
}