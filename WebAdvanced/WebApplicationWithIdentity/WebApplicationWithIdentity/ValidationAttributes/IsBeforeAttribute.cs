using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationWithIdentity.ValidationAttributes
{
    public class IsBeforeAttribute : ValidationAttribute
    {
        private const string DateTimeFormat = "dd/MM/yyyy";
        private readonly DateTime date;

        public IsBeforeAttribute(string dateInput)
        {
            this.date = DateTime.ParseExact(dateInput, DateTimeFormat, CultureInfo.InvariantCulture);
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value >= this.date)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;

        }
    }

}
