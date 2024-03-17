namespace EF.Core.Commons.Messages.Integrations;

public class PreparoPedidoFinalizadoEvent : IntegrationEvent
{
    public Guid PedidoCorrelacaoId { get; set; }
}