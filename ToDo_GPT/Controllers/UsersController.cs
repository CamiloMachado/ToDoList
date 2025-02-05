using Microsoft.AspNetCore.Mvc;
using ToDo_GPT.Logging;
using ToDo_GPT.Models;
using ToDo_GPT.Repositories;

namespace ToDo_GPT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userService;
    private readonly CustomerLogger _logger;

    public UsersController(IUserRepository userRepository, CustomerLogger logger)
    {
        _userService = userRepository;
        _logger = logger;
    }

    [HttpGet("{email}/{password}")]
    public async Task<IActionResult> GetUser(string email, string password)
    {
        var user = await _userService.GetUserAsync(email, password);
        _logger.LogInformation("Usuário encontrado com sucesso: {email}", user.UserEmail);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(User user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        _logger.LogInformation("Usuário criado com sucesso: {email}", createdUser.UserEmail);
        return CreatedAtAction(nameof(GetUser), new { email = createdUser.UserEmail, password = createdUser.UserPassword },
            createdUser);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        var updatedUser = await _userService.UpdateUserAsync(id, user);
        _logger.LogInformation("Usuário atualizado com sucesso: {email}", updatedUser.UserEmail);
        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deletedUser = await _userService.DeleteUserAsync(id);
        _logger.LogInformation("Usuário deletado com sucesso: {email}", deletedUser.UserEmail);
        return Ok(deletedUser);
    }
}