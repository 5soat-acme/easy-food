using EF.Domain.Commons.Messages;
using EF.Produtos.Domain.Models;

namespace EF.Produtos.Application.Commands;

public class AtualizarProdutoCommand : Command
{
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }
    public decimal ValorUnitario { get; set; }
    public ProdutoCategoria Categoria { get; set; }
    public bool Ativo { get; set; }
}

