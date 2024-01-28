using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Domain.Models;

namespace EF.Produtos.Application.Queries.Interfaces;

public interface IProdutoQuery
{
    Task<ProdutoDto> BuscarPorId(Guid id);
    Task<IEnumerable<ProdutoDto>> Buscar(ProdutoCategoria? categoria);
}