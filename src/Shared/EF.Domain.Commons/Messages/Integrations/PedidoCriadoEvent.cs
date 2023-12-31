namespace EF.Domain.Commons.Messages.Integrations;

public class PedidoCriadoEvent : IntegrationEvent
{
    public Guid SessionId { get; set; }
    public Guid? ClientId { get; set; }
    public List<ItemPedido> Itens { get; set; }

    public class ItemPedido
    {
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
    }
}