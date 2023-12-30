using EF.Domain.Commons.Messages;

namespace EF.PreparoEntrega.Application.Commands;

public class IncluirPedidoFilaPreparoCommand : Command
{
    public Guid CorrelacaoId { get; set; }
    public List<ItemPedido> Itens { get; set; }

    public class ItemPedido
    {
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int TempoPreparoEstimado { get; set; }
    }
}