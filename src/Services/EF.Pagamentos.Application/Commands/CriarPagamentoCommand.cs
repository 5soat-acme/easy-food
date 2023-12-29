using EF.Domain.Commons.Messages;
using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Application.Commands;

public class CriarPagamentoCommand : Command
{
    public Guid PedidoId { get; set; }
    public TipoFormaPagamento TipoFormaPagamento { get; set; }
    public decimal Valor { get; set; }
}