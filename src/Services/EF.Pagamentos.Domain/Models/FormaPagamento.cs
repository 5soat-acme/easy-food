using EF.Domain.Commons.DomainObjects;

namespace EF.Pagamentos.Domain.Models;

public class FormaPagamento : Entity, IAggregateRoot
{
    private FormaPagamento() { }

    public FormaPagamento(TipoFormaPagamento tipoFormaPagamento, string descricao)
    {
        if (!ValidarTipoFormaPagamento(tipoFormaPagamento)) throw new DomainException("TipoFormaPagamento inválido");

        TipoFormaPagamento = tipoFormaPagamento;
        Descricao = descricao;
    }

    public TipoFormaPagamento TipoFormaPagamento { get; private set; }
    public string Descricao { get; private set; }

    private bool ValidarTipoFormaPagamento(TipoFormaPagamento tipoFormaPagamento)
    {
        if (!Enum.IsDefined(typeof(TipoFormaPagamento), tipoFormaPagamento)) return false;

        return true;
    }
}