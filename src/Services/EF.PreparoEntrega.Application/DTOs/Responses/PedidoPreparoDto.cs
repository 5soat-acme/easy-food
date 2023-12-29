using EF.PreparoEntrega.Domain.Models;

namespace EF.PreparoEntrega.Application.DTOs.Responses;

public class PedidoPreparoDto
{
    public Guid PedidoId { get; set; }
    public string Codigo { get; set; }
    public DateTime DataCriacao { get; set; }
    public Status Status { get; set; }
    public List<ItemPreparoDto> Itens { get; set; }
}