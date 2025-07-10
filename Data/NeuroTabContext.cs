using Microsoft.EntityFrameworkCore;
using neurotab_api.Models;


namespace neurotab_api.Data;

public class NeuroTabContext : DbContext
{
    public NeuroTabContext(DbContextOptions<NeuroTabContext> options) :base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Tab> Tabs { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<Connection> Connections { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Connection>()
            .HasOne(c => c.FromContent)
            .WithMany(t => t.OutGoingConnections)
            .HasForeignKey(c => c.FromContentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Connection>()
            .HasOne(c => c.ToContent)
            .WithMany(t => t.IncomingConnections)
            .HasForeignKey(c => c.ToContentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}