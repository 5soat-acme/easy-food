namespace EF.Carrinho.Application.DTOs;

public class ResumoItemDto
{
    public Guid Id { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorDesconto { get; set; }
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; }
}