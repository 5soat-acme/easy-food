namespace EF.Carrinho.Application.DTOs.Responses;

public class ResumoCarrinhoDto
{
    public Guid Id { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorFinal { get; set; }
    public int EstimativaTempoPreparoMin { get; set; }
    public IEnumerable<ResumoItemDto> Itens { get; set; }
}