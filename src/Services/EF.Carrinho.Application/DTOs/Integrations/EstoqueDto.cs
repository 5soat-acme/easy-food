namespace EF.Carrinho.Application.DTOs.Integrations;

public class EstoqueDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}