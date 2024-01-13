using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Application.Queries.Interfaces;

namespace EF.Produtos.Application.Queries;

public class ProdutoQuery : IProdutoQuery
{
    public async Task<ProdutoDto> ObterPorId(Guid id)
    {
        // TODO: Retirar este mock
        return new ProdutoDto
        {
            ProdutoId = id,
            Nome = "Produto Teste",
            ValorUnitario = 35.53m,
            TempoPreparoEstimado = 15,
            Descricao = "Descrição do produto"
        };
    }
}