namespace EF.Domain.Commons.Messages.Integrations;

public class PedidoCriadoEvent : IntegrationEvent
{
    public Guid PedidoId { get; set; }
}