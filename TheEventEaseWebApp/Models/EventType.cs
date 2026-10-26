using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheEventEaseWebApp.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }

        [Required(ErrorMessage = "Event type name is required.")]
        [StringLength(100, ErrorMessage = "Event type name cannot exceed 100 characters.")]
        [Display(Name = "Event Type Name")]
        public string Name { get; set; } = null!;

        // Navigation property for related events
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}