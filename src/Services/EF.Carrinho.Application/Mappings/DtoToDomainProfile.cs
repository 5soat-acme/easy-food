using AutoMapper;
using EF.Carrinho.Application.DTOs;
using EF.Carrinho.Application.DTOs.Integrations;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Mappings;

public class DtoToDomainProfile : Profile
{
    public DtoToDomainProfile()
    {
        CreateMap<ProdutoDto, Item>();
    }
}