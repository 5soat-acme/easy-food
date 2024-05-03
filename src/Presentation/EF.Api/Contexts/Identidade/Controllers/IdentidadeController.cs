using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using EF.Identidade.Application.UseCases.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Contexts.Identidade.Controllers;

[Route("api/identidade")]
public class IdentidadeController(IIdentidadeUseCase useCase) : CustomControllerBase
{
    /*
    /// <summary>
    ///     Cria um novo usuário e associa ao cliente.
    /// </summary>
    /// <response code="200">Usuário criado com sucesso e token de acesso</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> RegistrarUsuario(NovoUsuario novoUsuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await useCase.CriarUsuario(novoUsuario);

        if (!result.IsValid) AddErrors(result.GetErrorMessages());

        return Respond(result.Data);
    }

    /// <summary>
    ///     Gera token de acesso para o cliente utilizar o sistema. O cliente pode optar se identificar por e-mail, CPF ou não
    ///     se identificar.
    /// </summary>
    /// <remarks>
    ///     Este método realiza a autenticação do usuário e gera um token JWT (JSON Web Token) que deve ser usado em
    ///     cabeçalhos de autenticação para futuras requisições. O token tem validade de 2 hora e pode ser configurado no
    ///     appsettings.json.
    ///     É importante garantir que o token seja armazenado de maneira segura no cliente para evitar vazamento de
    ///     informações.
    ///     Para usuários sem ideintificação por e-mail ou CPF, o body da requisição deve ser vazio. Para as demais maneiras,
    ///     segue o exemplo:
    ///     Identificação por E-mail:
    ///     <code>
    ///         POST /api/identidade/acessar
    ///         {
    ///             "Email": "exemplo@email.com"
    ///         }
    ///     </code>
    ///     Identificação por CPF:
    ///     <code>
    ///         POST /api/identidade/acessar
    ///         {
    ///             "Cpf": "01234567891"
    ///         }
    ///     </code>
    /// </remarks>
    /// <param name="usuario">O cliente pode optar se identificar por e-mail, CPF ou não se identificar</param>
    /// <response code="200">Acessao realizado com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost("acessar")]
    public async Task<IActionResult> Acessar(UsuarioAcesso? usuario = null)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await useCase.AcessarSistema(usuario);

        if (!result.IsValid) AddErrors(result.GetErrorMessages());

        return Respond(result.Data);
    }
    */

    /// <summary>
    ///     Gera token de acesso para o cliente utilizar o sistema sem cadastro.
    /// </summary>
    /// <remarks>
    ///     Este método gera um token JWT (JSON Web Token) que deve ser usado em cabeçalhos de autenticação para futuras requisições.
    /// </remarks>
    /// <response code="200">Acessao realizado com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpGet("acessar")]
    public async Task<IActionResult> Acessar()
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await useCase.AcessarSistema(null);

        if (!result.IsValid) AddErrors(result.GetErrorMessages());

        return Respond(result.Data);
    }
}