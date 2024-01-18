using EF.Domain.Commons.Messages;
using EF.Produtos.Domain.Models;

namespace EF.Produtos.Application.Commands;

public class BuscarProdutoCommand : Command
{
    public ProdutoCategoria Categoria { get; set; }
}

