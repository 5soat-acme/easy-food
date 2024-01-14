using AutoMapper;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Infra.Adapters.Produtos;

public class ProdutoToDomainProfile : Profile
{
    public ProdutoToDomainProfile()
    {
        CreateMap<EF.Produtos.Application.DTOs.Responses.ProdutoDto, Item?>().ConstructUsing((source, context) =>
            source is null
                ? null
                : new Item(source.Id, source.Nome, source.ValorUnitario, source.TempoPreparoEstimado));
    }
}