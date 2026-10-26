using System;
using System.ComponentModel.DataAnnotations;

namespace TheEventEaseWebApp.Models
{
    public class BookingViewModel
    {
        // Booking Info
        [Required]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime? BookingDate { get; set; }

        // Event Info
        [Required(ErrorMessage = "Please select an event.")]
        [Display(Name = "Event")]
        public int? EventId { get; set; }

        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime? EventDate { get; set; }

        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }

        [Display(Name = "Event Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string EventImageUrl { get; set; }

        // New EventType fields
        [Display(Name = "Event Type")]
        public int? EventTypeId { get; set; }

        [Display(Name = "Event Type Name")]
        public string EventTypeName { get; set; }

        // Venue Info
        [Required(ErrorMessage = "Please select a venue.")]
        [Display(Name = "Venue")]
        public int? VenueId { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }

        public string Location { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int? Capacity { get; set; }

        [Display(Name = "Venue Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string VenueImageUrl { get; set; }

        [Display(Name = "Venue Availability")]
        public bool VenueAvailability { get; set; }
    }
}
