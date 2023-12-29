using EF.Domain.Commons.Repository;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Pagamentos.Infra.Data.Repository;

public sealed class FormaPagamentoRepository : IFormaPagamentoRepository
{
    private readonly PagamentoDbContext _dbContext;

    public FormaPagamentoRepository(PagamentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task<IList<FormaPagamento>> BuscarTodas(CancellationToken cancellationToken)
    {
        return await _dbContext.FormasPagamento
            .ToListAsync(cancellationToken);
    }

    public async Task<FormaPagamento?> BuscarPorTipo(TipoFormaPagamento tipoFormaPagamento, CancellationToken cancellationToken)
    {
        return await _dbContext.FormasPagamento
            .Where(x => x.TipoFormaPagamento == tipoFormaPagamento)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}