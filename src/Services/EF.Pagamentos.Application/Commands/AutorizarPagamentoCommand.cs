using EF.Domain.Commons.Messages;

namespace EF.Pagamentos.Application.Commands;

public class AutorizarPagamentoCommand : Command
{
    public Guid PedidoId { get; set; }
    public string? Cpf { get; set; }
    public string TipoPagamento { get; set; }
    public decimal Valor { get; set; }
}