using AutoMapper;
using EF.Carrinho.Application.DTOs.Integrations;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Infra.Integrations;

public class ProdutoService(IMapper mapper) : IProdutoService
{
    public async Task<Item> ObterItemPorProdutoId(Guid produtoId)
    {
        // TODO: Consumir serviço de produtos
        var produto = new ProdutoDto
        {
            ProdutoId = produtoId,
            Nome = "Produto Teste",
            ValorUnitario = 35.90m,
            TempoEstimadoPreparo = 15
        };

        return mapper.Map<Item>(produto);
    }
}