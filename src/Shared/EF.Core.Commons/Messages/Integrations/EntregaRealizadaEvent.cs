namespace EF.Core.Commons.Messages.Integrations;

public class EntregaRealizadaEvent : IntegrationEvent
{
    public Guid PedidoCorrelacaoId { get; set; }
}