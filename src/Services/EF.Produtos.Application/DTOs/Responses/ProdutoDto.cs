namespace EF.Produtos.Application.DTOs.Responses;

public class ProdutoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal ValorUnitario { get; set; }
    public int TempoPreparoEstimado { get; set; }
}