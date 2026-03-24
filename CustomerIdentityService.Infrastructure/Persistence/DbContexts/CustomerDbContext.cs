using CustomerIdentityService.Core.Entities;
using CustomerIdentityService.Core.Models.Settings;
using Microsoft.EntityFrameworkCore;

namespace CustomerIdentityService.Infrastructure.Persistence.DbContexts;

public partial class CustomerDbContext : DbContext
{
    public CustomerDbContext()
    {
    }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAuthProvider> CustomerAuthProviders { get; set; }

    public virtual DbSet<CustomerCredential> CustomerCredentials { get; set; }

    public virtual DbSet<CustomerOtp> CustomerOtps { get; set; }

    public virtual DbSet<CustomerSession> CustomerSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
        });

        modelBuilder.Entity<CustomerAuthProvider>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAuthProviders).HasConstraintName("FK_AUTH_CUSTOMER");
        });

        modelBuilder.Entity<CustomerCredential>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerCredentials).HasConstraintName("FK_CRED_CUSTOMER");
        });

        modelBuilder.Entity<CustomerOtp>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsUsed).HasDefaultValue((byte)0);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerOtps).HasConstraintName("FK_OTP_CUSTOMER");
        });

        modelBuilder.Entity<CustomerSession>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerSessions).HasConstraintName("FK_SESSION_CUSTOMER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
