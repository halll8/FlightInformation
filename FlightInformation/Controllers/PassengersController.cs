using FlightInformation.Data;
using FlightInformation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FlightInformation.Controllers
{
    public class PassengersController : Controller
    {
        private readonly FlightInfoContext _context;

        public PassengersController(FlightInfoContext context)
        {
            _context = context;
        }

        // GET: Passengers
        public async Task<IActionResult> Index()
        {
            var passengers = _context.Passengers.AsNoTracking();
            return View(await passengers.ToListAsync());
        }

        // GET: Passengers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(m => m.PassengerID == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // GET: Passengers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PassengerID,FirstName,LastName")] Passenger passenger)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passenger);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(passenger);
        }

        // GET: Passengers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // POST: Passengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PassengerID,FirstName,LastName")] Passenger passenger)
        {
            if (id != passenger.PassengerID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passenger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassengerExists(passenger.PassengerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(passenger);
        }

        // GET: Passengers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(m => m.PassengerID == id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.PassengerID == id);
        }

        // Parameters: int PassengerID of a given passenger
        // Validates the uniqueness of a PassengerID
        // Returns: true if number is unique and validation warning message otherwise
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPassengerID(int PassengerID)
        {
            if (PassengerExists(PassengerID))
            {
                return Json($"A passenger with this ID already exists. Please choose a unique ID");
            }
            return Json(true);
        }
    }
}
