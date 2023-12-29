namespace EF.Domain.Commons.Messages.Integrations;

public class PedidoRecebidoEvent : IntegrationEvent
{
    public Guid PedidoId { get; set; }
}