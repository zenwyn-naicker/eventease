using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheEventEaseWebApp.Models;

namespace TheEventEaseWebApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly TheEventEaseDbContext _context;

        public BookingController(TheEventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index(string searchQuery)
        {
            var bookingsQuery = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var query = searchQuery.ToLower();
                bookingsQuery = bookingsQuery.Where(b =>
                    b.Event.EventName.ToLower().Contains(query) ||
                    b.Venue.VenueName.ToLower().Contains(query));
            }

            var bookings = await bookingsQuery.ToListAsync();
            return View(bookings);
        }

        // GET: Booking/Dashboard
        public async Task<IActionResult> Dashboard(
            string searchQuery,
            string searchType,
            int? eventTypeId,
            bool? venueAvailability,
            DateTime? startDate,
            DateTime? endDate)
        {
            // Use BookingDetails view for dashboard summary
            var bookingsQuery = _context.BookingDetails.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var query = searchQuery.ToLower();

                if (searchType == "id" && int.TryParse(searchQuery, out int bookingId))
                {
                    bookingsQuery = bookingsQuery.Where(b => b.BookingId == bookingId);
                }
                else if (searchType == "event")
                {
                    bookingsQuery = bookingsQuery.Where(b => b.EventName.ToLower().Contains(query));
                }
                else
                {
                    bookingsQuery = bookingsQuery.Where(b =>
                        b.BookingId.ToString() == searchQuery ||
                        b.EventName.ToLower().Contains(query) ||
                        b.VenueName.ToLower().Contains(query));
                }
            }

            if (eventTypeId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.EventTypeId == eventTypeId.Value);
            }

            if (venueAvailability.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.VenueAvailability == venueAvailability.Value);
            }

            if (startDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate <= endDate.Value);
            }

            var bookingViewModels = await bookingsQuery.ToListAsync();

            // Populate EventType dropdown list for filter using EventTypes table directly
            var eventTypes = await _context.EventTypes.ToListAsync();
            ViewData["EventTypes"] = new SelectList(eventTypes, "EventTypeId", "Name", eventTypeId);

            // Also pass the selected eventTypeId for the view logic
            ViewData["SelectedEventTypeId"] = eventTypeId?.ToString() ?? "";

            ViewData["SearchQuery"] = searchQuery;
            ViewData["SearchType"] = searchType;
            ViewData["VenueAvailability"] = venueAvailability;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

            return View(bookingViewModels);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            PopulateEventDropDownList();
            PopulateVenueDropDownList();
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (!booking.BookingDate.HasValue)
            {
                ModelState.AddModelError(nameof(booking.BookingDate), "Booking date is required.");
            }
            else
            {
                bool isConflict = await _context.Bookings.AnyAsync(b =>
                    b.EventId == booking.EventId &&
                    b.VenueId == booking.VenueId &&
                    b.BookingDate.HasValue &&
                    b.BookingDate.Value.Date == booking.BookingDate.Value.Date);

                if (isConflict)
                {
                    ModelState.AddModelError("", "This venue is already booked for this event on the selected date.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }

            PopulateEventDropDownList(booking.EventId);
            PopulateVenueDropDownList(booking.VenueId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            PopulateEventDropDownList(booking.EventId);
            PopulateVenueDropDownList(booking.VenueId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (id != booking.BookingId)
                return NotFound();

            if (!booking.BookingDate.HasValue)
            {
                ModelState.AddModelError(nameof(booking.BookingDate), "Booking date is required.");
            }
            else
            {
                bool isConflict = await _context.Bookings.AnyAsync(b =>
                    b.BookingId != booking.BookingId &&
                    b.EventId == booking.EventId &&
                    b.VenueId == booking.VenueId &&
                    b.BookingDate.HasValue &&
                    b.BookingDate.Value.Date == booking.BookingDate.Value.Date);

                if (isConflict)
                {
                    ModelState.AddModelError("", "This venue is already booked for this event on the selected date.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Dashboard));
            }

            PopulateEventDropDownList(booking.EventId);
            PopulateVenueDropDownList(booking.VenueId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }

        // Helpers to populate dropdown lists with nice display info
        private void PopulateEventDropDownList(object selectedEventId = null)
        {
            // First get the data from database, then do string manipulation in memory
            var events = _context.Events
                .Include(e => e.EventType)
                .OrderBy(e => e.EventName)
                .ToList() // Force evaluation here to bring data into memory
                .Select(e => new
                {
                    e.EventId,
                    DisplayName = e.EventName + (e.EventType != null ? $" ({e.EventType.Name})" : "")
                })
                .ToList();

            ViewBag.EventId = new SelectList(events, "EventId", "DisplayName", selectedEventId);
        }

        private void PopulateVenueDropDownList(object selectedVenueId = null)
        {
            var venues = _context.Venues
                .Where(v => v.Availability) // Only show available venues
                .OrderBy(v => v.VenueName)
                .ToList();

            ViewBag.VenueId = new SelectList(venues, "VenueId", "VenueName", selectedVenueId);
        }
    }
}