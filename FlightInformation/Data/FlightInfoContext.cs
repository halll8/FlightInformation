using FlightInformation.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightInformation.Data
{
    public class FlightInfoContext : DbContext
    {
        public FlightInfoContext(DbContextOptions<FlightInfoContext> options) : base(options)
        {
        }

        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passenger>().ToTable("Passenger");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<Flight>().ToTable("Flight");
            modelBuilder.Entity<Ticket>()                       
                .HasKey(t => new { t.PassengerID, t.FlightID }); 
        }
    }
}
