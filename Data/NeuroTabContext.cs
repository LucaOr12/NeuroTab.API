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
            .HasOne(c => c.FromTab)
            .WithMany(t => t.OutGoingConnections)
            .HasForeignKey(c => c.FromTabId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Connection>()
            .HasOne(c => c.ToTab)
            .WithMany(t => t.IncomingConnections)
            .HasForeignKey(c => c.ToTabId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}