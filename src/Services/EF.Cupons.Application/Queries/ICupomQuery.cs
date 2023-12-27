using EF.Cupons.Application.DTOs;

namespace EF.Cupons.Application.Queries;

public interface ICupomQuery
{
    public Task<CupomDto?> ObterCupom(string codigoCupom, CancellationToken cancellationToken);
}