namespace EF.Carrinho.Application.DTOs.Responses;

public class ItemCarrinhoDto
{
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; }
}