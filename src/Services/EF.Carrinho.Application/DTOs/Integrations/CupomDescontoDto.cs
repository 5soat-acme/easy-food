namespace EF.Carrinho.Application.DTOs.Integrations;

public class CupomDescontoDto
{
    public List<Guid> Ids { get; set; }
    public decimal Desconto { get; set; }
}