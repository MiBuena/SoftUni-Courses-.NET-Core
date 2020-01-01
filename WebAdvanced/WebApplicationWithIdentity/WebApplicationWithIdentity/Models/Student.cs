using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationWithIdentity.ModelBinder;
using WebApplicationWithIdentity.ValidationAttributes;

namespace WebApplicationWithIdentity.Models
{
    public class Student
    {
        [DataType(DataType.Date)]
        public int YearToBind { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Range(1, 200)]
        public int Number { get; set; }

        [BeforeCurrentYear(2000)]
        public DateTime ProductionYear { get; set; }

        [RegularExpression("[A-Z][a-z]+")]
        public string Description { get; set; }
    }
}
