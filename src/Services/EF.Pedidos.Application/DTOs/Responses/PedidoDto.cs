using EF.Domain.Commons.ValueObjects;
using EF.Pedidos.Domain.Models;

namespace EF.Pedidos.Application.DTOs.Responses;

public class PedidoDto
{
    public Guid CorrelacaoId { get; set; }
    public string Codigo { get; set; }
    public Guid? ClienteId { get; set; }
    public Cpf? Cpf { get; set; }
    public Status Status { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAtualizacao { get; set; }
    public IEnumerable<ItemPedidoDto> Itens { get; set; }
}