using EF.Pedidos.Domain.Models;

namespace EF.Pedidos.Application.DTOs;

public class PedidoDto
{
    public Guid Id { get; set; }
    public Guid CorrelacaoId { get; set; }
    public Status Status { get; set; }
    public Guid? ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public IEnumerable<ItemPedidoDto> Itens { get; set; }
}