namespace EF.Clientes.Application.DTOs;

public class CriarClienteDto
{
    public string Cpf { get; set; }
    public string PrimeiroNome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
}