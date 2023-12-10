using EF.Clientes.Domain.Models;
using EF.Clientes.Domain.Repository;
using EF.Domain.Commons.Repository;

namespace EF.Clientes.Infra.Data.Repository;

public class ClienteRepository : IClienteRepository
{
    private readonly ClienteDbContext _dbContext;

    public ClienteRepository(ClienteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task<Cliente> Criar(Cliente cliente)
    {
        var result = await _dbContext.Clientes.AddAsync(cliente);
        return result.Entity;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}