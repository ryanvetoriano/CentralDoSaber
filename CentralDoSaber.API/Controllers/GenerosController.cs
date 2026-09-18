using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralDoSaber.API.Controllers;

/// <summary>
/// Gêneros usados para classificar os conteúdos (livros, mangás, HQs, revistas).
/// Fluxo 100% sobre o repositório genérico <c>IRepository&lt;Genero&gt;</c>.
/// </summary>
[ApiController]
[Route("api/generos")]
public class GenerosController : ControllerBase
{
    private const string ProblemJson = "application/problem+json";

    private readonly IGeneroService _generoService;
    private readonly ILogger<GenerosController> _logger;

    public GenerosController(IGeneroService generoService, ILogger<GenerosController> logger)
    {
        _generoService = generoService;
        _logger = logger;
    }

    /// <summary>Lista todos os gêneros.</summary>
    /// <response code="200">Lista de gêneros (pode ser vazia).</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GeneroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<IReadOnlyList<GeneroResponse>>> GetAll()
    {
        return Ok(await _generoService.ListarTodos());
    }

    /// <summary>Busca um gênero pelo identificador.</summary>
    /// <param name="id">Id do gênero.</param>
    /// <response code="200">Gênero encontrado.</response>
    /// <response code="404">Gênero inexistente.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GeneroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<GeneroResponse>> GetById(Guid id)
    {
        return Ok(await _generoService.BuscarPorId(id));
    }

    /// <summary>Cadastra um novo gênero.</summary>
    /// <remarks>
    /// O nome é único (sem diferenciar maiúsculas/minúsculas).
    ///
    ///     POST /api/generos
    ///     { "nome": "Fantasia", "descricao": "Mundos imaginários e magia." }
    /// </remarks>
    /// <response code="201">Gênero criado.</response>
    /// <response code="400">Payload inválido ou regra de domínio violada.</response>
    /// <response code="409">Já existe gênero com o mesmo nome.</response>
    [HttpPost]
    [ProducesResponseType(typeof(GeneroResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<GeneroResponse>> Create([FromBody] GeneroRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation(
            "Criando gênero {GeneroNome}. TraceId: {TraceId}",
            request.Nome, traceId);

        var genero = await _generoService.Criar(request);

        _logger.LogInformation(
            "Gênero {GeneroId} ({GeneroNome}) criado com sucesso. TraceId: {TraceId}",
            genero.Id, genero.Nome, traceId);

        return CreatedAtAction(nameof(GetById), new { id = genero.Id }, genero);
    }

    /// <summary>Atualiza nome e descrição de um gênero.</summary>
    /// <param name="id">Id do gênero.</param>
    /// <param name="request">Novos dados.</param>
    /// <response code="200">Gênero atualizado.</response>
    /// <response code="400">Payload inválido ou regra de domínio violada.</response>
    /// <response code="404">Gênero inexistente.</response>
    /// <response code="409">Já existe outro gênero com o mesmo nome.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GeneroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<GeneroResponse>> Update(Guid id, [FromBody] GeneroRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation(
            "Atualizando gênero {GeneroId}. TraceId: {TraceId}",
            id, traceId);

        var genero = await _generoService.Atualizar(id, request);

        _logger.LogInformation(
            "Gênero {GeneroId} atualizado com sucesso. TraceId: {TraceId}",
            genero.Id, traceId);

        return Ok(genero);
    }

    /// <summary>Remove um gênero.</summary>
    /// <param name="id">Id do gênero.</param>
    /// <response code="204">Gênero removido.</response>
    /// <response code="404">Gênero inexistente.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation(
            "Removendo gênero {GeneroId}. TraceId: {TraceId}",
            id, HttpContext.TraceIdentifier);

        await _generoService.Remover(id);
        return NoContent();
    }
}
