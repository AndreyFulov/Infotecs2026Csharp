using InfotecsTestApplication.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InfotecsTestApplication.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base (options)
    {
    }
    public DbSet<ValueModel> Values { get; set; }
    public DbSet<ResultModel> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResultModel>().HasKey(c => c.Id);
        modelBuilder.Entity<ResultModel>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<ResultModel>()
            .HasMany(x => x.Values)
            .WithOne(x => x.Result)
            .HasForeignKey(x => x.ResultId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ValueModel>()
            .HasOne(value => value.Result)
            .WithMany(result => result.Values)
            .HasForeignKey(value => value.ResultId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}