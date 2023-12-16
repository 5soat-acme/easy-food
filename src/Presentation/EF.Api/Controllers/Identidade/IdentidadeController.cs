using EF.Identidade.Application.DTOs;
using EF.Identidade.Application.Services.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Identidade;

[Route("api/identidade")]
public class IdentidadeController(IAcessoAppService appService) : CustomControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CriarUsuario(NovoUsuario novoUsuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await appService.CriarUsuario(novoUsuario);

        if (!result.IsValid)
        {
            result.Errors.ForEach(AddError);
            return Respond(ModelState);
        }

        return Respond(result.Data);
    }

    [HttpPost("autenticar")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Login(UsuarioLogin usuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await appService.Autenticar(usuario);

        if (!result.IsValid)
        {
            result.Errors.ForEach(AddError);
            return Respond(ModelState);
        }

        return Respond(result.Data);
    }

    [HttpPost("acessar-sem-identificacao")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> AcessarSemIdentificacao(string? cpf = null)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = appService.GerarTokenAcessoNaoIdentificado(cpf);

        return Respond(result);
    }
}