using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Application.Queries.Interfaces;

namespace EF.Produtos.Application.Queries;

public class ProdutoQuery : IProdutoQuery
{
    public Task<ProdutoDto> ObterPorId(Guid id)
    {
        throw new NotImplementedException();
    }
}