using EF.Domain.Commons.Messages;

namespace EF.Cupons.Application.Commands
{
    public class AtualizarCupomCommand : Command
    {
        public Guid CupomId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string CodigoCupom { get; set; }
        public decimal PorcentagemDesconto { get; set; }
    }
}
