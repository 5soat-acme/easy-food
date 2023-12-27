namespace EF.Estoques.Application.DTOs;

public record EstoqueDto
{
    public Guid ProdutoId { get; init; }
    public int Quantidade { get; init; }
}