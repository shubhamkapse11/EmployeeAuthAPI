using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using webApp.Entities;
namespace webApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
    {
        
    }  

    public DbSet<User> AccountUser { get; set; }
    public DbSet<Employee> Employees { get; set; }
}
