using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using EF.Identidade.Application.DTOs.Requests;

namespace EF.Test.Utils.Builders.Identidade;

public class NovoUsuarioBuilder : Faker<NovoUsuario>
{
    public NovoUsuarioBuilder()
    {
        Locale = "pt_BR";

        CustomInstantiator(f => new NovoUsuario());
        RuleFor(a => a.Nome, f => f.Person.FirstName);
        RuleFor(a => a.Sobrenome, f => f.Person.LastName);
        RuleFor(a => a.Cpf, f => f.Person.Cpf());
        RuleFor(a => a.Email, f => f.Person.Email);
        RuleFor(a => a.Senha, f => "Teste@1234");
        RuleFor(a => a.SenhaConfirmacao, (f, u) => u.Senha);
    }

    public NovoUsuarioBuilder Cpf(string? cpf)
    {
        RuleFor(a => a.Cpf, () => cpf);
        return this;
    }

    public NovoUsuarioBuilder Email(string? email)
    {
        RuleFor(a => a.Email, () => email);
        return this;
    }

    public NovoUsuarioBuilder Senha(string? senha)
    {
        RuleFor(a => a.Senha, () => senha);
        return this;
    }

    public NovoUsuarioBuilder SenhaConfirmacao(string? senhaConfirmacao)
    {
        RuleFor(a => a.SenhaConfirmacao, () => senhaConfirmacao);
        return this;
    }
}