namespace EF.Carrinho.Application.DTOs.Integrations;

public class ProdutoDto
{
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }
    public decimal ValorUnitario { get; set; }
    public int TempoEstimadoPreparo { get; set; }
}