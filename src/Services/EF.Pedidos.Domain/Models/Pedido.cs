using EF.Domain.Commons.DomainObjects;

namespace EF.Pedidos.Domain.Models;

public class Pedido : Entity, IAggregateRoot
{
    private readonly List<Item> _itens;

    //EF
    protected Pedido()
    {
    }

    public Pedido(Guid? clienteId, decimal valorTotal)
    {
        Status = Status.Recebido;
        _itens = new List<Item>();
        ClienteId = clienteId;
        ValorTotal = ValidarValorTotal(valorTotal) ? ValorTotal : throw new DomainException("O valor total inválido");
    }

    public Guid CorrelacaoId { get; private set; }
    public Status Status { get; private set; }
    public Guid? ClienteId { get; private set; }
    public decimal ValorTotal { get; private set; }

    public IReadOnlyCollection<Item> Itens => _itens;

    public void AdicionarItem(Item item)
    {
        _itens.Add(item);
    }

    public bool ValidarValorTotal(decimal valorTotal)
    {
        if (valorTotal <= 0) return false;

        return true;
    }

    public void AssociarCorrelacao(Guid correlacaoId)
    {
        CorrelacaoId = correlacaoId;
    }
}