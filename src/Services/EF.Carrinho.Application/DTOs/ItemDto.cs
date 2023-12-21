namespace EF.Carrinho.Application.DTOs;

public class ItemDto
{
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; }
}