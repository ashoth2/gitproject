using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RegisterApiDAL.EFModels;

namespace RegisterApiDAL.Models
{
    public partial class UserDbContext : IdentityDbContext
    {
        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Registration> Registrations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=49.249.56.102;Database=Reg_as_poc; User ID=sql;Password=Optisol@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("Registration");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Statusid).HasColumnName("statusid");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.Roleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_Roles");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.Statusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_Status");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Roleid)
                    .ValueGeneratedNever()
                    .HasColumnName("roleid");

                entity.Property(e => e.Roles)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.Statusid).HasColumnName("statusid");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.Statusid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Roles_Status");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Statusid)
                    .ValueGeneratedNever()
                    .HasColumnName("statusid");

                entity.Property(e => e.Status1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
