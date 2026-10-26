using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheEventEaseWebApp.Models
{
    public partial class Venue
    {
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters.")]
        [Display(Name = "Venue Name")]
        public string VenueName { get; set; } = null!;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        [Display(Name = "Location")]
        public string Location { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Availability")]
        public bool Availability { get; set; } = true;  // default to available

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
