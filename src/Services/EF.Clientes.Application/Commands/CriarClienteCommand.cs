using EF.Domain.Commons.Messages;

namespace EF.Clientes.Application.Commands;

public class CriarClienteCommand : Command
{
    public string Cpf { get; set; }
    public string PrimeiroNome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
}