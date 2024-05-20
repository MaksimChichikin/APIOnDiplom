using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyProApiDiplom.Models;

public partial class IlecContext : DbContext
{
    public IlecContext()
    {
    }

    public IlecContext(DbContextOptions<IlecContext> options)
        : base(options)
    {
    }

    public virtual DbSet<All> Alls { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-R6H3MU1\\MSSQLSERVER01;Database=Ilec;User Id=1;Password=1;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<All>(entity =>
        {
            entity.ToTable("All");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Alls)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_All_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
