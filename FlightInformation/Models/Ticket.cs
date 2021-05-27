namespace FlightInformation.Models
{
    public class Ticket
    {
        public int PassengerID { get; set; }
        public int FlightID { get; set; }
        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
    }
}
