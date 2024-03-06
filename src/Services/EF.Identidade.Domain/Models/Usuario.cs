using EF.Core.Commons.DomainObjects;
using EF.Core.Commons.ValueObjects;

namespace EF.Identidade.Domain.Models;

public class Usuario : Entity, IAggregateRoot
{
    public Usuario(Cpf cpf, Email email)
    {
        Cpf = cpf;
        Email = email;
    }

    public Usuario(Guid id, Cpf cpf, Email email)
    {
        Id = id;
        Cpf = cpf;
        Email = email;
    }

    public Cpf Cpf { get; private set; }
    public Email Email { get; private set; }
}