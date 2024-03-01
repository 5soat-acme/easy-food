using EF.Identidade.Domain.Models;

namespace EF.Identidade.Domain.Services;

public interface IUsuarioService
{
    Task<List<string>> CriarUsuario(Usuario usuario);
    Task ExcluirUsuario(Usuario usuario);
    Task<Usuario?> ObterUsuarioPorEmail(string email);
    Task<Usuario?> ObterUsuarioPorCpf(string cpf);
}