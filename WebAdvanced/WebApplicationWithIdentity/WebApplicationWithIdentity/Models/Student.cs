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
    }
}
