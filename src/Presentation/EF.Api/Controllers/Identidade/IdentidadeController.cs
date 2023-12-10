using EF.Identidade.Application.DTOs;
using EF.Identidade.Application.Services;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Identidade;

[Route("api/identidade")]
public class IdentidadeController(IAcessoAppService appService) : CustomControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarUsuario(NovoUsuario novoUsuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await appService.CriarUsuario(novoUsuario);

        if (!result.ValidationResult.IsValid)
            return Respond(result.ValidationResult);

        return Respond(result.Token);
    }

    [HttpPost("autenticar")]
    public async Task<IActionResult> Login(UsuarioLogin usuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await appService.Autenticar(usuario);

        if (!result.ValidationResult.IsValid)
            return Respond(result.ValidationResult);

        return Respond(result.Token);
    }

    [HttpPost("acessar-sem-identificacao")]
    public async Task<IActionResult> AcessarSemIdentificacao(string? cpf = null)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = appService.GerarTokenAcessoNaoIdentificado(cpf);

        return Respond(result);
    }
}