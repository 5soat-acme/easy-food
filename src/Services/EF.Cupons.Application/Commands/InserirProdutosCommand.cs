using EF.Domain.Commons.Messages;

namespace EF.Cupons.Application.Commands;

public class InserirProdutosCommand : Command
{
    public Guid CupomId { get; set; }
    public IReadOnlyCollection<Guid> Produtos { get; set; }
}