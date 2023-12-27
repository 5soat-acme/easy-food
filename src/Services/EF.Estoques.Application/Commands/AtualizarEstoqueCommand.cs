using EF.Domain.Commons.Messages;
using EF.Estoques.Domain.Models;

namespace EF.Estoques.Application.Commands;

public class AtualizarEstoqueCommand : Command
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public TipoMovimentacaoEstoque TipoMovimentacao { get; set; }
    public OrigemMovimentacaoEstoque OrigemMovimentacao { get; set; }
}