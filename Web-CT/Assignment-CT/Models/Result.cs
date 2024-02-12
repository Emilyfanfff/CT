using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Assignment_CT.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorId { get; set; }
        public string ResultDescription { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}")]
        public DateTime ResultDate { get; set; }

        public String Email { get; set; }

        public int BookingId { get; set; }
    }

    public class CheckboxViewModel
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public bool IsChecked { get; set; }
        public String UserId { get; set; }
        public String DoctorName { get; set; }
        public String DoctorId { get; set; }
    }
}