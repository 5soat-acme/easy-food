using EF.Domain.Commons.DomainObjects;

namespace EF.PreparoEntrega.Domain.Models;

public class Item : Entity
{
    public Item(int quantidade, Guid produtoId)
    {
        Quantidade = quantidade;
        ProdutoId = produtoId;
    }

    public int Quantidade { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Guid PedidoId { get; private set; }
    public Pedido Pedido { get; private set; }
}