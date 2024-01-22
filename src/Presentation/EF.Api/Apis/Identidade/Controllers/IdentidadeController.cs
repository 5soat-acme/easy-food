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
    ///     Faz a identificação de um usuário previamente registrado.
    /// </summary>
    /// <response code="200">Usuário autenticado com sucesso e token de acesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost("identificar")]
    public async Task<IActionResult> Login(UsuarioLogin usuario)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await appService.IdentificarUsuarioRegistrado(usuario);

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond(result.Data);
    }

    /// <summary>
    ///     Gera um token de acesso para o usuário sem registro.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         O cliente tem a opção de se identificar mediante a um cadastro prévio ou não.
    ///     </para>
    ///     <para>
    ///         Independente se o cliente se identificar ou não, o sistema necessita que o token seja gerado para que o pedido
    ///         possa ser realizado.
    ///     </para>
    /// </remarks>
    /// <param name="cpf">CPF do usuário (opicional)</param>
    /// <response code="200">Retorna o token de acesso para o usuário anônimo.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RespostaTokenAcesso))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("acessar-anonimo")]
    public IActionResult AcessarSemIdentificacao(AcessoAnonimo? acessoAnonimo)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = appService.GerarTokenUsuarioNaoIdentificado(acessoAnonimo?.Cpf);

        return Respond(result);
    }
}