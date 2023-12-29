using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Application.DTOs.Responses;

public record FormaPagamentoDto
{
    public Guid Id { get; init; }
    public TipoFormaPagamento TipoFormaPagamento { get; init; }
    public string Descricao { get; init; }
}