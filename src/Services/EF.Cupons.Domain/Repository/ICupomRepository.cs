using EF.Cupons.Domain.Models;
using EF.Domain.Commons.Repository;

namespace EF.Cupons.Domain.Repository;

public interface ICupomRepository : IRepository<Cupom>
{
    Task<Cupom?> Buscar(Guid cupomId, CancellationToken cancellationToken);
    Task<CupomProduto?> BuscarCupomProduto(Guid cupomId, Guid produtoId, CancellationToken cancellationToken);
    Task<Cupom?> BuscarCupomVigente(string codigoCupom, CancellationToken cancellationToken);

    Task<IList<Cupom>> BuscarCupomVigenteEmPeriodo(string codigoCupom, DateTime dataInicio, DateTime dataFim,
        CancellationToken cancellationToken);

    Task<Cupom> Criar(Cupom cupom, CancellationToken cancellationToken);
    Cupom Atualizar(Cupom cupom, CancellationToken cancellationToken);
    Task InserirProdutos(IList<CupomProduto> produtos, CancellationToken cancellationToken);
    void RemoverProdutos(IList<CupomProduto> produtos, CancellationToken cancellationToken);
}