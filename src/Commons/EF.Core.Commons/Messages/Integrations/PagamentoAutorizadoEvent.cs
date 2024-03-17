namespace EF.Core.Commons.Messages.Integrations;

public class PagamentoAutorizadoEvent : IntegrationEvent
{
    public Guid PedidoId { get; set; }
}