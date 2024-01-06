namespace EF.Pedidos.Application.DTOs.Integrations;

public class EstoqueProdutoDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}