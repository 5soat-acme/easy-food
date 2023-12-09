namespace EF.Identidade.Application.DTOs;

public class UsuarioToken
{
    public string? Id { get; set; }
    public IEnumerable<UsuarioClaim> Claims { get; set; }
}