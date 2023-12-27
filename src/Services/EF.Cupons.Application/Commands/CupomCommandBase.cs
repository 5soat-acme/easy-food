using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Repository;
using EF.Domain.Commons.Messages;

namespace EF.Cupons.Application.Commands;

public class CupomCommandBase : CommandHandler
{
    protected readonly ICupomRepository _cupomRepository;

    public CupomCommandBase(ICupomRepository cupomRepository)
    {
        _cupomRepository = cupomRepository;
    }

    protected async Task<bool> ValidarCupom(Guid cupomId, CancellationToken cancellationToken,
        bool validarVigencia = true)
    {
        var cupom = await _cupomRepository.Buscar(cupomId, cancellationToken);
        if (cupom is null)
        {
            AddError("Cupom não encontrado");
            return false;
        }

        if (validarVigencia && cupom.DataInicio <= DateTime.Now.Date)
        {
            AddError("Não é possível alterar um cupom em vigência");
            return false;
        }

        return true;
    }

    protected async Task<bool> ValidarOutroCupomVigente(Cupom cupom, CancellationToken cancellationToken)
    {
        var listaCupom = await _cupomRepository.BuscarCupomVigenteEmPeriodo(cupom.CodigoCupom, cupom.DataInicio,
            cupom.DataFim, cancellationToken);
        listaCupom = listaCupom.Where(x => x.Id != cupom.Id).ToList();

        if (listaCupom.Count > 0)
        {
            AddError("Já existe cupom com este código em vigência para o mesmo período");
            return false;
        }

        return true;
    }
}