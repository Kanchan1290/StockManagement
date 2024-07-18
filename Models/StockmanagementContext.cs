using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StockManagementSystem.Models;

public partial class StockmanagementContext : DbContext
{
    public StockmanagementContext()
    {
    }

    public StockmanagementContext(DbContextOptions<StockmanagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PodCategory> PodCategories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=conn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PodCategory>(entity =>
        {
            entity.HasKey(e => e.PcId).HasName("PK__podCateg__83A267470712E64B");

            entity.ToTable("podCategory");

            entity.Property(e => e.PcId)
                .ValueGeneratedNever()
                .HasColumnName("pcId");
            entity.Property(e => e.PcDescription)
                .HasMaxLength(100)
                .HasColumnName("pcDescription");
            entity.Property(e => e.PcName)
                .HasMaxLength(50)
                .HasColumnName("pcName");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PId).HasName("PK__product__DD36D562F87C66DC");

            entity.ToTable("product");

            entity.Property(e => e.PId)
                .ValueGeneratedNever()
                .HasColumnName("pId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.PName)
                .HasMaxLength(50)
                .HasColumnName("pName");
            entity.Property(e => e.PPrice).HasColumnName("pPrice");
            entity.Property(e => e.Pimage).HasColumnName("PImage");
            entity.Property(e => e.PurchaseOuantity).HasColumnName("purchaseOuantity");
            entity.Property(e => e.SaleQuantity).HasColumnName("saleQuantity");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.SupplierId).HasColumnName("supplierId");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__product__categor__4E88ABD4");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__product__supplie__4F7CD00D");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PK__supplier__DDDED96E17E842CE");

            entity.ToTable("supplier");

            entity.Property(e => e.SId)
                .ValueGeneratedNever()
                .HasColumnName("sId");
            entity.Property(e => e.SAddress)
                .HasMaxLength(50)
                .HasColumnName("sAddress");
            entity.Property(e => e.SEmail)
                .HasMaxLength(50)
                .HasColumnName("sEmail");
            entity.Property(e => e.SName)
                .HasMaxLength(50)
                .HasColumnName("sName");
            entity.Property(e => e.SPhone)
                .HasMaxLength(50)
                .HasColumnName("sPhone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
