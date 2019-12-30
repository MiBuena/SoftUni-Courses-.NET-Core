using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationWithIdentity.Enums;

namespace WebApplicationWithIdentity.Models
{
    public class CreateInputModel
    {
        public string Name { get; set; }

        [Display(Name = "First description")]
        [DataType(DataType.Password)]
        public string FirstDescription { get; set; }

        public InternalModel InternalModel { get; set; }

        public BreadType BreadType { get; set; }
    }
}
