namespace EF.Pedidos.Application.DTOs.Adapters;

public class PagamentoDto
{
    public Guid PedidoId { get; set; }
    public string? Cpf { get; set; }
    public string TipoPagamento { get; set; }
    public decimal Valor { get; set; }
}