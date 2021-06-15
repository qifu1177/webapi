using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Dal.Models
{
    public partial class DataContext : DbContext
    {
        private string _connectString = "Server=THREE\\SQLEXPRESS;Database=ApartmentDB;User Id=sa;Password=sa;";
        public DataContext() : this("")
        {
        }
        public DataContext(string connectString)
        {
            _connectString = string.IsNullOrEmpty(connectString) ? _connectString : connectString;

        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<AppSession> AppSessions { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Cupboard> Cupboards { get; set; }
        public virtual DbSet<Grid> Grids { get; set; }
        public virtual DbSet<GridThring> GridThrings { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<RoleRight> RoleRights { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Thing> Things { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("AppRole");

                entity.Property(e => e.RoleId).HasMaxLength(50);

                entity.Property(e => e.RoleDesc)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AppSession>(entity =>
            {
                entity.HasKey(e => e.SessionId);

                entity.ToTable("AppSession");

                entity.Property(e => e.SessionId).HasMaxLength(50);

                entity.Property(e => e.CreateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AppSessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AppSession_AppUser");
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("AppUser");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.CreateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AppUser_AppRole");
            });

            modelBuilder.Entity<Cupboard>(entity =>
            {
                entity.Property(e => e.CreateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Cupboards)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cupboards)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Cupboards_AppUser");
            });

            modelBuilder.Entity<Grid>(entity =>
            {
                entity.Property(e => e.CreateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Cupboard)
                    .WithMany(p => p.Grids)
                    .HasForeignKey(d => d.CupboardId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Grids)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Grids_AppUser");
            });

            modelBuilder.Entity<GridThring>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Grid)
                    .WithMany()
                    .HasForeignKey(d => d.GridId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GridThrings_Grids");

                entity.HasOne(d => d.Thing)
                    .WithMany()
                    .HasForeignKey(d => d.ThingId)
                    .HasConstraintName("FK_GridThrings_Things");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RoleRight>(entity =>
            {
                entity.ToTable("RoleRight");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RoleRight1)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("RoleRight")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_RoleRight_Tabelle");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleRight_AppRole");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.CreateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Rooms_AppUser");
            });

            modelBuilder.Entity<Thing>(entity =>
            {
                entity.Property(e => e.CreateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdateTs)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Things)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Things_AppUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
