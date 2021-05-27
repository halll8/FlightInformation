using System.Collections.Generic;

namespace FlightInformation.Models.FlightInfoViewModels
{
    public class FlightIndexData
    {
        public IEnumerable<Flight> Flights { get; set; }
        public IEnumerable<Passenger> Passengers { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
