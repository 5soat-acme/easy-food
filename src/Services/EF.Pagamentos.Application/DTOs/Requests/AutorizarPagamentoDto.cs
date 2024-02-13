namespace EF.Pagamentos.Application.DTOs.Requests;

public class AutorizarPagamentoDto
{
    public Guid PedidoId { get; set; }
    public string? Cpf { get; set; }
    public string TipoPagamento { get; set; }
    public decimal Valor { get; set; }
}