using EF.Domain.Commons.Messages;
using EF.Domain.Commons.ValueObjects;

namespace EF.Pedidos.Application.Commands.Recebimento;

public class ReceberPedidoCommand : Command
{
    public Guid CorrelacaoId { get; set; }
    public Guid? ClienteId { get; set; }
    public Cpf? Cpf { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorFinal { get; set; }
    public List<ItemPedido> Itens { get; set; }

    public class ItemPedido
    {
        public decimal ValorUnitario { get; set; }
        public decimal? Desconto { get; set; }
        public decimal ValorFinal { get; set; }
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int TempoPreparoEstimado { get; set; }
    }
}