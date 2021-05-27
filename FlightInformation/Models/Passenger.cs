using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FlightInformation.Models
{
    public class Passenger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Remote(action: "VerifyPassengerID", controller: "Passengers")]
        [Display(Name = "Passenger ID")]
        public int PassengerID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        
    }
}
