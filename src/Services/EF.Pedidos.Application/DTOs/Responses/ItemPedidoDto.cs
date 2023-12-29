namespace EF.Pedidos.Application.DTOs.Responses;

public class ItemPedidoDto
{
    public Guid PedidoId { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal? Desconto { get; set; }
    public decimal ValorFinal { get; set; }
    public int Quantidade { get; set; }
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; }
}