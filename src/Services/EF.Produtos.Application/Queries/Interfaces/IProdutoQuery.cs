using EF.Produtos.Application.DTOs.Responses;

namespace EF.Produtos.Application.Queries.Interfaces;

public interface IProdutoQuery
{
    Task<ProdutoDto> ObterPorId(Guid id);
}