using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Web.Api.Core.Entity;

namespace Web.Api.Core.Context
{
    public partial class SportTestContext : DbContext
    {
        public SportTestContext()
        {
        }

        public SportTestContext(DbContextOptions<SportTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MstTest> MstTest { get; set; }
        public virtual DbSet<ErrorLogs> ErrorLogs { get; set; }
        public virtual DbSet<MstUser> MstUser { get; set; }
        public DbSet<SystemUsers> SystemUsers { get; set; }
        public virtual DbSet<TestAthleteMapping> TestAthleteMapping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=sharepoint03;Database=SportTest;Trusted_Connection=False;User=sa;password=Mahant@2984");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<MstTest>(entity =>
            {
                entity.ToTable("mstTest");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TestDate)
                    .HasColumnName("testDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TestType)
                    .HasColumnName("testType")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstUser>(entity =>
            {
                entity.ToTable("mstUser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ErrorLogs>(entity =>
            {
                entity.ToTable("ErrorLogs");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.actioName)
                    .HasColumnName("actioName");

                entity.Property(e => e.logType)
                    .HasColumnName("logType");

                entity.Property(e => e.description)
             .HasColumnName("description");
            });

            modelBuilder.Entity<SystemUsers>(entity =>
            {
                entity.ToTable("SystemUsers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Username)
                    .HasColumnName("Username")
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("PasswordHash");
                entity.Property(e => e.PasswordSalt)
                  .HasColumnName("PasswordSalt");
            });

            modelBuilder.Entity<TestAthleteMapping>(entity =>
            {
                entity.ToTable("testAthleteMapping");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AthleteId).HasColumnName("athleteId");
                entity.Property(e => e.testId).HasColumnName("testId");
                entity.Property(e => e.Distance)
                    .HasColumnName("distance")
                    .HasColumnType("numeric(18, 10)");

                entity.Property(e => e.FitnessRating)
                    .HasColumnName("fitnessRating")
                    .HasMaxLength(50);
            });
        }
    }
}
