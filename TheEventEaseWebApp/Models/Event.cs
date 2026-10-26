using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheEventEaseWebApp.Models
{
    public partial class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        [StringLength(150, ErrorMessage = "Event name cannot exceed 150 characters.")]
        [Display(Name = "Event Name")]
        public string EventName { get; set; } = null!;

        [Required(ErrorMessage = "Event date is required.")]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        // Foreign key for EventType
        [Display(Name = "Event Type")]
        public int? EventTypeId { get; set; }

        // Navigation property
        public virtual EventType? EventType { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
