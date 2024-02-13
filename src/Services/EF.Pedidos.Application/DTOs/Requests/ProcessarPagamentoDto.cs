using System.Text.Json.Serialization;

namespace EF.Pedidos.Application.DTOs.Requests;

public class ProcessarPagamentoDto
{
    [JsonIgnore] public Guid SessionId { get; set; }
    [JsonIgnore] public Guid? ClienteId { get; set; }
    [JsonIgnore] public string? ClienteCpf { get; set; }
    public Guid PedidoId { get; set; }
    public string TipoPagamento { get; set; }
    public decimal ValorTotal { get; set; }
}