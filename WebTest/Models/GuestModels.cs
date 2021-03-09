using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebTest.Models
{
    public class GuestModels
    {
        [Required(ErrorMessage = "Write name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Write email")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Write email correctly")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Write phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Write Your decision")]
        public bool? WillAttend { get; set; }
    }
}