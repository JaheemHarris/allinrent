using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Models;

namespace RazorAllinRent.Database;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AuthUser> AuthUsers { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<ViewItem> ViewItems { get; set; }

    public virtual DbSet<ViewRental> ViewRentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=allinrent;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC076F8E67EB");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.EmailAddress, "UQ__Admin__49A147400AE011C7").IsUnique();

            entity.Property(e => e.EmailAddress).HasMaxLength(120);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(120);
        });

        modelBuilder.Entity<AuthUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuthUser__3214EC077AEBDFA8");

            entity.ToTable("AuthUser");

            entity.HasIndex(e => e.EmailAddress, "UQ__AuthUser__49A1474066B51F0B").IsUnique();

            entity.HasIndex(e => e.IdNumber, "UQ__AuthUser__62DF8033E77789DD").IsUnique();

            entity.Property(e => e.EmailAddress).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.IdNumber).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(30);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Item__3214EC074045D436");

            entity.ToTable("Item");

            entity.Property(e => e.ImageFile).HasMaxLength(120);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(180);
            entity.Property(e => e.RentalFee).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.ItemType).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_ItemType");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ItemType__3214EC07143480AB");

            entity.ToTable("ItemType");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Label).HasMaxLength(180);
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rental__3214EC0715195975");

            entity.ToTable("Rental");

            entity.Property(e => e.Due).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RentalDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rental_Item");

            entity.HasOne(d => d.User).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rental_AuthUser");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stock__3214EC077406724E");

            entity.ToTable("Stock");

            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Admin).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Admin");

            entity.HasOne(d => d.Item).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Item");
        });

        modelBuilder.Entity<ViewItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Item");

            entity.Property(e => e.ImageFile).HasMaxLength(120);
            entity.Property(e => e.ItemTypeLabel).HasMaxLength(180);
            entity.Property(e => e.Name).HasMaxLength(180);
            entity.Property(e => e.RentalFee).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<ViewRental>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Rental");

            entity.Property(e => e.Due).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.ItemName).HasMaxLength(180);
            entity.Property(e => e.ItemTypeName).HasMaxLength(180);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
