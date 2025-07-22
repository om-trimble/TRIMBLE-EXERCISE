using Microsoft.EntityFrameworkCore;

namespace SupportPortal.Api.Data;

public class SupportTicketDbContext : DbContext
{
    public SupportTicketDbContext(DbContextOptions<SupportTicketDbContext> options) : base(options)
    {
    }

    public DbSet<SupportTicket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedDate).IsRequired();
        });
    }
} 