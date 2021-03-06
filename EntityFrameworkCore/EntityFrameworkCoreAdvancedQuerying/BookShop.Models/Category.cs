﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Models
{
    public class Category
    {
        public Category()
        {
            CategoryBooks = new HashSet<BookCategory>();
        }

        public int CategoryId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<BookCategory> CategoryBooks { get; set; }
    }
}
