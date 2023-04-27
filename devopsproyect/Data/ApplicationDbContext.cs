using Microsoft.EntityFrameworkCore;

namespace devopsproyect.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Models.Agendamiento> Agendamiento {get; set;}
}