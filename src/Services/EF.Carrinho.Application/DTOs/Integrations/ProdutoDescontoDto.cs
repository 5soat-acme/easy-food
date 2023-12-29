namespace EF.Carrinho.Application.DTOs.Integrations;

public class ProdutoDescontoDto
{
    public List<Guid> Ids { get; set; }
    public decimal Desconto { get; set; }
}