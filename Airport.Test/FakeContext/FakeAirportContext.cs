using Airport.Data.Contexts;
using Airport.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Test.FakeContext
{
    public class FakeAirportContext : DbContext
    {
        public FakeAirportContext()
        {
        }

        public FakeAirportContext(DbContextOptions<AirportContext> options)
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
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FakeAirport;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LiveUpdate>(entity =>
            {
                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.LiveUpdates)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("FK__LiveUpdat__Fligh__2FCF1A8A");

                entity.HasOne(d => d.StationNumberNavigation)
                    .WithMany(p => p.LiveUpdates)
                    .HasForeignKey(d => d.StationNumber)
                    .HasConstraintName("FK__LiveUpdat__Stati__30C33EC3");
            });

            modelBuilder.Entity<NextStation>(entity =>
            {
                entity.HasOne(d => d.Source)
                    .WithMany(p => p.NextStationSources)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK__NextStati__Sourc__2BFE89A6");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.NextStationTargets)
                    .HasForeignKey(d => d.TargetId)
                    .HasConstraintName("FK__NextStati__Targe__2CF2ADDF");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(e => e.StationNumber)
                    .HasName("PK__Station__26EDF8CC9B357DA9");

                entity.Property(e => e.StationNumber).ValueGeneratedNever();

                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("FK__Station__FlightI__29221CFB");
            });
        }
    }

}
