using AutoMapper;
using EF.Estoques.Application.DTOs.Responses;
using EF.Estoques.Domain.Models;

namespace EF.Estoques.Application.Mappings;

public class EstoqueDomainToDtoProfile : Profile
{
    public EstoqueDomainToDtoProfile()
    {
        CreateMap<Estoque, EstoqueDto>();
    }
}