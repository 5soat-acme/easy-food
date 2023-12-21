using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.DTOs;

public class CarrinhoClienteDto
{
    public Guid Id { get; set; }
    public Guid? ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public IEnumerable<ItemDto> Itens { get; set; }
}