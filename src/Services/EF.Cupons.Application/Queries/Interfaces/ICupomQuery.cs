using EF.Cupons.Application.DTOs.Responses;

namespace EF.Cupons.Application.Queries.Interfaces;

public interface ICupomQuery
{
    public Task<CupomDto?> ObterCupom(string codigoCupom, CancellationToken cancellationToken);
}