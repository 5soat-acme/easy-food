namespace EF.Domain.Commons.Messages.Integrations;

public class PedidoCanceladoEvent : IntegrationEvent
{
    public Guid PedidoId { get; set; }
}