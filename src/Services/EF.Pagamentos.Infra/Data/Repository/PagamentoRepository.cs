using EF.Core.Commons.Repository;
using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Pagamentos.Infra.Data.Repository;

public sealed class PagamentoRepository : IPagamentoRepository
{
    private readonly PagamentoDbContext _dbContext;

    public PagamentoRepository(PagamentoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task<Pagamento?> ObterPorId(Guid id)
    {
        return await _dbContext.Pagamentos
            .Include(i => i.Transacoes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pagamento?> ObterPorPedidoId(Guid pedidoId)
    {
        return await _dbContext.Pagamentos
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
    }

    public void Criar(Pagamento pagamento)
    {
        _dbContext.Pagamentos.Add(pagamento);
    }

    public void Atualizar(Pagamento pagamento)
    {
        _dbContext.Pagamentos.Update(pagamento);
    }

    public void AdicionarTransacao(Transacao transacao)
    {
        _dbContext.Transacoes.Add(transacao);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}