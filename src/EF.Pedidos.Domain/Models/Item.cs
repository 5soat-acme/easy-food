using EF.Domain.Commons.DomainObjects;

namespace EF.Pedidos.Domain.Models;

public class Item : Entity
{
    public Item(Guid pedidoId, Guid produtoId, int quantidade)
    {
        //TODO: Abstrair validações
        if (pedidoId == Guid.Empty) throw new DomainException("Um item deve estar associado a um pedido");

        PedidoId = pedidoId;
        ProdutoId = produtoId;
        Quantidade = quantidade;
    }

    public Guid ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public Guid PedidoId { get; private set; }
    public Pedido Pedido { get; private set; }
}