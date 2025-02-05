using ToDo_GPT.Models;

namespace ToDo_GPT.Repositories;

public interface ITaskToDoRepository
{
    Task<IEnumerable<TaskToDo>> GetTasksToDoAsync();

    Task<TaskToDo> GetTaskToDoAsync(int id);

    Task<TaskToDo> CreateTaskToDoAsync(TaskToDo taskToDo);

    Task<TaskToDo> UpdateTaskToDoAsync(int id, TaskToDo taskToDo);

    Task<TaskToDo> DeleteTaskToDoAsync(int id);
}