using EF.Domain.Commons.Messages;

namespace EF.Pedidos.Application.Commands;

public class GerarPedidoCommand : Command
{
    public Guid CarrinhoId { get; set; }
    public Guid? ClienteId { get; set; }
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}