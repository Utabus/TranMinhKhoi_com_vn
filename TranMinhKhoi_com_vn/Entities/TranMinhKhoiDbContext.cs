using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TranMinhKhoi_com_vn.Entities;

public partial class TranMinhKhoiDbContext : DbContext
{
    public TranMinhKhoiDbContext()
    {
    }

    public TranMinhKhoiDbContext(DbContextOptions<TranMinhKhoiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Fund> Funds { get; set; }

    public virtual DbSet<KeySePay> KeySePays { get; set; }

    public virtual DbSet<RequestCourse> RequestCourses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<VipAccount> VipAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=202.92.4.31;Database=abfwnemdhosting_TranminhKhoi;User Id=abfwnemdhosting;Password=6HJ8&6l^psDionrs;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.Birthday).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Accounts_Role");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.Cdt).HasColumnName("CDT");

            entity.HasOne(d => d.Account).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Blog_Accounts");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.Cdt).HasColumnName("CDT");
        });

        modelBuilder.Entity<Fund>(entity =>
        {
            entity.ToTable("Fund");

            entity.Property(e => e.Cdt).HasColumnName("CDT");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Account).WithMany(p => p.Funds)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Fund_Accounts");
        });

        modelBuilder.Entity<KeySePay>(entity =>
        {
            entity.ToTable("KeySePay");

            entity.Property(e => e.Cdt).HasColumnName("CDT");
            entity.Property(e => e.KeyApi).HasColumnName("KeyAPI");
        });

        modelBuilder.Entity<RequestCourse>(entity =>
        {
            entity.ToTable("RequestCourse");

            entity.Property(e => e.Cdt).HasColumnName("CDT");

            entity.HasOne(d => d.Account).WithMany(p => p.RequestCourses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_RequestCourse_Accounts");

            entity.HasOne(d => d.Course).WithMany(p => p.RequestCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_RequestCourse_Course");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
        });

        modelBuilder.Entity<VipAccount>(entity =>
        {
            entity.ToTable("VipAccount");

            entity.Property(e => e.Cdt).HasColumnName("CDT");

            entity.HasOne(d => d.Account).WithMany(p => p.VipAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_VipAccount_Accounts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
