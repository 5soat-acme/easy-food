using EF.Cupons.Application.DTOs;
using EF.Cupons.Domain.Repository;

namespace EF.Cupons.Application.Queries;

public class CupomQuery : ICupomQuery
{
    private readonly ICupomRepository _cupomRepository;

    public CupomQuery(ICupomRepository cupomRepository)
    {
        _cupomRepository = cupomRepository;
    }

    public async Task<CupomDto?> ObterCupom(string codigoCupom, CancellationToken cancellationToken)
    {
        var cupom = await _cupomRepository.BuscarCupomVigente(codigoCupom, cancellationToken);
        var dto = cupom is null
            ? null
            : new CupomDto
            {
                Id = cupom.Id,
                DataInicio = cupom.DataInicio,
                DataFim = cupom.DataFim,
                PorcentagemDesconto = cupom.PorcentagemDesconto,
                Produtos = cupom.CupomProdutos.Select(x => new CupomProdutoDto { ProdutoId = x.ProdutoId }).ToList()
            };

        return dto;
    }
}