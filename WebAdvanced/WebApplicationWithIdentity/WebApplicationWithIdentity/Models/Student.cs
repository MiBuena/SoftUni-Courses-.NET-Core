using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationWithIdentity.ModelBinder;

namespace WebApplicationWithIdentity.Models
{
    public class Student
    {
        [DataType(DataType.Date)]
        public int YearToBind { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Range(1, 200)]
        [Required]
        public int Number { get; set; }
    }
}
