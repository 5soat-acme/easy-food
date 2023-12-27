namespace EF.Carrinho.Application.DTOs;

public class CarrinhoClienteDto
{
    public Guid Id { get; set; }
    public Guid? ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorFinal { get; set; }
    public int EstimativaTempoPreparoMin { get; set; }
    public IEnumerable<ItemCarrinhoDto> Itens { get; set; }
}