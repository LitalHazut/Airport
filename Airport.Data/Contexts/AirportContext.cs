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
        public virtual DbSet<LiveUpdate> LiveUpdates { get; set; } = null!;
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
            modelBuilder.Entity<LiveUpdate>(entity =>
            {
                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.LiveUpdates)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("FK__LiveUpdat__Fligh__1EA48E88");

                entity.HasOne(d => d.StationNumberNavigation)
                    .WithMany(p => p.LiveUpdates)
                    .HasForeignKey(d => d.StationNumber)
                    .HasConstraintName("FK__LiveUpdat__Stati__1F98B2C1");
            });

            modelBuilder.Entity<NextStation>(entity =>
            {
                entity.HasOne(d => d.Source)
                    .WithMany(p => p.NextStationSources)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK__NextStati__Sourc__22751F6C");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.NextStationTargets)
                    .HasForeignKey(d => d.TargetId)
                    .HasConstraintName("FK__NextStati__Targe__236943A5");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(e => e.StationNumber)
                    .HasName("PK__Station__26EDF8CC23747422");

                entity.Property(e => e.StationNumber).ValueGeneratedNever();

                entity.Property(e => e.StationId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("FK__Station__FlightI__17F790F9");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
