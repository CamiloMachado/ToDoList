using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using ToDo_GPT.Context;
using ToDo_GPT.Logging;
using ToDo_GPT.Models;
using ToDo_GPT.Repositories;

namespace ToDo_GPT.Services;

public class UserService : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly CustomerLogger _logger;

    public UserService(AppDbContext context, CustomerLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User> GetUserAsync(string email, string password)
    {
        ValidateInput(email, password);

        var user = await _context.Users!.AsNoTracking().FirstOrDefaultAsync(u => u.UserEmail == email && u.UserPassword == password);

        validateIsNull(user!);

        _logger.LogInformation($"Usuário {user!.UserName} encontrado com sucesso.");
        return user;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        validateIsNull(user);

        user.UserName = CapitalizeNames(user.UserName!);

        // Adiciona os dados do usuáro na mémoria
        _context.Users!.Add(user);
        // Salva as mudanças no banco de dados
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Usuário {user.UserName} criado com sucesso.");
        return user;
    }

    public async Task<User> UpdateUserAsync(int id, User user)
    {
        if (id != user.UserId)
        {
            throw new ArgumentException("O id informado é inválido.");
        }

        var existingUser = await _context.Users!.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

        validateIsNull(existingUser!);

        // Atualiza os valores do usuário existente na mémoria
        _context.Entry(existingUser!).CurrentValues.SetValues(user);
        _context.Entry(existingUser!).State = EntityState.Modified;

        // Salva a atualização no banco de dados
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Usuário {user.UserName} atualizado com sucesso.");
        return user;
    }

    public async Task<User> DeleteUserAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException(nameof(id), "O id não pode ser zero.");
        }

        var user = await _context.Users!.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

        validateIsNull(user!);

        // Remove os dados do usuário existente na mémoria
        _context.Users!.Remove(user!);
        // Salva a remoção no banco de dados
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Usuário {user!.UserName} deletado com sucesso.");
        return user;
    }

    private static string CapitalizeNames(string name)
    {
        return Regex.Replace(name, @"\b[a-z]", m => m.Value.ToUpper(CultureInfo.InvariantCulture));
    }

    public void ValidateInput(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(email), "Email e senha são obrigatórios.");
        }
    }

    public void validateIsNull(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException("Usuário não encontrado.");
        }
    }
}