using EF.Domain.Commons.DomainObjects;

namespace EF.Pagamentos.Domain.Models;

public class Pagamento : Entity, IAggregateRoot
{
    private Pagamento()
    {
    }

    public Pagamento(Guid pedidoId, Guid formaPagamentoId, DateTime dataLancamento, decimal valor)
    {
        if (!ValidarPedido(pedidoId)) throw new DomainException("Um pagamento deve estar associado a um pedido");
        if (!ValidarFormaPagamento(formaPagamentoId)) throw new DomainException("FormaPagamento inválida");
        if (!ValidarValor(valor)) throw new DomainException("Valor inválido");

        PedidoId = pedidoId;
        FormaPagamentoId = formaPagamentoId;
        DataLancamento = dataLancamento;
        Valor = valor;
    }

    public Guid PedidoId { get; private set; }
    public FormaPagamento FormaPagamento { get; private set; }
    public Guid FormaPagamentoId { get; private set; }
    public DateTime DataLancamento { get; private set; }
    public decimal Valor { get; private set; }

    private bool ValidarPedido(Guid pedidoId)
    {
        if (pedidoId == Guid.Empty) return false;

        return true;
    }

    private bool ValidarFormaPagamento(Guid formaPagamentoId)
    {
        if (formaPagamentoId == Guid.Empty) return false;

        return true;
    }

    private bool ValidarValor(decimal valor)
    {
        if (valor <= 0) return false;

        return true;
    }
}