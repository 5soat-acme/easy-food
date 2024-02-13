using EF.Core.Commons.Repository;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Repository;

namespace EF.Pagamentos.Infra.Data.Repository;

public sealed class PagamentoRepository : IPagamentoRepository
{
    private readonly PagamentoDbContext _dbContext;

    public PagamentoRepository(PagamentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task<Pagamento> Criar(Pagamento pagamento)
    {
        await _dbContext.Pagamentos.AddAsync(pagamento);
        return pagamento;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}