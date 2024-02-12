using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_CT.Models
{
    public class Clinics
    {
        [Key]
        public int ClinicId { get; set; }
        public String ClinicName { get; set; }
        public String ClinicAddress { get; set; }
    }
}