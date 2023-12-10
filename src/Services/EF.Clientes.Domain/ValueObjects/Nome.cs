using EF.Domain.Commons.DomainObjects;

namespace EF.Clientes.Domain.ValueObjects;

public record Nome
{
    public Nome(string primeiroNome, string sobrenome)
    {
        if (!Validar(primeiroNome, sobrenome)) throw new DomainException("Nome inválido");

        PrimeiroNome = primeiroNome;
        Sobrenome = sobrenome;
    }

    protected Nome()
    {
    }

    public string PrimeiroNome { get; init; }
    public string Sobrenome { get; init; }

    public bool Validar(string primeiroNome, string sobrenome)
    {
        if (string.IsNullOrEmpty(primeiroNome) || string.IsNullOrEmpty(sobrenome)) return false;
        return true;
    }
}