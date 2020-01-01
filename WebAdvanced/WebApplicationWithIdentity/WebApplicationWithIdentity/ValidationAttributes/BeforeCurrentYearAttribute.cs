using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationWithIdentity.ValidationAttributes
{
    public class BeforeCurrentYearAttribute : ValidationAttribute
    {
        private readonly int _minimumYear;

        public BeforeCurrentYearAttribute(int minimumYear)
        {
            _minimumYear = minimumYear;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateTimeValue = DateTime.Parse(value.ToString());

            if(dateTimeValue > DateTime.UtcNow)
            {
                return new ValidationResult(validationContext.DisplayName + " is after current time");
            }

            if(dateTimeValue.Year < _minimumYear)
            {
                return new ValidationResult(validationContext.DisplayName + " is before allowed time");
            }

            return ValidationResult.Success;
        }
    }
}
