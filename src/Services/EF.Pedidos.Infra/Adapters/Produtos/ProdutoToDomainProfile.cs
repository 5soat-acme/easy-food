using AutoMapper;
using EF.Pedidos.Application.DTOs.Adapters;

namespace EF.Pedidos.Infra.Adapters.Produtos;

public class ProdutoToDomainProfile : Profile
{
    public ProdutoToDomainProfile()
    {
        CreateMap<EF.Produtos.Application.DTOs.Responses.ProdutoDto, ProdutoDto?>();
    }
}