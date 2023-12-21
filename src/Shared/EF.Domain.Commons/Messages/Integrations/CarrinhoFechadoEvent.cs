namespace EF.Domain.Commons.Messages.Integrations.CarrinhoIntegracao;

public class CarrinhoFechadoEvent : IntegrationEvent
{
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorFinal { get; set; }
    public List<ItemCarrinhoFechado> Itens { get; set; }

    public class ItemCarrinhoFechado
    {
        public decimal ValorUnitario { get; set; }
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
    }
}