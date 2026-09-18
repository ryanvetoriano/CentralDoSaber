using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralDoSaber.API.Controllers;

/// <summary>
/// Usuários da plataforma (cadastro, consulta, atualização e desativação).
/// Usa o repositório específico <c>IUserRepository</c> (consultas por e-mail).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private const string ProblemJson = "application/problem+json";

    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Lista todos os usuários.</summary>
    /// <response code="200">Lista de usuários (pode ser vazia).</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.ListarTodos();
        return Ok(users);
    }

    /// <summary>Busca um usuário pelo identificador.</summary>
    /// <param name="id">Id do usuário.</param>
    /// <response code="200">Usuário encontrado.</response>
    /// <response code="404">Usuário inexistente.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.BuscarPorId(id);
        return user is null ? NotFound() : Ok(user);
    }

    /// <summary>Cadastra um novo usuário.</summary>
    /// <remarks>
    /// Regras: e-mail único e válido, idade mínima de 13 anos, senha com 8+ caracteres.
    /// A senha nunca é devolvida na resposta.
    ///
    ///     POST /api/users
    ///     { "nome": "Maria Silva", "email": "maria@exemplo.com", "senha": "senhaSegura123", "dataNascimento": "2000-05-10" }
    /// </remarks>
    /// <response code="201">Usuário criado.</response>
    /// <response code="400">Payload inválido ou regra de domínio violada.</response>
    /// <response code="409">E-mail já cadastrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        // Nunca logar a senha: apenas o e-mail identifica a operação
        _logger.LogInformation(
            "Criando usuário {Email}. TraceId: {TraceId}",
            request.Email, traceId);

        var user = await _userService.CriarUsuario(request);

        _logger.LogInformation(
            "Usuário {UserId} criado com sucesso ({Email}). TraceId: {TraceId}",
            user.Id, user.Email, traceId);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>Atualiza parcialmente um usuário.</summary>
    /// <remarks>Todos os campos são opcionais; apenas os informados são alterados.</remarks>
    /// <param name="id">Id do usuário.</param>
    /// <param name="request">Campos a alterar.</param>
    /// <response code="200">Usuário atualizado.</response>
    /// <response code="400">Regra de domínio violada.</response>
    /// <response code="404">Usuário inexistente.</response>
    /// <response code="409">E-mail já usado por outro usuário.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation(
            "Atualizando usuário {UserId}. TraceId: {TraceId}",
            id, traceId);

        var user = await _userService.AtualizarUsuario(id, request);

        _logger.LogInformation(
            "Usuário {UserId} atualizado com sucesso. TraceId: {TraceId}",
            user.Id, traceId);

        return Ok(user);
    }

    /// <summary>Desativa um usuário (exclusão lógica).</summary>
    /// <param name="id">Id do usuário.</param>
    /// <response code="204">Usuário desativado.</response>
    /// <response code="404">Usuário inexistente.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await _userService.DesativarUsuario(id);
        return result ? NoContent() : NotFound();
    }
}
