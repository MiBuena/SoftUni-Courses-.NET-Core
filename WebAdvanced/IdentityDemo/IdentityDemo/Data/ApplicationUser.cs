﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Data
{
    public class ApplicationUser : IdentityUser<string>
    {
        public DateTime? DateOfBirth { get; set; }
    }
}
