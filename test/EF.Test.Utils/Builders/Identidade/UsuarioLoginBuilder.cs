using Bogus;
using EF.Identidade.Application.DTOs.Requests;

namespace EF.Test.Utils.Builders.Identidade;

public class UsuarioLoginBuilder : Faker<UsuarioLogin>
{
    public UsuarioLoginBuilder()
    {
        Locale = "pt_BR";

        CustomInstantiator(f => new UsuarioLogin());
        RuleFor(a => a.Email, f => f.Person.Email);
        RuleFor(a => a.Senha, f => "Teste@1234");
    }
}