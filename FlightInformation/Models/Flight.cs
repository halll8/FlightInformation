using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightInformation.Models
{
    public class Flight
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Flight Number")]
        [Remote(action: "VerifyFlightNumber", controller: "Flights", AdditionalFields="ID")]
        public int FlightNumber { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Air Line")]
        public string AirLine { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
