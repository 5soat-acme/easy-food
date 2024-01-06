using EF.Pedidos.Application.DTOs.Integrations;
using EF.Pedidos.Application.Ports;

namespace EF.Pedidos.Infra.Integrations;

public class ProdutoService : IProdutoService
{
    public async Task<ProdutoDto?> ObterProdutoPorId(Guid produtoId)
    {
        // TODO: Consumir serviço de produtos
        var produto = new ProdutoDto
        {
            ProdutoId = produtoId,
            Nome = "Produto Teste",
            Descricao = "Descricao do Produto Teste",
            ValorUnitario = 35.90m,
            TempoEstimadoPreparo = 15
        };

        return produto;
    }
}