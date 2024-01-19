using EF.Domain.Commons.Messages;

namespace EF.Produtos.Application.Commands;

public class RemoverProdutoCommand : Command
{
    public Guid ProdutoId { get; set; }
}

