namespace EF.Core.Commons.Messages.Integrations;

public class PagamentoProcessadoEvent : IntegrationEvent
{
    public Guid SessionId { get; set; }
    public Guid? ClientId { get; set; }
    public List<ItemPedido> Itens { get; set; }

    public class ItemPedido
    {
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int TempoPreparoEstimado { get; set; }
    }
}