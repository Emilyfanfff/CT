using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_CT.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public String UserId { get; set; }
        public virtual ApplicationUser AspNetUsers { get; set; }  // Navigation property for the doctor

        public byte[] Image { get; set; }


        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String DoctorId { get; set; }
        public String Email { get; set; }

        [Display(Name = "Other issues")]
        public String Description { get; set; }

        [NotMapped] // This property should not be mapped to database table, as it's only for handling file during upload
        public HttpPostedFileBase ImageUpload { get; set; }

        public int? Rating { get; set; }  // Can use nullable int to represent no rating yet
        public bool Rated { get; set; }

        public String RatingComment { get; set; }
        public Result Result { get; set; }

        public int EventId { get; set; }
    }

    public class BookingViewModel
    {
        public Booking Booking { get; set; }
        public List<DoctorEvents> DoctorEventsList { get; set; }
    }


}