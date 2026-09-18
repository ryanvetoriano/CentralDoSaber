using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralDoSaber.API.Controllers;

/// <summary>
/// Autores responsáveis pelos conteúdos.
/// Fluxo sobre o repositório genérico <c>IRepository&lt;Autor&gt;</c>.
/// </summary>
[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    private const string ProblemJson = "application/problem+json";

    private readonly IAutorService _autorService;
    private readonly ILogger<AutoresController> _logger;

    public AutoresController(IAutorService autorService, ILogger<AutoresController> logger)
    {
        _autorService = autorService;
        _logger = logger;
    }

    /// <summary>Lista todos os autores.</summary>
    /// <response code="200">Lista de autores (pode ser vazia).</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AutorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<IReadOnlyList<AutorResponse>>> GetAll()
    {
        return Ok(await _autorService.ListarTodos());
    }

    /// <summary>Busca um autor pelo identificador.</summary>
    /// <param name="id">Id do autor.</param>
    /// <response code="200">Autor encontrado.</response>
    /// <response code="404">Autor inexistente.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<AutorResponse>> GetById(Guid id)
    {
        return Ok(await _autorService.BuscarPorId(id));
    }

    /// <summary>Cadastra um novo autor.</summary>
    /// <remarks>
    ///     POST /api/autores
    ///     { "nome": "Machado de Assis", "biografia": "Escritor brasileiro.", "dataNascimento": "1839-06-21" }
    /// </remarks>
    /// <response code="201">Autor criado.</response>
    /// <response code="400">Payload inválido ou regra de domínio violada (ex.: data futura).</response>
    [HttpPost]
    [ProducesResponseType(typeof(AutorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<AutorResponse>> Create([FromBody] AutorRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation(
            "Criando autor {AutorNome}. TraceId: {TraceId}",
            request.Nome, traceId);

        var autor = await _autorService.Criar(request);

        _logger.LogInformation(
            "Autor {AutorId} ({AutorNome}) criado com sucesso. TraceId: {TraceId}",
            autor.Id, autor.Nome, traceId);

        return CreatedAtAction(nameof(GetById), new { id = autor.Id }, autor);
    }

    /// <summary>Atualiza os dados de um autor.</summary>
    /// <param name="id">Id do autor.</param>
    /// <param name="request">Novos dados.</param>
    /// <response code="200">Autor atualizado.</response>
    /// <response code="400">Payload inválido ou regra de domínio violada.</response>
    /// <response code="404">Autor inexistente.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, ProblemJson)]
    public async Task<ActionResult<AutorResponse>> Update(Guid id, [FromBody] AutorRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;

        _logger.LogInformation(
            "Atualizando autor {AutorId}. TraceId: {TraceId}",
            id, traceId);

        var autor = await _autorService.Atualizar(id, request);

        _logger.LogInformation(
            "Autor {AutorId} atualizado com sucesso. TraceId: {TraceId}",
            autor.Id, traceId);

        return Ok(autor);
    }
}
