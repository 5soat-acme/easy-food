namespace EF.Carrinho.Application.DTOs.Responses;

public class CarrinhoClienteDto
{
    public Guid Id { get; set; }
    public Guid? ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public IEnumerable<ItemCarrinhoDto> Itens { get; set; }
}