namespace EF.Pedidos.Application.DTOs.Adapters;

public class CupomDto
{
    public Guid Id { get; set; }
    public decimal Desconto { get; set; }
    
    public List<Guid> Produtos { get; set; }
}