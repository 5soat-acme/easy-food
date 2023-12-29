namespace EF.Domain.Commons.Messages.Integrations;

public class PedidoProntoParaRetiradaEvent : IntegrationEvent
{
    public Guid PedidoId { get; set; }
}