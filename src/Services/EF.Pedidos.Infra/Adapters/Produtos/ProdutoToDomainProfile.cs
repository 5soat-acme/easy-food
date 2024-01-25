using AutoMapper;
using EF.Produtos.Application.DTOs.Responses;

namespace EF.Pedidos.Infra.Adapters.Produtos;

public class ProdutoToDomainProfile : Profile
{
    public ProdutoToDomainProfile()
    {
        CreateMap<ProdutoDto, Application.DTOs.Adapters.ProdutoDto?>();
    }
}