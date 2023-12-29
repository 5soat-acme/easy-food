namespace EF.Domain.Commons.Messages.Integrations;

public class PedidoFinalizadoEvent : IntegrationEvent
{
    public Guid PedidoId { get; set; }
}