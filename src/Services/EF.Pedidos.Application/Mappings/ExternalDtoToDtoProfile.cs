using AutoMapper;
using EF.Cupons.Application.DTOs.Responses;
using EF.Estoques.Application.DTOs.Responses;
using EF.Pedidos.Application.DTOs.Integrations;

namespace EF.Pedidos.Application.Mappings;

public class ExternalDtoToDtoProfile : Profile
{
    public ExternalDtoToDtoProfile()
    {
        CreateMap<EstoqueDto, EstoqueProdutoDto>();
        CreateMap<CupomDto, CupomDescontoDto?>().ConstructUsing((source, context) =>
        {
            if (source is null) return null;

            var destination = new CupomDescontoDto
            {
                Desconto = source.PorcentagemDesconto
            };

            source.Produtos.ToList().ForEach(p => destination.Produtos.Add(p.ProdutoId));

            return destination;
        });
    }
}