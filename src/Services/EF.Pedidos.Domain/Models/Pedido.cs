using EF.Domain.Commons.DomainObjects;
using EF.Domain.Commons.ValueObjects;

namespace EF.Pedidos.Domain.Models;

public class Pedido : Entity, IAggregateRoot
{
    private readonly List<Item> _itens;

    //EF
    protected Pedido()
    {
    }

    public Pedido(Guid? clienteId, Cpf? cpf)
    {
        Status = Status.Recebido;
        _itens = new List<Item>();
        ClienteId = clienteId;
        Cpf = cpf;
    }

    public Guid CorrelacaoId { get; private set; }
    public Guid? ClienteId { get; private set; }
    public Cpf? Cpf { get; private set; }
    public Status Status { get; private set; }
    public decimal ValorTotal { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataUltimaAtualizacao { get; private set; }

    public IReadOnlyCollection<Item> Itens => _itens;

    public void AdicionarItem(Item item)
    {
        _itens.Add(item);
    }

    public void AssociarCorrelacao(Guid correlacaoId)
    {
        CorrelacaoId = correlacaoId;
    }

    public void IniciarPreparo()
    {
        Status = Status.EmPreparacao;
    }

    public void DisponibilizarRetirada()
    {
        Status = Status.Pronto;
    }

    public void Finalizar()
    {
        Status = Status.Finalizado;
    }

    public void Cancelar()
    {
        Status = Status.Cancelado;
    }

    public void CalcularValorTotal()
    {
        foreach (var item in _itens) item.CalcularValorFinal();

        ValorTotal = _itens.Sum(i => i.ValorFinal);
    }
}