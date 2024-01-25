using AutoMapper;
using EF.Carrinho.Domain.Models;
using EF.Produtos.Application.DTOs.Responses;

namespace EF.Carrinho.Infra.Adapters.Produtos;

public class ProdutoToDomainProfile : Profile
{
    public ProdutoToDomainProfile()
    {
        CreateMap<ProdutoDto, Item?>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ConstructUsing((source, context) =>
                source is null
                    ? null
                    : new Item(source.Id, source.Nome, source.ValorUnitario, source.TempoPreparoEstimado));
    }
}