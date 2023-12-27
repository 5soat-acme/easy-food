using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Cupons.Application.Commands;

public class CriarCupomCommandHandler : CupomCommandBase,
    IRequestHandler<CriarCupomCommand, CommandResult>
{
    public CriarCupomCommandHandler(ICupomRepository cupomRepository) : base(cupomRepository)
    {
    }

    public async Task<CommandResult> Handle(CriarCupomCommand request, CancellationToken cancellationToken)
    {
        var cupom = GetCupom(request);
        if (!await ValidarOutroCupomVigente(cupom, cancellationToken)) return CommandResult.Create(ValidationResult);
        await _cupomRepository.Criar(cupom, cancellationToken);
        var result = await PersistData(_cupomRepository.UnitOfWork);
        return CommandResult.Create(result, cupom.Id);
    }

    private Cupom GetCupom(CriarCupomCommand command)
    {
        var cupom = new Cupom(command.DataInicio, command.DataFim, command.CodigoCupom, command.PorcentagemDesconto,
            CupomStatus.Ativo);
        foreach (var prod in command.Produtos) cupom.AdicionarProduto(new CupomProduto(cupom.Id, prod.ProdutoId));
        return cupom;
    }
}