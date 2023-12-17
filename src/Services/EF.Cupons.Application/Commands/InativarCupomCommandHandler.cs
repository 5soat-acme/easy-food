using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands
{
    public class InativarCupomCommandHandler : CommandHandler,
        IRequestHandler<InativarCupomCommand, CommandResult>
    {
        private readonly ICupomRepository _cupomRepository;

        public InativarCupomCommandHandler(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public async Task<CommandResult> Handle(InativarCupomCommand request, CancellationToken cancellationToken)
        {
            var cupom = await GetCupom(request, cancellationToken);
            if (!ValidarCupom(cupom, cancellationToken)) return CommandResult.Create(ValidationResult);
            _cupomRepository.Atualizar(cupom!, cancellationToken);
            var result = await PersistData(_cupomRepository.UnitOfWork);
            return CommandResult.Create(result);
        }

        private async Task<Cupom?> GetCupom(InativarCupomCommand command, CancellationToken cancellationToken)
        {
            var cupomExistente = await _cupomRepository.Buscar(command.CupomId, cancellationToken);
            if (cupomExistente is not null)
            {
                cupomExistente.InativarCupom();
                return cupomExistente;
            }
            return null;
        }

        private bool ValidarCupom(Cupom? cupom, CancellationToken cancellationToken)
        {
            if (cupom is null)
            {
                AddError("Cupom não encontrado");
                return false;
            }
            return true;
        }
    }
}
