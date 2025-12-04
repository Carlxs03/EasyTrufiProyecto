using EasyTrufi.Core.Entities;
using EasyTrufi.Core.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EasyTrufi.Infraestructure.Data;

public partial class DataBaseEasyTrufiContext : DbContext
{
    public DataBaseEasyTrufiContext()
    {
    }

    public DataBaseEasyTrufiContext(DbContextOptions<DataBaseEasyTrufiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<NfcCard> NfcCards { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Security> Securities { get; set; }

    public virtual DbSet<Topup> Topups { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Validator> Validators { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQL2022;Database=DataBaseEasyTrufi;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__drivers__3213E83F8808B70D");

            entity.ToTable("drivers");

            entity.HasIndex(e => new { e.Cedula, e.Active }, "IX_drivers_cedula_active");

            entity.HasIndex(e => e.Cedula, "UQ_drivers_cedula").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.Cedula)
                .HasMaxLength(30)
                .HasColumnName("cedula");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .HasColumnName("full_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<NfcCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nfc_card__3213E83FFCCE73D4");

            entity.ToTable("nfc_cards");

            entity.HasIndex(e => e.Uid, "UQ_nfc_uid").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.IssuedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("issued_at");
            entity.Property(e => e.Uid)
                .HasMaxLength(128)
                .HasColumnName("uid");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.NfcCards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_nfc_user");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payments__3213E83F3772D70B");

            entity.ToTable("payments");

            entity.HasIndex(e => new { e.NfcCardId, e.CreatedAt }, "IX_payments_card_created");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AmountCents).HasColumnName("amount_cents");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.NfcCardId).HasColumnName("nfc_card_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ValidatorId)
                .HasMaxLength(100)
                .HasColumnName("validator_id");

            entity.HasOne(d => d.NfcCard).WithMany(p => p.Payments)
                .HasForeignKey(d => d.NfcCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_payment_card");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_payment_user");

            entity.HasOne(d => d.Validator).WithMany(p => p.Payments)
                .HasPrincipalKey(p => p.ValidatorCode)
                .HasForeignKey(d => d.ValidatorId)
                .HasConstraintName("FK_payment_validator");
        });

        modelBuilder.Entity<Security>(entity =>
        {
            entity.ToTable("Security");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasConversion(
                    x => x.ToString(),
                    x => (RoleType)Enum.Parse(typeof(RoleType), x));

        });

        modelBuilder.Entity<Topup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__topups__3213E83FD942377F");

            entity.ToTable("topups");

            entity.HasIndex(e => new { e.NfcCardId, e.Status, e.CreatedAt }, "IX_topups_card_status_created");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AmountCents).HasColumnName("amount_cents");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .HasColumnName("method");
            entity.Property(e => e.NfcCardId).HasColumnName("nfc_card_id");
            entity.Property(e => e.Reference)
                .HasMaxLength(255)
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.NfcCard).WithMany(p => p.Topups)
                .HasForeignKey(d => d.NfcCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_topup_card");

            entity.HasOne(d => d.User).WithMany(p => p.Topups)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_topup_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F03579BB7");

            entity.ToTable("users");

            entity.HasIndex(e => e.Cedula, "UQ_users_cedula").IsUnique();

            entity.HasIndex(e => e.Email, "UQ_users_email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cedula)
                .HasMaxLength(30)
                .HasColumnName("cedula");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerified).HasColumnName("email_verified");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .HasColumnName("full_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Validator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__validato__3213E83F1C56505E");

            entity.ToTable("validators");

            entity.HasIndex(e => new { e.ValidatorCode, e.Active }, "IX_validators_code_active");

            entity.HasIndex(e => e.ValidatorCode, "UQ_validators_code").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.LocationDescription)
                .HasMaxLength(200)
                .HasColumnName("location_description");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.ValidatorCode)
                .HasMaxLength(100)
                .HasColumnName("validator_code");
            entity.Property(e => e.VehicleId)
                .HasMaxLength(50)
                .HasColumnName("vehicle_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
