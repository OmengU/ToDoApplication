using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models
{
    public class ToDoManagementDbContext : DbContext
    {
        public ToDoManagementDbContext(DbContextOptions<ToDoManagementDbContext> options) : base(options) { }
        public DbSet<ToDo> ToDos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
