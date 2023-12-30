namespace EF.Pedidos.Application.DTOs.Integrations;

public class CupomDescontoDto
{
    public Guid Id { get; set; }
    public List<Guid> Produtos { get; set; }
    public decimal Desconto { get; set; }
}