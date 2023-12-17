using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands
{
    public class AtualizarCupomCommandHandler : CommandHandler,
        IRequestHandler<AtualizarCupomCommand, CommandResult>
    {
        private readonly ICupomRepository _cupomRepository;

        public AtualizarCupomCommandHandler(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public async Task<CommandResult> Handle(AtualizarCupomCommand request, CancellationToken cancellationToken)
        {
            var cupom = await GetCupom(request, cancellationToken);
            if (!await ValidarCupom(cupom, cancellationToken)) return CommandResult.Create(ValidationResult);
            _cupomRepository.Atualizar(cupom!, cancellationToken);
            var result = await PersistData(_cupomRepository.UnitOfWork);
            return CommandResult.Create(result);
        }

        private async Task<Cupom?> GetCupom(AtualizarCupomCommand command, CancellationToken cancellationToken)
        {
            var cupomExistente = await _cupomRepository.Buscar(command.CupomId, cancellationToken);
            if (cupomExistente is not null)
            {
                cupomExistente.AlterarDatas(command.DataInicio, command.DataFim);
                cupomExistente.AlterarCodigoCupom(command.CodigoCupom);
                cupomExistente.AlterarPorcentagemDesconto(command.PorcentagemDesconto);
                return cupomExistente;
            }
            return null;
        }

        private async Task<bool> ValidarCupom(Cupom? cupom, CancellationToken cancellationToken)
        {
            if (cupom is null)
            {
                AddError("Cupom não encontrado");
                return false;
            }
            else
            {
                var cupomExistente = await _cupomRepository.BuscarCupomVigenteEmPeriodo(cupom.Id, cupom.CodigoCupom, cupom.DataInicio, cupom.DataFim, cancellationToken);
                if (cupomExistente.Count > 0)
                {
                    AddError("Já existe cupom com este código em vigência para o mesmo período");
                    return false;
                }
            }
            return true;
        }
    }
}
