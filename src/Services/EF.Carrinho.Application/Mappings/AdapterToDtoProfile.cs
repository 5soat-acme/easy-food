using AutoMapper;
using EF.Carrinho.Application.DTOs.Adapters;

namespace EF.Carrinho.Application.Mappings;

public class AdapterToDtoProfile : Profile
{
    public AdapterToDtoProfile()
    {
        CreateMap<EF.Produtos.Application.DTOs.Responses.ProdutoDto, ProdutoDto?>();
    }
}