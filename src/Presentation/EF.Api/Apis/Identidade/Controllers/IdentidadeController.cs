using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using EF.Identidade.Application.Services.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Identidade.Controllers;

[Route("api/identidade")]
public class IdentidadeController(IAcessoAppService appService) : CustomControllerBase
{
    /// <summary>
    ///     Cria um novo usuário e associar ao cliente.
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

        var result = await appService.CriarUsuario(novoUsuario);

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond(result.Data);
    }

    /// <summary>
    ///     Gera token de acesso para o cliente utilizar o sistema. O cliente pode optar se identificar por e-mail, CPF ou não
    ///     se identificar.
    /// </summary>
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

        var result = await appService.Identificar(usuario);

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond(result.Data);
    }
}