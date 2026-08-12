using InfotecsTestApplication.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InfotecsTestApplication.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base (options)
    {
    }
    public DbSet<ValueModel> Values { get; set; }
}