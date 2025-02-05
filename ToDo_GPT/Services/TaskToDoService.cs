using ToDo_GPT.Context;
using ToDo_GPT.Logging;
using ToDo_GPT.Models;
using ToDo_GPT.Repositories;

namespace ToDo_GPT.Services
{
    public class TaskToDoService : ITaskToDoRepository
    {
        private readonly AppDbContext _context;
        private readonly CustomerLogger _logger;

        public TaskToDoService(AppDbContext context, CustomerLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<TaskToDo> CreateTaskToDoAsync(TaskToDo taskToDo)
        {
            throw new NotImplementedException();
        }

        public Task<TaskToDo> DeleteTaskToDoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskToDo>> GetTasksToDoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TaskToDo> GetTaskToDoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TaskToDo> UpdateTaskToDoAsync(int id, TaskToDo taskToDo)
        {
            throw new NotImplementedException();
        }
    }
}