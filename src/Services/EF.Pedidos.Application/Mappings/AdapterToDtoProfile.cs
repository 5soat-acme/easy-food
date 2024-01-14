using AutoMapper;
using EF.Pedidos.Application.DTOs.Adapters;

namespace EF.Pedidos.Application.Mappings;

public class AdapterToDtoProfile: Profile
{
    public AdapterToDtoProfile()
    {
        CreateMap<EF.Cupons.Application.DTOs.Responses.CupomDto, CupomDto?>()
            .ForMember(x => x.Produtos, opt => opt.Ignore())
            .ConstructUsing((source, context) =>
        {
            if (source is null) return null;

            var destination = new CupomDto
            {
                Id = source.Id,
                Desconto = source.PorcentagemDesconto,
                Produtos = new List<Guid>()
            };
            
            source.Produtos.ToList().ForEach(p => destination.Produtos.Add(p.ProdutoId));
            
            return destination;
        });

        CreateMap<EF.Produtos.Application.DTOs.Responses.ProdutoDto, ProdutoDto?>()
            .ForMember(dest => dest!.ProdutoId, opt => opt.MapFrom(src => src.Id));
    }
}