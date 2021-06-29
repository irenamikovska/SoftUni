﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andreys.Data
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }        
        public string Description { get; set; }      
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public Gender Gender { get; set; }

    }
}
