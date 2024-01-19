using EF.Domain.Commons.Messages;
using EF.Produtos.Domain.Models;

namespace EF.Produtos.Application.Commands;

public class CriarProdutoCommand : Command
{
    public string Nome { get; set; }
    public decimal ValorUnitario { get; set; }
    public ProdutoCategoria Categoria { get; set; }
    public int TempoPreparoEstimado { get; set; }
    public string Descricao { get; set; }
}

