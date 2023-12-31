namespace EF.Domain.Commons.Messages.Integrations;

public class EntregaRealizadaEvent : IntegrationEvent
{
    public Guid CorrelacaoId { get; set; }
}