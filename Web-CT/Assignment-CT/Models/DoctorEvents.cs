using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_CT.Models
{
    public class DoctorEvents
    {
        [Key]
        public int EventId { get; set; }

        public String DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

        [Required] 
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        public bool Booked { get; set; }

    }
}