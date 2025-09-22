using Employee.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.DBContext;

public class TodoContext  : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) {}

    public TodoContext() {}
    
    public DbSet<Todo> Todos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Table naming (avoid reserved words)
        modelBuilder.Entity<Todo>().ToTable("Todo");
        
        
    }
    
    
}