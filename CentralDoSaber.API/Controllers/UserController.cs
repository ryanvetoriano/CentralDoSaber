using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CentralDoSaber.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>GET /api/users — lista todos os usuários</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.ListarTodos();
        return Ok(users);
    }

    /// <summary>GET /api/users/{id} — busca usuário por ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.BuscarPorId(id);
        return user is null ? NotFound() : Ok(user);
    }

    /// <summary>POST /api/users — cria novo usuário</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = await _userService.CriarUsuario(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>PUT /api/users/{id} — atualiza usuário</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var user = await _userService.AtualizarUsuario(id, request);
        return Ok(user);
    }

    /// <summary>DELETE /api/users/{id} — desativa usuário</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await _userService.DesativarUsuario(id);
        return result ? NoContent() : NotFound();
    }
}