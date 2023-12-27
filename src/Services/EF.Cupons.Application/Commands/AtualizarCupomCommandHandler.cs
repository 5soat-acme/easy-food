using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands;

public class AtualizarCupomCommandHandler : CupomCommandBase,
    IRequestHandler<AtualizarCupomCommand, CommandResult>
{
    public AtualizarCupomCommandHandler(ICupomRepository cupomRepository) : base(cupomRepository)
    {
    }

    public async Task<CommandResult> Handle(AtualizarCupomCommand request, CancellationToken cancellationToken)
    {
        if (!await ValidarCupom(request.CupomId, cancellationToken)) return CommandResult.Create(ValidationResult);

        var cupom = await GetCupom(request, cancellationToken);
        if (!await ValidarOutroCupomVigente(cupom!, cancellationToken)) return CommandResult.Create(ValidationResult);

        _cupomRepository.Atualizar(cupom!, cancellationToken);
        var result = await PersistData(_cupomRepository.UnitOfWork);
        return CommandResult.Create(result);
    }

    private async Task<Cupom?> GetCupom(AtualizarCupomCommand command, CancellationToken cancellationToken)
    {
        var cupomExistente = await _cupomRepository.Buscar(command.CupomId, cancellationToken);
        cupomExistente!.AlterarCodigoCupom(command.CodigoCupom);
        cupomExistente.AlterarPorcentagemDesconto(command.PorcentagemDesconto);
        cupomExistente.AlterarDatas(command.DataInicio, command.DataFim);
        return cupomExistente;
    }
}