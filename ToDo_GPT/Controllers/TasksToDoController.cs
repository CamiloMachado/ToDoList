using ToDo_GPT.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ToDo_GPT.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDo_GPT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksToDoController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public TasksToDoController(AppDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskToDo>>> Get()
    {
        try
        {
            var tasks = await _context.TasksToDo!.AsNoTracking().ToListAsync();
            if (tasks == null || !tasks.Any())
            {
                return NotFound("Não há nenhuma tarefa.");
            }

            return Ok(tasks);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro no banco de dados ao buscar as tarefas.");
            return StatusCode(500, "Ocorreu um erro no banco de dados ao buscar as tarefas. Tente novamente mais tarde.");
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Erro de SQL ao buscar as tarefas.");
            return StatusCode(500, "Ocorreu um erro de SQL ao buscar as tarefas. Tente novamente mais tarde.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar as tarefas.");
            return StatusCode(500, "Ocorreu um erro ao buscar as tarefas. Tente novamente mais tarde.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        try
        {
            var task = await _context.TasksToDo!.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == id);

            if (task == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            return Ok(task);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro no banco de dados ao buscar a tarefa.");
            return StatusCode(500, "Ocorreu um erro no banco de dados ao buscar as tarefas. Tente novamente mais tarde.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar a tarefa.");
            return StatusCode(500, "Ocorreu um erro ao buscar a tarefa. Tente novamente mais tarde.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post(TaskToDo task)
    {
        try
        {
            if (task is null)
                return BadRequest("A tarefa não pode ser nula.");

            if (!ModelState.IsValid)
                return BadRequest("Os dados da tarefa são inválidos.");

            // Colocando a data do momento que irá ser enviado a tarefa ao banco
            task.CreatedAt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            _context.TasksToDo!.Add(task);
            await _context.SaveChangesAsync();

            // Retornar a lista de todas as tarefas, incluindo a nova
            var tasks = await _context.TasksToDo!.AsNoTracking().ToListAsync();
            return Ok(new { message = "Tarefa criada com sucesso.", tasks });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Conflito de concorrência ao criar a tarefa.");
            return StatusCode(409, "Conflito de concorrência ao criar a tarefa. Tente novamente.");
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro no banco de dados ao criar a tarefa.");
            return StatusCode(500, "Ocorreu um erro no banco de dados ao criar a tarefa. Tente novamente mais tarde.");
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Erro de SQL ao criar a tarefa.");
            return StatusCode(500, "Ocorreu um erro de SQL ao criar a tarefa. Tente novamente mais tarde.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar a tarefa.");
            return StatusCode(500, "Ocorreu um erro ao criar a tarefa. Tente novamente mais tarde.");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, TaskToDo task)
    {
        try
        {
            if (id != task.TaskId)
            {
                return BadRequest("Os dados da tarefa são inválidos.");
            }

            var existingTask = await _context.TasksToDo!.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == id);

            if (existingTask == null)
            {
                return NotFound("Tarefa não encontrado.");
            }

            // Colocando a data do momento que irá ser atualizado a tarefa no banco
            task.UpdatedAt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            _context.Entry(existingTask).CurrentValues.SetValues(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tarefa atualizada com sucesso.", task });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Conflito de concorrência ao atualizar a tarefa.");
            return StatusCode(409, "Conflito de concorrência ao atualizar a tarefa. Tente novamente.");
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Erro de banco de dados ao atualizar a tarefa.");
            return StatusCode(500, "Ocorreu um erro de banco de dados ao atualizar a tarefa. Tente novamente mais tarde.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar a tarefa.");
            return StatusCode(500, "Ocorreu um erro ao atualizar a tarefa. Tente novamente mais tarde.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var task = await _context.TasksToDo!.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == id);

            if (task == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            _context.TasksToDo!.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tarefa deletada com sucesso.", task });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Conflito de concorrência ao deletar a tarefe.");
            return StatusCode(409, "Conflito de concorrência ao deletar a tarefa. Tente novamente.");
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Erro de SQL ao deletar a tarefa.");
            return StatusCode(500, "Ocorreu um erro no banco de dados. Tente novamente mais tarde.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar a tarefa.");
            return StatusCode(500, "Ocorreu um erro ao deletar a tarefa. Tente novamente mais tarde.");
        }
    }
}