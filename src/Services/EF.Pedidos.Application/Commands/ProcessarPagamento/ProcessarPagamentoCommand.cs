using System.Text.Json.Serialization;
using EF.Domain.Commons.Messages;

namespace EF.Pedidos.Application.Commands.ProcessarPagamento;

public class ProcessarPagamentoCommand : Command
{
    [JsonIgnore] public Guid SessionId { get; set; }
    [JsonIgnore] public Guid? ClienteId { get; set; }
    [JsonIgnore] public string? ClienteCpf { get; set; }
    public Guid PedidoId { get; set; }
    public string TipoPagamento { get; set; }
    public decimal ValorTotal { get; set; }
}