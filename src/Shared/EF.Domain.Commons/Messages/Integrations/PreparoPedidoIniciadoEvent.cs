namespace EF.Domain.Commons.Messages.Integrations;

public class PreparoPedidoIniciadoEvent : IntegrationEvent
{
    public Guid PedidoCorrelacaoId { get; set; }
}