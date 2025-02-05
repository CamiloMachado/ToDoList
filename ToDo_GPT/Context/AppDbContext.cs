using Microsoft.EntityFrameworkCore;
using ToDo_GPT.Models;

namespace ToDo_GPT.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskToDo>? TasksToDo { get; set; }
    public DbSet<User>? Users { get; set; }
}