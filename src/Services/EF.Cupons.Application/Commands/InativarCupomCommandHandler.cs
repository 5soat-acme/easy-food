using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands;

public class InativarCupomCommandHandler : CupomCommandBase,
    IRequestHandler<InativarCupomCommand, CommandResult>
{
    public InativarCupomCommandHandler(ICupomRepository cupomRepository) : base(cupomRepository)
    {
    }

    public async Task<CommandResult> Handle(InativarCupomCommand request, CancellationToken cancellationToken)
    {
        if (!await ValidarCupom(request.CupomId, cancellationToken, false))
            return CommandResult.Create(ValidationResult);

        var cupom = await GetCupom(request, cancellationToken);
        _cupomRepository.Atualizar(cupom!, cancellationToken);
        var result = await PersistData(_cupomRepository.UnitOfWork);
        return CommandResult.Create(result);
    }

    private async Task<Cupom> GetCupom(InativarCupomCommand command, CancellationToken cancellationToken)
    {
        var cupomExistente = await _cupomRepository.Buscar(command.CupomId, cancellationToken);
        cupomExistente!.InativarCupom();
        return cupomExistente;
    }
}