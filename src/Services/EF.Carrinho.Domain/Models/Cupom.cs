namespace EF.Carrinho.Domain.Models;

public record Cupom
{
    public string Codigo { get; init; }
    public decimal Desconto { get; init; }
}