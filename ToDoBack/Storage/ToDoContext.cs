using Microsoft.EntityFrameworkCore;
using ToDoBack.Models;

namespace ToDoBack.Storage
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItem> Todos { get; set; }

       
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {

        }
    }
}
