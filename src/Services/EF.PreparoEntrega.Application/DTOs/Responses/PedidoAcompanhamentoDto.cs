using EF.PreparoEntrega.Domain.Models;

namespace EF.PreparoEntrega.Application.DTOs.Responses;

public class PedidoAcompanhamentoDto
{
    public Guid PedidoId { get; set; }
    public string Codigo { get; set; }
    public Status Status { get; set; }
}