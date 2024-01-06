using EF.PreparoEntrega.Domain.Models;

namespace EF.PreparoEntrega.Application.DTOs.Responses;

public class PedidoMonitorDto
{
    public Guid PedidoId { get; set; }
    public string Codigo { get; set; }
    public Status Status { get; set; }
}