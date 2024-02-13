using EF.Clientes.Domain.Models;
using EF.Core.Commons.Repository;

namespace EF.Clientes.Domain.Repository;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente> Criar(Cliente cliente);
}