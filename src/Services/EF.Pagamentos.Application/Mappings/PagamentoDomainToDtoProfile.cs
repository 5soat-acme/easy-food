using AutoMapper;
using EF.Pagamentos.Application.DTOs.Responses;
using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Application.Mappings;

public class PagamentoDomainToDtoProfile : Profile
{
    public PagamentoDomainToDtoProfile()
    {
        CreateMap<FormaPagamento, FormaPagamentoDto>();
    }
}