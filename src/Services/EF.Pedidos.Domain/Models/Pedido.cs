using EF.Domain.Commons.DomainObjects;

namespace EF.Pedidos.Domain.Models;

public class Pedido : Entity, IAggregateRoot
{
    private readonly List<Item> _itens;

    public Pedido(Guid clienteId)
    {
        _itens = new List<Item>();

        if (clienteId == Guid.Empty) throw new DomainException("Cliente não informado");

        ClienteId = clienteId;
    }

    // Necessário para o EF
    protected Pedido()
    {
    }

    public Guid ClienteId { get; private set; }

    public IReadOnlyCollection<Item> Itens => _itens;

    public void AdicionarItem(Item item)
    {
        _itens.Add(item);
    }
}