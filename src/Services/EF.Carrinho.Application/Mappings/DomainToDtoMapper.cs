using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Mappings;

public static class DomainToDtoMapper
{
    public static CarrinhoClienteDto? Map(CarrinhoCliente? model)
    {
        if (model is null) return null;

        return new CarrinhoClienteDto
        {
            Id = model.Id,
            ClienteId = model.ClienteId,
            Itens = model.Itens.Select(Map).ToList()
        };
    }

    public static ItemCarrinhoDto Map(Item model)
    {
        return new ItemCarrinhoDto
        {
            Id = model.Id,
            ProdutoId = model.ProdutoId,
            Quantidade = model.Quantidade
        };
    }
}