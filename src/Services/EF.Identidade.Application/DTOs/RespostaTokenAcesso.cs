namespace EF.Identidade.Application.DTOs;

public class RespostaTokenAcesso
{
    public string Token { get; set; }
    public double ExpiresIn { get; set; }
    public UsuarioToken User { get; set; }
}