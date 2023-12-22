using EF.Identidade.Application.DTOs;
using EF.Identidade.Application.Services.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Identidade;

[Route("api/identidade")]
public class IdentidadeController(IAcessoAppService appService) : CustomControllerBase
{
    /// <summary>
    /// Cria um novo usuário e associao ao cliente.
    /// </summary>
    /// <returns>
    ///   <see cref="RespostaTokenAcesso"/>
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost]
    public async Task<IActionResult> CriarUsuario(NovoUsuario novoUsuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await appService.CriarUsuario(novoUsuario);

        if (!result.IsValid)
        {
            AddErrors(result.Errors);
        }

        return Respond(result.Data);
    }

    /// <summary>
    /// Autentica o usuário no sistema e retorna um token de acesso.
    /// </summary>
    /// <returns>
    ///   <see cref="RespostaTokenAcesso"/>
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("autenticar")]
    public async Task<IActionResult> Login(UsuarioLogin usuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await appService.Autenticar(usuario);
        
        if (!result.IsValid)
        {
            AddErrors(result.Errors);
        }

        return Respond(result.Data);
    }

    /// <summary>
    /// Gera um token de acesso para o usuário sem identificação.
    /// </summary>
    /// <param name="cpf">CPF do usuário (opicional)</param>
    /// <returns>
    ///   <see cref="RespostaTokenAcesso"/>
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("acessar-sem-identificacao")]
    public  IActionResult AcessarSemIdentificacao(string? cpf = null)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = appService.GerarTokenAcessoNaoIdentificado(cpf);

        return Respond(result);
    }
}