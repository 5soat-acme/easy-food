namespace EF.Domain.Commons.Messages.Integrations;

public class PreparoPedidoIniciadoEvent : IntegrationEvent
{
    public Guid CorrelacaoId { get; set; }
}