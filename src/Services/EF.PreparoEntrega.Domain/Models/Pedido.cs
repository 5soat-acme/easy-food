using EF.Domain.Commons.DomainObjects;

namespace EF.PreparoEntrega.Domain.Models;

public class Pedido : Entity, IAggregateRoot
{
    public Pedido(Guid correlacaoId)
    {
        CorrelacaoId = correlacaoId;
        Status = Status.Recebido;
    }

    public Guid CorrelacaoId { get; private set; }
    public string Codigo { get; private set; }
    public Status Status { get; private set; }
    private readonly List<Item> _itens;
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataUltimaAtualizacao { get; private set; }
    public IReadOnlyCollection<Item> Itens => _itens;

    public void IniciarPreparo()
    {
        Status = Status.EmPreparacao;
    }
    
    public void FinalizarPreparo()
    {
        Status = Status.Pronto;
    }
    
    public void ConfirmarEntrega()
    {
        Status = Status.Finalizado;
    }

    public void AdicionarItem(Item item)
    {
        throw new NotImplementedException();
    }
}