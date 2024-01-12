using EF.Domain.Commons.DomainObjects;

namespace EF.Pagamentos.Domain.Models;

public class Transacao : Entity
{
    public Transacao(Guid pagamentoId)
    {
        PagamentoId = pagamentoId;
        Data = DateTime.Now.ToUniversalTime();
    }

    public Guid PagamentoId { get; set; }
    public DateTime Data { get; set; }
}