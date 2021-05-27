using FlightInformation.Data;
using FlightInformation.Models;
using FlightInformation.Models.FlightInfoViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FlightInformation.Controllers
{
    public class FlightsController : Controller
    {
        private readonly FlightInfoContext _context;

        public FlightsController(FlightInfoContext context)
        {
            _context = context;
        }

        // GET: Flights
        public async Task<IActionResult> Index(int? id, int? passengerID)
        {
            var viewModel = new FlightIndexData();
            viewModel.Flights = await _context.Flights
                  .Include(f => f.Tickets)
                    .ThenInclude(t => t.Passenger)
                  .AsNoTracking()
                  .OrderBy(f => f.FlightNumber) 
                  .ToListAsync();
            if (id != null)
            {
                ViewData["FlightID"] = id.Value;
                Flight flight = viewModel.Flights.Where(
                    f => f.ID == id.Value).Single();
                viewModel.Passengers = flight.Tickets.Select(t => t.Passenger);
            }
            if (passengerID != null)
            {
                ViewData["PassengerID"] = passengerID.Value;
            }
            return View(viewModel);
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.ID == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            var flight = new Flight();
            flight.Tickets = new List<Ticket>();
            PopulateRegisteredPassengerData(flight);
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightNumber,AirLine")] Flight flight, string[] selectedPassengers)
        {
            if(selectedPassengers != null)
            {
                flight.Tickets = new List<Ticket>();
                foreach (var passenger in selectedPassengers)
                {
                    var passengerToAdd = new Ticket { FlightID = flight.ID, PassengerID = int.Parse(passenger) };
                    flight.Tickets.Add(passengerToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateRegisteredPassengerData(flight);
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Tickets).ThenInclude(f => f.Passenger)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (flight == null)
            {
                return NotFound();
            }
            PopulateRegisteredPassengerData(flight);
            return View(flight);
        }

        // Parameters: Flight entity
        // Helper function used by Edit and Create methods
        // Reads through all Passenger entities in order to load a list of passengers using the view model class
        // The Passengers of a given flight are put into a HashSet to provide efficient lookup
        private void PopulateRegisteredPassengerData(Flight flight)
        {
            var allPassengers = _context.Passengers;
            var flightPassengers = new HashSet<int>(flight.Tickets.Select(p => p.PassengerID));
            var viewModel = new List<RegisteredPassengerData>();
            foreach (var passenger in allPassengers)
            {
                viewModel.Add(new RegisteredPassengerData
                {
                    PassengerID = passenger.PassengerID,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    Registered = flightPassengers.Contains(passenger.PassengerID)
                });
            }
            ViewData["Passengers"] = viewModel;
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedPassengers)
        {
            if (id == null)
            {
                return NotFound();
            }
            var flightToUpdate = await _context.Flights
                .Include(f => f.Tickets)
                    .ThenInclude(f => f.Passenger)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Flight>(
                flightToUpdate,
                "",
                f => f.FlightNumber, f => f.AirLine))
            {
                UpdateFlightPassengers(selectedPassengers, flightToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateFlightPassengers(selectedPassengers, flightToUpdate);
            PopulateRegisteredPassengerData(flightToUpdate);
            return View(flightToUpdate);
        }

        // Parameters: String array of PassengerIDs, Flight entity to update
        // Helper function used by Edit method
        // Updates the Tickets navigation property of a given flight
        // If no passengers were selected, Tickets is initialized with empty collection
        private void UpdateFlightPassengers(string[] selectedPassengers, Flight flightToUpdate)
        {
            if (selectedPassengers == null)
            {
                flightToUpdate.Tickets = new List<Ticket>();
                return;
            }
            var selectedPassengersHS = new HashSet<string>(selectedPassengers);
            var flightPassengers = new HashSet<int>
                (flightToUpdate.Tickets.Select(t => t.Passenger.PassengerID));
            foreach (var passenger in _context.Passengers)
            {
                if (selectedPassengersHS.Contains(passenger.PassengerID.ToString()))
                {
                    if (!flightPassengers.Contains(passenger.PassengerID))
                    {
                        flightToUpdate.Tickets.Add(new Ticket { FlightID = flightToUpdate.ID, PassengerID = passenger.PassengerID });
                    }
                }
                else
                {
                    if (flightPassengers.Contains(passenger.PassengerID))
                    {
                        Ticket passengerToRemove = flightToUpdate.Tickets.FirstOrDefault(t => t.PassengerID == passenger.PassengerID);
                        _context.Remove(passengerToRemove);
                    }
                }
            }
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.ID == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Flight flight = await _context.Flights
                .Include(f => f.Tickets)
                .SingleAsync(f => f.ID == id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.ID == id);
        }

        // Parameters: int FlightNumber, and int Flight id 
        // Validate the uniqueness of a flight number when new flight is edited or created
        // Returns: true if flightNumber is unique, and warning validation message otherwise
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyFlightNumber(int FlightNumber, int ID)
        {
           foreach(var flight in _context.Flights)
            {
                if(flight.FlightNumber == FlightNumber && flight.ID != ID)
                {
                    return Json($"A flight with this number already exists. Please choose a unique flight number");
                }
            }
            return Json(true);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
