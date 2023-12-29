using AutoMapper;
using EF.Carrinho.Application.DTOs.Integrations;
using EF.Cupons.Application.DTOs.Responses;
using EF.Estoques.Application.DTOs.Responses;

namespace EF.Carrinho.Application.Mappings;

public class ExternalDtoToDtoProfile : Profile
{
    public ExternalDtoToDtoProfile()
    {
        CreateMap<EstoqueDto, ProdutoEstoqueDto>();
        CreateMap<CupomDto, ProdutoDescontoDto?>().ConstructUsing((source, context) =>
        {
            if (source is null) return null;

            var destination = new ProdutoDescontoDto
            {
                Desconto = source.PorcentagemDesconto
            };

            source.Produtos.ToList().ForEach(p => destination.Ids.Add(p.ProdutoId));

            return destination;
        });
    }
}