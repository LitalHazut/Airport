using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Airport.Data.Model;

namespace Airport.Data.Contexts
{
    public partial class AirportContext : DbContext
    {
        public AirportContext()
        {
        }

        public AirportContext(DbContextOptions<AirportContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flight> Flights { get; set; } = null!;
        public virtual DbSet<NextStation> NextStations { get; set; } = null!;
        public virtual DbSet<Station> Stations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Airport;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NextStation>(entity =>
            {
                entity.HasKey(e => new { e.SourceId, e.TargetId })
                    .HasName("NextStationId");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.NextStationSources)
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NextStati__Sourc__440B1D61");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.NextStationTargets)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NextStati__Targe__44FF419A");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("FK__Station__FlightI__412EB0B6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
