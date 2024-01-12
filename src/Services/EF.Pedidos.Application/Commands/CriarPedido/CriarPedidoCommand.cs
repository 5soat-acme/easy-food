using System.Text.Json.Serialization;
using EF.Domain.Commons.Messages;

namespace EF.Pedidos.Application.Commands.CriarPedido;

public class CriarPedidoCommand : Command
{
    [JsonIgnore] public Guid SessionId { get; set; }
    [JsonIgnore] public Guid? ClienteId { get; set; }
    [JsonIgnore] public string? ClienteCpf { get; set; }
    public string MetodoPagamento { get; set; }
    public decimal ValorTotal { get; set; }
    public string? CodigoCupom { get; set; }
    public List<ItemPedido> Itens { get; set; }

    public class ItemPedido
    {
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
    }
}