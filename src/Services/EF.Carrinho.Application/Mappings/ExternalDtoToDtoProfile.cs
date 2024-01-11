using AutoMapper;
using EF.Carrinho.Application.DTOs.Integrations;
using EF.Estoques.Application.DTOs.Responses;

namespace EF.Carrinho.Application.Mappings;

public class ExternalDtoToDtoProfile : Profile
{
    public ExternalDtoToDtoProfile()
    {
        CreateMap<EstoqueDto, EstoqueProdutoDto>();
    }
}