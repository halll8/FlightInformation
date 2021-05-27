using FlightInformation.Models;
using System.Linq;

namespace FlightInformation.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FlightInfoContext context)
        {
            //context.Database.EnsureCreated();

            if (context.Flights.Any())
            {
                return;
            }

            var flights = new Flight[]
            {
            new Flight{FlightNumber=1,AirLine="Alaska Airlines"},
            new Flight{FlightNumber=2,AirLine="Delta Airlines"},
            new Flight{FlightNumber=3,AirLine="Air Canada"}
            };
            foreach (Flight f in flights)
            {
                context.Flights.Add(f);
            }
            context.SaveChanges();

            var passengers = new Passenger[]
            {
            new Passenger{PassengerID=15,FirstName="Lucas",LastName="Hall"},
            new Passenger{PassengerID=42,FirstName="Esther",LastName="Chong"},
            new Passenger{PassengerID=41,FirstName="Danny",LastName="Hassler"}
            };
            foreach (Passenger p in passengers)
            {
                context.Passengers.Add(p);
            }
            context.SaveChanges();

            var tickets = new Ticket[]
            {
                new Ticket {
                    FlightID = flights.Single(f => f.AirLine == "Alaska Airlines").ID,
                    PassengerID = passengers.Single(p => p.LastName == "Hall").PassengerID
                },
                new Ticket {
                    FlightID = flights.Single(f => f.AirLine == "Delta Airlines").ID,
                    PassengerID = passengers.Single(p => p.LastName == "Chong").PassengerID
                },
                new Ticket {
                    FlightID = flights.Single(f => f.AirLine == "Air Canada").ID,
                    PassengerID = passengers.Single(p => p.LastName == "Hassler").PassengerID
                },

            };
            foreach (Ticket t in tickets)
            {
                context.Tickets.Add(t);
            }
            context.SaveChanges();
        }
    }
}
