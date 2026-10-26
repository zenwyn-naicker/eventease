using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheEventEaseWebApp.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    [Required(ErrorMessage = "Event is required.")]
    [Display(Name = "Event")]
    public int? EventId { get; set; }

    [Required(ErrorMessage = "Venue is required.")]
    [Display(Name = "Venue")]
    public int? VenueId { get; set; }

    [Required(ErrorMessage = "Booking date is required.")]
    [Display(Name = "Booking Date")]
    [DataType(DataType.Date)]
    public DateTime? BookingDate { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Venue? Venue { get; set; }
}
