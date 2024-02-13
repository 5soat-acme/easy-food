using EF.Clientes.Domain.ValueObjects;
using EF.Core.Commons.DomainObjects;
using EF.Core.Commons.ValueObjects;

namespace EF.Clientes.Domain.Models;

public class Cliente : Entity, IAggregateRoot
{
    public Cliente(string cpf, string primeiroNome, string sobrenome, string email)
    {
        Cpf = new Cpf(cpf);
        Nome = new Nome(primeiroNome, sobrenome);
        Email = new Email(email);
    }

    protected Cliente()
    {
    }

    public Cpf Cpf { get; private set; }
    public Nome Nome { get; private set; }
    public Email Email { get; private set; }
}