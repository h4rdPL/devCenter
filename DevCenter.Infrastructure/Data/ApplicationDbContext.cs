using DevCenter.Domain.Entieties;
using Microsoft.EntityFrameworkCore;

namespace DevCenter.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    

    public DbSet<User> Users { get; set; }
}
