namespace EF.Domain.Commons.Messages.Integrations;

public class PreparoPedidoFinalizadoEvent : IntegrationEvent
{
    public Guid PedidoCorrelacaoId { get; set; }
}