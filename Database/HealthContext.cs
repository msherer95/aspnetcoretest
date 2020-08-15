using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace AnalyticsWebapps.Database
{
    public partial class HealthContext : DbContext
    {
        private IConfiguration configuration;
        public HealthContext(DbContextOptions<HealthContext> options, IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public virtual DbSet<DrugInformation> DrugInformation { get; set; }
        public virtual DbSet<SyndromicEvents> SyndromicEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("Health"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DrugInformation>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DrugName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullText)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SyndromicEvents>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("syndromic_events");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.ChiefComplaint)
                    .HasColumnName("chief_complaint")
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.EspEvent)
                    .HasColumnName("esp_event")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsForeign).HasColumnName("is_foreign");

                entity.Property(e => e.Race)
                    .HasColumnName("race")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Red).HasColumnName("red");

                entity.Property(e => e.Site)
                    .HasColumnName("site")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SyndromicId).HasColumnName("syndromic_id");

                entity.Property(e => e.VisitDate)
                    .HasColumnName("visit_date")
                    .HasColumnType("date");

                entity.Property(e => e.ZipCode).HasColumnName("zip_code");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
